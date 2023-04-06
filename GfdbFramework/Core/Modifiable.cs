using GfdbFramework.Attribute;
using GfdbFramework.DataSource;
using GfdbFramework.Enum;
using GfdbFramework.Field;
using GfdbFramework.Interface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 对数据源提供一个可修改或查询的操作对象。
    /// </summary>
    /// <typeparam name="TSource">数据源中每个成员的类型。</typeparam>
    /// <typeparam name="TSelect">对数据源进行查询返回后新数据源中每个成员的类型。</typeparam>
    public class Modifiable<TSource, TSelect> : Queryable<TSource, TSelect> where TSource : class, new()
    {
        private static readonly Type _BoolType = typeof(bool);

        /// <summary>
        /// 使用指定的数据操作上下文以及数据源初始化一个新的 <see cref="Modifiable{TSource, TSelect}"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该查询对象所使用的数据操作上下文。</param>
        /// <param name="dataSource">该对象所使用的数据源。</param>
        internal Modifiable(IDataContext dataContext, TableDataSource dataSource)
            : base(dataContext, dataSource)
        {
        }

        /// <summary>
        /// 向当前可修改数据源对象中的插入一条数据（成员为默认值时默认不参与插入，除非该成员上标记的 <see cref="FieldAttribute.IsInsertForDefault"/> 属性值为 true）。
        /// </summary>
        /// <param name="entity">需要插入的数据实体对象（若该实体映射到的数据表有自增字段，则新增成功后会自动修改该实体的自增成员值）。</param>
        /// <returns>插入成功时返回 true，否则返回 false。</returns>
        public bool Insert(TSource entity)
        {
            List<OriginalField> fields = new List<OriginalField>();
            List<BasicField> args = new List<BasicField>();

            TableDataSource dataSource = (TableDataSource)DataSource;

            foreach (var item in ((ObjectField)dataSource.RootField).Members)
            {
                OriginalField originalField = (OriginalField)item.Value.Field;

                if (originalField.IsAutoincrement)
                    continue;

                object value = null;

                if (item.Value.Member.MemberType == MemberTypes.Property)
                    value = ((PropertyInfo)item.Value.Member).GetValue(entity, null);
                else if (item.Value.Member.MemberType == MemberTypes.Field)
                    value = ((FieldInfo)item.Value.Member).GetValue(entity);

                if (originalField.IsInsertForDefault || !Helper.CheckIsDefault(value))
                {
                    args.Add(new ConstantField(DataContext, value == null ? originalField.DataType : value.GetType(), value));

                    fields.Add((OriginalField)item.Value.Field);
                }
            }

            if (args.Count < 1)
                throw new Exception($"在对 {typeof(TSource).FullName} 类型所映射的数据表插入数据时未能从参数实体对象中提取出任何需要插入的字段值信息");

            IParameterContext parameterContext = DataContext.CreateParameterContext(true);

            string insertSql = DataContext.SqlFactory.GenerateInsertSql(parameterContext, dataSource, fields, args);

            bool result;

            if (dataSource.Autoincrement != null)
            {
                result = DataContext.DatabaseOperation.ExecuteNonQuery(insertSql, System.Data.CommandType.Text, parameterContext.ToList(), out long value) == 1;

                object autoincrementValue = value;

                switch (dataSource.Autoincrement.Field.DataType.FullName)
                {
                    case "System.Int16":
                        autoincrementValue = (short)value;
                        break;
                    case "System.UInt16":
                        autoincrementValue = (ushort)value;
                        break;
                    case "System.Int32":
                        autoincrementValue = (int)value;
                        break;
                    case "System.UInt32":
                        autoincrementValue = (uint)value;
                        break;
                    case "System.UInt64":
                        autoincrementValue = (ulong)value;
                        break;
                    case "System.Byte":
                        autoincrementValue = (byte)value;
                        break;
                    case "System.SByte":
                        autoincrementValue = (sbyte)value;
                        break;
                    case "System.Decimal":
                        autoincrementValue = (decimal)value;
                        break;
                    case "System.Double":
                        autoincrementValue = (double)value;
                        break;
                    case "System.Single":
                        autoincrementValue = (float)value;
                        break;
                }

                if (dataSource.Autoincrement.Member.MemberType == MemberTypes.Property)
                    ((PropertyInfo)dataSource.Autoincrement.Member).SetValue(entity, autoincrementValue, null);
                else if (dataSource.Autoincrement.Member.MemberType == MemberTypes.Field)
                    ((FieldInfo)dataSource.Autoincrement.Member).SetValue(entity, autoincrementValue);
            }
            else
            {
                result = DataContext.DatabaseOperation.ExecuteNonQuery(insertSql, System.Data.CommandType.Text, parameterContext.ToList()) == 1;
            }

            return result;
        }

        /// <summary>
        /// 向当前可修改数据源对象中的插入多条数据（若 <paramref name="entitys"/> 参数为 <see cref="Queryable{TSource, TSelect}"/> 对象则将采用 insert Table(fields...) select fields... from Table 的方式实现，同时也不会应用成员的 <see cref="FieldAttribute.IsInsertForDefault"/> 特性）。
        /// </summary>
        /// <param name="entitys">需要插入的实体对象枚举器。</param>
        /// <returns>插入成功的数据条数。</returns>
        public int Insert(IEnumerable<TSource> entitys)
        {
            int result = 0;

            if (entitys != null)
            {
                if (entitys is Queryable queryable)
                {
                    IParameterContext parameterContext = DataContext.CreateParameterContext(true);

                    string insertSql = DataContext.SqlFactory.GenerateInsertSql(parameterContext, (TableDataSource)DataSource, queryable.DataSource);

                    result = DataContext.DatabaseOperation.ExecuteNonQuery(insertSql, System.Data.CommandType.Text, parameterContext.ToList());
                }
                else
                {
                    DataContext.DatabaseOperation.OpenConnection(OpenedMode.Framework);

                    try
                    {
                        foreach (var item in entitys)
                        {
                            if (Insert(item))
                                result += 1;
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        DataContext.DatabaseOperation.CloseConnection(OpenedMode.Framework);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 向当前可修改数据源对象中的插入一条数据。
        /// </summary>
        /// <param name="entity">需要插入的数据实体对象表达式树。</param>
        /// <returns>插入成功返回 true，否则返回 false。</returns>
        public bool Insert(Expression<Func<TSource>> entity)
        {
            int startTableAliasIndex = 0;

            List<OriginalField> fields = new List<OriginalField>();

            List<BasicField> args = new List<BasicField>();

            TableDataSource dataSource = (TableDataSource)DataSource;

            ObjectField rootField = (ObjectField)dataSource.RootField;

            ObjectField insertField = (ObjectField)Helper.ExtractField(DataContext, entity.Body, ExtractWay.Other, new ReadOnlyDictionary<string, DataSource.DataSource>(null), ref startTableAliasIndex);

            if (insertField.Members != null && insertField.Members.Count > 0)
            {
                foreach (var item in insertField.Members)
                {
                    if (rootField.Members.TryGetValue(item.Key, out MemberInfo memberInfo))
                    {
                        fields.Add((OriginalField)memberInfo.Field);

                        args.Add((BasicField)item.Value.Field);
                    }
                    else
                    {
                        throw new Exception($"在对 {typeof(TSource).FullName} 类型所映射的数据表插入数据时表达式树中至少有一个实体成员未能找到对应的数据库表字段信息");
                    }
                }
            }

            if (args.Count < 1)
                throw new Exception($"在对 {typeof(TSource).FullName} 类型所映射的数据表插入数据时未能从参数实体对象中提取出任何需要插入的字段值信息");

            IParameterContext parameterContext = DataContext.CreateParameterContext(true);

            string insertSql = DataContext.SqlFactory.GenerateInsertSql(parameterContext, dataSource, fields, args);

            return DataContext.DatabaseOperation.ExecuteNonQuery(insertSql, System.Data.CommandType.Text, parameterContext.ToList()) == 1;
        }

        /// <summary>
        /// 删除该对象数据源中所有的数据信息。
        /// </summary>
        /// <returns>删除成功的数据条数。</returns>
        public int Delete()
        {
            IParameterContext parameterContext = DataContext.CreateParameterContext(true);

            string deleteSql = DataContext.SqlFactory.GenerateDeleteSql(parameterContext, new TableDataSource[] { (TableDataSource)DataSource }, DataSource, null);

            return DataContext.DatabaseOperation.ExecuteNonQuery(deleteSql, System.Data.CommandType.Text, parameterContext.ToList());
        }

        /// <summary>
        /// 根据主键值删除数据源中的数据行。
        /// </summary>
        /// <typeparam name="TPrimaryKey">主键值类型。</typeparam>
        /// <param name="primaryKey">需要删除数据行的主键值。</param>
        /// <returns>删除成功返回 true，否则返回 false。</returns>
        public bool Delete<TPrimaryKey>(TPrimaryKey primaryKey) where TPrimaryKey : struct
        {
            return Delete((object)primaryKey);
        }

        /// <summary>
        /// 根据主键值删除数据源中的数据行。
        /// </summary>
        /// <param name="primaryKey">需要删除数据行的主键值。</param>
        /// <returns>删除成功返回 true，否则返回 false。</returns>
        public bool Delete(string primaryKey)
        {
            return Delete((object)primaryKey);
        }

        /// <summary>
        /// 使用指定的条件删除数据源中的数据行。
        /// </summary>
        /// <param name="where">需要删除数据行的条件限定表达式树。</param>
        /// <returns>删除成功的数据条数。</returns>
        public int Delete(Expression<Func<TSource, bool>> where)
        {
            BasicField whereField = null;

            if (where != null)
            {
                int nextTableAliasIndex = DataSource.AliasIndex + 1;

                whereField = (BasicField)Helper.ExtractField(DataContext, where.Body, ExtractWay.Other, new ReadOnlyDictionary<string, DataSource.DataSource>(new Dictionary<string, DataSource.DataSource>() { { where.Parameters[0].Name, DataSource } }), ref nextTableAliasIndex);
            }

            IParameterContext parameterContext = DataContext.CreateParameterContext(true);

            string deleteSql = DataContext.SqlFactory.GenerateDeleteSql(parameterContext, new TableDataSource[] { (TableDataSource)DataSource }, DataSource, whereField);

            return DataContext.DatabaseOperation.ExecuteNonQuery(deleteSql, System.Data.CommandType.Text, parameterContext.ToList());
        }

        /// <summary>
        /// 根据主键值删除数据源中的数据行。
        /// </summary>
        /// <param name="primaryKey">需要删除数据行的主键值。</param>
        /// <returns>删除成功返回 true，否则返回 false。</returns>
        private bool Delete(object primaryKey)
        {
            TableDataSource dataSource = (TableDataSource)DataSource;

            if (dataSource.PrimaryKey == null)
                throw new Exception($"根据主键值删除数据行时未能找到 {typeof(TSource).FullName} 类所映射数据表的主键字段信息");

            Type primaryKeyType = primaryKey == null ? dataSource.PrimaryKey.Field.DataType : primaryKey.GetType();

            if (primaryKeyType.IsEnum)
                primaryKey = Convert.ToInt32((System.Enum)primaryKey);

            IParameterContext parameterContext = DataContext.CreateParameterContext(true);

            string deleteSql = DataContext.SqlFactory.GenerateDeleteSql(parameterContext, new TableDataSource[] { (TableDataSource)DataSource }, DataSource, new BinaryField(DataContext, _BoolType, OperationType.Equal, (BasicField)dataSource.PrimaryKey.Field, new ConstantField(DataContext, primaryKeyType, primaryKey)));

            return DataContext.DatabaseOperation.ExecuteNonQuery(deleteSql, System.Data.CommandType.Text, parameterContext.ToList()) == 1;
        }

        /// <summary>
        /// 根据实体中的主键成员值修改实体其他成员所映射数据库表字段的值（将以 <paramref name="entity"/> 参数中对应数据库主键字段成员的值作为更新条件，同时若某个实体成员值为默认值，则该成员不参与修改操作）。
        /// </summary>
        /// <param name="entity">更新后的数据字段对应实体值。</param>
        /// <returns>修改成功返回 true，否则返回 false。</returns>
        public bool Update(TSource entity)
        {
            return Update(entity, false) == 1;
        }

        /// <summary>
        /// 更新当前对象映射数据源中的数据（实体成员值为默认值或映射数据库表字段为主键、自增字段时，则该成员不参与修改操作）。
        /// </summary>
        /// <param name="entity">更新后的数据字段对应实体值。</param>
        /// <param name="updateAll">是否更新当前对象映射数据源中的所有数据行（若为 false 则只修改与实体映射主键值相匹配的数据行）。</param>
        /// <returns>修改成功的数据条数。</returns>
        public int Update(TSource entity, bool updateAll)
        {
            BinaryField whereField = null;

            if (!updateAll)
            {
                TableDataSource dataSource = (TableDataSource)DataSource;

                object primaryKeyValue = null;

                if (dataSource.PrimaryKey == null)
                    throw new Exception($"在对 {typeof(TSource).FullName} 类型所映射的数据表执行数据修改操作时未能找到成员映射的主键字段信息");

                if (dataSource.PrimaryKey.Member.MemberType == MemberTypes.Property)
                    primaryKeyValue = ((PropertyInfo)dataSource.PrimaryKey.Member).GetValue(entity, null);
                else if (dataSource.PrimaryKey.Member.MemberType == MemberTypes.Field)
                    primaryKeyValue = ((FieldInfo)dataSource.PrimaryKey.Member).GetValue(entity);

                whereField = new BinaryField(DataContext, _BoolType, OperationType.Equal, (BasicField)dataSource.PrimaryKey.Field, new ConstantField(DataContext, primaryKeyValue == null ? dataSource.PrimaryKey.Field.DataType : primaryKeyValue.GetType(), primaryKeyValue));
            }

            return Update(entity, whereField);
        }

        /// <summary>
        /// 使用指定的条件约束更新当前对象映射数据源中的数据。
        /// </summary>
        /// <param name="entity">更新后的数据字段对应实体值。</param>
        /// <param name="where">更新时的条件约束表达式树。</param>
        /// <returns>更新成功的数据条数。</returns>
        public int Update(TSource entity, Expression<Func<TSource, bool>> where)
        {
            BasicField whereField = null;

            if (where != null)
            {
                int nextTableAliasIndex = DataSource.AliasIndex + 1;

                whereField = (BasicField)Helper.ExtractField(DataContext, where.Body, ExtractWay.Other, new ReadOnlyDictionary<string, DataSource.DataSource>(new Dictionary<string, DataSource.DataSource>() { { where.Parameters[0].Name, DataSource } }), ref nextTableAliasIndex);
            }

            return Update(entity, whereField);
        }

        /// <summary>
        /// 更新当前对象映射数据源中所有的数据。
        /// </summary>
        /// <param name="entity">更新后的数据字段对应实体表达式树。</param>
        /// <returns>更新成功的数据条数。</returns>
        public int Update(Expression<Func<TSource, TSource>> entity)
        {
            return Update(entity, null);
        }

        /// <summary>
        /// 使用指定的条件约束更新当前对象映射数据源中的数据。
        /// </summary>
        /// <param name="entity">更新后的数据字段对应实体表达式树。</param>
        /// <param name="where">更新时的条件约束表达式树。</param>
        /// <returns>更新成功的数据条数。</returns>
        public int Update(Expression<Func<TSource, TSource>> entity, Expression<Func<TSource, bool>> where)
        {
            int nextTableAliasIndex = DataSource.AliasIndex + 1;

            ObjectField updateField = (ObjectField)Helper.ExtractField(DataContext, entity.Body, ExtractWay.Other, new ReadOnlyDictionary<string, DataSource.DataSource>(new Dictionary<string, DataSource.DataSource>() { { entity.Parameters[0].Name, DataSource } }), ref nextTableAliasIndex);

            List<UpdateItem> updateFields = new List<UpdateItem>();

            TableDataSource dataSource = (TableDataSource)DataSource;

            ObjectField rootField = (ObjectField)dataSource.RootField;

            if (updateField.Members != null && updateField.Members.Count > 0)
            {
                foreach (var item in updateField.Members)
                {
                    if (item.Value.Field is BasicField valueField)
                    {
                        if (rootField.Members.TryGetValue(item.Key, out MemberInfo memberInfo))
                            updateFields.Add(new UpdateItem((OriginalField)memberInfo.Field, valueField));
                        else
                            throw new Exception($"在对 {typeof(TSource).FullName} 类型所映射的数据表执行数据修改操作时成员 {item.Key} 未能找到对应的数据表字段信息");
                    }
                    else
                    {
                        throw new Exception($"在对 {typeof(TSource).FullName} 类型所映射的数据表执行数据修改操作时成员 {item.Key} 的值不是基础数据类型");
                    }
                }
            }

            if (updateFields.Count < 1)
                throw new Exception($"在对 {typeof(TSource).FullName} 类型所映射的数据表执行数据修改操作时未能提取到任何需要修改的字段信息");

            BasicField whereField = null;

            if (where != null)
                whereField = (BasicField)Helper.ExtractField(DataContext, where.Body, ExtractWay.Other, new ReadOnlyDictionary<string, DataSource.DataSource>(new Dictionary<string, DataSource.DataSource>() { { where.Parameters[0].Name, DataSource } }), ref nextTableAliasIndex);

            IParameterContext parameterContext = DataContext.CreateParameterContext(true);

            string updateSql = DataContext.SqlFactory.GenerateUpdateSql(parameterContext, new List<UpdateGroup>() { new UpdateGroup(dataSource, updateFields) }, dataSource, whereField);

            return DataContext.DatabaseOperation.ExecuteNonQuery(updateSql, System.Data.CommandType.Text, parameterContext.ToList());
        }

        /// <summary>
        /// 使用指定的条件约束更新当前对象映射数据源中的数据。
        /// </summary>
        /// <param name="entity">更新后的数据字段对应实体值。</param>
        /// <param name="whereField">更新时的条件约束字段信息。</param>
        /// <returns>更新成功的数据条数。</returns>
        private int Update(TSource entity, BasicField whereField)
        {
            List<UpdateItem> updateFields = new List<UpdateItem>();

            TableDataSource dataSource = (TableDataSource)DataSource;

            foreach (var item in ((ObjectField)dataSource.RootField).Members)
            {
                OriginalField originalField = (OriginalField)item.Value.Field;

                if (originalField.IsAutoincrement || originalField.IsPrimaryKey)
                    continue;

                object value = null;
                Type valueType = null;

                if (item.Value.Member.MemberType == MemberTypes.Property)
                {
                    value = ((PropertyInfo)item.Value.Member).GetValue(entity, null);

                    valueType = ((PropertyInfo)item.Value.Member).PropertyType;
                }
                else if (item.Value.Member.MemberType == MemberTypes.Field)
                {
                    value = ((FieldInfo)item.Value.Member).GetValue(entity);

                    valueType = ((FieldInfo)item.Value.Member).FieldType;
                }

                if (originalField.IsUpdateForDefault || !Helper.CheckIsDefault(value))
                    updateFields.Add(new UpdateItem(originalField, new ConstantField(DataContext, valueType, value)));
            }

            if (updateFields.Count < 1)
                throw new Exception($"在对 {typeof(TSource).FullName} 类型所映射的数据表执行数据修改操作时未能从指定实体中提取出任何需要修改的字段信息");

            IParameterContext parameterContext = DataContext.CreateParameterContext(true);

            string updateSql = DataContext.SqlFactory.GenerateUpdateSql(parameterContext, new List<UpdateGroup>() { new UpdateGroup(dataSource, updateFields) }, dataSource, whereField);

            return DataContext.DatabaseOperation.ExecuteNonQuery(updateSql, System.Data.CommandType.Text, parameterContext.ToList());
        }

        /// <summary>
        /// 以当前对象数据源做左连接操作。
        /// </summary>
        /// <typeparam name="TJoinSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TJoinSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联后的多表操作对象。</returns>
        public new ModifiableMultipleJoin<TSource, TSource, TSelect, TJoinSource, TJoinSelect> LeftJoin<TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSource, TJoinSource, bool>> on)
        {
            return (ModifiableMultipleJoin<TSource, TSource, TSelect, TJoinSource, TJoinSelect>)Join(SourceType.LeftJoin, right, on, null);
        }

        /// <summary>
        /// 以当前对象数据源做右连接操作。
        /// </summary>
        /// <typeparam name="TJoinSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TJoinSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联后的多表操作对象。</returns>
        public new ModifiableMultipleJoin<TSource, TSource, TSelect, TJoinSource, TJoinSelect> RightJoin<TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSource, TJoinSource, bool>> on)
        {
            return (ModifiableMultipleJoin<TSource, TSource, TSelect, TJoinSource, TJoinSelect>)Join(SourceType.RightJoin, right, on, null);
        }

        /// <summary>
        /// 以当前对象数据源做直连接操作。
        /// </summary>
        /// <typeparam name="TJoinSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TJoinSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联后的多表操作对象。</returns>
        public new ModifiableMultipleJoin<TSource, TSource, TSelect, TJoinSource, TJoinSelect> InnerJoin<TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSource, TJoinSource, bool>> on)
        {
            return (ModifiableMultipleJoin<TSource, TSource, TSelect, TJoinSource, TJoinSelect>)Join(SourceType.InnerJoin, right, on, null);
        }

        /// <summary>
        /// 以当前对象数据源做全连接操作。
        /// </summary>
        /// <typeparam name="TJoinSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TJoinSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联后的多表操作对象。</returns>
        public new ModifiableMultipleJoin<TSource, TSource, TSelect, TJoinSource, TJoinSelect> FullJoin<TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSource, TJoinSource, bool>> on)
        {
            return (ModifiableMultipleJoin<TSource, TSource, TSelect, TJoinSource, TJoinSelect>)Join(SourceType.FullJoin, right, on, null);
        }

        /// <summary>
        /// 以当前对象数据源做交叉连接操作。
        /// </summary>
        /// <typeparam name="TJoinSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TJoinSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <returns>关联后的多表操作对象。</returns>
        public new ModifiableMultipleJoin<TSource, TSource, TSelect, TJoinSource, TJoinSelect> CrossJoin<TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right)
        {
            return (ModifiableMultipleJoin<TSource, TSource, TSelect, TJoinSource, TJoinSelect>)Join(SourceType.CrossJoin, right, null, null);
        }

        /// <summary>
        /// 以当前对象数据源做关联操作。
        /// </summary>
        /// <typeparam name="TJoinSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TJoinSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="joinType">关联类型。</param>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <param name="existentParameters">调用该方法时已经传入的参数集合。</param>
        /// <returns>关联后的多表操作对象。</returns>
        internal override MultipleJoin Join<TJoinSource, TJoinSelect>(SourceType joinType, Queryable<TJoinSource, TJoinSelect> right, LambdaExpression on, ReadOnlyDictionary<string, DataSource.DataSource> existentParameters)
        {
            MultipleJoin<TSource, TSelect, TJoinSource, TJoinSelect> multipleJoin = (MultipleJoin<TSource, TSelect, TJoinSource, TJoinSelect>)base.Join(joinType, right, on, existentParameters);

            return new ModifiableMultipleJoin<TSource, TSource, TSelect, TJoinSource, TJoinSelect>(multipleJoin.DataContext, multipleJoin.Left, multipleJoin.Right, multipleJoin.On, multipleJoin.JoinType);
        }
    }
}

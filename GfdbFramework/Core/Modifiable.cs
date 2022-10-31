using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using GfdbFramework.DataSource;
using GfdbFramework.Enum;
using GfdbFramework.Field;
using GfdbFramework.Interface;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 对数据源提供一个可修改的操作对象。
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
        internal Modifiable(IDataContext dataContext, BasicDataSource dataSource)
            : base(dataContext, dataSource)
        {
        }

        /// <summary>
        /// 向当前可修改数据源对象中的插入一条数据（成员为默认值时默认不参与插入，除非该成员上标记的 <see cref="Attribute.FieldAttribute.IsInsertForDefault"/> 属性值为 true）。
        /// </summary>
        /// <param name="entity">需要插入的数据实体对象（若该实体映射到的数据表有自增字段，则新增成功后会自动修改该实体的自增成员值）。</param>
        /// <returns>插入成功时返回 true，否则返回 false。</returns>
        public bool Insert(TSource entity)
        {
            List<OriginalField> fields = new List<OriginalField>();
            List<BasicField> pars = new List<BasicField>();

            OriginalDataSource dataSource = (OriginalDataSource)DataSource;

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
                    pars.Add(new ConstantField(value == null ? originalField.DataType : value.GetType(), value));
                    fields.Add((OriginalField)item.Value.Field);
                }
            }

            if (pars.Count < 1)
                throw new Exception(string.Format("在对 {0} 类型所映射的数据表插入数据时未能从参数实体对象中提取出任何需要插入的字段值信息", typeof(TSource).FullName));

            string insertSql = DataContext.SqlFactory.GenerateInsertSql(DataContext, dataSource, (Realize.ReadOnlyList<OriginalField>)fields, (Realize.ReadOnlyList<BasicField>)pars, out Interface.IReadOnlyList<DbParameter> parameters);
            bool result;

            if (dataSource.Autoincrement != null)
            {
                result = DataContext.DatabaseOperation.ExecuteNonQuery(insertSql, System.Data.CommandType.Text, parameters, out long value) == 1;

                object autoincrementValue = value;

                switch (dataSource.Autoincrement.Field.DataType.FullName)
                {
                    case "System.Int32":
                        autoincrementValue = (int)value;
                        break;
                    case "System.UInt32":
                        autoincrementValue = (uint)value;
                        break;
                    case "System.UInt64":
                        autoincrementValue = (ulong)value;
                        break;
                    case "System.Int16":
                        autoincrementValue = (short)value;
                        break;
                    case "System.UInt16":
                        autoincrementValue = (ushort)value;
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
                result = DataContext.DatabaseOperation.ExecuteNonQuery(insertSql, System.Data.CommandType.Text, parameters) == 1;
            }

            return result;
        }

        /// <summary>
        /// 向当前可修改数据源对象中的插入多条数据（若 <paramref name="entitys"/> 参数为 <see cref="Queryable{TSource, TSelect}"/> 对象则将采用 insert Table(fields...) select fields... from Table 的方式实现，同时也不会应用成员的 <see cref="Attribute.FieldAttribute.IsInsertForDefault"/> 特性）。
        /// </summary>
        /// <param name="entitys">需要插入的实体对象枚举器。</param>
        /// <returns>插入成功的数据条数。</returns>
        public int Insert(IEnumerable<TSource> entitys)
        {
            if (entitys == null)
                throw new ArgumentNullException(nameof(entitys), string.Format("在对 {0} 类型所映射的数据表插入多条数据时参数对象不能为空", typeof(TSource).FullName));

            int result = 0;

            if (entitys is Queryable queryable)
            {
                List<OriginalField> fields = new List<OriginalField>();

                ObjectField queryField = (ObjectField)(queryable.DataSource.SelectField ?? queryable.DataSource.RootField);
                ObjectField rootField = (ObjectField)DataSource.RootField;

                bool needResetQueryField = false;
                Dictionary<string, MemberInfo> selectFields = new Dictionary<string, MemberInfo>();

                foreach (var item in queryField.Members)
                {
                    OriginalField originalField = (OriginalField)rootField.Members[item.Key].Field;

                    if (originalField.IsAutoincrement)
                    {
                        needResetQueryField = true;

                        continue;
                    }

                    selectFields.Add(item.Key, item.Value);

                    fields.Add(originalField);
                }

                string insertSql = DataContext.SqlFactory.GenerateInsertSql(DataContext, (OriginalDataSource)DataSource, (Realize.ReadOnlyList<OriginalField>)fields, needResetQueryField ? queryable.DataSource.Copy().SetSelectField(new ObjectField(queryField.DataType, queryField.ConstructorInfo, selectFields)) : queryable.DataSource, out Interface.IReadOnlyList<DbParameter> parameters);

                result = DataContext.DatabaseOperation.ExecuteNonQuery(insertSql, System.Data.CommandType.Text, parameters);
            }
            else
            {
                DataContext.DatabaseOperation.OpenConnection(ConnectionOpenedMode.Framework);

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
                    DataContext.DatabaseOperation.CloseConnection(ConnectionOpenedMode.Framework);
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

            List<BasicField> pars = new List<BasicField>();

            OriginalDataSource dataSource = (OriginalDataSource)DataSource;

            ObjectField rootField = (ObjectField)dataSource.RootField;

            ObjectField insertField = (ObjectField)Helper.ExtractField(entity.Body, ExtractType.Default, null, ref startTableAliasIndex);

            if (insertField.Members != null && insertField.Members.Count > 0)
            {
                foreach (var item in insertField.Members)
                {
                    if (rootField.Members.TryGetValue(item.Key, out MemberInfo memberInfo))
                    {
                        fields.Add((OriginalField)memberInfo.Field);
                        pars.Add((BasicField)item.Value.Field);
                    }
                    else
                    {
                        throw new Exception(string.Format("在对 {0} 类型所映射的数据表插入数据时表达式树中至少有一个实体成员未能找到对应的数据库表字段信息", typeof(TSource).FullName));
                    }
                }
            }

            if (pars.Count < 1)
                throw new Exception(string.Format("在对 {0} 类型所映射的数据表插入数据时未能从参数实体对象中提取出任何需要插入的字段值信息", typeof(TSource).FullName));

            string insertSql = DataContext.SqlFactory.GenerateInsertSql(DataContext, dataSource, (Realize.ReadOnlyList<OriginalField>)fields, (Realize.ReadOnlyList<BasicField>)pars, out Interface.IReadOnlyList<DbParameter> parameters);

            return DataContext.DatabaseOperation.ExecuteNonQuery(insertSql, System.Data.CommandType.Text, parameters) == 1;
        }

        /// <summary>
        /// 删除该对象数据源中所有的数据信息。
        /// </summary>
        /// <returns>删除成功的数据条数。</returns>
        public int Delete()
        {
            string deleteSql = DataContext.SqlFactory.GenerateDeleteSql(DataContext, new OriginalDataSource[] { (OriginalDataSource)DataSource }, DataSource, null, out Interface.IReadOnlyList<DbParameter> parameters);

            return DataContext.DatabaseOperation.ExecuteNonQuery(deleteSql, System.Data.CommandType.Text, parameters);
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

                whereField = (BasicField)Helper.ExtractField(where.Body, ExtractType.Default, new Realize.ReadOnlyDictionary<string, ParameterInfo>(new KeyValuePair<string, ParameterInfo>(where.Parameters[0].Name, new ParameterInfo(true, DataSource))), ref nextTableAliasIndex);
            }

            string deleteSql = DataContext.SqlFactory.GenerateDeleteSql(DataContext, new OriginalDataSource[] { (OriginalDataSource)DataSource }, DataSource, whereField, out Interface.IReadOnlyList<DbParameter> parameters);

            return DataContext.DatabaseOperation.ExecuteNonQuery(deleteSql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 根据主键值删除数据源中的数据行。
        /// </summary>
        /// <param name="primaryKey">需要删除数据行的主键值。</param>
        /// <returns>删除成功返回 true，否则返回 false。</returns>
        private bool Delete(object primaryKey)
        {
            OriginalDataSource dataSource = (OriginalDataSource)DataSource;

            if (dataSource.PrimaryKey == null)
                throw new Exception(string.Format("根据主键值删除数据行时未能找到 {0} 类所映射数据表的主键字段信息", typeof(TSource).FullName));

            Type primaryKeyType = primaryKey == null ? dataSource.PrimaryKey.Field.DataType : primaryKey.GetType();

            if (primaryKeyType.IsEnum)
                primaryKey = Convert.ToInt32((System.Enum)primaryKey);

            string deleteSql = DataContext.SqlFactory.GenerateDeleteSql(DataContext, new OriginalDataSource[] { (OriginalDataSource)DataSource }, DataSource, new BinaryField(_BoolType, OperationType.Equal, (BasicField)dataSource.PrimaryKey.Field, new ConstantField(primaryKeyType, primaryKey)), out Interface.IReadOnlyList<DbParameter> parameters);

            return DataContext.DatabaseOperation.ExecuteNonQuery(deleteSql, System.Data.CommandType.Text, parameters) == 1;
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
        /// 根据实体中的主键成员值修改实体其他成员所映射数据库表字段的值（实体成员值为默认值或映射数据库表字段为主键、自增字段时，则该成员不参与修改操作）。
        /// </summary>
        /// <param name="entity">更新后的数据字段对应实体值。</param>
        /// <param name="updateAll">是否更新当前对象映射数据源中的所有数据行。</param>
        /// <returns>修改成功的数据条数。</returns>
        public int Update(TSource entity, bool updateAll)
        {
            BinaryField whereField = null;

            if (!updateAll)
            {
                OriginalDataSource dataSource = (OriginalDataSource)DataSource;

                object primaryKeyValue = null;

                if (dataSource.PrimaryKey == null)
                    throw new Exception(string.Format("在对 {0} 类型所映射的数据表执行数据修改操作时未能找到成员映射的主键字段信息", typeof(TSource).FullName));

                if (dataSource.PrimaryKey.Member.MemberType == MemberTypes.Property)
                    primaryKeyValue = ((PropertyInfo)dataSource.PrimaryKey.Member).GetValue(entity, null);
                else if (dataSource.PrimaryKey.Member.MemberType == MemberTypes.Field)
                    primaryKeyValue = ((FieldInfo)dataSource.PrimaryKey.Member).GetValue(entity);

                whereField = new BinaryField(_BoolType, OperationType.Equal, (BasicField)dataSource.PrimaryKey.Field, new ConstantField(primaryKeyValue == null ? dataSource.PrimaryKey.Field.DataType : primaryKeyValue.GetType(), primaryKeyValue));
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

                whereField = (BasicField)Helper.ExtractField(where.Body, ExtractType.Default, new Realize.ReadOnlyDictionary<string, ParameterInfo>(new KeyValuePair<string, ParameterInfo>(where.Parameters[0].Name, new ParameterInfo(true, DataSource))), ref nextTableAliasIndex);
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

            ObjectField updateField = (ObjectField)Helper.ExtractField(entity.Body, ExtractType.Default, new Realize.ReadOnlyDictionary<string, ParameterInfo>(new KeyValuePair<string, ParameterInfo>(entity.Parameters[0].Name, new ParameterInfo(true, DataSource))), ref nextTableAliasIndex);

            List<ModifyInfo> modifyFields = new List<ModifyInfo>();

            OriginalDataSource dataSource = (OriginalDataSource)DataSource;

            ObjectField rootField = (ObjectField)dataSource.RootField;

            if (updateField.Members != null && updateField.Members.Count > 0)
            {
                foreach (var item in updateField.Members)
                {
                    if (rootField.Members.TryGetValue(item.Key, out MemberInfo memberInfo))
                        modifyFields.Add(new ModifyInfo((OriginalField)memberInfo.Field, dataSource, (BasicField)item.Value.Field));
                    else
                        throw new Exception(string.Format("在对 {0} 类型所映射的数据表执行数据修改操作时成员 {1} 未能找到对应的数据表字段信息", typeof(TSource).FullName, item.Key));
                }
            }

            if (modifyFields.Count < 1)
                throw new Exception(string.Format("在对 {0} 类型所映射的数据表执行数据修改操作时未能提取出任何需要修改的字段信息", typeof(TSource).FullName));

            BasicField whereField = null;

            if (where != null)
                whereField = (BasicField)Helper.ExtractField(where.Body, ExtractType.Default, new Realize.ReadOnlyDictionary<string, ParameterInfo>(new KeyValuePair<string, ParameterInfo>(where.Parameters[0].Name, new ParameterInfo(true, DataSource))), ref nextTableAliasIndex);

            if (modifyFields.Count < 1)
                throw new Exception(string.Format("在对 {0} 类型所映射的数据表执行数据修改操作时未能提取出任何需要修改的字段信息", typeof(TSource).FullName));

            string updateSql = DataContext.SqlFactory.GenerateUpdateSql(DataContext, (Realize.ReadOnlyList<ModifyInfo>)modifyFields, dataSource, whereField, out Interface.IReadOnlyList<DbParameter> parameters);

            return DataContext.DatabaseOperation.ExecuteNonQuery(updateSql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 使用指定的条件约束更新当前对象映射数据源中的数据。
        /// </summary>
        /// <param name="entity">更新后的数据字段对应实体值。</param>
        /// <param name="whereField">更新时的条件约束字段信息。</param>
        /// <returns>更新成功的数据条数。</returns>
        private int Update(TSource entity, BasicField whereField)
        {
            List<ModifyInfo> modifyFields = new List<ModifyInfo>();

            OriginalDataSource dataSource = (OriginalDataSource)DataSource;

            foreach (var item in ((ObjectField)dataSource.RootField).Members)
            {
                OriginalField originalField = (OriginalField)item.Value.Field;

                if (originalField.IsAutoincrement || originalField.IsPrimaryKey)
                    continue;

                object value = null;

                if (item.Value.Member.MemberType == MemberTypes.Property)
                    value = ((PropertyInfo)item.Value.Member).GetValue(entity, null);
                else if (item.Value.Member.MemberType == MemberTypes.Field)
                    value = ((FieldInfo)item.Value.Member).GetValue(entity);

                if (originalField.IsUpdateForDefault || !Helper.CheckIsDefault(value))
                    modifyFields.Add(new ModifyInfo(originalField, (OriginalDataSource)DataSource, new ConstantField(value.GetType(), value)));
            }

            if (modifyFields.Count < 1)
                throw new Exception(string.Format("在对 {0} 类型所映射的数据表执行数据修改操作时未能提取出任何需要修改的字段信息", typeof(TSource).FullName));

            string updateSql = DataContext.SqlFactory.GenerateUpdateSql(DataContext, (Realize.ReadOnlyList<ModifyInfo>)modifyFields, dataSource, whereField, out Interface.IReadOnlyList<DbParameter> parameters);

            return DataContext.DatabaseOperation.ExecuteNonQuery(updateSql, System.Data.CommandType.Text, parameters);
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
            return (ModifiableMultipleJoin<TSource, TSource, TSelect, TJoinSource, TJoinSelect>)Join(DataSourceType.LeftJoin, right, on, null);
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
            return (ModifiableMultipleJoin<TSource, TSource, TSelect, TJoinSource, TJoinSelect>)Join(DataSourceType.RightJoin, right, on, null);
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
            return (ModifiableMultipleJoin<TSource, TSource, TSelect, TJoinSource, TJoinSelect>)Join(DataSourceType.InnerJoin, right, on, null);
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
            return (ModifiableMultipleJoin<TSource, TSource, TSelect, TJoinSource, TJoinSelect>)Join(DataSourceType.FullJoin, right, on, null);
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
            return (ModifiableMultipleJoin<TSource, TSource, TSelect, TJoinSource, TJoinSelect>)Join(DataSourceType.CrossJoin, right, null, null);
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
        internal override MultipleJoin Join<TJoinSource, TJoinSelect>(DataSourceType joinType, Queryable<TJoinSource, TJoinSelect> right, LambdaExpression on, Interface.IReadOnlyDictionary<string, ParameterInfo> existentParameters)
        {
            MultipleJoin<TSource, TSelect, TJoinSource, TJoinSelect> multipleJoin = (MultipleJoin<TSource, TSelect, TJoinSource, TJoinSelect>)base.Join(joinType, right, on, existentParameters);

            return new ModifiableMultipleJoin<TSource, TSource, TSelect, TJoinSource, TJoinSelect>(multipleJoin.DataContext, (BasicDataSource)multipleJoin.Left, multipleJoin.Right, multipleJoin.On, multipleJoin.JoinType);
        }

        /// <summary>
        /// 以当前对象为蓝本复制出一个新的可查询对象。
        /// </summary>
        /// <param name="startAliasIndex">复制后新数据源的起始表别名下标。</param>
        /// <returns>复制后新的可查询操作对象。</returns>
        internal override Queryable Copy(ref int startAliasIndex)
        {
            return new Modifiable<TSource, TSelect>(DataContext, (BasicDataSource)DataSource.Copy(ref startAliasIndex));
        }
    }
}

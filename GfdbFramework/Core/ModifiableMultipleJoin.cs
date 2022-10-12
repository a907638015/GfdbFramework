using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;
using GfdbFramework.DataSource;
using GfdbFramework.Enum;
using GfdbFramework.Field;
using GfdbFramework.Interface;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 可修改的多表关联操作类。
    /// </summary>
    /// <typeparam name="TFirstSource">关联对象中最左侧数据源中的原始成员类型。</typeparam>
    /// <typeparam name="TLeftSource">左数据源中的原始成员类型</typeparam>
    /// <typeparam name="TLeftSelect">左数据源中的每个成员类型</typeparam>
    /// <typeparam name="TJoinSource">右数据源中的原始成员类型</typeparam>
    /// <typeparam name="TJoinSelect">右数据源中的每个成员类型</typeparam>
    public class ModifiableMultipleJoin<TFirstSource, TLeftSource, TLeftSelect, TJoinSource, TJoinSelect> : MultipleJoin<TLeftSource, TLeftSelect, TJoinSource, TJoinSelect>
    {
        /// <summary>
        /// 使用指定的操作上下文、左侧关联对象、右侧关联对象、关联条件字段以及关联类型初始化一个新的 <see cref="MultipleJoin{TLeftSource, TLeftSelect, TJoinSource, TJoinSelect}"/> 类实例。
        /// </summary>
        /// <param name="dataContext">所使用的操作上下文对象。</param>
        /// <param name="left">需要关联的左侧对象。</param>
        /// <param name="right">需要关联右侧对象的数据源信息。</param>
        /// <param name="on">左右两侧关联的条件字段。</param>
        /// <param name="joinType">关联类型。</param>
        internal ModifiableMultipleJoin(IDataContext dataContext, BasicDataSource left, BasicDataSource right, BasicField on, DataSourceType joinType)
            : base(dataContext, left, right, on, joinType)
        {
        }

        /// <summary>
        /// 使用指定的操作上下文、左侧关联对象、右侧关联对象、关联条件字段以及关联类型初始化一个新的 <see cref="MultipleJoin{TLeftSource, TLeftSelect, TJoinSource, TJoinSelect}"/> 类实例。
        /// </summary>
        /// <param name="dataContext">所使用的操作上下文对象。</param>
        /// <param name="left">需要关联的左侧对象。</param>
        /// <param name="right">需要关联右侧对象的数据源信息。</param>
        /// <param name="on">左右两侧关联的条件字段。</param>
        /// <param name="joinType">关联类型。</param>
        internal ModifiableMultipleJoin(IDataContext dataContext, MultipleJoin left, BasicDataSource right, BasicField on, DataSourceType joinType)
            : base(dataContext, left, right, on, joinType)
        {
        }

        /// <summary>
        /// 以当前对象数据源做左连接操作。
        /// </summary>
        /// <typeparam name="TRightSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TRightSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联操作后新的操作对象。</returns>
        public new ModifiableMultipleJoin<TFirstSource, LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect> LeftJoin<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right, Expression<Func<LeftObjectInfo<TLeftSource, TJoinSource>, TRightSource, bool>> on)
        {
            return (ModifiableMultipleJoin<TFirstSource, LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect>)Join(right, on, DataSourceType.LeftJoin, null);
        }

        /// <summary>
        /// 以当前对象数据源做右连接操作。
        /// </summary>
        /// <typeparam name="TRightSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TRightSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联操作后新的操作对象。</returns>
        public new ModifiableMultipleJoin<TFirstSource, LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect> RightJoin<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right, Expression<Func<LeftObjectInfo<TLeftSource, TJoinSource>, TRightSource, bool>> on)
        {
            return (ModifiableMultipleJoin<TFirstSource, LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect>)Join(right, on, DataSourceType.RightJoin, null);
        }

        /// <summary>
        /// 以当前对象数据源做直连接操作。
        /// </summary>
        /// <typeparam name="TRightSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TRightSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联操作后新的操作对象。</returns>
        public new ModifiableMultipleJoin<TFirstSource, LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect> InnerJoin<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right, Expression<Func<LeftObjectInfo<TLeftSource, TJoinSource>, TRightSource, bool>> on)
        {
            return (ModifiableMultipleJoin<TFirstSource, LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect>)Join(right, on, DataSourceType.InnerJoin, null);
        }

        /// <summary>
        /// 以当前对象数据源做全连接操作。
        /// </summary>
        /// <typeparam name="TRightSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TRightSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联操作后新的操作对象。</returns>
        public new ModifiableMultipleJoin<TFirstSource, LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect> FullJoin<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right, Expression<Func<LeftObjectInfo<TLeftSource, TJoinSource>, TRightSource, bool>> on)
        {
            return (ModifiableMultipleJoin<TFirstSource, LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect>)Join(right, on, DataSourceType.FullJoin, null);
        }

        /// <summary>
        /// 以当前对象数据源做交叉连接操作。
        /// </summary>
        /// <typeparam name="TRightSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TRightSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <returns>关联操作后新的操作对象。</returns>
        public new ModifiableMultipleJoin<TFirstSource, LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect> CrossJoin<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right)
        {
            return (ModifiableMultipleJoin<TFirstSource, LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect>)Join(right, null, DataSourceType.CrossJoin, null);
        }

        /// <summary>
        /// 以当前对象数据源做连接操作。
        /// </summary>
        /// <typeparam name="TRightSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TRightSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <param name="joinType">关联类型。</param>
        /// <param name="existentParameters">调用该方法时已经传入的参数集合。</param>
        /// <returns>关联操作后新的操作对象。</returns>
        internal override MultipleJoin Join<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right, LambdaExpression on, DataSourceType joinType, Interface.IReadOnlyDictionary<string, ParameterInfo> existentParameters)
        {
            MultipleJoin<LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect> multipleJoin = (MultipleJoin<LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect>)base.Join(right, on, joinType, existentParameters);

            if (multipleJoin.Left is BasicDataSource dataSource)
                return new ModifiableMultipleJoin<TFirstSource, LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect>(multipleJoin.DataContext, dataSource, multipleJoin.Right, multipleJoin.On, multipleJoin.JoinType);
            else
                return new ModifiableMultipleJoin<TFirstSource, LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect>(multipleJoin.DataContext, (MultipleJoin)multipleJoin.Left, multipleJoin.Right, multipleJoin.On, multipleJoin.JoinType);
        }

        /// <summary>
        /// 删除该多表关联对象中最左侧数据源中所有关联到的数据行。
        /// </summary>
        /// <returns>删除成功的数据条数。</returns>
        public int Delete()
        {
            return Delete(null, null);
        }

        /// <summary>
        /// 删除该多表关联对象中最左侧数据源中的数据行。
        /// </summary>
        /// <param name="where">需要删除数据行的条件限定表达式树。</param>
        /// <returns>删除成功的数据条数。</returns>
        public int Delete(Expression<Func<LeftObjectInfo<TLeftSource, TJoinSource>, bool>> where)
        {
            return Delete(null, where);
        }

        /// <summary>
        /// 删除该多表关联对象中多个数据源中的数据行（部分数据库可能不支持一次性删除多个数据表的数据）。
        /// </summary>
        /// <param name="extraDeleteSources">除最左侧数据源额外还需要删除的数据源表达式树。</param>
        /// <param name="where">需要删除数据行的条件限定表达式树。</param>
        /// <returns>删除成功的数据条数。</returns>
        public int Delete(Expression<Func<LeftObjectInfo<TLeftSource, TJoinSource>, object>> extraDeleteSources, Expression<Func<LeftObjectInfo<TLeftSource, TJoinSource>, bool>> where)
        {
            int nextTableAliasIndex = Right.AliasIndex + 1;
            BasicField whereField = null;
            OriginalDataSource[] sources;

            if (extraDeleteSources != null)
            {
                Dictionary<string, ParameterInfo> extraParameters = new Dictionary<string, ParameterInfo>
                {
                    { extraDeleteSources.Parameters[0].Name, new ParameterInfo(this) }
                };

                Field.Field extraField = Helper.ExtractField(extraDeleteSources.Body, ExtractType.DataSource, (Realize.ReadOnlyDictionary<string, ParameterInfo>)extraParameters, ref nextTableAliasIndex);

                List<OriginalDataSource> extraDataSources = new List<OriginalDataSource>();

                ExtractDataSource(extraField, extraDataSources, "未能从额外需要删除数据源的表达式树中提取出有效的删除数据源");

                sources = new OriginalDataSource[extraDataSources.Count + 1];

                extraDataSources.CopyTo(sources, 1);

                extraDataSources[0] = GetFirstOriginalDataSource(this);
            }
            else
            {
                sources = new OriginalDataSource[1]
                {
                    GetFirstOriginalDataSource(this)
                };
            }

            if (where != null)
            {
                Dictionary<string, ParameterInfo> whereParameters = new Dictionary<string, ParameterInfo>
                {
                    { where.Parameters[0].Name, new ParameterInfo(this) }
                };

                whereField = (BasicField)Helper.ExtractField(where.Body, ExtractType.Join, (Realize.ReadOnlyDictionary<string, ParameterInfo>)whereParameters, ref nextTableAliasIndex);
            }

            string deleteSql = DataContext.SqlFactory.GenerateDeleteSql(DataContext, sources, ToDataSource(), whereField, out Interface.IReadOnlyList<DbParameter> parameters);

            return DataContext.DatabaseOperation.ExecuteNonQuery(deleteSql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 使用指定的条件约束更新该多表关联对象中最左侧数据表中的数据。
        /// </summary>
        /// <param name="updateEntity">更新后的实体表达式树。</param>
        /// <param name="where">更新时的条件约束表达式树。</param>
        /// <returns>删除成功的数据条数。</returns>
        public int Update(Expression<Func<LeftObjectInfo<TLeftSource, TJoinSource>, TFirstSource>> updateEntity, Expression<Func<LeftObjectInfo<TLeftSource, TJoinSource>, bool>> where)
        {
            int nextTableAliasIndex = Right.AliasIndex + 1;

            OriginalDataSource dataSource = GetFirstOriginalDataSource(this);

            Dictionary<string, ParameterInfo> updateParameters = new Dictionary<string, ParameterInfo>
            {
                { updateEntity.Parameters[0].Name, new ParameterInfo(true, this) }
            };

            ObjectField updateField = (ObjectField)Helper.ExtractField(updateEntity.Body, ExtractType.Default, (Realize.ReadOnlyDictionary<string, ParameterInfo>)updateParameters, ref nextTableAliasIndex);

            List<ModifyInfo> modifyFields = new List<ModifyInfo>();

            ObjectField rootField = (ObjectField)dataSource.RootField;

            if (updateField.Members != null && updateField.Members.Count > 0)
            {
                foreach (var item in updateField.Members)
                {
                    if (rootField.Members.TryGetValue(item.Key, out MemberInfo memberInfo))
                        modifyFields.Add(new ModifyInfo((OriginalField)memberInfo.Field, dataSource, (BasicField)item.Value.Field));
                    else
                        throw new Exception(string.Format("在对 {0} 类型所映射的数据表执行关联数据修改操作时成员 {1} 未能找到对应的数据表字段信息", typeof(TFirstSource).FullName, item.Key));
                }
            }

            if (modifyFields.Count < 1)
                throw new Exception(string.Format("在对 {0} 类型所映射的数据表执行关联数据修改操作时未能提取出任何需要修改的字段信息", typeof(TFirstSource).FullName));

            BasicField whereField = null;

            if (where != null)
                whereField = (BasicField)Helper.ExtractField(where.Body, ExtractType.Default, new Realize.ReadOnlyDictionary<string, ParameterInfo>(new KeyValuePair<string, ParameterInfo>(where.Parameters[0].Name, new ParameterInfo(this))), ref nextTableAliasIndex);

            string updateSql = DataContext.SqlFactory.GenerateUpdateSql(DataContext, (Realize.ReadOnlyList<ModifyInfo>)modifyFields, ToDataSource(), whereField, out Interface.IReadOnlyList<DbParameter> parameters);

            return DataContext.DatabaseOperation.ExecuteNonQuery(updateSql, System.Data.CommandType.Text, parameters);
        }

        /// <summary>
        /// 更新该多表关联对象中最左侧数据表中的数据。
        /// </summary>
        /// <param name="updateEntity">更新后的实体表达式树。</param>
        /// <returns>删除成功的数据条数。</returns>
        public int Update(Expression<Func<LeftObjectInfo<TLeftSource, TJoinSource>, TFirstSource>> updateEntity)
        {
            return Update(updateEntity, null);
        }

        /// <summary>
        /// 提取指定字段中所有的原始数据源信息。
        /// </summary>
        /// <param name="field">待提取原始数据源信息的字段。</param>
        /// <param name="dataSources">用于保存提取到数据源信息的集合。</param>
        /// <param name="exceptionMessage">提取失败时抛出异常的错误信息。</param>
        private static void ExtractDataSource(Field.Field field, List<OriginalDataSource> dataSources, string exceptionMessage)
        {
            if (field.Type == FieldType.Object)
            {
                ObjectField objectField = (ObjectField)field;

                if (objectField.Members != null && objectField.Members.Count > 0)
                {
                    foreach (var item in objectField.Members)
                    {
                        ExtractDataSource(item.Value.Field, dataSources, exceptionMessage);
                    }
                }
            }
            else if (field.Type == FieldType.Collection)
            {
                CollectionField collectionField = (CollectionField)field;

                foreach (var item in collectionField)
                {
                    ExtractDataSource(item, dataSources, exceptionMessage);
                }
            }
            else if (field.Type == FieldType.Constant)
            {
                object value = ((ConstantField)field).Value;

                if (value is OriginalDataSource originalDataSource)
                    dataSources.Add(originalDataSource);
                else
                    throw new Exception(exceptionMessage);
            }
            else
            {
                throw new Exception(exceptionMessage);
            }
        }

        /// <summary>
        /// 获取指定多表关联对象中最左侧第一个原始数据源信息。
        /// </summary>
        /// <param name="multipleJoin">待获取原始数据源信息的多表关联对象。</param>
        /// <returns>若最左侧第一个数据源是原始数据源信息则返回该数据源，否则返回 null。</returns>
        private static OriginalDataSource GetFirstOriginalDataSource(MultipleJoin multipleJoin)
        {
            if (multipleJoin.Left is BasicDataSource basicDataSource)
            {
                if (basicDataSource.Type == DataSourceType.Table || basicDataSource.Type == DataSourceType.View)
                    return (OriginalDataSource)basicDataSource;

                return null;
            }
            else
            {
                return GetFirstOriginalDataSource((MultipleJoin)multipleJoin.Left);
            }
        }
    }
}

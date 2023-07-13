using GfdbFramework.DataSource;
using GfdbFramework.Enum;
using GfdbFramework.Field;
using GfdbFramework.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 可修改的多表关联操作类。
    /// </summary>
    /// <typeparam name="TFirstSource">关联对象中最左侧数据源中的原始成员类型。</typeparam>
    /// <typeparam name="TLeftSelect">左数据源中的每个成员类型</typeparam>
    /// <typeparam name="TJoinSelect">右数据源中的每个成员类型</typeparam>
    public class ModifiableMultipleJoin<TFirstSource, TLeftSelect, TJoinSelect> : MultipleJoin<TLeftSelect, TJoinSelect>
    {
        /// <summary>
        /// 使用指定的操作上下文、左侧关联对象、右侧关联对象、关联条件字段以及关联类型初始化一个新的 <see cref="ModifiableMultipleJoin{TFirstSource, TLeftSelect, TJoinSelect}"/> 类实例。
        /// </summary>
        /// <param name="dataContext">所使用的操作上下文对象。</param>
        /// <param name="left">需要关联的左侧对象。</param>
        /// <param name="right">需要关联右侧对象的数据源信息。</param>
        /// <param name="on">左右两侧关联的条件字段。</param>
        /// <param name="joinType">关联类型。</param>
        internal ModifiableMultipleJoin(IDataContext dataContext, DataSource.DataSource left, BasicDataSource right, BasicField on, SourceType joinType)
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
        public new ModifiableMultipleJoin<TFirstSource, JoinItem<TLeftSelect, TJoinSelect>, TRightSelect> LeftJoin<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right, Expression<Func<JoinItem<TLeftSelect, TJoinSelect>, TRightSelect, bool>> on)
        {
            return (ModifiableMultipleJoin<TFirstSource, JoinItem<TLeftSelect, TJoinSelect>, TRightSelect>)Join(right, on, SourceType.LeftJoin, null);
        }

        /// <summary>
        /// 以当前对象数据源做右连接操作。
        /// </summary>
        /// <typeparam name="TRightSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TRightSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联操作后新的操作对象。</returns>
        public new ModifiableMultipleJoin<TFirstSource, JoinItem<TLeftSelect, TJoinSelect>, TRightSelect> RightJoin<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right, Expression<Func<JoinItem<TLeftSelect, TJoinSelect>, TRightSelect, bool>> on)
        {
            return (ModifiableMultipleJoin<TFirstSource, JoinItem<TLeftSelect, TJoinSelect>, TRightSelect>)Join(right, on, SourceType.RightJoin, null);
        }

        /// <summary>
        /// 以当前对象数据源做直连接操作。
        /// </summary>
        /// <typeparam name="TRightSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TRightSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联操作后新的操作对象。</returns>
        public new ModifiableMultipleJoin<TFirstSource, JoinItem<TLeftSelect, TJoinSelect>, TRightSelect> InnerJoin<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right, Expression<Func<JoinItem<TLeftSelect, TJoinSelect>, TRightSelect, bool>> on)
        {
            return (ModifiableMultipleJoin<TFirstSource, JoinItem<TLeftSelect, TJoinSelect>, TRightSelect>)Join(right, on, SourceType.InnerJoin, null);
        }

        /// <summary>
        /// 以当前对象数据源做全连接操作。
        /// </summary>
        /// <typeparam name="TRightSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TRightSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联操作后新的操作对象。</returns>
        public new ModifiableMultipleJoin<TFirstSource, JoinItem<TLeftSelect, TJoinSelect>, TRightSelect> FullJoin<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right, Expression<Func<JoinItem<TLeftSelect, TJoinSelect>, TRightSelect, bool>> on)
        {
            return (ModifiableMultipleJoin<TFirstSource, JoinItem<TLeftSelect, TJoinSelect>, TRightSelect>)Join(right, on, SourceType.FullJoin, null);
        }

        /// <summary>
        /// 以当前对象数据源做交叉连接操作。
        /// </summary>
        /// <typeparam name="TRightSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TRightSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <returns>关联操作后新的操作对象。</returns>
        public new ModifiableMultipleJoin<TFirstSource, JoinItem<TLeftSelect, TJoinSelect>, TRightSelect> CrossJoin<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right)
        {
            return (ModifiableMultipleJoin<TFirstSource, JoinItem<TLeftSelect, TJoinSelect>, TRightSelect>)Join(right, null, SourceType.CrossJoin, null);
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
        internal override MultipleJoin Join<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right, LambdaExpression on, SourceType joinType, ReadOnlyDictionary<string, DataSource.DataSource> existentParameters)
        {
            MultipleJoin<JoinItem<TLeftSelect, TJoinSelect>, TRightSelect> multipleJoin = (MultipleJoin<JoinItem<TLeftSelect, TJoinSelect>, TRightSelect>)base.Join(right, on, joinType, existentParameters);

            return new ModifiableMultipleJoin<TFirstSource, JoinItem<TLeftSelect, TJoinSelect>, TRightSelect>(multipleJoin.DataContext, multipleJoin.Left, multipleJoin.Right, multipleJoin.On, multipleJoin.JoinType);
        }

        /// <summary>
        /// 删除该多表关联对象中最左侧数据源中所有关联到的数据行。
        /// </summary>
        /// <returns>删除成功的数据条数。</returns>
        public int Delete()
        {
            return Delete(null);
        }

        /// <summary>
        /// 删除该多表关联对象中最左侧数据源中的数据行。
        /// </summary>
        /// <param name="where">需要删除数据行的条件限定表达式树。</param>
        /// <returns>删除成功的数据条数。</returns>
        public int Delete(Expression<Func<JoinItem<TLeftSelect, TJoinSelect>, bool>> where)
        {
            int nextTableAliasIndex = Right.AliasIndex + 1;
            BasicField whereField = null;
            TableDataSource[] sources;
            var selfDataSource = ToDataSource();

            sources = new TableDataSource[1]
            {
                GetFirstOriginalDataSource(selfDataSource)
            };

            if (where != null)
            {
                Dictionary<string, DataSource.DataSource> whereParameters = new Dictionary<string, DataSource.DataSource>
                {
                    { where.Parameters[0].Name, selfDataSource }
                };

                whereField = (BasicField)Helper.ExtractField(DataContext, where.Body, ExtractWay.Other, whereParameters, ref nextTableAliasIndex);
            }

            IParameterContext parameterContext = DataContext.CreateParameterContext(true);

            string deleteSql = DataContext.SqlFactory.GenerateDeleteSql(parameterContext, sources, selfDataSource, whereField);

            return DataContext.DatabaseOperation.ExecuteNonQuery(deleteSql, System.Data.CommandType.Text, parameterContext.ToList());
        }

        /// <summary>
        /// 使用指定的条件约束更新该多表关联对象中最左侧数据表中的数据。
        /// </summary>
        /// <param name="updateEntity">更新后的实体表达式树。</param>
        /// <param name="where">更新时的条件约束表达式树。</param>
        /// <returns>删除成功的数据条数。</returns>
        public int Update(Expression<Func<JoinItem<TLeftSelect, TJoinSelect>, TFirstSource>> updateEntity, Expression<Func<JoinItem<TLeftSelect, TJoinSelect>, bool>> where)
        {
            int nextTableAliasIndex = Right.AliasIndex + 1;
            var selfDataSource = ToDataSource();

            TableDataSource dataSource = GetFirstOriginalDataSource(selfDataSource);

            Dictionary<string, DataSource.DataSource> updateParameters = new Dictionary<string, DataSource.DataSource>
            {
                { updateEntity.Parameters[0].Name, selfDataSource }
            };

            ObjectField updateField = (ObjectField)Helper.ExtractField(DataContext, updateEntity.Body, ExtractWay.Other, updateParameters, ref nextTableAliasIndex);

            List<UpdateItem> modifyFields = new List<UpdateItem>();

            ObjectField rootField = (ObjectField)dataSource.RootField;

            if (updateField.Members != null && updateField.Members.Count > 0)
            {
                foreach (var item in updateField.Members)
                {
                    if (item.Value.Field is BasicField valueField)
                    {
                        if (rootField.Members.TryGetValue(item.Key, out MemberInfo memberInfo) && memberInfo.Field.Type == FieldType.Original)
                            modifyFields.Add(new UpdateItem((OriginalField)memberInfo.Field, valueField));
                        else
                            throw new Exception($"在对 {typeof(TFirstSource).FullName} 类型所映射的数据表执行数据修改操作时成员 {item.Key} 未能找到对应的数据表字段信息");
                    }
                    else
                    {
                        throw new Exception($"在对 {typeof(TFirstSource).FullName} 类型所映射的数据表执行数据修改操作时成员 {item.Key} 的值不是基础数据类型");
                    }
                }
            }

            if (modifyFields.Count < 1)
                throw new Exception($"在对 {typeof(TFirstSource).FullName} 类型所映射的数据表执行数据修改操作时未能提取出任何需要修改的字段信息");

            BasicField whereField = null;

            if (where != null)
                whereField = (BasicField)Helper.ExtractField(DataContext, where.Body, ExtractWay.Other, new ReadOnlyDictionary<string, DataSource.DataSource>(new Dictionary<string, DataSource.DataSource>() { { where.Parameters[0].Name, selfDataSource } }), ref nextTableAliasIndex);

            IParameterContext parameterContext = DataContext.CreateParameterContext(true);

            string updateSql = DataContext.SqlFactory.GenerateUpdateSql(parameterContext, new ReadOnlyList<UpdateGroup>(new UpdateGroup(dataSource, modifyFields)), selfDataSource, whereField);

            return DataContext.DatabaseOperation.ExecuteNonQuery(updateSql, System.Data.CommandType.Text, parameterContext.ToList());
        }

        /// <summary>
        /// 更新该多表关联对象中最左侧数据表中的数据。
        /// </summary>
        /// <param name="updateEntity">更新后的实体表达式树。</param>
        /// <returns>删除成功的数据条数。</returns>
        public int Update(Expression<Func<JoinItem<TLeftSelect, TJoinSelect>, TFirstSource>> updateEntity)
        {
            return Update(updateEntity, null);
        }

        /// <summary>
        /// 获取指定多表关联数据源中最左侧第一个原始数据源信息。
        /// </summary>
        /// <param name="dataSource">待获取原始数据源信息的多表关联数据源。</param>
        /// <returns>若最左侧第一个数据源是原始数据源信息则返回该数据源，否则返回 null。</returns>
        private static TableDataSource GetFirstOriginalDataSource(JoinDataSource dataSource)
        {
            if (dataSource.Left is BasicDataSource)
            {
                if (dataSource.Left.Type == SourceType.Table)
                    return (TableDataSource)dataSource.Left;

                return null;
            }
            else
            {
                return GetFirstOriginalDataSource((JoinDataSource)dataSource.Left);
            }
        }
    }
}

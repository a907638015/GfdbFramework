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
    /// 多表关联操作类。
    /// </summary>
    public abstract class MultipleJoin
    {
        /// <summary>
        /// 使用指定的操作上下文、左侧关联对象、右侧关联对象、关联条件字段以及关联类型初始化一个新的 <see cref="MultipleJoin"/> 类实例。
        /// </summary>
        /// <param name="dataContext">所使用的操作上下文对象。</param>
        /// <param name="left">需要关联的左侧对象。</param>
        /// <param name="right">需要关联右侧对象的数据源信息。</param>
        /// <param name="on">左右两侧关联的条件字段。</param>
        /// <param name="joinType">关联类型。</param>
        internal MultipleJoin(IDataContext dataContext, DataSource.DataSource left, BasicDataSource right, BasicField on, SourceType joinType)
        {
            DataContext = dataContext;
            Left = left;
            Right = right;
            On = on;
            JoinType = joinType;
        }

        /// <summary>
        /// 获取多表关联查询时的左侧关联对象。
        /// </summary>
        internal DataSource.DataSource Left { get; }

        /// <summary>
        /// 获取多表关联查询时的右侧关联对象。
        /// </summary>
        internal BasicDataSource Right { get; }

        /// <summary>
        /// 获取左右两侧对象的关联条件字段。
        /// </summary>
        internal BasicField On { get; }

        /// <summary>
        /// 获取左右两侧对象的关联类型。
        /// </summary>
        internal SourceType JoinType { get; }

        /// <summary>
        /// 获取当前对象所使用的操作上下文。
        /// </summary>
        internal IDataContext DataContext { get; }

        /// <summary>
        /// 以当前对象为蓝本复制一个新的多表关联对象。。
        /// </summary>
        /// <param name="copiedDataSources">已经复制过的数据源集合。</param>
        /// <param name="copiedFields">已经复制好的字段信息。</param>
        /// <param name="startAliasIndex">第一个关联对象的起始别名下标。</param>
        internal abstract MultipleJoin Copy(Dictionary<DataSource.DataSource, DataSource.DataSource> copiedDataSources, Dictionary<Field.Field, Field.Field> copiedFields, ref int startAliasIndex);

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
        internal abstract MultipleJoin Join<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right, LambdaExpression on, SourceType joinType, ReadOnlyDictionary<string, DataSource.DataSource> existentParameters);

        /// <summary>
        /// 对当前对象中的关联数据源进行查询。
        /// </summary>
        /// <typeparam name="TResult">查询后返回新对象中的数据源每个成员类型。</typeparam>
        /// <param name="selector">对数据源进行查询操作的表达式树。</param>
        /// <param name="where">查询操作的条件限定表达式树。</param>
        /// <param name="existentParameters">调用该方法时已经传入的参数集合。</param>
        /// <returns>查询后新的操作对象。</returns>
        internal abstract Queryable<TResult, TResult> Select<TResult>(LambdaExpression selector, LambdaExpression where, ReadOnlyDictionary<string, DataSource.DataSource> existentParameters);

        /// <summary>
        /// 将当前多数据源关联查询对象转换成数据源。
        /// </summary>
        /// <returns>转换后的等价数据源信息。</returns>
        internal abstract JoinDataSource ToDataSource();
    }

    /// <summary>
    /// 多表关联操作类。
    /// </summary>
    /// <typeparam name="TLeftSource">左数据源中的原始成员类型</typeparam>
    /// <typeparam name="TLeftSelect">左数据源中的每个成员类型</typeparam>
    /// <typeparam name="TJoinSource">右数据源中的原始成员类型</typeparam>
    /// <typeparam name="TJoinSelect">右数据源中的每个成员类型</typeparam>
    public class MultipleJoin<TLeftSource, TLeftSelect, TJoinSource, TJoinSelect> : MultipleJoin
    {
        /// <summary>
        /// 使用指定的操作上下文、左侧关联对象、右侧关联对象、关联条件字段以及关联类型初始化一个新的 <see cref="MultipleJoin{TLeftSource, TLeftSelect, TJoinSource, TJoinSelect}"/> 类实例。
        /// </summary>
        /// <param name="dataContext">所使用的操作上下文对象。</param>
        /// <param name="left">需要关联的左侧对象。</param>
        /// <param name="right">需要关联右侧对象的数据源信息。</param>
        /// <param name="on">左右两侧关联的条件字段。</param>
        /// <param name="joinType">关联类型。</param>
        internal MultipleJoin(IDataContext dataContext, DataSource.DataSource left, BasicDataSource right, BasicField on, SourceType joinType)
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
        public MultipleJoin<JoinItem<TLeftSource, TJoinSource>, JoinItem<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect> LeftJoin<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right, Expression<Func<JoinItem<TLeftSource, TJoinSource>, TRightSource, bool>> on)
        {
            return (MultipleJoin<JoinItem<TLeftSource, TJoinSource>, JoinItem<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect>)Join(right, on, SourceType.LeftJoin, null);
        }

        /// <summary>
        /// 以当前对象数据源做右连接操作。
        /// </summary>
        /// <typeparam name="TRightSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TRightSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联操作后新的操作对象。</returns>
        public MultipleJoin<JoinItem<TLeftSource, TJoinSource>, JoinItem<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect> RightJoin<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right, Expression<Func<JoinItem<TLeftSource, TJoinSource>, TRightSource, bool>> on)
        {
            return (MultipleJoin<JoinItem<TLeftSource, TJoinSource>, JoinItem<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect>)Join(right, on, SourceType.RightJoin, null);
        }

        /// <summary>
        /// 以当前对象数据源做直连接操作。
        /// </summary>
        /// <typeparam name="TRightSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TRightSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联操作后新的操作对象。</returns>
        public MultipleJoin<JoinItem<TLeftSource, TJoinSource>, JoinItem<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect> InnerJoin<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right, Expression<Func<JoinItem<TLeftSource, TJoinSource>, TRightSource, bool>> on)
        {
            return (MultipleJoin<JoinItem<TLeftSource, TJoinSource>, JoinItem<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect>)Join(right, on, SourceType.InnerJoin, null);
        }

        /// <summary>
        /// 以当前对象数据源做全连接操作。
        /// </summary>
        /// <typeparam name="TRightSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TRightSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联操作后新的操作对象。</returns>
        public MultipleJoin<JoinItem<TLeftSource, TJoinSource>, JoinItem<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect> FullJoin<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right, Expression<Func<JoinItem<TLeftSource, TJoinSource>, TRightSource, bool>> on)
        {
            return (MultipleJoin<JoinItem<TLeftSource, TJoinSource>, JoinItem<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect>)Join(right, on, SourceType.FullJoin, null);
        }

        /// <summary>
        /// 以当前对象数据源做交叉连接操作。
        /// </summary>
        /// <typeparam name="TRightSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TRightSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <returns>关联操作后新的操作对象。</returns>
        public MultipleJoin<JoinItem<TLeftSource, TJoinSource>, JoinItem<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect> CrossJoin<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right)
        {
            return (MultipleJoin<JoinItem<TLeftSource, TJoinSource>, JoinItem<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect>)Join(right, null, SourceType.CrossJoin, null);
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
            BasicField onField = null;

            int nextTableAliasIndex = Right.AliasIndex + 1;

            BasicDataSource rightDataSource = (BasicDataSource)right.DataSource.Copy(new Dictionary<DataSource.DataSource, DataSource.DataSource>(), new Dictionary<Field.Field, Field.Field>(), ref nextTableAliasIndex);

            if (on != null)
            {
                Dictionary<string, DataSource.DataSource> parameters = new Dictionary<string, DataSource.DataSource>();

                if (existentParameters != null)
                {
                    foreach (var item in existentParameters)
                    {
                        parameters.Add(item.Key, item.Value);
                    }
                }

                parameters[on.Parameters[0].Name] = ToDataSource();
                parameters[on.Parameters[1].Name] = rightDataSource;

                onField = (BasicField)Helper.ExtractField(DataContext, on.Body, ExtractWay.Other, parameters, ref nextTableAliasIndex);
            }

            return new MultipleJoin<JoinItem<TLeftSource, TJoinSource>, JoinItem<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect>(DataContext, ToDataSource(), rightDataSource, onField, joinType);
        }

        /// <summary>
        /// 对当前对象中的关联数据源进行查询。
        /// </summary>
        /// <typeparam name="TResult">查询后返回新对象中的数据源每个成员类型。</typeparam>
        /// <param name="selector">对数据源进行查询操作的表达式树。</param>
        /// <returns>查询后新的操作对象。</returns>
        public Queryable<TResult, TResult> Select<TResult>(Expression<Func<JoinItem<TLeftSelect, TJoinSelect>, TResult>> selector)
        {
            return Select<TResult>(selector, null);
        }

        /// <summary>
        /// 对当前对象中的关联数据源进行查询。
        /// </summary>
        /// <typeparam name="TResult">查询后返回新对象中的数据源每个成员类型。</typeparam>
        /// <param name="where">查询操作的条件限定表达式树。</param>
        /// <param name="selector">对数据源进行查询操作的表达式树。</param>
        /// <returns>查询后新的操作对象。</returns>
        public Queryable<TResult, TResult> Select<TResult>(Expression<Func<JoinItem<TLeftSelect, TJoinSelect>, TResult>> selector, Expression<Func<JoinItem<TLeftSource, TLeftSource>, bool>> where)
        {
            return Select<TResult>(selector, where, null);
        }

        /// <summary>
        /// 对当前对象中的关联数据源进行查询。
        /// </summary>
        /// <typeparam name="TResult">查询后返回新对象中的数据源每个成员类型。</typeparam>
        /// <param name="selector">对数据源进行查询操作的表达式树。</param>
        /// <param name="where">查询操作的条件限定表达式树。</param>
        /// <param name="existentParameters">调用该方法时已经传入的参数集合。</param>
        /// <returns>查询后新的操作对象。</returns>
        internal override Queryable<TResult, TResult> Select<TResult>(LambdaExpression selector, LambdaExpression where, ReadOnlyDictionary<string, DataSource.DataSource> existentParameters)
        {
            int nextTableAliasIndex = Right.AliasIndex + 1;

            Dictionary<string, DataSource.DataSource> selectorParameters = new Dictionary<string, DataSource.DataSource>();
            Dictionary<string, DataSource.DataSource> whereParameters = where == null ? null : new Dictionary<string, DataSource.DataSource>();
            JoinDataSource selfParameter = ToDataSource();

            if (existentParameters != null)
            {
                foreach (var item in existentParameters)
                {
                    selectorParameters.Add(item.Key, item.Value);
                    whereParameters?.Add(item.Key, item.Value);
                }
            }

            selectorParameters[selector.Parameters[0].Name] = selfParameter;

            if (whereParameters != null)
                whereParameters[where.Parameters[0].Name] = selfParameter;

            Field.Field whereField = where == null ? null : Helper.ExtractField(DataContext, where.Body, ExtractWay.Other, whereParameters, ref nextTableAliasIndex);

            if (whereField != null && (!(whereField is BasicField basicField) || !basicField.IsBoolDataType))
                throw new Exception($"在对多表关联对象进行 Where 条件筛选时未能正确提取到限定字段信息，具体表达式为：{where}");

            int fieldAliasIndex = 0;

            Field.Field selectField = Helper.ExtractField(DataContext, selector.Body, ExtractWay.SelectNew, selectorParameters, ref nextTableAliasIndex).ResetAlias(ref fieldAliasIndex);

            BasicDataSource dataSource = new SelectDataSource(DataContext, selectField, selectField, ToDataSource(), nextTableAliasIndex).AddWhere((BasicField)whereField); ;

            return new Queryable<TResult, TResult>(DataContext, dataSource);
        }

        /// <summary>
        /// 将当前多数据源关联查询对象转换成数据源。
        /// </summary>
        /// <returns>转换后的等价数据源信息。</returns>
        internal override JoinDataSource ToDataSource()
        {
            return new JoinDataSource(DataContext, JoinType, Left, Right, On);
        }

        /// <summary>
        /// 以当前对象为蓝本复制一个新的多表关联对象。。
        /// </summary>
        /// <param name="copiedDataSources">已经复制过的数据源集合。</param>
        /// <param name="copiedFields">已经复制好的字段信息。</param>
        /// <param name="startAliasIndex">第一个关联对象的起始别名下标。</param>
        internal override MultipleJoin Copy(Dictionary<DataSource.DataSource, DataSource.DataSource> copiedDataSources, Dictionary<Field.Field, Field.Field> copiedFields, ref int startAliasIndex)
        {
            DataSource.DataSource left;
            DataSource.DataSource right;

            if (!copiedDataSources.TryGetValue(Left, out left))
                left = Left.Copy(copiedDataSources, copiedFields, ref startAliasIndex);

            if (!copiedDataSources.TryGetValue(Right, out right))
                right = Right.Copy(copiedDataSources, copiedFields, ref startAliasIndex);

            BasicField on = (BasicField)On?.Copy(copiedDataSources, copiedFields, ref startAliasIndex);

            return new MultipleJoin<TLeftSource, TLeftSelect, TJoinSource, TJoinSelect>(DataContext, left, (BasicDataSource)right, on, JoinType);
        }
    }
}

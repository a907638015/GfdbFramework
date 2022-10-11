using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using GfdbFramework.DataSource;
using GfdbFramework.Enum;
using GfdbFramework.Field;
using GfdbFramework.Interface;

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
        /// <param name="right">需要关联左侧对象的数据源信息。</param>
        /// <param name="right">需要关联右侧对象的数据源信息。</param>
        /// <param name="on">左右两侧关联的条件字段。</param>
        /// <param name="joinType">关联类型。</param>
        internal MultipleJoin(IDataContext dataContext, BasicDataSource left, BasicDataSource right, BasicField on, DataSourceType joinType)
        {
            DataContext = dataContext;
            Left = left;
            Right = right;
            On = on;
            JoinType = joinType;
        }

        /// <summary>
        /// 使用指定的操作上下文、左侧关联对象、右侧关联对象、关联条件字段以及关联类型初始化一个新的 <see cref="MultipleJoin"/> 类实例。
        /// </summary>
        /// <param name="dataContext">所使用的操作上下文对象。</param>
        /// <param name="left">需要关联的左侧对象。</param>
        /// <param name="right">需要关联右侧对象的数据源信息。</param>
        /// <param name="on">左右两侧关联的条件字段。</param>
        /// <param name="joinType">关联类型。</param>
        internal MultipleJoin(IDataContext dataContext, MultipleJoin left, BasicDataSource right, BasicField on, DataSourceType joinType)
        {
            DataContext = dataContext;
            Left = left;
            Right = right;
            On = on;
            JoinType = joinType;
        }

        /// <summary>
        /// 获取多表关联查询时的左侧关联对象（只可能是 <see cref="BasicDataSource"/> 或 <see cref="MultipleJoin{TLeftSource, TLeftSelect, TJoinSource, TJoinSelect}"/> 中的一种）。
        /// </summary>
        internal object Left { get; }

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
        internal DataSourceType JoinType { get; }

        /// <summary>
        /// 获取当前对象所使用的操作上下文。
        /// </summary>
        internal IDataContext DataContext { get; }

        /// <summary>
        /// 以当前对象为蓝本复制一个新的多表关联对象。。
        /// </summary>
        /// <param name="startAliasIndex">第一个关联对象的起始别名下标。</param>
        internal abstract MultipleJoin Copy(ref int startAliasIndex);

        /// <summary>
        /// 以当前对象数据源做连接操作。
        /// </summary>
        /// <typeparam name="TRightSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TRightSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <param name="existentParameters">调用该方法时已经传入的参数集合。</param>
        /// <returns>关联操作后新的操作对象。</returns>
        internal abstract MultipleJoin Join<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right, LambdaExpression on, DataSourceType joinType, Interface.IReadOnlyDictionary<string, ParameterInfo> existentParameters);

        /// <summary>
        /// 对当前对象中的关联数据源进行查询。
        /// </summary>
        /// <typeparam name="TResult">查询后返回新对象中的数据源每个成员类型。</typeparam>
        /// <param name="selector">对数据源进行查询操作的表达式树。</param>
        /// <param name="existentParameters">调用该方法时已经传入的参数集合。</param>
        /// <returns>查询后新的操作对象。</returns>
        internal abstract Queryable<TResult, TResult> Select<TResult>(LambdaExpression selector, Interface.IReadOnlyDictionary<string, ParameterInfo> existentParameters);

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
        /// <param name="right">需要关联左侧对象的数据源信息。</param>
        /// <param name="right">需要关联右侧对象的数据源信息。</param>
        /// <param name="on">左右两侧关联的条件字段。</param>
        /// <param name="joinType">关联类型。</param>
        internal MultipleJoin(IDataContext dataContext, BasicDataSource left, BasicDataSource right, BasicField on, DataSourceType joinType)
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
        internal MultipleJoin(IDataContext dataContext, MultipleJoin left, BasicDataSource right, BasicField on, DataSourceType joinType)
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
        public MultipleJoin<LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect> LeftJoin<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right, Expression<Func<LeftObjectInfo<TLeftSource, TJoinSource>, TRightSource, bool>> on)
        {
            return (MultipleJoin<LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect>)Join(right, on, DataSourceType.LeftJoin, null);
        }

        /// <summary>
        /// 以当前对象数据源做右连接操作。
        /// </summary>
        /// <typeparam name="TRightSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TRightSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联操作后新的操作对象。</returns>
        public MultipleJoin<LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect> RightJoin<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right, Expression<Func<LeftObjectInfo<TLeftSource, TJoinSource>, TRightSource, bool>> on)
        {
            return (MultipleJoin<LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect>)Join(right, on, DataSourceType.RightJoin, null);
        }

        /// <summary>
        /// 以当前对象数据源做直连接操作。
        /// </summary>
        /// <typeparam name="TRightSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TRightSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联操作后新的操作对象。</returns>
        public MultipleJoin<LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect> InnerJoin<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right, Expression<Func<LeftObjectInfo<TLeftSource, TJoinSource>, TRightSource, bool>> on)
        {
            return (MultipleJoin<LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect>)Join(right, on, DataSourceType.InnerJoin, null);
        }

        /// <summary>
        /// 以当前对象数据源做全连接操作。
        /// </summary>
        /// <typeparam name="TRightSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TRightSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联操作后新的操作对象。</returns>
        public MultipleJoin<LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect> FullJoin<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right, Expression<Func<LeftObjectInfo<TLeftSource, TJoinSource>, TRightSource, bool>> on)
        {
            return (MultipleJoin<LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect>)Join(right, on, DataSourceType.FullJoin, null);
        }

        /// <summary>
        /// 以当前对象数据源做交叉连接操作。
        /// </summary>
        /// <typeparam name="TRightSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TRightSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <returns>关联操作后新的操作对象。</returns>
        public MultipleJoin<LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect> CrossJoin<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right)
        {
            return (MultipleJoin<LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect>)Join(right, null, DataSourceType.CrossJoin, null);
        }

        /// <summary>
        /// 以当前对象数据源做连接操作。
        /// </summary>
        /// <typeparam name="TRightSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TRightSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联操作后新的操作对象。</returns>
        internal override MultipleJoin Join<TRightSource, TRightSelect>(Queryable<TRightSource, TRightSelect> right, LambdaExpression on, DataSourceType joinType, Interface.IReadOnlyDictionary<string, ParameterInfo> existentParameters)
        {
            BasicField onField = null;

            int nextTableAliasIndex = Right.AliasIndex + 1;

            BasicDataSource rightDataSource = (BasicDataSource)right.DataSource.Copy(ref nextTableAliasIndex);

            if (on != null)
            {
                Dictionary<string, ParameterInfo> parameters = new Dictionary<string, ParameterInfo>();

                if (existentParameters != null)
                {
                    foreach (var item in existentParameters)
                    {
                        parameters.Add(item.Key, item.Value.Copy());
                    }
                }

                parameters[on.Parameters[0].Name] = new ParameterInfo(this);
                parameters[on.Parameters[1].Name] = new ParameterInfo(rightDataSource);

                onField = (BasicField)Helper.ExtractField(on.Body, ExtractType.Join, (Realize.ReadOnlyDictionary<string, ParameterInfo>)parameters, ref nextTableAliasIndex);
            }

            return new MultipleJoin<LeftObjectInfo<TLeftSource, TJoinSource>, LeftObjectInfo<TLeftSelect, TJoinSelect>, TRightSource, TRightSelect>(DataContext, this, rightDataSource, onField, joinType);
        }

        /// <summary>
        /// 对当前对象中的关联数据源进行查询。
        /// </summary>
        /// <typeparam name="TResult">查询后返回新对象中的数据源每个成员类型。</typeparam>
        /// <param name="selector">对数据源进行查询操作的表达式树。</param>
        /// <returns>查询后新的操作对象。</returns>
        public Queryable<TResult, TResult> Select<TResult>(Expression<Func<LeftObjectInfo<TLeftSelect, TJoinSelect>, TResult>> selector)
        {
            return Select<TResult>(selector, null);
        }

        /// <summary>
        /// 对当前对象中的关联数据源进行查询。
        /// </summary>
        /// <typeparam name="TResult">查询后返回新对象中的数据源每个成员类型。</typeparam>
        /// <param name="selector">对数据源进行查询操作的表达式树。</param>
        /// <param name="existentParameters">调用该方法时已经传入的参数集合。</param>
        /// <returns>查询后新的操作对象。</returns>
        internal override Queryable<TResult, TResult> Select<TResult>(LambdaExpression selector, Interface.IReadOnlyDictionary<string, ParameterInfo> existentParameters)
        {
            int nextTableAliasIndex = Right.AliasIndex + 1;
            int nextFieldAliasIndex = 0;

            Dictionary<string, ParameterInfo> parameters = new Dictionary<string, ParameterInfo>();

            if (existentParameters != null)
            {
                foreach (var item in existentParameters)
                {
                    parameters.Add(item.Key, item.Value.Copy());
                }
            }

            parameters[selector.Parameters[0].Name] = new ParameterInfo(this);

            Field.Field field = Helper.ExtractField(selector.Body, ExtractType.Select, (Realize.ReadOnlyDictionary<string, ParameterInfo>)parameters, ref nextTableAliasIndex);

            Helper.ResetFieldAlias(DataContext, new HashSet<Field.Field>(), field, ref nextFieldAliasIndex);

            BasicDataSource dataSource = new ResultDataSource(DataContext, DataSourceType.QueryResult, field, nextTableAliasIndex, ToDataSource());

            return new Queryable<TResult, TResult>(DataContext, dataSource);
        }

        /// <summary>
        /// 以当前对象为蓝本复制一个新的多表关联对象。。
        /// </summary>
        /// <param name="startAliasIndex">第一个关联对象的起始别名下标。</param>
        internal override MultipleJoin Copy(ref int startAliasIndex)
        {
            return Copy(new Dictionary<DataSource.DataSource, DataSource.DataSource>(), ref startAliasIndex);
        }

        /// <summary>
        /// 以当前对象为蓝本复制一个新的多表关联对象。。
        /// </summary>
        /// <param name="copiedDataSources">已经复制过的数据源对象。</param>
        /// <param name="startAliasIndex">第一个关联对象的起始别名下标。</param>
        private MultipleJoin Copy(Dictionary<DataSource.DataSource, DataSource.DataSource> copiedDataSources, ref int startAliasIndex)
        {
            if (Left is BasicDataSource dataSource)
            {
                BasicDataSource left = (BasicDataSource)dataSource.Copy(ref startAliasIndex);
                BasicDataSource right = (BasicDataSource)Right.Copy(ref startAliasIndex);

                copiedDataSources[dataSource] = left;
                copiedDataSources[Right] = right;

                BasicField on = (BasicField)On?.Copy(DataContext, true, copiedDataSources, new Dictionary<Field.Field, Field.Field>(), ref startAliasIndex);

                return new MultipleJoin<TLeftSource, TLeftSelect, TJoinSource, TJoinSelect>(DataContext, left, right, on, JoinType);
            }
            else
            {
                MultipleJoin left = ((MultipleJoin)Left).Copy(ref startAliasIndex);
                BasicDataSource right = (BasicDataSource)Right.Copy(ref startAliasIndex);

                copiedDataSources[Right] = right;

                BasicField on = (BasicField)On?.Copy(DataContext, true, copiedDataSources, new Dictionary<Field.Field, Field.Field>(), ref startAliasIndex);

                return new MultipleJoin<TLeftSource, TLeftSelect, TJoinSource, TJoinSelect>(DataContext, left, right, on, JoinType);
            }
        }

        /// <summary>
        /// 将当前多数据源关联查询对象转换成数据源。
        /// </summary>
        /// <returns>转换后的等价数据源信息。</returns>
        internal override JoinDataSource ToDataSource()
        {
            if (Left is BasicDataSource basicDataSource)
                return new JoinDataSource(DataContext, JoinType, basicDataSource, Right, On);
            else
                return new JoinDataSource(DataContext, JoinType, ((MultipleJoin)Left).ToDataSource(), Right, On);
        }
    }
}

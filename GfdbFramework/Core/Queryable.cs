﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using GfdbFramework.DataSource;
using GfdbFramework.Enum;
using GfdbFramework.Field;
using GfdbFramework.Interface;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 对数据源提供一个可查询的操作对象。
    /// </summary>
    public abstract class Queryable
    {
        /// <summary>
        /// 使用指定的数据操作上下文以及数据源初始化一个新的 <see cref="Queryable"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该查询对象所使用的数据操作上下文。</param>
        /// <param name="dataSource">该对象所使用的数据源。</param>
        internal Queryable(IDataContext dataContext, BasicDataSource dataSource)
        {
            DataContext = dataContext;
            DataSource = dataSource;
        }

        /// <summary>
        /// 获取当前可查询对象的数据源。
        /// </summary>
        internal BasicDataSource DataSource { get; }

        /// <summary>
        /// 获取当前可查询对象所使用的数据操作上下文。
        /// </summary>
        internal IDataContext DataContext { get; }

        /// <summary>
        /// 对当前对象中的数据源进行查询。
        /// </summary>
        /// <typeparam name="TResult">查询后返回新对象中的数据源每个成员类型。</typeparam>
        /// <param name="selector">对数据源进行查询操作的表达式树。</param>
        /// <param name="existentParameters">调用该方法时已经传入的参数集合。</param>
        /// <returns>查询后新的操作对象。</returns>
        internal abstract Queryable Select<TResult>(LambdaExpression selector, Interface.IReadOnlyDictionary<string, ParameterInfo> existentParameters);

        /// <summary>
        /// 对当前对象中的数据源进行条件筛选。
        /// </summary>
        /// <param name="where">对数据源进行筛选的表达式树。</param>
        /// <param name="existentParameters">调用该方法时已经传入的参数集合。</param>
        /// <returns>筛选后新的操作对象。</returns>
        internal abstract Queryable Where(LambdaExpression where, Interface.IReadOnlyDictionary<string, ParameterInfo> existentParameters);

        /// <summary>
        /// 对当前对象中的数据源进行排序操作。
        /// </summary>
        /// <param name="sortType">排序方式。</param>
        /// <param name="sorting">对数据源进行排序的表达式树。</param>
        /// <param name="existentParameters">调用该方法时已经传入的参数集合。</param>
        /// <returns>排序后新的操作对象。</returns>
        internal abstract Queryable Sorting(SortType sortType, LambdaExpression sorting, Interface.IReadOnlyDictionary<string, ParameterInfo> existentParameters);

        /// <summary>
        /// 对当前对象中的数据源进行分组操作。
        /// </summary>
        /// <param name="grouping">对数据源进行分组的表达式树。</param>
        /// <param name="existentParameters">调用该方法时已经传入的参数集合。</param>
        /// <returns>分组后新的操作对象。</returns>
        internal abstract Queryable Group(LambdaExpression grouping, Interface.IReadOnlyDictionary<string, ParameterInfo> existentParameters);

        /// <summary>
        /// 对当前对象中的数据源查询返回结果进行限定。
        /// </summary>
        /// <param name="limit">需要限定返回的数据行信息。</param>
        /// <returns>对数据源查询返回结果进行限定后的一个新操作对象。</returns>
        internal abstract Queryable Limit(Limit limit);

        /// <summary>
        /// 以当前对象为蓝本复制出一个新的可查询对象。
        /// </summary>
        /// <param name="startAliasIndex">复制后新数据源的起始表别名下标。</param>
        /// <returns>复制后新的可查询操作对象。</returns>
        internal abstract Queryable Copy(ref int startAliasIndex);

        /// <summary>
        /// 以当前对象数据源做关联查询。
        /// </summary>
        /// <typeparam name="TResult">查询后返回新对象中的数据源每个成员类型。</typeparam>
        /// <param name="joinType">关联类型。</param>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="selector">对关联数据源进行查询操作的表达式树。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <param name="existentParameters">调用该方法时已经传入的参数集合。</param>
        /// <returns>关联查询后新的操作对象。</returns>
        internal abstract Queryable Join<TResult>(DataSourceType joinType, Queryable right, LambdaExpression selector, LambdaExpression on, Interface.IReadOnlyDictionary<string, ParameterInfo> existentParameters);

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
        internal abstract MultipleJoin Join<TJoinSource, TJoinSelect>(DataSourceType joinType, Queryable<TJoinSource, TJoinSelect> right, LambdaExpression on, Interface.IReadOnlyDictionary<string, ParameterInfo> existentParameters);
    }

    /// <summary>
    /// 对数据源提供一个可查询的操作对象。
    /// </summary>
    /// <typeparam name="TSource">数据源中每个成员的类型。</typeparam>
    /// <typeparam name="TSelect">对数据源进行查询返回后新数据源中每个成员的类型。</typeparam>
    public class Queryable<TSource, TSelect> : Queryable, IEnumerable<TSelect>, Interface.IReadOnlyList<TSelect>
    {
        private List<TSelect> _Result = null;

        /// <summary>
        /// 使用指定的数据操作上下文以及数据源初始化一个新的 <see cref="Queryable{TSource, TSelect}"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该查询对象所使用的数据操作上下文。</param>
        /// <param name="dataSource">该对象所使用的数据源。</param>
        internal Queryable(IDataContext dataContext, BasicDataSource dataSource)
            : base(dataContext, dataSource)
        {
        }

        /// <summary>
        /// 对当前对象中的数据源进行查询。
        /// </summary>
        /// <typeparam name="TResult">查询后返回新对象中的数据源每个成员类型。</typeparam>
        /// <param name="selector">对数据源进行查询操作的表达式树。</param>
        /// <returns>查询后新的操作对象。</returns>
        public Queryable<TSelect, TResult> Select<TResult>(Expression<Func<TSelect, TResult>> selector)
        {
            return (Queryable<TSelect, TResult>)Select<TResult>(selector, null);
        }

        /// <summary>
        /// 对当前对象中的数据源进行查询。
        /// </summary>
        /// <typeparam name="TResult">查询后返回新对象中的数据源每个成员类型。</typeparam>
        /// <param name="selector">对数据源进行查询操作的表达式树。</param>
        /// <param name="existentParameters">调用该方法时传入的参数集合。</param>
        /// <returns>查询后新的操作对象。</returns>
        internal override Queryable Select<TResult>(LambdaExpression selector, Interface.IReadOnlyDictionary<string, ParameterInfo> existentParameters)
        {
            int nextTableAliasIndex = DataSource.AliasIndex + 1;
            int nextFieldAliasIndex = 0;

            Dictionary<string, ParameterInfo> parameters = new Dictionary<string, ParameterInfo>();

            if (existentParameters != null)
            {
                foreach (var item in existentParameters)
                {
                    parameters.Add(item.Key, item.Value.Copy());
                }
            }

            parameters[selector.Parameters[0].Name] = new ParameterInfo(true, DataSource);

            Field.Field field = Helper.ExtractField(selector.Body, ExtractType.Select, (Realize.ReadOnlyDictionary<string, ParameterInfo>)parameters, ref nextTableAliasIndex);

            Helper.ResetFieldAlias(DataContext, new HashSet<Field.Field>(), field, ref nextFieldAliasIndex);

            BasicDataSource dataSource;

            if (DataSource.SelectField == null)
                dataSource = DataSource.Copy().SetSelectField(field);
            else
                dataSource = new ResultDataSource(DataContext, DataSourceType.QueryResult, Helper.ToQuoteField(DataSource.SelectField, new Dictionary<Field.Field, Field.Field>(), DataSource), nextTableAliasIndex, DataSource).SetSelectField(field);

            return new Queryable<TSelect, TResult>(DataContext, dataSource);
        }

        /// <summary>
        /// 对当前对象中的数据源进行条件筛选。
        /// </summary>
        /// <param name="where">对数据源进行筛选的表达式树。</param>
        /// <returns>筛选后新的操作对象。</returns>
        public Queryable<TSource, TSelect> Where(Expression<Func<TSource, bool>> where)
        {
            return (Queryable<TSource, TSelect>)Where(where, null);
        }

        /// <summary>
        /// 对当前对象中的数据源进行条件筛选。
        /// </summary>
        /// <param name="where">对数据源进行筛选的表达式树。</param>
        /// <param name="existentParameters">调用该方法时已经传入的参数集合。</param>
        /// <returns>筛选后新的操作对象。</returns>
        internal override Queryable Where(LambdaExpression where, Interface.IReadOnlyDictionary<string, ParameterInfo> existentParameters)
        {
            int nextTableAliasIndex = DataSource.AliasIndex + 1;

            Dictionary<string, ParameterInfo> parameters = new Dictionary<string, ParameterInfo>();

            if (existentParameters != null)
            {
                foreach (var item in existentParameters)
                {
                    parameters.Add(item.Key, item.Value.Copy());
                }
            }

            parameters[where.Parameters[0].Name] = new ParameterInfo(true, DataSource);

            Field.Field field = Helper.ExtractField(where.Body, ExtractType.Default, (Realize.ReadOnlyDictionary<string, ParameterInfo>)parameters, ref nextTableAliasIndex);

            if (field is BasicField basicField)
                return new Queryable<TSource, TSelect>(DataContext, DataSource.Copy().AddWhere(basicField));

            throw new Exception(string.Format("在对数据源进行 Where 条件筛选时未能正确提取到限定字段信息，具体表达式为：{0}", where.ToString()));
        }

        /// <summary>
        /// 对当前对象中的数据源进行正序排序操作。
        /// </summary>
        /// <typeparam name="TSorting">需要排序的字段类型。</typeparam>
        /// <param name="sorting">对数据源进行排序的表达式树。</param>
        /// <returns>排序后新的操作对象。</returns>
        public Queryable<TSource, TSelect> Ascending<TSorting>(Expression<Func<TSource, TSorting>> sorting)
        {
            return (Queryable<TSource, TSelect>)Sorting(SortType.Ascending, sorting, null);
        }

        /// <summary>
        /// 对当前对象中的数据源进行倒序排序操作。
        /// </summary>
        /// <typeparam name="TSorting">需要排序的字段类型。</typeparam>
        /// <param name="sorting">对数据源进行排序的表达式树。</param>
        /// <returns>排序后新的操作对象。</returns>
        public Queryable<TSource, TSelect> Descending<TSorting>(Expression<Func<TSource, TSorting>> sorting)
        {
            return (Queryable<TSource, TSelect>)Sorting(SortType.Descending, sorting, null);
        }

        /// <summary>
        /// 对当前对象中的数据源进行排序操作。
        /// </summary>
        /// <param name="sortType">排序方式。</param>
        /// <param name="sorting">对数据源进行排序的表达式树。</param>
        /// <param name="existentParameters">调用该方法时已经传入的参数集合。</param>
        /// <returns>排序后新的操作对象。</returns>
        internal override Queryable Sorting(SortType sortType, LambdaExpression sorting, Interface.IReadOnlyDictionary<string, ParameterInfo> existentParameters)
        {
            int nextTableAliasIndex = DataSource.AliasIndex + 1;

            Dictionary<string, ParameterInfo> parameters = new Dictionary<string, ParameterInfo>();

            if (existentParameters != null)
            {
                foreach (var item in existentParameters)
                {
                    parameters.Add(item.Key, item.Value.Copy());
                }
            }

            parameters[sorting.Parameters[0].Name] = new ParameterInfo(true, DataSource);

            Field.Field field = Helper.ExtractField(sorting.Body, ExtractType.Default, (Realize.ReadOnlyDictionary<string, ParameterInfo>)parameters, ref nextTableAliasIndex);

            List<SortItem> sortItems = new List<SortItem>();

            AddSortFields(field, sortType, sortItems);

            return new Queryable<TSource, TSelect>(DataContext, DataSource.Copy().AddSortItems(sortItems));
        }

        /// <summary>
        /// 对当前对象中的数据源做去重处理。
        /// </summary>
        /// <returns>去重后新的可查询对象。</returns>
        public Queryable<TSource, TSelect> Distinct()
        {
            return new Queryable<TSource, TSelect>(DataContext, DataSource.Copy().SetDistinctly(true));
        }

        /// <summary>
        /// 以当前对象数据源做左连接操作。
        /// </summary>
        /// <typeparam name="TJoinSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TJoinSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联后的多表操作对象。</returns>
        public MultipleJoin<TSource, TSelect, TJoinSource, TJoinSelect> LeftJoin<TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSource, TJoinSource, bool>> on)
        {
            return (MultipleJoin<TSource, TSelect, TJoinSource, TJoinSelect>)Join(DataSourceType.LeftJoin, right, on, null);
        }

        /// <summary>
        /// 以当前对象数据源做右连接操作。
        /// </summary>
        /// <typeparam name="TJoinSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TJoinSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联后的多表操作对象。</returns>
        public MultipleJoin<TSource, TSelect, TJoinSource, TJoinSelect> RightJoin<TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSource, TJoinSource, bool>> on)
        {
            return (MultipleJoin<TSource, TSelect, TJoinSource, TJoinSelect>)Join(DataSourceType.RightJoin, right, on, null);
        }

        /// <summary>
        /// 以当前对象数据源做直连接操作。
        /// </summary>
        /// <typeparam name="TJoinSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TJoinSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联后的多表操作对象。</returns>
        public MultipleJoin<TSource, TSelect, TJoinSource, TJoinSelect> InnerJoin<TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSource, TJoinSource, bool>> on)
        {
            return (MultipleJoin<TSource, TSelect, TJoinSource, TJoinSelect>)Join(DataSourceType.InnerJoin, right, on, null);
        }

        /// <summary>
        /// 以当前对象数据源做全连接操作。
        /// </summary>
        /// <typeparam name="TJoinSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TJoinSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联后的多表操作对象。</returns>
        public MultipleJoin<TSource, TSelect, TJoinSource, TJoinSelect> FullJoin<TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSource, TJoinSource, bool>> on)
        {
            return (MultipleJoin<TSource, TSelect, TJoinSource, TJoinSelect>)Join(DataSourceType.FullJoin, right, on, null);
        }

        /// <summary>
        /// 以当前对象数据源做交叉连接操作。
        /// </summary>
        /// <typeparam name="TJoinSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TJoinSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <returns>关联后的多表操作对象。</returns>
        public MultipleJoin<TSource, TSelect, TJoinSource, TJoinSelect> CrossJoin<TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right)
        {
            return (MultipleJoin<TSource, TSelect, TJoinSource, TJoinSelect>)Join(DataSourceType.CrossJoin, right, null, null);
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
            Dictionary<string, ParameterInfo> onParameters = on == null ? null : new Dictionary<string, ParameterInfo>();

            int nextTableAliasIndex = DataSource.AliasIndex + 1;

            BasicDataSource rightDataSource = (BasicDataSource)right.DataSource.Copy(ref nextTableAliasIndex);

            BasicField onField = null;

            if (onParameters != null)
            {
                if (existentParameters != null)
                {
                    foreach (var item in existentParameters)
                    {
                        onParameters?.Add(item.Key, item.Value.Copy());
                    }
                }

                onParameters[on.Parameters[0].Name] = new ParameterInfo(DataSource);
                onParameters[on.Parameters[1].Name] = new ParameterInfo(rightDataSource);

                onField = (BasicField)Helper.ExtractField(on.Body, ExtractType.Join, (Realize.ReadOnlyDictionary<string, ParameterInfo>)onParameters, ref nextTableAliasIndex);
            }

            return new MultipleJoin<TSource, TSelect, TJoinSource, TJoinSelect>(DataContext, DataSource, rightDataSource, onField, joinType);
        }

        /// <summary>
        /// 以当前对象数据源做左连接查询。
        /// </summary>
        /// <typeparam name="TResult">查询后返回新对象中的数据源每个成员类型。</typeparam>
        /// <typeparam name="TJoinSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TJoinSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="selector">对关联数据源进行查询操作的表达式树。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联查询后新的操作对象。</returns>
        public Queryable<TResult, TResult> LeftJoin<TResult, TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSelect, TJoinSelect, TResult>> selector, Expression<Func<TSource, TJoinSource, bool>> on)
        {
            return (Queryable<TResult, TResult>)Join<TResult>(DataSourceType.LeftJoin, right, selector, on, null);
        }

        /// <summary>
        /// 以当前对象数据源做右连接查询。
        /// </summary>
        /// <typeparam name="TResult">查询后返回新对象中的数据源每个成员类型。</typeparam>
        /// <typeparam name="TJoinSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TJoinSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="selector">对关联数据源进行查询操作的表达式树。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联查询后新的操作对象。</returns>
        public Queryable<TResult, TResult> RightJoin<TResult, TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSelect, TJoinSelect, TResult>> selector, Expression<Func<TSource, TJoinSource, bool>> on)
        {
            return (Queryable<TResult, TResult>)Join<TResult>(DataSourceType.RightJoin, right, selector, on, null);
        }

        /// <summary>
        /// 以当前对象数据源做直连接查询。
        /// </summary>
        /// <typeparam name="TResult">查询后返回新对象中的数据源每个成员类型。</typeparam>
        /// <typeparam name="TJoinSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TJoinSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="selector">对关联数据源进行查询操作的表达式树。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联查询后新的操作对象。</returns>
        public Queryable<TResult, TResult> InnerJoin<TResult, TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSelect, TJoinSelect, TResult>> selector, Expression<Func<TSource, TJoinSource, bool>> on)
        {
            return (Queryable<TResult, TResult>)Join<TResult>(DataSourceType.InnerJoin, right, selector, on, null);
        }

        /// <summary>
        /// 以当前对象数据源做全连接查询。
        /// </summary>
        /// <typeparam name="TResult">查询后返回新对象中的数据源每个成员类型。</typeparam>
        /// <typeparam name="TJoinSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TJoinSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="selector">对关联数据源进行查询操作的表达式树。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联查询后新的操作对象。</returns>
        public Queryable<TResult, TResult> FullJoin<TResult, TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSelect, TJoinSelect, TResult>> selector, Expression<Func<TSource, TJoinSource, bool>> on)
        {
            return (Queryable<TResult, TResult>)Join<TResult>(DataSourceType.FullJoin, right, selector, on, null);
        }

        /// <summary>
        /// 以当前对象数据源做交叉连接查询。
        /// </summary>
        /// <typeparam name="TResult">查询后返回新对象中的数据源每个成员类型。</typeparam>
        /// <typeparam name="TJoinSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TJoinSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="selector">对关联数据源进行查询操作的表达式树。</param>
        /// <returns>关联查询后新的操作对象。</returns>
        public Queryable<TResult, TResult> CrossJoin<TResult, TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSelect, TJoinSelect, TResult>> selector)
        {
            return (Queryable<TResult, TResult>)Join<TResult>(DataSourceType.CrossJoin, right, selector, null, null);
        }

        /// <summary>
        /// 以当前对象数据源做关联查询。
        /// </summary>
        /// <typeparam name="TResult">查询后返回新对象中的数据源每个成员类型。</typeparam>
        /// <param name="joinType">关联类型。</param>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="selector">对关联数据源进行查询操作的表达式树。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <param name="existentParameters">调用该方法时已经传入的参数集合。</param>
        /// <returns>关联查询后新的操作对象。</returns>
        internal override Queryable Join<TResult>(DataSourceType joinType, Queryable right, LambdaExpression selector, LambdaExpression on, Interface.IReadOnlyDictionary<string, ParameterInfo> existentParameters)
        {
            Dictionary<string, ParameterInfo> selectorParameters = new Dictionary<string, ParameterInfo>();
            Dictionary<string, ParameterInfo> onParameters = on == null ? null : new Dictionary<string, ParameterInfo>();

            if (existentParameters != null)
            {
                foreach (var item in existentParameters)
                {
                    ParameterInfo parameterInfo = item.Value.Copy();

                    selectorParameters.Add(item.Key, parameterInfo);
                    onParameters?.Add(item.Key, parameterInfo);
                }
            }

            int nextTableAliasIndex = DataSource.AliasIndex + 1;
            BasicDataSource rightDataSource = (BasicDataSource)right.DataSource.Copy(ref nextTableAliasIndex);

            selectorParameters[selector.Parameters[0].Name] = new ParameterInfo(DataSource);
            selectorParameters[selector.Parameters[1].Name] = new ParameterInfo(rightDataSource);

            if (onParameters != null)
            {
                onParameters[on.Parameters[0].Name] = new ParameterInfo(DataSource);
                onParameters[on.Parameters[1].Name] = new ParameterInfo(rightDataSource);
            }

            int nextFieldAliasIndex = 0;

            Field.Field selectField = Helper.ExtractField(selector.Body, ExtractType.Select, (Realize.ReadOnlyDictionary<string, ParameterInfo>)selectorParameters, ref nextTableAliasIndex);
            BasicField onField = null;

            Helper.ResetFieldAlias(DataContext, new HashSet<Field.Field>(), selectField, ref nextFieldAliasIndex);

            if (on != null)
                onField = (BasicField)Helper.ExtractField(on.Body, ExtractType.Join, (Realize.ReadOnlyDictionary<string, ParameterInfo>)onParameters, ref nextTableAliasIndex);

            BasicDataSource dataSource = new ResultDataSource(DataContext, DataSourceType.QueryResult, selectField, nextTableAliasIndex, new JoinDataSource(DataContext, joinType, DataSource, rightDataSource, onField));

            return new Queryable<TResult, TResult>(DataContext, dataSource);
        }

        /// <summary>
        /// 对当前对象中的数据源进行分组操作。
        /// </summary>
        /// <typeparam name="TGroup">需要分组的字段类型。</typeparam>
        /// <param name="grouping">对数据源进行分组的表达式树。</param>
        /// <returns>分组后新的操作对象。</returns>
        public Queryable<TSource, TSelect> Group<TGroup>(Expression<Func<TSource, TGroup>> grouping)
        {
            return (Queryable<TSource, TSelect>)Group(grouping, null);
        }

        /// <summary>
        /// 对当前对象中的数据源进行分组操作。
        /// </summary>
        /// <param name="grouping">对数据源进行分组的表达式树。</param>
        /// <param name="existentParameters">调用该方法时已经传入的参数集合。</param>
        /// <returns>分组后新的操作对象。</returns>
        internal override Queryable Group(LambdaExpression grouping, Interface.IReadOnlyDictionary<string, ParameterInfo> existentParameters)
        {
            int nextTableAliasIndex = DataSource.AliasIndex + 1;

            Dictionary<string, ParameterInfo> parameters = new Dictionary<string, ParameterInfo>();

            if (existentParameters != null)
            {
                foreach (var item in existentParameters)
                {
                    parameters.Add(item.Key, item.Value.Copy());
                }
            }

            parameters[grouping.Parameters[0].Name] = new ParameterInfo(true, DataSource);

            Field.Field field = Helper.ExtractField(grouping.Body, ExtractType.Group, (Realize.ReadOnlyDictionary<string, ParameterInfo>)parameters, ref nextTableAliasIndex);

            List<BasicField> groupFields = new List<BasicField>();

            AddGroupFields(field, groupFields);

            if (DataSource.GroupFields == null || DataSource.GroupFields.Count < 1)
            {
                return new Queryable<TSource, TSelect>(DataContext, DataSource.Copy().SetGroupFields(groupFields));
            }
            else
            {
                Dictionary<Field.Field, Field.Field> convertedFields = new Dictionary<Field.Field, Field.Field>();

                BasicDataSource dataSource = new ResultDataSource(DataContext, DataSourceType.QueryResult, Helper.ToQuoteField(DataSource.RootField, convertedFields, DataSource), nextTableAliasIndex, DataSource).SetGroupFields(groupFields);

                if (DataSource.SelectField != null)
                    dataSource.SetSelectField(Helper.ToQuoteField(DataSource.SelectField, convertedFields, DataSource));

                return new Queryable<TSource, TSelect>(DataContext, dataSource);
            }
        }

        /// <summary>
        /// 对当前对象中的数据源查询返回结果行进行限定。
        /// </summary>
        /// <param name="startIndex">允许返回数据行的起始下标。</param>
        /// <param name="count">需要返回的数据行数。</param>
        /// <returns>对数据源查询返回结果行进行限定后的一个新操作对象。</returns>
        public Queryable<TSource, TSelect> Limit(int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(string.Format("调用 {0} 方法时，{1} 参数不能小于 0", nameof(Limit), nameof(startIndex)));

            if (count < 1)
                throw new ArgumentOutOfRangeException(string.Format("调用 {0} 方法时，{1} 参数不能小于 0", nameof(Limit), nameof(count)));

            return (Queryable<TSource, TSelect>)Limit(new Limit(startIndex, count));
        }

        /// <summary>
        /// 对当前对象中的数据源查询返回结果行进行限定。
        /// </summary>
        /// <param name="count">所需返回结果集中最顶部的数据条数。</param>
        /// <returns>对数据源查询返回结果行进行限定后的一个新操作对象。</returns>
        public Queryable<TSource, TSelect> Top(int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException(string.Format("调用 {0} 方法时，{1} 参数不能小于 1", nameof(Top), nameof(count)));

            return (Queryable<TSource, TSelect>)Limit(new Limit(count));
        }

        /// <summary>
        /// 获取当前对象数据源中的第一条成员信息（此方法内部使用 <see cref="Top(int)"/> 原理实现）。
        /// </summary>
        /// <returns>若当前对象数据源中有数据时则返回数据源中第一条成员数据，否则抛出异常。</returns>
        public TSelect First()
        {
            Queryable<TSource, TSelect> queryable = Top(1);

            if (queryable.Count < 1)
                throw new ArgumentOutOfRangeException(string.Format("请在调用 {0} 方法前确保当前可查询对象数据源中有至少一条成员数据信息", nameof(First)));

            return queryable[0];
        }

        /// <summary>
        /// 获取当前对象数据源中的最后一条成员信息（若当前对象中包含有排序字段时，则会将这些字段进行反序排序然后再调用 <see cref="Top(int)"/> 方法来实现，否则将直接执行当前对象所代表的 Sql 语句，然后再返回结果集中的最后一行）。
        /// </summary>
        /// <returns>若当前对象数据源中有数据时则返回数据源中最后一条成员数据，否则抛出异常。</returns>
        public TSelect Last()
        {
            Queryable<TSource, TSelect> queryable;

            if (DataSource.SortItems != null && DataSource.SortItems.Count > 0)
            {
                SortItem[] sortItems = new SortItem[DataSource.SortItems.Count];

                for (int i = 0; i < DataSource.SortItems.Count; i++)
                {
                    var item = DataSource.SortItems[i];

                    sortItems[i] = new SortItem(item.Field, item.Type == SortType.Ascending ? SortType.Descending : SortType.Ascending);
                }

                queryable = new Queryable<TSource, TSelect>(DataContext, DataSource.Copy().SetSortItems(sortItems)).Top(1);
            }
            else
            {
                queryable = this;
            }

            if (queryable.Count < 1)
                throw new ArgumentOutOfRangeException(string.Format("请在调用 {0} 方法前确保当前可查询对象数据源中有至少一条成员数据信息", nameof(Last)));

            return queryable[Count - 1];
        }

        /// <summary>
        /// 确认当前对象数据源中是否包含某一指定成员（直接调用会立即执行数据源的查询操作并执行判定动作，而在子查询或用作 Where 条件时将采用 Sql 的 in 方法实现）。
        /// </summary>
        /// <param name="item">需要确定是否包含的成员对象。</param>
        /// <returns>若当前对象数据源中包含有该成员则返回 true，否则返回 false。</returns>
        public bool Contains(TSelect item)
        {
            InitResult();

            return _Result.Contains(item);
        }

        /// <summary>
        /// 获取该对象对应的 Sql 查询语句。
        /// </summary>
        /// <param name="parameters">查询 Sql 所需的参数集合。</param>
        /// <returns>Sql 查询语句。</returns>
        public string GetSql(out Interface.IReadOnlyList<DbParameter> parameters)
        {
            return DataContext.SqlFactory.GenerateQuerySql(DataContext, DataSource, out parameters);
        }

        /// <summary>
        /// 对当前对象中的数据源查询返回结果进行限定。
        /// </summary>
        /// <param name="limit">需要限定返回的数据行信息。</param>
        /// <returns>对数据源查询返回结果进行限定后的一个新操作对象。</returns>
        internal override Queryable Limit(Limit limit)
        {
            return new Queryable<TSource, TSelect>(DataContext, DataSource.Copy().AddLimit(limit));
        }

        /// <summary>
        /// 以当前对象为蓝本复制出一个新的可查询对象。
        /// </summary>
        /// <param name="startAliasIndex">复制后新数据源的起始表别名下标。</param>
        /// <returns>复制后新的可查询操作对象。</returns>
        internal override Queryable Copy(ref int startAliasIndex)
        {
            return new Queryable<TSource, TSelect>(DataContext, (BasicDataSource)DataSource.Copy(ref startAliasIndex));
        }

        /// <summary>
        /// 初始化当前对象的查询结果数据。
        /// </summary>
        private void InitResult()
        {
            if (_Result == null)
            {
                _Result = new List<TSelect>();

                string querySql = DataContext.SqlFactory.GenerateQuerySql(DataContext, DataSource, out Interface.IReadOnlyList<DbParameter> parameters);

                Field.Field queryField = DataSource.SelectField ?? DataSource.RootField;

                DataContext.DatabaseOperation.ExecuteReader(querySql, parameters, dr =>
                {
                    _Result.Add((TSelect)GetFieldValue(queryField, dr));

                    return true;
                });
            }
        }

        /// <summary>
        /// 从指定的数据读取器中读取出指定字段的值。
        /// </summary>
        /// <param name="field">待读取值的字段信息。</param>
        /// <param name="dr">存有该字段值的数据读取器。</param>
        /// <returns>读取到的字段值。</returns>
        private object GetFieldValue(Field.Field field, DbDataReader dr)
        {
            if (field.Type == FieldType.Object)
            {
                ObjectField objectField = (ObjectField)field;

                object[] constructorParameters = objectField.ConstructorInfo.Parameters != null && objectField.ConstructorInfo.Parameters.Count > 0 ? new object[objectField.ConstructorInfo.Parameters.Count] : null;

                if (constructorParameters != null)
                {
                    for (int i = 0; i < constructorParameters.Length; i++)
                    {
                        constructorParameters[i] = GetFieldValue(objectField.ConstructorInfo.Parameters[i], dr);
                    }
                }

                object result = objectField.ConstructorInfo.Constructor.Invoke(constructorParameters);

                if (objectField.IsNeededInitMembers && objectField.Members != null && objectField.Members.Count > 0)
                {
                    foreach (var item in objectField.Members)
                    {
                        if (item.Value.Member.MemberType == System.Reflection.MemberTypes.Property)
                            ((System.Reflection.PropertyInfo)item.Value.Member).SetValue(result, GetFieldValue(item.Value.Field, dr), null);
                        else if (item.Value.Member.MemberType == System.Reflection.MemberTypes.Field)
                            ((System.Reflection.FieldInfo)item.Value.Member).SetValue(result, GetFieldValue(item.Value.Field, dr));
                    }
                }

                return result;
            }
            else if (field.Type == FieldType.Collection)
            {
                CollectionField collectionField = (CollectionField)field;

                object[] constructorParameters = collectionField.ConstructorInfo.Parameters != null && collectionField.ConstructorInfo.Parameters.Count > 0 ? new object[collectionField.ConstructorInfo.Parameters.Count] : null;

                if (constructorParameters != null)
                {
                    for (int i = 0; i < constructorParameters.Length; i++)
                    {
                        constructorParameters[i] = GetFieldValue(collectionField.ConstructorInfo.Parameters[i], dr);
                    }
                }

                if (collectionField.DataType.IsArray)
                {
                    Array result = (Array)collectionField.ConstructorInfo.Constructor.Invoke(new object[] { collectionField.Count });

                    for (int i = 0; i < collectionField.Count; i++)
                    {
                        result.SetValue(GetFieldValue(collectionField[i], dr), i);
                    }

                    return result;
                }
                else
                {
                    IList list = (IList)collectionField.ConstructorInfo.Constructor.Invoke(null);

                    foreach (var item in collectionField)
                    {
                        list.Add(GetFieldValue(item, dr));
                    }

                    return list;
                }
            }
            else
            {
                object result = dr[((BasicField)field).Alias];

                if (result == DBNull.Value)
                    return null;
                else
                    return result;
            }
        }

        /// <summary>
        /// 添加排序字段到指定的集合。
        /// </summary>
        /// <param name="field">待添加到集合中的排序字段。</param>
        /// <param name="sortType">排序类型。</param>
        /// <param name="sortItems">用于保存排序字段的集合。</param>
        private void AddSortFields(Field.Field field, SortType sortType, List<SortItem> sortItems)
        {
            if (field.Type == FieldType.Object)
            {
                ObjectField objectField = (ObjectField)field;

                if (objectField.Members != null && objectField.Members.Count > 0)
                {
                    foreach (var item in objectField.Members)
                    {
                        AddSortFields(item.Value.Field, sortType, sortItems);
                    }
                }
            }
            else if (field.Type == FieldType.Collection)
            {
                foreach (var item in (CollectionField)field)
                {
                    AddSortFields(item, sortType, sortItems);
                }
            }
            else
            {
                sortItems.Add(new SortItem((BasicField)field, sortType));
            }
        }

        /// <summary>
        /// 添加分组字段到指定的集合。
        /// </summary>
        /// <param name="field">待添加到集合中的分组字段。</param>
        /// <param name="groupFields">用于保存分组字段的集合。</param>
        private void AddGroupFields(Field.Field field, List<BasicField> groupFields)
        {
            if (field.Type == FieldType.Object)
            {
                ObjectField objectField = (ObjectField)field;

                if (objectField.Members != null && objectField.Members.Count > 0)
                {
                    foreach (var item in objectField.Members)
                    {
                        AddGroupFields(item.Value.Field, groupFields);
                    }
                }
            }
            else if (field.Type == FieldType.Collection)
            {
                foreach (var item in (CollectionField)field)
                {
                    AddGroupFields(item, groupFields);
                }
            }
            else
            {
                groupFields.Add((BasicField)field);
            }
        }

        #region 实现 Foreach 以及 IReadOnlyList 接口

        /// <summary>
        /// 获取当前对象中所有的成员个数（调用此属时若当前对象尚未对数据源的进行查询，则此操作将立即触发查询操作）。
        /// </summary>
        public int Count
        {
            get
            {
                InitResult();

                return _Result.Count;
            }
        }

        /// <summary>
        /// 获取当前对象中指定索引处的成员信息（调用此属时若当前对象尚未对数据源的结果进行查询，则此操作将立即触发查询操作）。
        /// </summary>
        /// <param name="index">待获取成员所在的下标。</param>
        /// <returns>指定索引处的成员信息。</returns>
        public TSelect this[int index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException("获取可查询对象中某一索引处的成员时索引下标超出范围");

                InitResult();

                return _Result[index];
            }
        }

        /// <summary>
        /// 获取一个用于枚举当前对象中所有成员的枚举器（调用此属时若当前对象尚未对数据源的结果进行查询，则此操作将立即触发查询操作）。
        /// </summary>
        /// <returns>用于枚举当前对象中所有成员的枚举器。</returns>
        IEnumerator<TSelect> IEnumerable<TSelect>.GetEnumerator()
        {
            InitResult();

            return _Result.GetEnumerator();
        }

        /// <summary>
        /// 获取一个用于枚举当前对象中所有成员的枚举器（调用此属时若当前对象尚未对数据源的结果进行查询，则此操作将立即触发查询操作）。
        /// </summary>
        /// <returns>用于枚举当前对象中所有成员的枚举器。</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            InitResult();

            return _Result.GetEnumerator();
        }

        #endregion
    }
}
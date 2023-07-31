using GfdbFramework.DataSource;
using GfdbFramework.Enum;
using GfdbFramework.Field;
using GfdbFramework.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;

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
        /// 对当前对象中的数据源进行查询。
        /// </summary>
        /// <typeparam name="TResult">查询后返回新对象中的数据源每个成员类型。</typeparam>
        /// <param name="selector">对数据源进行查询操作的表达式树。</param>
        /// <param name="existentParameters">调用该方法时已经传入的参数集合。</param>
        /// <returns>查询后新的操作对象。</returns>
        internal abstract Queryable Select<TResult>(LambdaExpression selector, ReadOnlyDictionary<string, DataSource.DataSource> existentParameters);

        /// <summary>
        /// 对当前对象中的数据源进行条件筛选。
        /// </summary>
        /// <param name="where">对数据源进行筛选的表达式树。</param>
        /// <param name="existentParameters">调用该方法时已经传入的参数集合。</param>
        /// <returns>筛选后新的操作对象。</returns>
        internal abstract Queryable Where(LambdaExpression where, ReadOnlyDictionary<string, DataSource.DataSource> existentParameters);

        /// <summary>
        /// 对当前对象中的数据源进行排序操作。
        /// </summary>
        /// <param name="sortType">排序方式。</param>
        /// <param name="sorting">对数据源进行排序的表达式树。</param>
        /// <param name="existentParameters">调用该方法时已经传入的参数集合。</param>
        /// <returns>排序后新的操作对象。</returns>
        internal abstract Queryable Sorting(SortType sortType, LambdaExpression sorting, ReadOnlyDictionary<string, DataSource.DataSource> existentParameters);

        /// <summary>
        /// 对当前对象中的数据源进行分组操作。
        /// </summary>
        /// <param name="grouping">对数据源进行分组的表达式树。</param>
        /// <param name="existentParameters">调用该方法时已经传入的参数集合。</param>
        /// <returns>分组后新的操作对象。</returns>
        internal abstract Queryable Group(LambdaExpression grouping, ReadOnlyDictionary<string, DataSource.DataSource> existentParameters);

        /// <summary>
        /// 对当前对象中的数据源查询返回结果进行限定。
        /// </summary>
        /// <param name="limit">需要限定返回的数据行信息。</param>
        /// <returns>对数据源查询返回结果进行限定后的一个新操作对象。</returns>
        internal abstract Queryable Limit(Limit limit);

        /// <summary>
        /// 以当前对象为蓝本复制出一个新的可查询对象。
        /// </summary>
        /// <param name="copiedDataSources">已经复制过的数据源集合。</param>
        /// <param name="copiedFields">已经复制好的字段信息。</param>
        /// <param name="startAliasIndex">复制后新数据源的起始表别名下标。</param>
        /// <returns>复制后新的可查询操作对象。</returns>
        internal abstract Queryable Copy(Dictionary<DataSource.DataSource, DataSource.DataSource> copiedDataSources, Dictionary<Field.Field, Field.Field> copiedFields, ref int startAliasIndex);

        /// <summary>
        /// 以当前对象数据源做关联查询。
        /// </summary>
        /// <typeparam name="TResult">查询后返回新对象中的数据源每个成员类型。</typeparam>
        /// <param name="joinType">关联类型。</param>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="selector">对关联数据源进行查询操作的表达式树。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <param name="where">查询操作的条件限定表达式树。</param>
        /// <param name="existentParameters">调用该方法时已经传入的参数集合。</param>
        /// <returns>关联查询后新的操作对象。</returns>
        internal abstract Queryable Join<TResult>(SourceType joinType, Queryable right, LambdaExpression selector, LambdaExpression on, LambdaExpression where, ReadOnlyDictionary<string, DataSource.DataSource> existentParameters);

        /// <summary>
        /// 获取当前查询对象获取最后一行成员的查询对象。
        /// </summary>
        /// <returns>对应当前查询对象用于获取最后一个成员的查询对象。</returns>
        internal abstract Queryable GetLastQueryable();

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
        internal abstract MultipleJoin Join<TJoinSource, TJoinSelect>(SourceType joinType, Queryable<TJoinSource, TJoinSelect> right, LambdaExpression on, ReadOnlyDictionary<string, DataSource.DataSource> existentParameters);

        /// <summary>
        /// 将当前查询对象中的数据与另一个查询对象中所有数据合并到一个新的查询对象。
        /// </summary>
        /// <param name="queryable">需要合并的另一个查询对象。</param>
        /// <param name="unionType">合并类型。</param>
        /// <returns>合并后的新查询对象。</returns>
        internal abstract Queryable Union(Queryable queryable, UnionType unionType);

        /// <summary>
        /// 获取当前可查询对象的数据源。
        /// </summary>
        internal BasicDataSource DataSource { get; }

        /// <summary>
        /// 获取当前可查询对象所使用的数据操作上下文。
        /// </summary>
        internal IDataContext DataContext { get; }

        /// <summary>
        /// 获取该对象对应的 Sql 查询语句。
        /// </summary>
        /// <param name="parameterContext">生成查询 Sql 时如需用到参数化操作时的上下文对象。</param>
        /// <returns>Sql 查询语句。</returns>
        public abstract string GetSql(IParameterContext parameterContext);
    }

    /// <summary>
    /// 对数据源提供一个可查询的操作对象。
    /// </summary>
    /// <typeparam name="TSource">数据源中每个成员的类型。</typeparam>
    /// <typeparam name="TSelect">对数据源进行查询返回后新数据源中每个成员的类型。</typeparam>
    [DebuggerDisplay("{GetDebugResult()}")]
    public class Queryable<TSource, TSelect> : Queryable, IEnumerable<TSelect>
    {
        private List<TSelect> _Result = null;
        private string _QuerySql = null;
        private IParameterContext _ParameterContext = null;
        private static readonly Type _NullableType = typeof(int?).GetGenericTypeDefinition();

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
        internal override Queryable Select<TResult>(LambdaExpression selector, ReadOnlyDictionary<string, DataSource.DataSource> existentParameters)
        {
            int nextTableAliasIndex = DataSource.AliasIndex + 1;

            Dictionary<string, DataSource.DataSource> parameters = new Dictionary<string, DataSource.DataSource>();

            if (existentParameters != null)
            {
                foreach (var item in existentParameters)
                {
                    parameters.Add(item.Key, item.Value);
                }
            }

            parameters[selector.Parameters[0].Name] = DataSource;

            var convertedFields = new Dictionary<Field.Field, Field.Field>();

            var fieldAliasIndex = 0;

            Field.Field field = Helper.ExtractField(DataContext, selector.Body, DataSource.SelectField == null ? ExtractWay.SelectNewAlias : ExtractWay.SelectNew, parameters, convertedFields, ref nextTableAliasIndex).ResetAlias(ref fieldAliasIndex);

            BasicDataSource dataSource;

            if (DataSource.SelectField == null)
                dataSource = DataSource.ShallowCopy().SetSelectField(field);
            else
                dataSource = new SelectDataSource(DataContext, field, DataSource.SelectField.ToQuoteField(DataSource, convertedFields), DataSource, nextTableAliasIndex);

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
        internal override Queryable Where(LambdaExpression where, ReadOnlyDictionary<string, DataSource.DataSource> existentParameters)
        {
            int nextTableAliasIndex = DataSource.AliasIndex + 1;

            Dictionary<string, DataSource.DataSource> parameters = new Dictionary<string, DataSource.DataSource>();

            if (existentParameters != null)
            {
                foreach (var item in existentParameters)
                {
                    parameters.Add(item.Key, item.Value);
                }
            }

            parameters[where.Parameters[0].Name] = DataSource;

            Field.Field field = Helper.ExtractField(DataContext, where.Body, ExtractWay.Other, parameters, ref nextTableAliasIndex);

            if (field is BasicField basicField)
            {
                BasicDataSource dataSource;

                if (DataSource.GroupFields == null || DataSource.GroupFields.Count < 1)
                {
                    dataSource = DataSource.ShallowCopy().AddWhere(basicField);
                }
                else
                {
                    var convertedFields = new Dictionary<Field.Field, Field.Field>();

                    var selectField = (DataSource.SelectField ?? DataSource.RootField).ToQuoteField(DataSource, convertedFields);
                    var rootField = DataSource.RootField.ToQuoteField(DataSource, convertedFields);

                    dataSource = new SelectDataSource(DataContext, selectField, rootField, DataSource, nextTableAliasIndex).AddWhere(basicField);
                }

                return new Queryable<TSource, TSelect>(DataContext, dataSource);
            }
            else
            {
                throw new Exception($"在对数据源进行 Where 条件筛选时未能正确提取到限定字段信息，具体表达式为：{where}");
            }
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
        internal override Queryable Sorting(SortType sortType, LambdaExpression sorting, ReadOnlyDictionary<string, DataSource.DataSource> existentParameters)
        {
            int nextTableAliasIndex = DataSource.AliasIndex + 1;

            Dictionary<string, DataSource.DataSource> parameters = new Dictionary<string, DataSource.DataSource>();

            if (existentParameters != null)
            {
                foreach (var item in existentParameters)
                {
                    parameters.Add(item.Key, item.Value);
                }
            }

            parameters[sorting.Parameters[0].Name] = DataSource;

            Field.Field field = Helper.ExtractField(DataContext, sorting.Body, ExtractWay.Other, parameters, ref nextTableAliasIndex);

            List<SortItem> sortItems = new List<SortItem>();

            AddSortFields(field, sortType, sortItems);

            return new Queryable<TSource, TSelect>(DataContext, DataSource.ShallowCopy().AddSortItems(sortItems));
        }

        /// <summary>
        /// 对当前对象中的数据源做去重处理。
        /// </summary>
        /// <returns>去重后新的可查询对象。</returns>
        public Queryable<TSource, TSelect> Distinct()
        {
            return new Queryable<TSource, TSelect>(DataContext, DataSource.ShallowCopy().SetDistinctly(true));
        }

        /// <summary>
        /// 以当前对象数据源做左连接操作。
        /// </summary>
        /// <typeparam name="TJoinSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TJoinSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联后的多表操作对象。</returns>
        public MultipleJoin<TSelect, TJoinSelect> LeftJoin<TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSelect, TJoinSelect, bool>> on)
        {
            return (MultipleJoin<TSelect, TJoinSelect>)Join(SourceType.LeftJoin, right, on, null);
        }

        /// <summary>
        /// 以当前对象数据源做右连接操作。
        /// </summary>
        /// <typeparam name="TJoinSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TJoinSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联后的多表操作对象。</returns>
        public MultipleJoin<TSelect, TJoinSelect> RightJoin<TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSelect, TJoinSelect, bool>> on)
        {
            return (MultipleJoin<TSelect, TJoinSelect>)Join(SourceType.RightJoin, right, on, null);
        }

        /// <summary>
        /// 以当前对象数据源做直连接操作。
        /// </summary>
        /// <typeparam name="TJoinSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TJoinSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联后的多表操作对象。</returns>
        public MultipleJoin<TSelect, TJoinSelect> InnerJoin<TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSelect, TJoinSelect, bool>> on)
        {
            return (MultipleJoin<TSelect, TJoinSelect>)Join(SourceType.InnerJoin, right, on, null);
        }

        /// <summary>
        /// 以当前对象数据源做全连接操作。
        /// </summary>
        /// <typeparam name="TJoinSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TJoinSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <returns>关联后的多表操作对象。</returns>
        public MultipleJoin<TSelect, TJoinSelect> FullJoin<TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSelect, TJoinSelect, bool>> on)
        {
            return (MultipleJoin<TSelect, TJoinSelect>)Join(SourceType.FullJoin, right, on, null);
        }

        /// <summary>
        /// 以当前对象数据源做交叉连接操作。
        /// </summary>
        /// <typeparam name="TJoinSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TJoinSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <returns>关联后的多表操作对象。</returns>
        public MultipleJoin<TSelect, TJoinSelect> CrossJoin<TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right)
        {
            return (MultipleJoin<TSelect, TJoinSelect>)Join(SourceType.CrossJoin, right, null, null);
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
            Dictionary<string, DataSource.DataSource> onParameters = on == null ? null : new Dictionary<string, DataSource.DataSource>();

            int nextTableAliasIndex = DataSource.AliasIndex + 1;

            BasicDataSource leftDataSource = DataSource;
            BasicDataSource rightDataSource = (BasicDataSource)right.DataSource.Copy(new Dictionary<DataSource.DataSource, DataSource.DataSource>(), new Dictionary<Field.Field, Field.Field>(), ref nextTableAliasIndex);

            if (leftDataSource.SelectField == null && (leftDataSource.Type == SourceType.View || leftDataSource.Type == SourceType.Table) && (leftDataSource.Limit.HasValue || (leftDataSource.SortItems != null && leftDataSource.SortItems.Count > 0) || (leftDataSource.GroupFields != null && leftDataSource.GroupFields.Count > 0) || leftDataSource.IsDistinctly || leftDataSource.Where != null))
                leftDataSource = leftDataSource.ShallowCopy().SetSelectField(leftDataSource.RootField.ToQuoteField(leftDataSource.Alias, new Dictionary<Field.Field, Field.Field>(), true));

            if (rightDataSource.SelectField == null && (rightDataSource.Type == SourceType.View || rightDataSource.Type == SourceType.Table) && (rightDataSource.Limit.HasValue || (rightDataSource.SortItems != null && rightDataSource.SortItems.Count > 0) || (rightDataSource.GroupFields != null && rightDataSource.GroupFields.Count > 0) || rightDataSource.IsDistinctly || rightDataSource.Where != null))
                rightDataSource = rightDataSource.SetSelectField(rightDataSource.RootField.ToQuoteField(rightDataSource.Alias, new Dictionary<Field.Field, Field.Field>(), true));

            BasicField onField = null;

            if (onParameters != null)
            {
                if (existentParameters != null)
                {
                    foreach (var item in existentParameters)
                    {
                        onParameters?.Add(item.Key, item.Value);
                    }
                }

                onParameters[on.Parameters[0].Name] = leftDataSource;
                onParameters[on.Parameters[1].Name] = rightDataSource;

                onField = (BasicField)Helper.ExtractField(DataContext, on.Body, ExtractWay.SelectNew, onParameters, ref nextTableAliasIndex);
            }

            return new MultipleJoin<TSelect, TJoinSelect>(DataContext, leftDataSource, rightDataSource, onField, joinType);
        }

        /// <summary>
        /// 将当前查询对象中的数据与另一个查询对象中所有数据合并到一个新的查询对象（包括重复行）。
        /// </summary>
        /// <typeparam name="TAffiliationSource">需要合并数据源的原始数据类型。</typeparam>
        /// <param name="queryable">需要合并的另一个查询对象。</param>
        /// <returns>合并后的新查询对象。</returns>
        public Queryable<TSelect, TSelect> UnionAll<TAffiliationSource>(Queryable<TAffiliationSource, TSelect> queryable)
        {
            return (Queryable<TSelect, TSelect>)Union(queryable, UnionType.UnionALL);
        }

        /// <summary>
        /// 将当前查询对象中的数据与另一个查询对象中所有数据合并到一个新的查询对象（不包括重复行）。
        /// </summary>
        /// <typeparam name="TAffiliationSource">需要合并数据源的原始数据类型。</typeparam>
        /// <param name="queryable">需要合并的另一个查询对象。</param>
        /// <returns>合并后的新查询对象。</returns>
        public Queryable<TSelect, TSelect> Union<TAffiliationSource>(Queryable<TAffiliationSource, TSelect> queryable)
        {
            return (Queryable<TSelect, TSelect>)Union(queryable, UnionType.Union);
        }

        /// <summary>
        /// 将当前查询对象中的数据与另一个查询对象中所有数据合并到一个新的查询对象（取交集）。
        /// </summary>
        /// <typeparam name="TAffiliationSource">需要合并数据源的原始数据类型。</typeparam>
        /// <param name="queryable">需要合并的另一个查询对象。</param>
        /// <returns>合并后的新查询对象。</returns>
        public Queryable<TSelect, TSelect> Intersect<TAffiliationSource>(Queryable<TAffiliationSource, TSelect> queryable)
        {
            return (Queryable<TSelect, TSelect>)Union(queryable, UnionType.Intersect);
        }

        /// <summary>
        /// 将当前查询对象中的数据与另一个查询对象中所有数据合并到一个新的查询对象（取行差）。
        /// </summary>
        /// <typeparam name="TAffiliationSource">需要合并数据源的原始数据类型。</typeparam>
        /// <param name="queryable">需要合并的另一个查询对象。</param>
        /// <returns>合并后的新查询对象。</returns>
        public Queryable<TSelect, TSelect> Minus<TAffiliationSource>(Queryable<TAffiliationSource, TSelect> queryable)
        {
            return (Queryable<TSelect, TSelect>)Union(queryable, UnionType.Minus);
        }

        /// <summary>
        /// 将当前查询对象中的数据与另一个查询对象中所有数据合并到一个新的查询对象。
        /// </summary>
        /// <param name="queryable">需要合并的另一个查询对象。</param>
        /// <param name="unionType">合并类型。</param>
        /// <returns>合并后的新查询对象。</returns>
        internal override Queryable Union(Queryable queryable, UnionType unionType)
        {
            if ((queryable.DataSource.SortItems != null && queryable.DataSource.SortItems.Count > 0) || (DataSource.SortItems != null && DataSource.SortItems.Count > 0))
                throw new ArgumentException($"合并的两个数据源不能有任何排序字段");

            return new Queryable<TSelect, TSelect>(DataContext, new UnionDataSource(DataContext, DataSource, queryable.DataSource.AlignUnionSource(DataSource), unionType, DataSource.AliasIndex));
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
        public Queryable<TResult, TResult> LeftJoin<TResult, TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSelect, TJoinSelect, TResult>> selector, Expression<Func<TSelect, TJoinSelect, bool>> on)
        {
            return (Queryable<TResult, TResult>)Join<TResult>(SourceType.LeftJoin, right, selector, on, null, null);
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
        /// <param name="where">查询操作的条件限定表达式树。</param>
        /// <returns>关联查询后新的操作对象。</returns>
        public Queryable<TResult, TResult> LeftJoin<TResult, TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSelect, TJoinSelect, TResult>> selector, Expression<Func<TSelect, TJoinSelect, bool>> on, Expression<Func<TSelect, TJoinSelect, bool>> where)
        {
            return (Queryable<TResult, TResult>)Join<TResult>(SourceType.LeftJoin, right, selector, on, where, null);
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
        public Queryable<TResult, TResult> RightJoin<TResult, TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSelect, TJoinSelect, TResult>> selector, Expression<Func<TSelect, TJoinSelect, bool>> on)
        {
            return (Queryable<TResult, TResult>)Join<TResult>(SourceType.RightJoin, right, selector, on, null, null);
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
        /// <param name="where">查询操作的条件限定表达式树。</param>
        /// <returns>关联查询后新的操作对象。</returns>
        public Queryable<TResult, TResult> RightJoin<TResult, TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSelect, TJoinSelect, TResult>> selector, Expression<Func<TSelect, TJoinSelect, bool>> on, Expression<Func<TSelect, TJoinSelect, bool>> where)
        {
            return (Queryable<TResult, TResult>)Join<TResult>(SourceType.RightJoin, right, selector, on, where, null);
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
        public Queryable<TResult, TResult> InnerJoin<TResult, TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSelect, TJoinSelect, TResult>> selector, Expression<Func<TSelect, TJoinSelect, bool>> on)
        {
            return (Queryable<TResult, TResult>)Join<TResult>(SourceType.InnerJoin, right, selector, on, null, null);
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
        /// <param name="where">查询操作的条件限定表达式树。</param>
        /// <returns>关联查询后新的操作对象。</returns>
        public Queryable<TResult, TResult> InnerJoin<TResult, TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSelect, TJoinSelect, TResult>> selector, Expression<Func<TSelect, TJoinSelect, bool>> on, Expression<Func<TSelect, TJoinSelect, bool>> where)
        {
            return (Queryable<TResult, TResult>)Join<TResult>(SourceType.InnerJoin, right, selector, on, where, null);
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
        public Queryable<TResult, TResult> FullJoin<TResult, TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSelect, TJoinSelect, TResult>> selector, Expression<Func<TSelect, TJoinSelect, bool>> on)
        {
            return (Queryable<TResult, TResult>)Join<TResult>(SourceType.FullJoin, right, selector, on, null, null);
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
        /// <param name="where">查询操作的条件限定表达式树。</param>
        /// <returns>关联查询后新的操作对象。</returns>
        public Queryable<TResult, TResult> FullJoin<TResult, TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSelect, TJoinSelect, TResult>> selector, Expression<Func<TSelect, TJoinSelect, bool>> on, Expression<Func<TSelect, TJoinSelect, bool>> where)
        {
            return (Queryable<TResult, TResult>)Join<TResult>(SourceType.FullJoin, right, selector, on, where, null);
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
            return (Queryable<TResult, TResult>)Join<TResult>(SourceType.CrossJoin, right, selector, null, null, null);
        }

        /// <summary>
        /// 以当前对象数据源做交叉连接查询。
        /// </summary>
        /// <typeparam name="TResult">查询后返回新对象中的数据源每个成员类型。</typeparam>
        /// <typeparam name="TJoinSource">右数据源中的原始成员类型。</typeparam>
        /// <typeparam name="TJoinSelect">右数据源中的每个成员类型。</typeparam>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="selector">对关联数据源进行查询操作的表达式树。</param>
        /// <param name="where">查询操作的条件限定表达式树。</param>
        /// <returns>关联查询后新的操作对象。</returns>
        public Queryable<TResult, TResult> CrossJoin<TResult, TJoinSource, TJoinSelect>(Queryable<TJoinSource, TJoinSelect> right, Expression<Func<TSelect, TJoinSelect, TResult>> selector, Expression<Func<TSelect, TJoinSelect, bool>> where)
        {
            return (Queryable<TResult, TResult>)Join<TResult>(SourceType.CrossJoin, right, selector, null, where, null);
        }

        /// <summary>
        /// 以当前对象数据源做关联查询。
        /// </summary>
        /// <typeparam name="TResult">查询后返回新对象中的数据源每个成员类型。</typeparam>
        /// <param name="joinType">关联类型。</param>
        /// <param name="right">需要关联的右侧查询对象。</param>
        /// <param name="selector">对关联数据源进行查询操作的表达式树。</param>
        /// <param name="on">对左右数据源进行条件关联的表达式树。</param>
        /// <param name="where">查询操作的条件限定表达式树。</param>
        /// <param name="existentParameters">调用该方法时已经传入的参数集合。</param>
        /// <returns>关联查询后新的操作对象。</returns>
        internal override Queryable Join<TResult>(SourceType joinType, Queryable right, LambdaExpression selector, LambdaExpression on, LambdaExpression where, ReadOnlyDictionary<string, DataSource.DataSource> existentParameters)
        {
            Dictionary<string, DataSource.DataSource> selectorParameters = new Dictionary<string, DataSource.DataSource>();
            Dictionary<string, DataSource.DataSource> onParameters = on == null ? null : new Dictionary<string, DataSource.DataSource>();
            Dictionary<string, DataSource.DataSource> whereParameters = where == null ? null : new Dictionary<string, DataSource.DataSource>();

            if (existentParameters != null)
            {
                foreach (var item in existentParameters)
                {
                    selectorParameters.Add(item.Key, item.Value);
                    onParameters?.Add(item.Key, item.Value);
                    whereParameters?.Add(item.Key, item.Value);
                }
            }

            int nextTableAliasIndex = DataSource.AliasIndex + 1;
            BasicDataSource leftDataSource = DataSource;
            BasicDataSource rightDataSource = (BasicDataSource)right.DataSource.Copy(new Dictionary<DataSource.DataSource, DataSource.DataSource>(), new Dictionary<Field.Field, Field.Field>(), ref nextTableAliasIndex);

            if (leftDataSource.SelectField == null && (leftDataSource.Type == SourceType.View || leftDataSource.Type == SourceType.Table) && (leftDataSource.Limit.HasValue || (leftDataSource.SortItems != null && leftDataSource.SortItems.Count > 0) || (leftDataSource.GroupFields != null && leftDataSource.GroupFields.Count > 0) || leftDataSource.IsDistinctly || leftDataSource.Where != null))
                leftDataSource = leftDataSource.ShallowCopy().SetSelectField(leftDataSource.RootField.ToQuoteField(leftDataSource.Alias, new Dictionary<Field.Field, Field.Field>(), true));

            if (rightDataSource.SelectField == null && (rightDataSource.Type == SourceType.View || rightDataSource.Type == SourceType.Table) && (rightDataSource.Limit.HasValue || (rightDataSource.SortItems != null && rightDataSource.SortItems.Count > 0) || (rightDataSource.GroupFields != null && rightDataSource.GroupFields.Count > 0) || rightDataSource.IsDistinctly || rightDataSource.Where != null))
                rightDataSource = rightDataSource.SetSelectField(rightDataSource.RootField.ToQuoteField(rightDataSource.Alias, new Dictionary<Field.Field, Field.Field>(), true));

            selectorParameters[selector.Parameters[0].Name] = leftDataSource;
            selectorParameters[selector.Parameters[1].Name] = rightDataSource;

            if (onParameters != null)
            {
                onParameters[on.Parameters[0].Name] = leftDataSource;
                onParameters[on.Parameters[1].Name] = rightDataSource;
            }

            if (whereParameters != null)
            {
                whereParameters[where.Parameters[0].Name] = leftDataSource;
                whereParameters[where.Parameters[1].Name] = rightDataSource;
            }

            int fieldAliasIndex = 0;

            Field.Field selectField = Helper.ExtractField(DataContext, selector.Body, ExtractWay.SelectNew, selectorParameters, ref nextTableAliasIndex).ResetAlias(ref fieldAliasIndex);
            BasicField onField = null;
            BasicField whereField = null;

            if (on != null)
                onField = (BasicField)Helper.ExtractField(DataContext, on.Body, ExtractWay.SelectNew, onParameters, ref nextTableAliasIndex);

            if (where != null)
                whereField = (BasicField)Helper.ExtractField(DataContext, where.Body, ExtractWay.SelectNew, whereParameters, ref nextTableAliasIndex);

            BasicDataSource dataSource = new SelectDataSource(DataContext, selectField, selectField, new JoinDataSource(DataContext, joinType, leftDataSource, rightDataSource, onField), nextTableAliasIndex);

            return new Queryable<TResult, TResult>(DataContext, dataSource.AddWhere(whereField));
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
        internal override Queryable Group(LambdaExpression grouping, ReadOnlyDictionary<string, DataSource.DataSource> existentParameters)
        {
            int nextTableAliasIndex = DataSource.AliasIndex + 1;

            Dictionary<string, DataSource.DataSource> parameters = new Dictionary<string, DataSource.DataSource>();

            if (existentParameters != null)
            {
                foreach (var item in existentParameters)
                {
                    parameters.Add(item.Key, item.Value);
                }
            }

            parameters[grouping.Parameters[0].Name] = DataSource;

            Field.Field field = Helper.ExtractField(DataContext, grouping.Body, ExtractWay.Other, parameters, ref nextTableAliasIndex);

            List<BasicField> groupFields = new List<BasicField>();

            AddGroupFields(field, groupFields);

            BasicDataSource dataSource;

            if (DataSource.GroupFields == null || DataSource.GroupFields.Count < 1)
            {
                dataSource = DataSource.ShallowCopy().SetGroupFields(groupFields);
            }
            else
            {
                var convertedFields = new Dictionary<Field.Field, Field.Field>();

                var selectField = (DataSource.SelectField ?? DataSource.RootField).ToQuoteField(DataSource, convertedFields);
                var rootField = DataSource.RootField.ToQuoteField(DataSource, convertedFields);

                dataSource = new SelectDataSource(DataContext, selectField, rootField, DataSource, nextTableAliasIndex).SetGroupFields(groupFields);
            }

            return new Queryable<TSource, TSelect>(DataContext, dataSource);
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
                throw new ArgumentOutOfRangeException($"调用 {nameof(Limit)} 方法时，{nameof(startIndex)} 参数不能小于 0");

            if (count < 0)
                throw new ArgumentOutOfRangeException($"调用 {nameof(Limit)} 方法时，{nameof(count)} 参数不能小于 0");

            return (Queryable<TSource, TSelect>)Limit(new Limit(startIndex, count));
        }

        /// <summary>
        /// 对当前对象中的数据源查询返回结果行进行限定。
        /// </summary>
        /// <param name="count">所需返回结果集中最顶部的数据条数。</param>
        /// <returns>对数据源查询返回结果行进行限定后的一个新操作对象。</returns>
        public Queryable<TSource, TSelect> Top(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException($"调用 {nameof(Limit)} 方法时，{nameof(count)} 参数不能小于 0");

            return (Queryable<TSource, TSelect>)Limit(new Limit(count));
        }

        /// <summary>
        /// 对当前对象中的数据源查询返回结果进行限定。
        /// </summary>
        /// <param name="limit">需要限定返回的数据行信息。</param>
        /// <returns>对数据源查询返回结果进行限定后的一个新操作对象。</returns>
        internal override Queryable Limit(Limit limit)
        {
            return new Queryable<TSource, TSelect>(DataContext, DataSource.ShallowCopy().AddLimit(limit));
        }

        /// <summary>
        /// 获取当前对象数据源中的第一条成员信息（此方法内部使用 <see cref="Top(int)"/> 原理实现）。
        /// </summary>
        /// <returns>若当前对象数据源中有数据时则返回数据源中第一条成员数据，否则抛出异常。</returns>
        public TSelect First()
        {
            Queryable<TSource, TSelect> queryable = Top(1);

            if (queryable.Count < 1)
                throw new ArgumentOutOfRangeException($"请在调用 {nameof(First)} 方法前确保当前可查询对象数据源中有至少一条成员数据信息");

            return queryable[0];
        }

        /// <summary>
        /// 获取当前对象数据源中的第一条成员信息（此方法内部使用 <see cref="Top(int)"/> 原理实现，若数据源中一条数据都没有的情况下则返回一个默认值）。
        /// </summary>
        /// <returns>若当前对象数据源中有数据时则返回数据源中第一条成员数据，否则返回 <typeparamref name="TSelect"/> 类型的默认值。</returns>
        public TSelect FirstOrDefault()
        {
            Queryable<TSource, TSelect> queryable = Top(1);

            if (queryable.Count < 1)
                return default;
            else
                return queryable[0];
        }

        /// <summary>
        /// 获取当前对象数据源中的最后一条成员信息（若当前对象中包含有排序字段时，则会将这些字段进行反序排序然后再调用 <see cref="First"/> 方法来实现，否则将直接执行当前对象所代表的 Sql 语句，然后再返回结果集中的最后一行）。
        /// </summary>
        /// <returns>若当前对象数据源中有数据时则返回数据源中最后一条成员数据，否则抛出异常。</returns>
        public TSelect Last()
        {
            Queryable<TSource, TSelect> queryable = (Queryable<TSource, TSelect>)GetLastQueryable();

            if (queryable.Count < 1)
                throw new ArgumentOutOfRangeException($"请在调用 {nameof(Last)} 方法前确保当前可查询对象数据源中有至少一条成员数据信息");

            return queryable[Count - 1];
        }

        /// <summary>
        /// 获取当前对象数据源中的最后一条成员信息（若当前对象中包含有排序字段时，则会将这些字段进行反序排序然后再调用 <see cref="First"/> 方法来实现，否则将直接执行当前对象所代表的 Sql 语句，然后再返回结果集中的最后一行）。
        /// </summary>
        /// <returns>若当前对象数据源中有数据时则返回数据源中最后一条成员数据，否则返回 <typeparamref name="TSelect"/> 类型的默认值。</returns>
        public TSelect LastOrDefault()
        {
            Queryable<TSource, TSelect> queryable = (Queryable<TSource, TSelect>)GetLastQueryable();

            if (queryable.Count < 1)
                return default;
            else
                return queryable[Count - 1];
        }

        /// <summary>
        /// 获取当前查询对象获取最后一行成员的查询对象。
        /// </summary>
        /// <returns>对应当前查询对象用于获取最后一个成员的查询对象。</returns>
        internal override Queryable GetLastQueryable()
        {
            if (DataSource.SortItems != null && DataSource.SortItems.Count > 0)
            {
                SortItem[] sortItems = new SortItem[DataSource.SortItems.Count];

                for (int i = 0; i < DataSource.SortItems.Count; i++)
                {
                    var item = DataSource.SortItems[i];

                    sortItems[i] = new SortItem(item.Field, item.Type == SortType.Ascending ? SortType.Descending : SortType.Ascending);
                }

                return new Queryable<TSource, TSelect>(DataContext, DataSource.ShallowCopy().SetSortItems(sortItems)).Top(1);
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// 获取该对象对应的 Sql 查询语句。
        /// </summary>
        /// <param name="parameterContext">生成查询 Sql 时如需用到参数化操作时的上下文对象。</param>
        /// <returns>Sql 查询语句。</returns>
        public override string GetSql(IParameterContext parameterContext)
        {
            if (_QuerySql == null)
            {
                _ParameterContext = parameterContext;

                _QuerySql = DataContext.SqlFactory.GenerateSelectSql(parameterContext, DataSource);
            }

            return _QuerySql;
        }

        /// <summary>
        /// 获取该对象字段的调试显示结果。
        /// </summary>
        /// <returns>该对象对应的 Sql 查询语句。</returns>
        private string GetDebugResult()
        {
            return DataContext.SqlFactory.GenerateSelectSql(DataContext.CreateParameterContext(false), DataSource);
        }

        /// <summary>
        /// 获取该查询对象所能查询到的数据条数（内部使用 select count 实现）。
        /// </summary>
        /// <returns>用 select count 查询到的数据条数。</returns>
        public int SelectCount()
        {
            return Select(item => DBFun.Count()).FirstOrDefault();
        }

        /// <summary>
        /// 将当前可查询对象中的所有数据转换成一个 List 集合。
        /// </summary>
        /// <returns>转换后的集合对象。</returns>
        public List<TSelect> ToList()
        {
            InitResult();

            return new List<TSelect>(_Result);
        }

        /// <summary>
        /// 将当前可查询对象中的所有数据转换成一个数组对象。
        /// </summary>
        /// <returns>转换后的数组对象。</returns>
        public TSelect[] ToArray()
        {
            InitResult();

            return _Result.ToArray();
        }

        /// <summary>
        /// 以当前对象为蓝本复制出一个新的可查询对象。
        /// </summary>
        /// <param name="copiedDataSources">已经复制过的数据源集合。</param>
        /// <param name="copiedFields">已经复制好的字段信息。</param>
        /// <param name="startAliasIndex">复制后新数据源的起始表别名下标。</param>
        /// <returns>复制后新的可查询操作对象。</returns>
        internal override Queryable Copy(Dictionary<DataSource.DataSource, DataSource.DataSource> copiedDataSources, Dictionary<Field.Field, Field.Field> copiedFields, ref int startAliasIndex)
        {
            return new Queryable<TSource, TSelect>(DataContext, (BasicDataSource)DataSource.Copy(copiedDataSources, copiedFields, ref startAliasIndex));
        }

        /// <summary>
        /// 初始化当前对象的查询结果数据。
        /// </summary>
        private void InitResult()
        {
            if (_Result == null)
            {
                _Result = new List<TSelect>();

                _ParameterContext = _ParameterContext ?? DataContext.CreateParameterContext(true);

                Field.Field queryField = DataSource.SelectField ?? DataSource.RootField;

                DataContext.DatabaseOperation.ExecuteReader(GetSql(_ParameterContext), System.Data.CommandType.Text, _ParameterContext.ToList(), dr =>
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

                if (objectField.WhetherNecessaryInit && objectField.Members != null && objectField.Members.Count > 0)
                {
                    foreach (var item in objectField.Members)
                    {
                        if (item.Value.Member.MemberType == System.Reflection.MemberTypes.Property)
                        {
                            System.Reflection.PropertyInfo propertyInfo = (System.Reflection.PropertyInfo)item.Value.Member;

                            propertyInfo.SetValue(result, GetFieldValue(item.Value.Field, dr), null);
                        }
                        else if (item.Value.Member.MemberType == System.Reflection.MemberTypes.Field)
                        {
                            System.Reflection.FieldInfo fieldInfo = (System.Reflection.FieldInfo)item.Value.Member;

                            fieldInfo.SetValue(result, GetFieldValue(item.Value.Field, dr));
                        }
                    }
                }

                return result;
            }
            else if (field.Type == FieldType.Collection)
            {
                CollectionField collectionField = (CollectionField)field;

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
                    object[] constructorParameters = collectionField.ConstructorInfo.Parameters != null && collectionField.ConstructorInfo.Parameters.Count > 0 ? new object[collectionField.ConstructorInfo.Parameters.Count] : null;

                    if (constructorParameters != null)
                    {
                        for (int i = 0; i < constructorParameters.Length; i++)
                        {
                            constructorParameters[i] = GetFieldValue(collectionField.ConstructorInfo.Parameters[i], dr);
                        }
                    }

                    object list = collectionField.ConstructorInfo.Constructor.Invoke(constructorParameters);

                    foreach (var item in collectionField)
                    {
                        collectionField.AddMethodInfo.Invoke(list, new object[] { GetFieldValue(item, dr) });
                    }

                    return list;
                }
            }
            else
            {
                object result = dr[((BasicField)field).Alias];

                if (result == null || result == DBNull.Value)
                {
                    if (field.Type == FieldType.DefaultOrValue)
                        return Activator.CreateInstance(field.DataType);
                    else
                        return null;
                }
                else
                {
                    return ToTargetType(field.DataType, result);
                }
            }
        }

        /// <summary>
        /// 将指定值转换成目标类型的值。
        /// </summary>
        /// <param name="targetType">目标值类型。</param>
        /// <param name="value">待转换的值。</param>
        /// <returns>若转换成功则返回转换后的值，否则抛出异常。</returns>
        private object ToTargetType(Type targetType, object value)
        {
            Type valueType = value?.GetType();

            if (valueType != null && valueType == targetType)
                return value;

            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == _NullableType)
            {
                if (value == null)
                    return null;
                else
                    return ToTargetType(targetType.GetGenericArguments()[0], value);
            }

            if (targetType.IsEnum)
            {
                if (value == null)
                    throw new Exception($"无法将 null 值转换成 {targetType.FullName} 类型的枚举值");
                else if (value is int intValue)
                    return System.Enum.ToObject(targetType, intValue);
                else if (value is uint uintValue)
                    return System.Enum.ToObject(targetType, uintValue);
                else if (value is long longValue)
                    return System.Enum.ToObject(targetType, longValue);
                else if (value is ulong ulongValue)
                    return System.Enum.ToObject(targetType, ulongValue);
                else if (value is short shortValue)
                    return System.Enum.ToObject(targetType, shortValue);
                else if (value is ushort ushortValue)
                    return System.Enum.ToObject(targetType, ushortValue);
                else if (value is byte byteValue)
                    return System.Enum.ToObject(targetType, byteValue);
                else if (value is sbyte sbyteValue)
                    return System.Enum.ToObject(targetType, sbyteValue);
                else if (value is string stringValue)
                    return System.Enum.Parse(targetType, stringValue);
                else
                    throw new Exception($"无法将 {value.GetType().FullName} 类型的值（{value}）转换成 {targetType.FullName} 类型的枚举值");
            }
            else if (targetType.FullName == "System.Int32")
            {
                if (value == null)
                    throw new Exception($"无法将 null 值转换成 {targetType.FullName} 类型的值");
                else if (value is uint uintValue)
                    return (int)uintValue;
                else if (value is long longValue)
                    return (int)longValue;
                else if (value is ulong ulongValue)
                    return (int)ulongValue;
                else if (value is short shortValue)
                    return (int)shortValue;
                else if (value is ushort ushortValue)
                    return (int)ushortValue;
                else if (value is byte byteValue)
                    return (int)byteValue;
                else if (value is sbyte sbyteValue)
                    return (int)sbyteValue;
                else if (value is string stringValue)
                    return int.Parse(stringValue);
                else
                    throw new Exception($"无法将 {valueType.FullName} 类型的值（{value}）转换成 {targetType.FullName} 类型的值");
            }
            else if (targetType.FullName == "System.UInt32")
            {
                if (value == null)
                    throw new Exception($"无法将 null 值转换成 {targetType.FullName} 类型的值");
                else if (value is int intValue)
                    return (uint)intValue;
                else if (value is long longValue)
                    return (uint)longValue;
                else if (value is ulong ulongValue)
                    return (uint)ulongValue;
                else if (value is short shortValue)
                    return (uint)shortValue;
                else if (value is ushort ushortValue)
                    return (uint)ushortValue;
                else if (value is byte byteValue)
                    return (uint)byteValue;
                else if (value is sbyte sbyteValue)
                    return (uint)sbyteValue;
                else if (value is string stringValue)
                    return uint.Parse(stringValue);
                else
                    throw new Exception($"无法将 {valueType.FullName} 类型的值（{value}）转换成 {targetType.FullName} 类型的值");
            }
            else if (targetType.FullName == "System.Int64")
            {
                if (value == null)
                    throw new Exception($"无法将 null 值转换成 {targetType.FullName} 类型的值");
                else if (value is int intValue)
                    return (long)intValue;
                else if (value is uint uintValue)
                    return (long)uintValue;
                else if (value is ulong ulongValue)
                    return (long)ulongValue;
                else if (value is short shortValue)
                    return (long)shortValue;
                else if (value is ushort ushortValue)
                    return (long)ushortValue;
                else if (value is byte byteValue)
                    return (long)byteValue;
                else if (value is sbyte sbyteValue)
                    return (long)sbyteValue;
                else if (value is string stringValue)
                    return long.Parse(stringValue);
                else
                    throw new Exception($"无法将 {valueType.FullName} 类型的值（{value}）转换成 {targetType.FullName} 类型的值");
            }
            else if (targetType.FullName == "System.UInt64")
            {
                if (value == null)
                    throw new Exception($"无法将 null 值转换成 {targetType.FullName} 类型的值");
                else if (value is int intValue)
                    return (ulong)intValue;
                else if (value is uint uintValue)
                    return (ulong)uintValue;
                else if (value is long longValue)
                    return (ulong)longValue;
                else if (value is short shortValue)
                    return (ulong)shortValue;
                else if (value is ushort ushortValue)
                    return (ulong)ushortValue;
                else if (value is byte byteValue)
                    return (ulong)byteValue;
                else if (value is sbyte sbyteValue)
                    return (ulong)sbyteValue;
                else if (value is string stringValue)
                    return ulong.Parse(stringValue);
                else
                    throw new Exception($"无法将 {valueType.FullName} 类型的值（{value}）转换成 {targetType.FullName} 类型的值");
            }
            else if (targetType.FullName == "System.Int16")
            {
                if (value == null)
                    throw new Exception($"无法将 null 值转换成 {targetType.FullName} 类型的值");
                else if (value is int intValue)
                    return (short)intValue;
                else if (value is uint uintValue)
                    return (short)uintValue;
                else if (value is long longValue)
                    return (short)longValue;
                else if (value is ulong ulongValue)
                    return (short)ulongValue;
                else if (value is ushort ushortValue)
                    return (short)ushortValue;
                else if (value is byte byteValue)
                    return (short)byteValue;
                else if (value is sbyte sbyteValue)
                    return (short)sbyteValue;
                else if (value is string stringValue)
                    return short.Parse(stringValue);
                else
                    throw new Exception($"无法将 {valueType.FullName} 类型的值（{value}）转换成 {targetType.FullName} 类型的值");
            }
            else if (targetType.FullName == "System.UInt16")
            {
                if (value == null)
                    throw new Exception($"无法将 null 值转换成 {targetType.FullName} 类型的值");
                else if (value is int intValue)
                    return (ushort)intValue;
                else if (value is uint uintValue)
                    return (ushort)uintValue;
                else if (value is long longValue)
                    return (ushort)longValue;
                else if (value is ulong ulongValue)
                    return (ushort)ulongValue;
                else if (value is short shortValue)
                    return (ushort)shortValue;
                else if (value is byte byteValue)
                    return (ushort)byteValue;
                else if (value is sbyte sbyteValue)
                    return (ushort)sbyteValue;
                else if (value is string stringValue)
                    return ushort.Parse(stringValue);
                else
                    throw new Exception($"无法将 {valueType.FullName} 类型的值（{value}）转换成 {targetType.FullName} 类型的值");
            }
            else if (targetType.FullName == "System.Byte")
            {
                if (value == null)
                    throw new Exception($"无法将 null 值转换成 {targetType.FullName} 类型的值");
                else if (value is int intValue)
                    return (byte)intValue;
                else if (value is uint uintValue)
                    return (byte)uintValue;
                else if (value is long longValue)
                    return (byte)longValue;
                else if (value is ulong ulongValue)
                    return (byte)ulongValue;
                else if (value is short shortValue)
                    return (byte)shortValue;
                else if (value is ushort ushortValue)
                    return (byte)ushortValue;
                else if (value is sbyte sbyteValue)
                    return (byte)sbyteValue;
                else if (value is string stringValue)
                    return byte.Parse(stringValue);
                else
                    throw new Exception($"无法将 {valueType.FullName} 类型的值（{value}）转换成 {targetType.FullName} 类型的值");
            }
            else if (targetType.FullName == "System.SByte")
            {
                if (value == null)
                    throw new Exception($"无法将 null 值转换成 {targetType.FullName} 类型的值");
                else if (value is int intValue)
                    return (sbyte)intValue;
                else if (value is uint uintValue)
                    return (sbyte)uintValue;
                else if (value is long longValue)
                    return (sbyte)longValue;
                else if (value is ulong ulongValue)
                    return (sbyte)ulongValue;
                else if (value is short shortValue)
                    return (sbyte)shortValue;
                else if (value is ushort ushortValue)
                    return (sbyte)ushortValue;
                else if (value is byte byteValue)
                    return (sbyte)byteValue;
                else if (value is string stringValue)
                    return sbyte.Parse(stringValue);
                else
                    throw new Exception($"无法将 {valueType.FullName} 类型的值（{value}）转换成 {targetType.FullName} 类型的值");
            }
            else if (targetType.FullName == "System.Single")
            {
                if (value == null)
                    throw new Exception($"无法将 null 值转换成 {targetType.FullName} 类型的值");
                else if (value is int intValue)
                    return (float)intValue;
                else if (value is uint uintValue)
                    return (float)uintValue;
                else if (value is long longValue)
                    return (float)longValue;
                else if (value is ulong ulongValue)
                    return (float)ulongValue;
                else if (value is short shortValue)
                    return (float)shortValue;
                else if (value is ushort ushortValue)
                    return (float)ushortValue;
                else if (value is byte byteValue)
                    return (float)byteValue;
                else if (value is sbyte sbyteValue)
                    return (float)sbyteValue;
                else if (value is double doubleValue)
                    return (float)doubleValue;
                else if (value is decimal decimalValue)
                    return (float)decimalValue;
                else if (value is string stringValue)
                    return float.Parse(stringValue);
                else
                    throw new Exception($"无法将 {valueType.FullName} 类型的值（{value}）转换成 {targetType.FullName} 类型的值");
            }
            else if (targetType.FullName == "System.Double")
            {
                if (value == null)
                    throw new Exception($"无法将 null 值转换成 {targetType.FullName} 类型的值");
                else if (value is int intValue)
                    return (double)intValue;
                else if (value is uint uintValue)
                    return (double)uintValue;
                else if (value is long longValue)
                    return (double)longValue;
                else if (value is ulong ulongValue)
                    return (double)ulongValue;
                else if (value is short shortValue)
                    return (double)shortValue;
                else if (value is ushort ushortValue)
                    return (double)ushortValue;
                else if (value is byte byteValue)
                    return (double)byteValue;
                else if (value is sbyte sbyteValue)
                    return (double)sbyteValue;
                else if (value is float floatValue)
                    return (double)floatValue;
                else if (value is decimal decimalValue)
                    return (double)decimalValue;
                else if (value is string stringValue)
                    return double.Parse(stringValue);
                else
                    throw new Exception($"无法将 {valueType.FullName} 类型的值（{value}）转换成 {targetType.FullName} 类型的值");
            }
            else if (targetType.FullName == "System.Decimal")
            {
                if (value == null)
                    throw new Exception($"无法将 null 值转换成 {targetType.FullName} 类型的值");
                else if (value is int intValue)
                    return (decimal)intValue;
                else if (value is uint uintValue)
                    return (decimal)uintValue;
                else if (value is long longValue)
                    return (decimal)longValue;
                else if (value is ulong ulongValue)
                    return (decimal)ulongValue;
                else if (value is short shortValue)
                    return (decimal)shortValue;
                else if (value is ushort ushortValue)
                    return (decimal)ushortValue;
                else if (value is byte byteValue)
                    return (decimal)byteValue;
                else if (value is sbyte sbyteValue)
                    return (decimal)sbyteValue;
                else if (value is float floatValue)
                    return (decimal)floatValue;
                else if (value is double doubleValue)
                    return (decimal)doubleValue;
                else if (value is string stringValue)
                    return decimal.Parse(stringValue);
                else
                    throw new Exception($"无法将 {valueType.FullName} 类型的值（{value}）转换成 {targetType.FullName} 类型的值");
            }
            else if (targetType.FullName == "System.DateTime")
            {
                if (value == null)
                    throw new Exception($"无法将 null 值转换成 {targetType.FullName} 类型的值");
                else if (value is string stringValue)
                    return DateTime.Parse(stringValue);
                else
                    throw new Exception($"无法将 {valueType.FullName} 类型的值（{value}）转换成 {targetType.FullName} 类型的值");
            }
            else if (targetType.FullName == "System.Boolean")
            {
                if (value == null)
                {
                    throw new Exception($"无法将 null 值转换成 {targetType.FullName} 类型的值");
                }
                else if (value is int intValue)
                {
                    if (intValue == 0)
                        return false;
                    else if (intValue == 1)
                        return true;
                }
                else if (value is uint uintValue)
                {
                    if (uintValue == 0)
                        return false;
                    else if (uintValue == 1)
                        return true;
                }
                else if (value is long longValue)
                {
                    if (longValue == 0)
                        return false;
                    else if (longValue == 1)
                        return true;
                }
                else if (value is ulong ulongValue)
                {
                    if (ulongValue == 0)
                        return false;
                    else if (ulongValue == 1)
                        return true;
                }
                else if (value is short shortValue)
                {
                    if (shortValue == 0)
                        return false;
                    else if (shortValue == 1)
                        return true;
                }
                else if (value is ushort ushortValue)
                {
                    if (ushortValue == 0)
                        return false;
                    else if (ushortValue == 1)
                        return true;
                }
                else if (value is byte byteValue)
                {
                    if (byteValue == 0)
                        return false;
                    else if (byteValue == 1)
                        return true;
                }
                else if (value is sbyte sbyteValue)
                {
                    if (sbyteValue == 0)
                        return false;
                    else if (sbyteValue == 1)
                        return true;
                }
                else if (value is string stringValue)
                {
                    stringValue = stringValue?.Trim().ToLower();

                    if (stringValue == "0" || stringValue == "false")
                        return false;
                    else if (stringValue == "1" || stringValue == "true")
                        return true;
                }
                else
                {
                    throw new Exception($"无法将 {valueType.FullName} 类型的值（{value}）转换成 {targetType.FullName} 类型的值");
                }
            }
            else if (targetType.FullName == "System.Guid")
            {
                if (value == null)
                    throw new Exception($"无法将 null 值转换成 {targetType.FullName} 类型的值");
                else if (value is string stringValue && Guid.TryParse(stringValue.Trim(), out Guid guidValue))
                    return guidValue;
                else
                    throw new Exception($"无法将 {valueType.FullName} 类型的值（{value}）转换成 {targetType.FullName} 类型的值");
            }
            else if (targetType.FullName == "System.String")
            {
                return value?.ToString();
            }

            throw new Exception($"无法将 {valueType.FullName} 类型的值（{value}）转换成 {targetType.FullName} 类型的值");
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
    }
}
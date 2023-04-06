using GfdbFramework.Attribute;
using GfdbFramework.DataSource;
using GfdbFramework.Enum;
using GfdbFramework.Field;
using GfdbFramework.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 框架静态帮助类。
    /// </summary>
    public static class Helper
    {
        private static readonly Type _NullableType = typeof(int?).GetGenericTypeDefinition();
        private static readonly Type _QueryableType = typeof(Queryable);
        private static readonly Type _ExtendType = typeof(Extend);
        private static readonly Type _ModifiableType = typeof(Modifiable<FieldAttribute, FieldAttribute>).GetGenericTypeDefinition();
        private static readonly Type _ModifiableMultipleJoinType = typeof(ModifiableMultipleJoin<int, int, int, int, int>).GetGenericTypeDefinition();
        private static readonly Type _LambdaExpressionType = typeof(LambdaExpression);
        private static readonly Type _DBFunType = typeof(DBFun);
        private static readonly Type _GuidType = typeof(Guid);
        private static readonly Type _StringType = typeof(string);
        private static readonly Type _Int32Type = typeof(int);
        private static readonly Type _DecimalType = typeof(decimal);
        private static readonly Type _TimeSpanType = typeof(TimeSpan);
        private static readonly Type _DateTimeOffsetType = typeof(DateTimeOffset);
        private static readonly Type _DateTimeType = typeof(DateTime);
        private static readonly Type _MultipleJoinType = typeof(MultipleJoin);
        private static readonly Type _IListType = typeof(IList<int>).GetGenericTypeDefinition();
        private static readonly Type _IEnumerableType = typeof(System.Collections.IEnumerable);
        private static readonly Type _ICollectionType = typeof(ICollection<int>).GetGenericTypeDefinition();
        private static readonly Type _SourceTypeType = typeof(SourceType);
        private static readonly Type _ExistentParametersType = typeof(ReadOnlyDictionary<string, DataSource.DataSource>);
        private static readonly string _QueryableCountPropName = nameof(Queryable<int, int>.Count);
        private static readonly string _JointItemLeftPropName = nameof(JoinItem<int, int>.Left);
        private static readonly string _JointItemRightPropName = nameof(JoinItem<int, int>.Right);
        private static readonly string _NullableValuePropName = nameof(Nullable<int>.Value);
        private static readonly string _ArrayLengthPropName = nameof(Array.Length);
        private static readonly string _ListCountPropName = nameof(List<int>.Count);
        private static readonly string _DBFunAddMillisecondMethodName = nameof(DBFun.AddMillisecond);
        private static readonly string _QueryableLimitMethodName = nameof(Queryable.Limit);
        private static readonly string _QueryableTopMethodName = nameof(Queryable<int, int>.Top);
        private static readonly string _QueryableSelectCountMethodName = nameof(Queryable<int, int>.SelectCount);
        private static readonly string _QueryableDistinctMethodName = nameof(Queryable<int, int>.Distinct);
        private static readonly string _QueryableSelectMethodName = nameof(Queryable.Select);
        private static readonly string _QueryableWhereMethodName = nameof(Queryable.Where);
        private static readonly string _QueryableAscendingMethodName = nameof(Queryable<int, int>.Ascending);
        private static readonly string _QueryableDescendingMethodName = nameof(Queryable<int, int>.Descending);
        private static readonly string _QueryableGroupMethodName = nameof(Queryable<int, int>.Group);
        private static readonly string _QueryableJoinMethodName = nameof(Queryable<int, int>.Join);
        private static readonly string _QueryableInnerJoinMethodName = nameof(Queryable<int, int>.InnerJoin);
        private static readonly string _QueryableLeftJoinMethodName = nameof(Queryable<int, int>.LeftJoin);
        private static readonly string _QueryableRightJoinMethodName = nameof(Queryable<int, int>.RightJoin);
        private static readonly string _QueryableFullJoinMethodName = nameof(Queryable<int, int>.FullJoin);
        private static readonly string _QueryableCrossJoinMethodName = nameof(Queryable<int, int>.CrossJoin);
        private static readonly string _QueryableFirstMethodName = nameof(Queryable<int, int>.First);
        private static readonly string _QueryableFirstOrDefaultMethodName = nameof(Queryable<int, int>.FirstOrDefault);
        private static readonly string _QueryableLastMethodName = nameof(Queryable<int, int>.Last);
        private static readonly string _QueryableLastOrDefaultMethodName = nameof(Queryable<int, int>.LastOrDefault);
        private static readonly string _QueryableContainsMethodName = nameof(Queryable<int, int>.Contains);
        private static readonly string _QueryableUnionAllMethodName = nameof(Queryable<int, int>.UnionAll);
        private static readonly string _QueryableUnionMethodName = nameof(Queryable<int, int>.Union);
        private static readonly string _QueryableIntersectMethodName = nameof(Queryable<int, int>.Intersect);
        private static readonly string _QueryableMinusMethodName = nameof(Queryable<int, int>.Minus);
        private static readonly string _MultipleJoinJoinMethodName = nameof(MultipleJoin<int, int, int, int>.Join);
        private static readonly string _MultipleJoinInnerJoinMethodName = nameof(MultipleJoin<int, int, int, int>.InnerJoin);
        private static readonly string _MultipleJoinLeftJoinMethodName = nameof(MultipleJoin<int, int, int, int>.LeftJoin);
        private static readonly string _MultipleJoinRightJoinMethodName = nameof(MultipleJoin<int, int, int, int>.RightJoin);
        private static readonly string _MultipleJoinFullJoinMethodName = nameof(MultipleJoin<int, int, int, int>.FullJoin);
        private static readonly string _MultipleJoinCrossJoinMethodName = nameof(MultipleJoin<int, int, int, int>.CrossJoin);
        private static readonly string _MultipleJoinSelectMethodName = nameof(MultipleJoin<int, int, int, int>.Select);
        private static readonly string _ICollectionContainsMethodName = nameof(ICollection<int>.Contains);
        private static readonly string _ExtendToNullMethodName = nameof(Extend.ToNull);
        private static readonly string _ExtendLikeMethodName = nameof(Extend.Like);
        private static readonly string _ExtendContainsMethodName = nameof(Extend.Contains);
        private static MethodInfo _DBFunCountMethod = null;
        private static MethodInfo _DBFunAddMillisecondMethod = null;
        private static MethodInfo _QueryableDistinctMethod = null;
        private static MethodInfo _QueryableSelectMethod = null;
        private static MethodInfo _QueryableJoinMethod1 = null;
        private static MethodInfo _QueryableJoinMethod2 = null;
        private static MethodInfo _MultipleJoinJoinMethod = null;
        private static MethodInfo _MultipleJoinSelectMethod = null;

        /// <summary>
        /// 从指定表达式树中提取对应的字段信息。
        /// </summary>
        /// <param name="dataContext">提取字段时所使用的数据操作上下文。</param>
        /// <param name="body">待提取字段的表达式树。</param>
        /// <param name="extractWay">字段提取方式。</param>
        /// <param name="parameters">执行该表达式时所传递的参数集合。</param>
        /// <param name="startDataSourceAliasIndex">提取到子查询字段需要重置数据源别名时的起始数据源下标。</param>
        /// <returns>提取到的字段信息。</returns>
        internal static Field.Field ExtractField(IDataContext dataContext, Expression body, ExtractWay extractWay, ReadOnlyDictionary<string, DataSource.DataSource> parameters, ref int startDataSourceAliasIndex)
        {
            return ExtractField(dataContext, body, extractWay, parameters, new Dictionary<Field.Field, Field.Field>(), ref startDataSourceAliasIndex);
        }

        /// <summary>
        /// 从指定表达式树中提取对应的字段信息。
        /// </summary>
        /// <param name="dataContext">提取字段时所使用的数据操作上下文。</param>
        /// <param name="body">待提取字段的表达式树。</param>
        /// <param name="extractWay">字段提取方式。</param>
        /// <param name="parameters">执行该表达式时所传递的参数集合。</param>
        /// <param name="convertedFields">已经转换过的参数字段集合。</param>
        /// <param name="startDataSourceAliasIndex">提取到子查询字段需要重置数据源别名时的起始数据源下标。</param>
        /// <returns>提取到的字段信息。</returns>
        internal static Field.Field ExtractField(IDataContext dataContext, Expression body, ExtractWay extractWay, ReadOnlyDictionary<string, DataSource.DataSource> parameters, Dictionary<Field.Field, Field.Field> convertedFields, ref int startDataSourceAliasIndex)
        {
            OperationType operationType = OperationType.Default;
            Field.Field resultField = null;

            switch (body.NodeType)
            {
                #region 二元操作字段提取
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Divide:
                case ExpressionType.Coalesce:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.LeftShift:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.Modulo:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.Power:
                case ExpressionType.RightShift:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.ArrayIndex:
                    BinaryExpression binaryExpression = (BinaryExpression)body;

                    Field.Field left = ExtractField(dataContext, binaryExpression.Left, extractWay, parameters, convertedFields, ref startDataSourceAliasIndex);
                    Field.Field right = ExtractField(dataContext, binaryExpression.Right, extractWay, parameters, convertedFields, ref startDataSourceAliasIndex);

                    if (body.NodeType == ExpressionType.ArrayIndex)
                    {
                        if (right.Type != FieldType.Constant || !(((ConstantField)right).Value is int || ((ConstantField)right).Value is long))
                            throw new Exception("对于从数组集合中引用某个字段时，所使用的索引值必须是运行时常量，而不能是 Sql 中的字段信息");

                        int index = int.Parse(((ConstantField)right).Value.ToString());

                        if (left.Type == FieldType.Constant && left.DataType.IsArray)
                            resultField = new ConstantField(dataContext, body.Type, ((Array)((ConstantField)left).Value).GetValue(index));
                        else if (left.Type == FieldType.Collection)
                            resultField = ((CollectionField)left)[index];
                        else
                            throw new Exception("读取某个数组索引处的字段信息时出现错误，被读取的字段并非集合字段类型");
                    }
                    else
                    {
                        switch (body.NodeType)
                        {
                            case ExpressionType.Add:
                            case ExpressionType.AddChecked:
                                operationType = OperationType.Add;
                                break;
                            case ExpressionType.And:
                                operationType = OperationType.And;
                                break;
                            case ExpressionType.AndAlso:
                                operationType = OperationType.AndAlso;
                                break;
                            case ExpressionType.Divide:
                                operationType = OperationType.Divide;
                                break;
                            case ExpressionType.Coalesce:
                                operationType = OperationType.Coalesce;
                                break;
                            case ExpressionType.ExclusiveOr:
                                operationType = OperationType.ExclusiveOr;
                                break;
                            case ExpressionType.GreaterThan:
                                operationType = OperationType.GreaterThan;
                                break;
                            case ExpressionType.GreaterThanOrEqual:
                                operationType = OperationType.GreaterThanOrEqual;
                                break;
                            case ExpressionType.Equal:
                                operationType = OperationType.Equal;
                                break;
                            case ExpressionType.NotEqual:
                                operationType = OperationType.NotEqual;
                                break;
                            case ExpressionType.LeftShift:
                                operationType = OperationType.LeftShift;
                                break;
                            case ExpressionType.LessThan:
                                operationType = OperationType.LessThan;
                                break;
                            case ExpressionType.LessThanOrEqual:
                                operationType = OperationType.LessThanOrEqual;
                                break;
                            case ExpressionType.Modulo:
                                operationType = OperationType.Modulo;
                                break;
                            case ExpressionType.Multiply:
                            case ExpressionType.MultiplyChecked:
                                operationType = OperationType.Multiply;
                                break;
                            case ExpressionType.Or:
                                operationType = OperationType.Or;
                                break;
                            case ExpressionType.OrElse:
                                operationType = OperationType.OrElse;
                                break;
                            case ExpressionType.Power:
                                operationType = OperationType.Power;
                                break;
                            case ExpressionType.RightShift:
                                operationType = OperationType.RightShift;
                                break;
                            case ExpressionType.Subtract:
                            case ExpressionType.SubtractChecked:
                                operationType = OperationType.Subtract;
                                break;
                        }

                        resultField = new BinaryField(dataContext, body.Type, operationType, (BasicField)left, (BasicField)right);
                    }
                    break;
                #endregion
                #region 方法字段提取
                case ExpressionType.Call:
                    MethodCallExpression methodCallExpression = (MethodCallExpression)body;
                    Field.Field methodExampleField = methodCallExpression.Object == null ? null : ExtractField(dataContext, methodCallExpression.Object, extractWay, parameters, convertedFields, ref startDataSourceAliasIndex);
                    Field.Field[] methodParameters = null;
                    bool isExistFieldParameter = false;     //是否存在非常量字段类型的参数信息

                    if (methodCallExpression.Arguments != null && methodCallExpression.Arguments.Count > 0)
                    {
                        methodParameters = new Field.Field[methodCallExpression.Arguments.Count];

                        for (int i = 0; i < methodCallExpression.Arguments.Count; i++)
                        {
                            var parameter = ExtractField(dataContext, methodCallExpression.Arguments[i], extractWay, parameters, convertedFields, ref startDataSourceAliasIndex);

                            if (parameter.Type != FieldType.Constant)
                                isExistFieldParameter = true;

                            methodParameters[i] = parameter;
                        }
                    }

                    //若调用的是 DBFun 类的函数，直接返回 MethodField 字段交友接口实现者处理
                    if (methodCallExpression.Method.ReflectedType == _DBFunType)
                    {
                        resultField = new MethodField(dataContext, null, methodParameters, methodCallExpression.Method);
                    }
                    //若调用实例对象不为 null
                    else if (methodExampleField != null)
                    {
                        //若调用之前的对象类型是 Queryable 类型，则需要对调用方法进行二次处理
                        if (methodCallExpression.Method.ReflectedType.IsSubclassOf(_QueryableType))
                        {
                            if (methodExampleField.Type != FieldType.Constant || !methodExampleField.DataType.IsSubclassOf(_QueryableType))
                                throw new Exception($"提取子查询字段信息时发现调用实例对象字段不为 ConstantField 或常量值类型不为 Queryable 类型，具体表达式为：{body}");

                            Queryable queryable = (Queryable)((ConstantField)methodExampleField).Value;

                            //若调用后返回值也是 Queryable 类型，则直接调用相关方法得到返回值后包装成 ConstantField 字段返回
                            if (body.Type.IsSubclassOf(_QueryableType))
                            {
                                //Limit 方法
                                if (methodCallExpression.Method.Name == _QueryableLimitMethodName && methodCallExpression.Arguments != null && methodCallExpression.Arguments.Count == 2 && methodCallExpression.Arguments[0].Type == _Int32Type && methodCallExpression.Arguments[1].Type == _Int32Type)
                                {
                                    if (methodParameters == null || isExistFieldParameter || methodParameters.Length != 2 || !(((ConstantField)methodParameters[0]).Value is int startIndex) || !(((ConstantField)methodParameters[1]).Value is int count))
                                        throw new Exception($"对于子查询调用 {_QueryableLimitMethodName} 方法而言，其使用的方法参数必须是运行时常量，即不允许使用字段作为参数");

                                    resultField = new ConstantField(dataContext, body.Type, queryable.Limit(new Limit(startIndex, count)));
                                }
                                //Top 方法
                                else if (methodCallExpression.Method.Name == _QueryableTopMethodName && methodCallExpression.Arguments != null && methodCallExpression.Arguments.Count == 1 && methodCallExpression.Arguments[0].Type == _Int32Type)
                                {
                                    if (methodParameters == null || isExistFieldParameter || methodParameters.Length != 1 || !(((ConstantField)methodParameters[0]).Value is int count))
                                        throw new Exception($"对于子查询调用 {_QueryableTopMethodName} 方法而言，其使用的方法参数必须是运行时常量，即不允许使用字段作为参数");

                                    resultField = new ConstantField(dataContext, body.Type, queryable.Limit(new Limit(count)));
                                }
                                //Distinct 方法
                                else if (methodCallExpression.Method.Name == _QueryableDistinctMethodName && methodParameters == null)
                                {
                                    if (_QueryableDistinctMethod == null)
                                    {
                                        _QueryableDistinctMethod = queryable.GetType().GetMethod(_QueryableDistinctMethodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, Type.DefaultBinder, new Type[0], null);

                                        if (_QueryableDistinctMethod == null)
                                            throw new Exception("获取 Queryable 类型的 Distinct 方法信息出错");
                                    }

                                    resultField = new ConstantField(dataContext, body.Type, _QueryableDistinctMethod.Invoke(queryable, null));
                                }
                                else if (methodParameters != null && !isExistFieldParameter)
                                {
                                    if (methodParameters.Length == 1 && methodCallExpression.Arguments[0].Type.IsSubclassOf(_LambdaExpressionType))
                                    {
                                        LambdaExpression lambdaExpression = (LambdaExpression)((ConstantField)methodParameters[0]).Value;

                                        //Select 方法
                                        if (methodCallExpression.Method.Name == _QueryableSelectMethodName && methodCallExpression.Method.IsGenericMethod)
                                        {
                                            //获取 Queryable 类型的 Select 方法
                                            if (_QueryableSelectMethod == null)
                                            {
                                                _QueryableSelectMethod = _QueryableType.GetMethod(_QueryableSelectMethodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, Type.DefaultBinder, new Type[] { _LambdaExpressionType, _ExistentParametersType }, null);

                                                if (_QueryableSelectMethod == null)
                                                    throw new Exception($"获取 Queryable 类型的 {_QueryableSelectMethodName} 方法信息出错");
                                            }

                                            //修改 Select 泛型方法的类型为待执行 Select 方法的泛型
                                            MethodInfo methodInfo = _QueryableSelectMethod.MakeGenericMethod(methodCallExpression.Method.GetGenericArguments());

                                            //调用 Select 方法并将得到的 Queryable 对象封装成 ConstantField 字段
                                            resultField = new ConstantField(dataContext, body.Type, methodInfo.Invoke(queryable, new object[] { lambdaExpression, parameters }));
                                        }
                                        //Where 方法
                                        else if (methodCallExpression.Method.Name == _QueryableWhereMethodName)
                                        {
                                            resultField = new ConstantField(dataContext, body.Type, queryable.Where(lambdaExpression, parameters));
                                        }
                                        //Ascending 方法
                                        else if (methodCallExpression.Method.Name == _QueryableAscendingMethodName && methodCallExpression.Method.IsGenericMethod)
                                        {
                                            resultField = new ConstantField(dataContext, body.Type, queryable.Sorting(SortType.Ascending, lambdaExpression, parameters));
                                        }
                                        //Descending 方法
                                        else if (methodCallExpression.Method.Name == _QueryableDescendingMethodName && methodCallExpression.Method.IsGenericMethod)
                                        {
                                            resultField = new ConstantField(dataContext, body.Type, queryable.Sorting(SortType.Descending, lambdaExpression, parameters));
                                        }
                                        //Group 方法
                                        else if (methodCallExpression.Method.Name == _QueryableGroupMethodName && methodCallExpression.Method.IsGenericMethod)
                                        {
                                            resultField = new ConstantField(dataContext, body.Type, queryable.Group(lambdaExpression, parameters));
                                        }
                                    }
                                    //各种 Union 方法
                                    else if (methodParameters.Length == 1 && methodParameters[0].DataType.IsSubclassOf(_QueryableType) && queryable.GetType() == body.Type && methodCallExpression.Arguments[0].Type.IsSubclassOf(_QueryableType) && methodCallExpression.Method.IsGenericMethod && methodCallExpression.Method.GetGenericArguments().Length == 1)
                                    {
                                        UnionType unionType;

                                        if (methodCallExpression.Method.Name == _QueryableUnionAllMethodName)
                                            unionType = UnionType.UnionALL;
                                        else if (methodCallExpression.Method.Name == _QueryableUnionMethodName)
                                            unionType = UnionType.Union;
                                        else if (methodCallExpression.Method.Name == _QueryableIntersectMethodName)
                                            unionType = UnionType.Intersect;
                                        else if (methodCallExpression.Method.Name == _QueryableMinusMethodName)
                                            unionType = UnionType.Minus;
                                        else
                                            throw new Exception($"对 {methodCallExpression.Method.DeclaringType.FullName} 类型中的 {methodCallExpression.Method.ReflectedType.Name} 方法提取字段信息时未能正确提取到它的数据合并类型");

                                        resultField = new ConstantField(dataContext, body.Type, queryable.Union((Queryable)((ConstantField)methodParameters[0]).Value, unionType));
                                    }
                                    //各种关联查询方法
                                    else if (methodCallExpression.Method.IsGenericMethod && methodCallExpression.Method.GetGenericArguments().Length == 3 && methodParameters.Length > 1 && methodParameters.Length < 5 && methodParameters[0].Type == FieldType.Constant && methodCallExpression.Arguments[0].Type.IsSubclassOf(_QueryableType) && methodCallExpression.Arguments[1].Type.IsSubclassOf(_LambdaExpressionType) && (methodParameters.Length == 2 || (methodCallExpression.Arguments[2].Type.IsSubclassOf(_LambdaExpressionType) && (methodParameters.Length == 3 || methodCallExpression.Arguments[3].Type.IsSubclassOf(_LambdaExpressionType)))))
                                    {
                                        //获取 Queryable 类型的 Join 方法
                                        if (_QueryableJoinMethod1 == null)
                                        {
                                            _QueryableJoinMethod1 = _QueryableType.GetMethod(_QueryableJoinMethodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, Type.DefaultBinder, new Type[] { _SourceTypeType, _QueryableType, _LambdaExpressionType, _LambdaExpressionType, _LambdaExpressionType, _ExistentParametersType }, null);

                                            if (_QueryableJoinMethod1 == null)
                                                throw new Exception($"获取 Queryable 类型的 {_QueryableJoinMethodName} 方法信息出错");
                                        }

                                        SourceType joinType = SourceType.InnerJoin;

                                        if (methodCallExpression.Method.Name == _QueryableInnerJoinMethodName)
                                            joinType = SourceType.InnerJoin;
                                        else if (methodCallExpression.Method.Name == _QueryableLeftJoinMethodName)
                                            joinType = SourceType.LeftJoin;
                                        else if (methodCallExpression.Method.Name == _QueryableRightJoinMethodName)
                                            joinType = SourceType.RightJoin;
                                        else if (methodCallExpression.Method.Name == _QueryableFullJoinMethodName)
                                            joinType = SourceType.FullJoin;
                                        else if (methodCallExpression.Method.Name == _QueryableCrossJoinMethodName)
                                            joinType = SourceType.CrossJoin;
                                        else
                                            throw new Exception($"对 {methodCallExpression.Method.DeclaringType.FullName} 类型中的 {methodCallExpression.Method.ReflectedType.Name} 方法提取字段信息时未能正确提取到它的关联查询类型");

                                        Queryable rightQueryable = (Queryable)((ConstantField)methodParameters[0]).Value;
                                        LambdaExpression selectorLambda = (LambdaExpression)((ConstantField)methodParameters[1]).Value;
                                        LambdaExpression onLambda = methodParameters.Length > 2 ? null : (LambdaExpression)((ConstantField)methodParameters[2]).Value;
                                        LambdaExpression whereLambda = methodParameters.Length > 3 ? null : (LambdaExpression)((ConstantField)methodParameters[3]).Value;

                                        //修改 Join 泛型方法的类型为待执行 Join 方法的泛型
                                        MethodInfo methodInfo = _QueryableJoinMethod1.MakeGenericMethod(methodCallExpression.Method.GetGenericArguments()[0]);

                                        //调用 Join 方法并将得到的 Queryable 对象封装成 ConstantField 字段
                                        resultField = new ConstantField(dataContext, body.Type, methodInfo.Invoke(queryable, new object[] { joinType, rightQueryable, selectorLambda, onLambda, whereLambda, parameters }));
                                    }
                                }
                            }
                            //若调用后返回值为 MultipleJoin 类型，则直接调用相关方法得到返回值后包装成 ConstantField 字段返回
                            else if (body.Type.IsSubclassOf(_MultipleJoinType) && methodParameters != null && (methodParameters.Length == 1 || methodParameters.Length == 2) && methodParameters[0].Type == FieldType.Constant && methodCallExpression.Arguments[0].Type.IsSubclassOf(_QueryableType) && (methodParameters.Length == 1 || (methodParameters[1].Type == FieldType.Constant && methodCallExpression.Arguments[1].Type.IsSubclassOf(_LambdaExpressionType))))
                            {
                                //获取 Queryable 类型的 Join 方法
                                if (_QueryableJoinMethod2 == null)
                                {
                                    var joinMethods = _QueryableType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);

                                    if (joinMethods != null && joinMethods.Length > 0)
                                    {
                                        foreach (var item in joinMethods)
                                        {
                                            if (item.Name == _QueryableJoinMethodName && item.IsGenericMethod && item.GetGenericArguments().Length == 2 && item.ReturnType == _MultipleJoinType)
                                            {
                                                var itemParameters = item.GetParameters();

                                                if (itemParameters != null && itemParameters.Length == 4)
                                                {
                                                    _QueryableJoinMethod2 = item;

                                                    break;
                                                }
                                            }
                                        }
                                    }

                                    if (_QueryableJoinMethod2 == null)
                                        throw new Exception($"获取 Queryable 类型的 {_QueryableJoinMethodName} 方法信息出错（多表关联）");
                                }

                                SourceType joinType = SourceType.InnerJoin;

                                if (methodCallExpression.Method.Name == _QueryableInnerJoinMethodName)
                                    joinType = SourceType.InnerJoin;
                                else if (methodCallExpression.Method.Name == _QueryableLeftJoinMethodName)
                                    joinType = SourceType.LeftJoin;
                                else if (methodCallExpression.Method.Name == _QueryableRightJoinMethodName)
                                    joinType = SourceType.RightJoin;
                                else if (methodCallExpression.Method.Name == _QueryableFullJoinMethodName)
                                    joinType = SourceType.FullJoin;
                                else if (methodCallExpression.Method.Name == _QueryableCrossJoinMethodName)
                                    joinType = SourceType.CrossJoin;
                                else
                                    throw new Exception($"对 {methodCallExpression.Method.DeclaringType.FullName} 类型中的 {methodCallExpression.Method.ReflectedType.Name} 方法提取字段信息时未能正确提取到它的关联查询类型");

                                Queryable rightQueryable = (Queryable)((ConstantField)methodParameters[0]).Value;
                                LambdaExpression onLambda = methodParameters.Length == 1 ? null : (LambdaExpression)((ConstantField)methodParameters[1]).Value;

                                //修改 Join 泛型方法的类型为待执行 Join 方法的泛型
                                MethodInfo methodInfo = _QueryableJoinMethod2.MakeGenericMethod(methodCallExpression.Method.GetGenericArguments());

                                //调用 Join 方法并将得到的 MultipleJoin 对象封装成 ConstantField 字段
                                resultField = new ConstantField(dataContext, body.Type, methodInfo.Invoke(queryable, new object[] { joinType, rightQueryable, onLambda, parameters }));
                            }
                            //若调用后返回值不是 Queryable 类型但调用实例类型是 Queryable 类型，则需要对这些方法进行处理后再进行二次转换
                            else
                            {
                                //First 或 FirstOrDefault 方法，将转换成子查询
                                if ((methodCallExpression.Method.Name == _QueryableFirstMethodName || methodCallExpression.Method.Name == _QueryableFirstOrDefaultMethodName) && methodParameters == null)
                                {
                                    if (methodCallExpression.Method.Name == _QueryableFirstMethodName || methodCallExpression.Method.ReturnType.CheckIsBasicType())
                                    {
                                        queryable = queryable.Limit(new Limit(0, 1));

                                        var selectField = queryable.DataSource.SelectField ?? queryable.DataSource.RootField;

                                        if (selectField.Type != FieldType.DefaultOrValue && methodCallExpression.Method.Name != _QueryableFirstMethodName)
                                            selectField = new IsolateField(dataContext, FieldType.DefaultOrValue, (BasicField)selectField);

                                        startDataSourceAliasIndex = queryable.DataSource.AliasIndex + 1;

                                        resultField = selectField.ToSubqueryField(queryable.DataSource, new Dictionary<Field.Field, Field.Field>());
                                    }
                                    else
                                    {
                                        throw new Exception($"子查询只支持基础数据类型的 {_QueryableFirstOrDefaultMethodName} 方法，请将其改为 {_QueryableFirstMethodName} 实现");
                                    }
                                }
                                //Last 或 LastOrDefault 方法，将转换成子查询
                                else if ((methodCallExpression.Method.Name == _QueryableLastMethodName || methodCallExpression.Method.Name == _QueryableLastOrDefaultMethodName) && methodParameters == null)
                                {
                                    if (queryable.DataSource.SortItems == null || queryable.DataSource.SortItems.Count < 1)
                                        throw new Exception($"对于子查询调用 {_QueryableLastMethodName} 方法而言必须指定查询时的排序字段，因为子查询的 {_QueryableLastMethodName} 方法内部是将排序字段进行方向排序后再取第一条数据");

                                    if (methodCallExpression.Method.Name == _QueryableLastMethodName || methodCallExpression.Method.ReturnType.CheckIsBasicType())
                                    {
                                        queryable = queryable.GetLastQueryable();

                                        var selectField = queryable.DataSource.SelectField ?? queryable.DataSource.RootField;

                                        if (selectField.Type != FieldType.DefaultOrValue && methodCallExpression.Method.Name != _QueryableLastMethodName)
                                            selectField = new IsolateField(dataContext, FieldType.DefaultOrValue, (BasicField)selectField);

                                        startDataSourceAliasIndex = queryable.DataSource.AliasIndex + 1;

                                        resultField = selectField.ToSubqueryField(queryable.DataSource, new Dictionary<Field.Field, Field.Field>());
                                    }
                                    else
                                    {
                                        throw new Exception($"子查询只支持基础数据类型的 {_QueryableLastOrDefaultMethodName} 方法，请将其改为 {_QueryableLastMethodName} 实现");
                                    }
                                }
                                //Contains 方法，返回 BinaryField 的 In 字段
                                else if (methodCallExpression.Method.Name == _QueryableContainsMethodName && methodParameters != null && methodParameters.Length == 1)
                                {
                                    var queryableType = queryable.GetType();
                                    Type[] argumentTypes = queryableType.GetGenericArguments();

                                    if (!argumentTypes[1].CheckIsBasicType())
                                        throw new Exception($"被用于子查询或 Where 条件的 {_QueryableContainsMethodName} 方法时，调用对象 Queryable 的查询结果成员必须是基础数据类型");

                                    resultField = new BinaryField(dataContext, body.Type, OperationType.In, (BasicField)methodParameters[0], new ConstantField(dataContext, queryableType, queryable));
                                }
                                //SelectCount 方法，将转换成子查询
                                else if (methodCallExpression.Method.Name == _QueryableSelectCountMethodName && methodParameters == null)
                                {
                                    if (_DBFunCountMethod == null)
                                        _DBFunCountMethod = _DBFunType.GetMethod(nameof(DBFun.Count), BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, Type.DefaultBinder, new Type[0], null);

                                    resultField = new SubqueryField(dataContext, new MethodField(dataContext, null, null, _DBFunCountMethod), queryable.DataSource);
                                }
                                else
                                {
                                    throw new Exception($"子查询不支持 {(methodCallExpression.Method.ReflectedType.IsSubclassOf(_ModifiableType) ? "Modifiable" : "Queryable")}.{methodCallExpression.Method.Name} 方法");
                                }
                            }
                        }
                        //若调用之前的对象类型是 MultipleJoin 类型
                        else if (methodCallExpression.Method.ReflectedType.IsSubclassOf(_MultipleJoinType))
                        {
                            if (methodExampleField.Type != FieldType.Constant || !methodExampleField.DataType.IsSubclassOf(_MultipleJoinType))
                                throw new Exception($"提取子查询字段信息时发现调用实例对象字段不为 ConstantField 或常量值类型不为 MultipleJoin 类型，具体表达式为：{body}");

                            //调用后的返回值也是 MultipleJoin 类型，则需要多调用方法进行二次处理（各种关联方法）
                            if (body.Type.IsSubclassOf(_MultipleJoinType) && methodParameters != null && (methodParameters.Length == 2 || methodParameters.Length == 1) && methodParameters[0].Type == FieldType.Constant && methodCallExpression.Arguments[0].Type.IsSubclassOf(_QueryableType) && (methodParameters.Length == 1 || (methodParameters[1].Type == FieldType.Constant && methodCallExpression.Arguments[1].Type.IsSubclassOf(_LambdaExpressionType))))
                            {
                                if (_MultipleJoinJoinMethod == null)
                                {
                                    var joinMethods = _MultipleJoinType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);

                                    if (joinMethods != null && joinMethods.Length > 0)
                                    {
                                        foreach (var item in joinMethods)
                                        {
                                            if (item.Name == _MultipleJoinJoinMethodName && item.IsGenericMethod && item.GetGenericArguments().Length == 2 && item.ReturnType == _MultipleJoinType)
                                            {
                                                var itemParameters = item.GetParameters();

                                                if (itemParameters != null && itemParameters.Length == 4)
                                                {
                                                    _MultipleJoinJoinMethod = item;

                                                    break;
                                                }
                                            }
                                        }
                                    }

                                    if (_MultipleJoinJoinMethod == null)
                                        throw new Exception($"获取 MultipleJoin 类型的 {_MultipleJoinJoinMethodName} 方法信息出错");
                                }

                                SourceType joinType = SourceType.InnerJoin;

                                if (methodCallExpression.Method.Name == _MultipleJoinInnerJoinMethodName)
                                    joinType = SourceType.InnerJoin;
                                else if (methodCallExpression.Method.Name == _MultipleJoinLeftJoinMethodName)
                                    joinType = SourceType.LeftJoin;
                                else if (methodCallExpression.Method.Name == _MultipleJoinRightJoinMethodName)
                                    joinType = SourceType.RightJoin;
                                else if (methodCallExpression.Method.Name == _MultipleJoinFullJoinMethodName)
                                    joinType = SourceType.FullJoin;
                                else if (methodCallExpression.Method.Name == _MultipleJoinCrossJoinMethodName)
                                    joinType = SourceType.CrossJoin;
                                else
                                    throw new Exception($"对 {methodCallExpression.Method.DeclaringType.FullName} 类型中的 {methodCallExpression.Method.ReflectedType.Name} 方法提取字段信息时未能正确提取到它的关联查询类型");

                                Queryable rightQueryable = (Queryable)((ConstantField)methodParameters[0]).Value;
                                LambdaExpression onLambda = methodParameters.Length == 1 ? null : (LambdaExpression)((ConstantField)methodParameters[1]).Value;

                                //修改 Join 泛型方法的类型为待执行 Join 方法的泛型
                                MethodInfo methodInfo = _MultipleJoinJoinMethod.MakeGenericMethod(methodCallExpression.Method.GetGenericArguments());

                                //将得到的 MultipleJoin 对象封装成 ConstantField 字段
                                resultField = new ConstantField(dataContext, body.Type, (MultipleJoin)methodInfo.Invoke(((ConstantField)methodExampleField).Value, new object[] { rightQueryable, onLambda, joinType, parameters }));
                            }
                            //调用之前的对象类型是 MultipleJoin 类型且返回值为 Queryable 类型
                            else if (body.Type.IsSubclassOf(_QueryableType))
                            {
                                if (_MultipleJoinSelectMethod == null)
                                {
                                    _MultipleJoinSelectMethod = _MultipleJoinType.GetMethod(_MultipleJoinSelectMethodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, Type.DefaultBinder, new Type[] { _LambdaExpressionType, _LambdaExpressionType, _ExistentParametersType }, null);

                                    if (_MultipleJoinSelectMethod == null)
                                        throw new Exception($"获取 MultipleJoin 类型的 {_MultipleJoinSelectMethodName} 方法信息出错");
                                }

                                //修改 Select 泛型方法的类型为待执行 Select 方法的泛型
                                MethodInfo methodInfo = _MultipleJoinSelectMethod.MakeGenericMethod(methodCallExpression.Method.GetGenericArguments());

                                //调用 Select 方法并将得到的 Queryable 对象封装成 ConstantField 字段
                                resultField = new ConstantField(dataContext, body.Type, methodInfo.Invoke(((ConstantField)methodExampleField).Value, new object[] { ((ConstantField)methodParameters[0]).Value, ((ConstantField)methodParameters[1]).Value, parameters }));
                            }
                            //不支持其他方法作为子查询
                            else
                            {
                                throw new Exception($"子查询不支持 {(methodCallExpression.Method.ReflectedType.IsSubclassOf(_ModifiableMultipleJoinType) ? "ModifiableMultipleJoin" : "MultipleJoin")}.{methodCallExpression.Method.Name} 方法");
                            }
                        }
                        //若调用之前的对象类型不是 Queryable 以及 MultipleJoin 类型但返回值为 Queryable 类型
                        else if (body.Type.IsSubclassOf(_QueryableType))
                        {
                            //若包含了非运行时常量参数则抛出异常
                            if (isExistFieldParameter)
                                throw new Exception($"对于调用方法获取 Queryable 对象时，调用参数不得包含非运行时常量，调用方法为（实例方法）：{methodCallExpression.Method.ReflectedType.FullName}.{methodCallExpression.Method.Name}");
                            else
                                resultField = new ConstantField(dataContext, body.Type, ((Queryable)ExtractConstant(body)).Copy(new Dictionary<DataSource.DataSource, DataSource.DataSource>(), new Dictionary<Field.Field, Field.Field>(), ref startDataSourceAliasIndex));
                        }
                        //若调用之前的对象类型不是 Queryable 以及 MultipleJoin 类型但返回值为 MultipleJoin 类型
                        else if (body.Type.IsSubclassOf(_MultipleJoinType))
                        {
                            //若包含了非运行时常量参数则抛出异常
                            if (isExistFieldParameter)
                                throw new Exception($"对于调用方法获取 MultipleJoin 对象时，调用参数不得包含非运行时常量，调用方法为（实例方法）：{methodCallExpression.Method.ReflectedType.FullName}.{methodCallExpression.Method.Name}");
                            else
                                resultField = new ConstantField(dataContext, body.Type, ((MultipleJoin)ExtractConstant(body)).Copy(new Dictionary<DataSource.DataSource, DataSource.DataSource>(), new Dictionary<Field.Field, Field.Field>(), ref startDataSourceAliasIndex));
                        }
                        //若调用方法实例是非常量字段或调用参数包含非常量字段（实例方法）
                        else if (isExistFieldParameter || methodExampleField.Type != FieldType.Constant)
                        {
                            //若调用方法为 ICollection<?>.Contains 方法
                            if (methodCallExpression.Method.Name == _ICollectionContainsMethodName && methodExampleField.Type == FieldType.Constant && methodCallExpression.Method.DeclaringType.CheckIsCollection() && methodParameters != null && methodParameters.Length == 1 && methodParameters[0] is BasicField basicParameter && methodParameters[0].DataType.CheckIsBasicType())
                                resultField = new BinaryField(dataContext, body.Type, OperationType.In, basicParameter, (ConstantField)methodExampleField);
                            else
                                resultField = new MethodField(dataContext, methodExampleField, methodParameters, methodCallExpression.Method);
                        }
                        //打上 DBFunAttribute 标记的方法直接返回 MethodField 字段（实例方法）
                        else if (methodCallExpression.Method.GetCustomAttribute<DBFunAttribute>(true) != null)
                        {
                            resultField = new MethodField(dataContext, methodExampleField, methodParameters, methodCallExpression.Method);
                        }
                        //若调用实例是常量且参数也都是常量，那么直接调用该方法提取出常量值返回（实例方法）
                        else
                        {
                            resultField = new ConstantField(dataContext, body.Type, ExtractConstant(body));
                        }
                    }
                    //若调用之前的对象类型不是 Queryable 以及 MultipleJoin 类型但返回值为 Queryable 类型，则调用方法得到的 Queryable 对象后再重置别名即可（静态方法）
                    else if (body.Type.IsSubclassOf(_QueryableType))
                    {
                        if (isExistFieldParameter)
                            throw new Exception($"对于调用方法获取 Queryable 对象时，调用参数不得包含非运行时常量，调用方法为（静态方法）：{methodCallExpression.Method.ReflectedType.FullName}.{methodCallExpression.Method.Name}");

                        resultField = new ConstantField(dataContext, body.Type, ((Queryable)ExtractConstant(body)).Copy(new Dictionary<DataSource.DataSource, DataSource.DataSource>(), new Dictionary<Field.Field, Field.Field>(), ref startDataSourceAliasIndex));
                    }
                    //若调用之前的对象类型不是 Queryable 以及 MultipleJoin 类型但返回值为 MultipleJoin 类型，则调用方法得到的 MultipleJoin 对象后再重置别名即可（静态方法）
                    else if (body.Type.IsSubclassOf(_MultipleJoinType))
                    {
                        if (isExistFieldParameter)
                            throw new Exception($"对于调用方法获取 MultipleJoin 对象时，调用参数不得包含非运行时常量，调用方法为（静态方法）：{methodCallExpression.Method.ReflectedType.FullName}.{methodCallExpression.Method.Name}");

                        resultField = new ConstantField(dataContext, body.Type, ((MultipleJoin)ExtractConstant(body)).Copy(new Dictionary<DataSource.DataSource, DataSource.DataSource>(), new Dictionary<Field.Field, Field.Field>(), ref startDataSourceAliasIndex));
                    }
                    //若是 Extend 调用扩展类中的方法
                    else if (methodCallExpression.Method.ReflectedType == _ExtendType)
                    {
                        //若调用方法为 ToNull 方法，直接返回函数参数中的第一个参数值即可
                        if (methodCallExpression.Method.Name == _ExtendToNullMethodName && methodParameters != null && methodParameters.Length == 1 && methodParameters[0].DataType.CheckIsBasicType())
                            resultField = methodParameters[0];
                        //若调用方法为 Like 方法，不管调用实例或参数是否含有字段信息都直接转换成 BinaryField 的 Like 字段
                        else if (methodCallExpression.Method.Name == _ExtendLikeMethodName && methodParameters != null && methodParameters.Length == 2 && methodParameters[0] is BasicField basicObject && methodParameters[1] is BasicField basicRule)
                            resultField = new BinaryField(dataContext, body.Type, OperationType.Like, basicObject, basicRule);
                        //若调用方法为 Contains 方法，且参数一为 Queryable 对象或参数二为非常量字段，则返回 BinaryField 的 In 字段
                        else if (methodCallExpression.Method.Name == _ExtendContainsMethodName && methodParameters != null && methodParameters.Length == 2 && methodParameters[0].Type == FieldType.Constant && methodParameters[1] is BasicField basicField && (isExistFieldParameter || methodParameters[0].DataType.IsSubclassOf(_QueryableType)))
                            resultField = new BinaryField(dataContext, body.Type, OperationType.In, basicField, (ConstantField)methodParameters[0]);
                        //否则返回 MethodField 字段
                        else
                            resultField = new MethodField(dataContext, null, methodParameters, methodCallExpression.Method);
                    }
                    //调用方法打上了 DBFun 标记或参数包含非运行常量字段时直接返回方法字段交给实现者处理
                    else if (isExistFieldParameter || methodCallExpression.Method.GetCustomAttribute<DBFunAttribute>(true) != null)
                    {
                        resultField = new MethodField(dataContext, null, methodParameters, methodCallExpression.Method);
                    }
                    //若调用参数都是常量，那么直接调用该方法提取出常量值返回（静态方法）
                    else
                    {
                        resultField = new ConstantField(dataContext, body.Type, ExtractConstant(body));
                    }
                    break;
                #endregion
                #region 三元操作字段提取
                case ExpressionType.Conditional:
                    ConditionalExpression conditionalExpression = (ConditionalExpression)body;

                    resultField = new ConditionalField(dataContext, body.Type, (BasicField)ExtractField(dataContext, conditionalExpression.Test, extractWay, parameters, convertedFields, ref startDataSourceAliasIndex), (BasicField)ExtractField(dataContext, conditionalExpression.IfTrue, extractWay, parameters, convertedFields, ref startDataSourceAliasIndex), (BasicField)ExtractField(dataContext, conditionalExpression.IfFalse, extractWay, parameters, convertedFields, ref startDataSourceAliasIndex));
                    break;
                #endregion
                #region 常量值提取
                case ExpressionType.Constant:
                    resultField = new ConstantField(dataContext, body.Type, ((ConstantExpression)body).Value);
                    break;
                #endregion
                #region 一元操作字段提取
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.UnaryPlus:
                case ExpressionType.Not:
                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                    UnaryExpression unaryExpression = (UnaryExpression)body;

                    if (body.NodeType == ExpressionType.Quote)
                    {
                        resultField = new ConstantField(dataContext, body.Type, unaryExpression.Operand);
                    }
                    else
                    {
                        Field.Field operand = ExtractField(dataContext, unaryExpression.Operand, extractWay, parameters, convertedFields, ref startDataSourceAliasIndex);

                        if (body.NodeType == ExpressionType.UnaryPlus)
                        {
                            resultField = operand;
                        }
                        else if (body.NodeType == ExpressionType.ArrayLength)
                        {
                            if (operand.Type == FieldType.Constant && operand.DataType.IsArray)
                                resultField = new ConstantField(dataContext, body.Type, ((Array)((ConstantField)operand).Value).Length);
                            else if (operand.Type == FieldType.Collection)
                                resultField = new ConstantField(dataContext, body.Type, ((CollectionField)operand).Count);
                            else
                                throw new Exception("获取某个对象的数组长度值时发现被获取的对象字段并非集合字段类型");
                        }
                        else
                        {
                            //将结构体转换成可为 null 类型的操作直接返回，不做任何处理
                            if (body.NodeType == ExpressionType.Convert && body.Type.IsGenericType && body.Type.GetGenericArguments().Length == 1 && body.Type.GetGenericTypeDefinition() == _NullableType)
                            {
                                resultField = operand;
                            }
                            else
                            {
                                switch (body.NodeType)
                                {
                                    case ExpressionType.Convert:
                                    case ExpressionType.ConvertChecked:
                                        resultField = new UnaryField(dataContext, body.Type, OperationType.Convert, (BasicField)operand);
                                        break;
                                    case ExpressionType.Negate:
                                    case ExpressionType.NegateChecked:
                                        resultField = new UnaryField(dataContext, body.Type, OperationType.Negate, (BasicField)operand);
                                        break;
                                    case ExpressionType.Not:
                                        if (operand.Type == FieldType.Binary && (((BinaryField)operand).OperationType == OperationType.In || ((BinaryField)operand).OperationType == OperationType.NotIn))
                                        {
                                            BinaryField binaryField = (BinaryField)operand;

                                            resultField = new BinaryField(dataContext, binaryField.DataType, binaryField.OperationType == OperationType.In ? OperationType.NotIn : OperationType.In, binaryField.Left, binaryField.Right);
                                        }
                                        else if (operand.Type == FieldType.Binary && (((BinaryField)operand).OperationType == OperationType.Like || ((BinaryField)operand).OperationType == OperationType.NotLike))
                                        {
                                            BinaryField binaryField = (BinaryField)operand;

                                            resultField = new BinaryField(dataContext, binaryField.DataType, binaryField.OperationType == OperationType.Like ? OperationType.NotLike : OperationType.Like, binaryField.Left, binaryField.Right);
                                        }
                                        else if (operand.Type == FieldType.Unary && ((UnaryField)operand).OperationType == OperationType.Not)
                                        {
                                            resultField = ((UnaryField)operand).Operand;
                                        }
                                        else
                                        {
                                            resultField = new UnaryField(dataContext, body.Type, OperationType.Not, (BasicField)operand);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    break;
                #endregion
                #region Switch 分支字段提取
                case ExpressionType.Switch:
                    SwitchExpression switchExpression = (SwitchExpression)body;

                    Field.Field switchValue = ExtractField(dataContext, switchExpression.SwitchValue, extractWay, parameters, convertedFields, ref startDataSourceAliasIndex);

                    if (switchValue != null && switchValue is BasicField basicSwitchValue)
                    {
                        List<SwitchCase> switchCases = null;
                        BasicField defaultBody = null;

                        if (switchExpression.Cases != null && switchExpression.Cases.Count > 0)
                        {
                            switchCases = new List<SwitchCase>();

                            foreach (var item in switchExpression.Cases)
                            {
                                Field.Field caseBody = ExtractField(dataContext, item.Body, extractWay, parameters, convertedFields, ref startDataSourceAliasIndex);

                                if (caseBody != null && caseBody is BasicField basicCaseBody)
                                {
                                    List<ConstantField> testValues = new List<ConstantField>();

                                    foreach (var testValueItem in item.TestValues)
                                    {
                                        Field.Field caseTestValue = ExtractField(dataContext, testValueItem, extractWay, parameters, convertedFields, ref startDataSourceAliasIndex);

                                        if (caseTestValue != null && caseTestValue.Type == FieldType.Constant)
                                            testValues.Add((ConstantField)caseTestValue);
                                        else
                                            throw new Exception($"获取某个 Switch 语句中某个分支测定值时获取到的字段{(caseBody == null ? "为 null" : "类型不是常量字段")}，具体表达式为：{testValueItem}");
                                    }

                                    switchCases.Add(new SwitchCase(basicCaseBody, testValues));
                                }
                                else
                                {
                                    throw new Exception($"获取某个 Switch 语句中某个分支返回值主体时获取到的字段{(caseBody == null ? "为 null" : "类型不是基础数据类型字段，而对于 Sql 而言分支返回值只能是基础数据类型的字段")}，具体表达式为：{item.Body}");
                                }
                            }
                        }

                        if (switchExpression.DefaultBody != null)
                        {
                            Field.Field tempDefaultBody = ExtractField(dataContext, switchExpression.DefaultBody, extractWay, parameters, convertedFields, ref startDataSourceAliasIndex);

                            if (tempDefaultBody != null && tempDefaultBody is BasicField basicSwitchDefault)
                                defaultBody = basicSwitchDefault;
                            else
                                throw new Exception($"获取某个 Switch 语句的默认分支返回值时获取到的字段{(tempDefaultBody == null ? "为 null" : "类型不是基础数据类型字段，而对于 Sql 而言分支返回值只能是基础数据类型的字段")}，具体表达式为：{switchExpression.DefaultBody}");
                        }

                        resultField = new SwitchField(dataContext, body.Type, basicSwitchValue, switchCases, defaultBody);
                    }
                    else
                    {
                        throw new Exception($"获取某个 Switch 语句的判定值字段时获取到的字段{(switchValue == null ? "为 null" : "类型不是基础数据类型字段，而对于 Sql 而言判断值只能是基础数据类型的字段")}，具体表达式为：{switchExpression.SwitchValue}");
                    }
                    break;
                #endregion
                #region List 集合初始化字段提取 
                case ExpressionType.ListInit:
                    ListInitExpression listInitExpression = (ListInitExpression)body;

                    if (listInitExpression.NewExpression != null)
                    {
                        resultField = ExtractField(dataContext, listInitExpression.NewExpression, extractWay, parameters, convertedFields, ref startDataSourceAliasIndex);

                        if (resultField.Type == FieldType.Collection && listInitExpression.Initializers != null && listInitExpression.Initializers.Count > 0)
                        {
                            CollectionField collectionField = (CollectionField)resultField;

                            List<Field.Field> members = collectionField.Count > 0 ? new List<Field.Field>(collectionField) : new List<Field.Field>();

                            MethodInfo addMethodInfo = collectionField.AddMethodInfo;

                            foreach (var item in listInitExpression.Initializers)
                            {
                                if (item.Arguments.Count != 1)
                                    throw new Exception("暂不支持多维集合或数组对象");

                                addMethodInfo = item.AddMethod;

                                members.Add(ExtractField(dataContext, item.Arguments[0], extractWay, parameters, convertedFields, ref startDataSourceAliasIndex));
                            }

                            resultField = new CollectionField(dataContext, body.Type, collectionField.ConstructorInfo, addMethodInfo, members);
                        }
                    }
                    break;
                #endregion
                #region 属性字段提取
                case ExpressionType.MemberAccess:
                    MemberExpression memberExpression = (MemberExpression)body;

                    if (memberExpression.Expression != null)
                    {
                        Field.Field exampleField = ExtractField(dataContext, memberExpression.Expression, extractWay, parameters, convertedFields, ref startDataSourceAliasIndex);

                        if (exampleField.Type == FieldType.Object)
                        {
                            ObjectField objectField = (ObjectField)exampleField;

                            if (objectField.Members == null || objectField.Members.Count < 1 || !objectField.Members.TryGetValue(memberExpression.Member.Name, out MemberInfo memberInfo))
                                throw new Exception($"未能找到指定 {memberExpression.Member.DeclaringType.FullName} 类型对象中的 {memberExpression.Member.Name} 成员所对应的字段信息");

                            resultField = memberInfo.Field;
                        }
                        else if (exampleField.Type == FieldType.Constant)
                        {
                            object constantValue = ((ConstantField)exampleField).Value;

                            //如果常量对象是 Queryable 类型，则要将其单独处理后转换成子查询字段
                            if (exampleField.DataType.IsSubclassOf(_QueryableType))
                            {
                                //Count 属性
                                if (memberExpression.Member.Name == _QueryableCountPropName && memberExpression.Member.MemberType == MemberTypes.Property)
                                {
                                    if (_DBFunCountMethod == null)
                                        _DBFunCountMethod = _DBFunType.GetMethod(nameof(DBFun.Count), BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, Type.DefaultBinder, new Type[0], null);

                                    resultField = new SubqueryField(dataContext, new MethodField(dataContext, null, null, _DBFunCountMethod), (((Queryable)constantValue)).DataSource);
                                }
                            }
                            else
                            {
                                if (constantValue == null)
                                    throw new NullReferenceException($"调用 {memberExpression.Member.ReflectedType.FullName} 类型的 {memberExpression.Member.Name} 成员时未将对象引用设置到对象实例");

                                //属性为 JoinItem.Left 或 JoinItem.Right 时需做特殊处理
                                if (constantValue != null && (memberExpression.Member.Name == _JointItemLeftPropName || memberExpression.Member.Name == _JointItemRightPropName) && constantValue is JoinDataSource joinDataSource)
                                {
                                    DataSource.DataSource valueDataSource = memberExpression.Member.Name == _JointItemLeftPropName ? joinDataSource.Left : joinDataSource.Right;

                                    if (valueDataSource is BasicDataSource basicDataSource)
                                    {
                                        if (extractWay != ExtractWay.Other)
                                        {
                                            resultField = basicDataSource.SelectField ?? basicDataSource.RootField;

                                            if (extractWay == ExtractWay.SelectNew)
                                                resultField = resultField.ToQuoteField(basicDataSource, convertedFields);
                                            else
                                                resultField = resultField.ToNewAliasField(convertedFields);
                                        }
                                        else
                                        {
                                            resultField = basicDataSource.RootField;
                                        }
                                    }
                                    else
                                    {
                                        resultField = new ConstantField(dataContext, body.Type, valueDataSource);
                                    }
                                }

                                if (resultField == null)
                                {
                                    //如果标记了 DBMemberAttribute 特性，则直接返回 MemberField 字段
                                    if (memberExpression.Member.GetCustomAttribute<DBMemberAttribute>(true) != null)
                                        resultField = new MemberField(dataContext, body.Type, exampleField, memberExpression.Member);
                                    //否则直接取成员值
                                    else if (memberExpression.Member.MemberType == MemberTypes.Property)
                                        resultField = new ConstantField(dataContext, body.Type, ((PropertyInfo)memberExpression.Member).GetValue(constantValue, null));
                                    else if (memberExpression.Member.MemberType == MemberTypes.Field)
                                        resultField = new ConstantField(dataContext, body.Type, ((FieldInfo)memberExpression.Member).GetValue(constantValue));
                                }
                            }
                        }
                        //如果是 Nullable 类型的 Value 属性，则直接返回该字段值
                        else if (memberExpression.Member.MemberType == MemberTypes.Property && memberExpression.Member.Name == _NullableValuePropName && memberExpression.Member.ReflectedType.IsGenericType && memberExpression.Member.ReflectedType.GetGenericTypeDefinition() == _NullableType)
                        {
                            resultField = exampleField;
                        }
                        else if (exampleField.Type == FieldType.Collection && ((memberExpression.Member.Name == _ArrayLengthPropName && exampleField.DataType.IsArray) || (memberExpression.Member.Name == _ListCountPropName && exampleField.DataType.CheckIsList())))
                        {
                            resultField = new ConstantField(dataContext, body.Type, ((CollectionField)exampleField).Count);
                        }

                        //若没有正确提取到字段信息则返回 MemberField 类型的字段交由接口实现者处理
                        if (resultField == null)
                            resultField = new MemberField(dataContext, body.Type, exampleField, memberExpression.Member);
                    }
                    else if (memberExpression.Member.GetCustomAttribute<DBMemberAttribute>(true) != null)
                    {
                        resultField = new MemberField(dataContext, body.Type, null, memberExpression.Member);
                    }
                    else if (memberExpression.Member.MemberType == MemberTypes.Property)
                    {
                        resultField = new ConstantField(dataContext, body.Type, ((PropertyInfo)memberExpression.Member).GetValue(null, null));
                    }
                    else if (memberExpression.Member.MemberType == MemberTypes.Field)
                    {
                        resultField = new ConstantField(dataContext, body.Type, ((FieldInfo)memberExpression.Member).GetValue(null));
                    }
                    else
                    {
                        resultField = new MemberField(dataContext, body.Type, null, memberExpression.Member);
                    }

                    if (resultField != null && resultField.Type == FieldType.Constant && resultField.DataType.IsGenericType)
                    {
                        object constantvalue = ((ConstantField)resultField).Value;

                        //如果是静态成员调用或成员实例不是 MultipleJoin 以及 Queryable 对象，则继续判断
                        if (constantvalue != null && (memberExpression.Expression == null || !(memberExpression.Expression.Type.IsSubclassOf(_QueryableType) || memberExpression.Expression.Type.IsSubclassOf(_MultipleJoinType))))
                        {
                            Type constantvalueType = constantvalue.GetType();

                            //若得到的是 Queryable 对象
                            if (constantvalueType.IsSubclassOf(_QueryableType))
                                resultField = new ConstantField(dataContext, body.Type, ((Queryable)constantvalue).Copy(new Dictionary<DataSource.DataSource, DataSource.DataSource>(), new Dictionary<Field.Field, Field.Field>(), ref startDataSourceAliasIndex));
                            //若得到的是 MultipleJoin 对象
                            else if (constantvalueType.IsSubclassOf(_MultipleJoinType))
                                resultField = new ConstantField(dataContext, body.Type, ((MultipleJoin)constantvalue).Copy(new Dictionary<DataSource.DataSource, DataSource.DataSource>(), new Dictionary<Field.Field, Field.Field>(), ref startDataSourceAliasIndex));
                        }
                    }
                    break;
                #endregion
                #region 属性初始化字段提取
                case ExpressionType.MemberInit:
                    MemberInitExpression memberInitExpression = (MemberInitExpression)body;

                    if (memberInitExpression.NewExpression != null)
                    {
                        Field.Field newField = ExtractField(dataContext, memberInitExpression.NewExpression, extractWay, parameters, convertedFields, ref startDataSourceAliasIndex);

                        if (newField is ObjectField objectField)
                        {
                            Dictionary<string, MemberInfo> members = new Dictionary<string, MemberInfo>();

                            if (objectField.Members != null && objectField.Members.Count > 0)
                            {
                                foreach (var item in objectField.Members)
                                {
                                    members.Add(item.Key, item.Value);
                                }
                            }

                            foreach (var item in memberInitExpression.Bindings)
                            {
                                if (item.BindingType != MemberBindingType.Assignment)
                                    throw new Exception($"从某一表达式树中的成员初始化节点中提取初始化成员信息时出错，表达式为：{item}");

                                Field.Field field = ExtractField(dataContext, ((MemberAssignment)item).Expression, extractWay, parameters, convertedFields, ref startDataSourceAliasIndex);

                                members[item.Member.Name] = new MemberInfo(item.Member, field);
                            }

                            resultField = new ObjectField(dataContext, body.Type, objectField.ConstructorInfo, members, true);
                        }
                        else if (newField.Type == FieldType.Constant)
                        {
                            foreach (var item in memberInitExpression.Bindings)
                            {
                                if (item.BindingType != MemberBindingType.Assignment)
                                    throw new Exception($"从某一表达式树中的成员初始化节点中提取初始化成员信息时出错，表达式为：{item}");

                                Field.Field field = ExtractField(dataContext, ((MemberAssignment)item).Expression, extractWay, parameters, convertedFields, ref startDataSourceAliasIndex);

                                if (field.Type != FieldType.Constant)
                                    throw new Exception($"运行时常量对象在初始化属性时属性值不能是非运行时常量，表达式为：{item}");

                                if (item.Member.MemberType == MemberTypes.Property)
                                    ((PropertyInfo)item.Member).SetValue(((ConstantField)newField).Value, ((ConstantField)field).Value, null);
                                else if (item.Member.MemberType == MemberTypes.Field)
                                    ((FieldInfo)item.Member).SetValue(((ConstantField)newField).Value, ((ConstantField)field).Value);
                            }

                            resultField = newField;
                        }
                    }
                    break;
                #endregion
                #region 实例化成员字段提取
                case ExpressionType.New:
                    NewExpression newExpression = (NewExpression)body;
                    List<Field.Field> arguments = null;
                    isExistFieldParameter = false;
                    object[] constructorArgs = null;

                    if (newExpression.Arguments != null && newExpression.Arguments.Count > 0)
                    {
                        arguments = new List<Field.Field>();
                        constructorArgs = new object[newExpression.Arguments.Count];
                        int i = 0;

                        foreach (var item in newExpression.Arguments)
                        {
                            Field.Field itemParameter = ExtractField(dataContext, item, extractWay, parameters, convertedFields, ref startDataSourceAliasIndex);

                            arguments.Add(itemParameter);

                            if (itemParameter.Type != FieldType.Constant)
                                isExistFieldParameter = true;
                            else if (!isExistFieldParameter)
                                constructorArgs[i++] = ((ConstantField)itemParameter).Value;
                        }
                    }

                    //若是 new Guid(string)
                    if (resultField == null && body.Type == _GuidType && arguments.Count == 1 && arguments[0].DataType == _StringType && arguments[0] is BasicField)
                    {
                        if (isExistFieldParameter)
                            resultField = new UnaryField(dataContext, _GuidType, OperationType.Convert, (BasicField)arguments[0]);
                        else
                            resultField = new ConstantField(dataContext, _GuidType, newExpression.Constructor.Invoke(constructorArgs));
                    }
                    //若是 new DateTime()
                    else if (resultField == null && body.Type == _DateTimeType && (arguments.Count == 3 || arguments.Count == 6 || arguments.Count == 7))
                    {
                        bool isNeedConvert = true;

                        foreach (var item in arguments)
                        {
                            if (item.DataType != _Int32Type)
                            {
                                isNeedConvert = false;

                                break;
                            }
                        }

                        if (isNeedConvert)
                        {
                            if (isExistFieldParameter)
                            {
                                ConstantField delimiterField = new ConstantField(dataContext, _StringType, "-");

                                BinaryField parameterField = new BinaryField(dataContext, _StringType, OperationType.Add, new UnaryField(dataContext, _StringType, OperationType.Convert, (BasicField)arguments[0]), delimiterField);
                                parameterField = new BinaryField(dataContext, _StringType, OperationType.Add, parameterField, new UnaryField(dataContext, _StringType, OperationType.Convert, (BasicField)arguments[1]));
                                parameterField = new BinaryField(dataContext, _StringType, OperationType.Add, parameterField, delimiterField);
                                parameterField = new BinaryField(dataContext, _StringType, OperationType.Add, parameterField, new UnaryField(dataContext, _StringType, OperationType.Convert, (BasicField)arguments[2]));

                                if (arguments.Count > 3)
                                {
                                    delimiterField = new ConstantField(dataContext, _StringType, ":");

                                    parameterField = new BinaryField(dataContext, _StringType, OperationType.Add, parameterField, new ConstantField(dataContext, _StringType, " "));
                                    parameterField = new BinaryField(dataContext, _StringType, OperationType.Add, parameterField, new UnaryField(dataContext, _StringType, OperationType.Convert, (BasicField)arguments[3]));
                                    parameterField = new BinaryField(dataContext, _StringType, OperationType.Add, parameterField, delimiterField);
                                    parameterField = new BinaryField(dataContext, _StringType, OperationType.Add, parameterField, new UnaryField(dataContext, _StringType, OperationType.Convert, (BasicField)arguments[4]));
                                    parameterField = new BinaryField(dataContext, _StringType, OperationType.Add, parameterField, delimiterField);
                                    parameterField = new BinaryField(dataContext, _StringType, OperationType.Add, parameterField, new UnaryField(dataContext, _StringType, OperationType.Convert, (BasicField)arguments[5]));
                                }

                                resultField = new UnaryField(dataContext, _DateTimeType, OperationType.Convert, parameterField);

                                if (arguments.Count == 7)
                                {
                                    if (_DBFunAddMillisecondMethod == null)
                                        _DBFunAddMillisecondMethod = _DBFunType.GetMethod(_DBFunAddMillisecondMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, Type.DefaultBinder, new Type[] { _DateTimeType, _Int32Type }, null);

                                    resultField = new MethodField(dataContext, null, new ReadOnlyList<Field.Field>(resultField, arguments[6]), _DBFunAddMillisecondMethod);
                                }
                            }
                            else
                            {
                                resultField = new ConstantField(dataContext, body.Type, newExpression.Constructor.Invoke(constructorArgs));
                            }
                        }
                    }

                    if (resultField == null)
                    {
                        //若是数组或集合，则返回 CollectionField 类型的字段，否则返回 ObjectField 类型的字段
                        if (body.Type.IsArray || body.Type.CheckIsList())
                        {
                            if (!body.Type.IsArray && arguments != null && arguments.Count > 0)
                                throw new Exception($"在提取某一指定表达式树中的字段信息时，初始化 IList<T> 实现类的实例时不能有构造参数，具体表达式为：{newExpression}");

                            resultField = new CollectionField(dataContext, body.Type, new ConstructorInfo(newExpression.Constructor, arguments), null, null);
                        }
                        else
                        {
                            Dictionary<string, MemberInfo> members = null;

                            if (newExpression.Arguments != null && newExpression.Arguments.Count > 0 && newExpression.Members != null && newExpression.Members.Count > 0 && newExpression.Arguments.Count == newExpression.Members.Count)
                            {
                                members = new Dictionary<string, MemberInfo>();

                                for (int i = 0; i < newExpression.Arguments.Count; i++)
                                {
                                    var item = newExpression.Members[i];

                                    members.Add(item.Name, new MemberInfo(item, arguments[i]));
                                }
                            }

                            resultField = new ObjectField(dataContext, body.Type, new ConstructorInfo(newExpression.Constructor, arguments), members, false);
                        }
                    }
                    break;
                #endregion
                #region 数组初始化字段提取
                case ExpressionType.NewArrayInit:
                    NewArrayExpression newArrayExpression = (NewArrayExpression)body;

                    System.Reflection.ConstructorInfo[] constructorInfos = body.Type.GetConstructors();

                    if (constructorInfos == null || constructorInfos.Length != 1)
                        throw new Exception("未能获取到 .NET 数组类的构造函数信息");

                    var constructorParameters = constructorInfos[0].GetParameters();

                    if (constructorParameters == null || constructorParameters.Length != 1 || constructorParameters[0].ParameterType != _Int32Type)
                        throw new Exception("获取到的 .NET 数组类构造函数参数个数或类型不正确");

                    if (newArrayExpression.Expressions != null && newArrayExpression.Expressions.Count > 0)
                    {
                        List<Field.Field> members = new List<Field.Field>();

                        foreach (var item in newArrayExpression.Expressions)
                        {
                            members.Add(ExtractField(dataContext, item, extractWay, parameters, convertedFields, ref startDataSourceAliasIndex));
                        }

                        resultField = new CollectionField(dataContext, body.Type, new ConstructorInfo(constructorInfos[0], null), null, members);
                    }
                    else
                    {
                        resultField = new CollectionField(dataContext, body.Type, new ConstructorInfo(constructorInfos[0], null), null, null);
                    }
                    break;
                #endregion
                #region 参数字段提取
                case ExpressionType.Parameter:
                    if (parameters == null)
                        throw new Exception("在提取指定表达式树中的字段信息时需要提取函数的参数信息，但是该函数并未传递任何参数");

                    ParameterExpression parameterExpression = (ParameterExpression)body;

                    if (parameters.TryGetValue(parameterExpression.Name, out DataSource.DataSource parameterInfo))
                    {
                        if (parameterInfo is BasicDataSource basicDataSource)
                        {
                            if (extractWay != ExtractWay.Other)
                            {
                                resultField = basicDataSource.SelectField ?? basicDataSource.RootField;

                                if (extractWay == ExtractWay.SelectNew)
                                    resultField = resultField.ToQuoteField(basicDataSource, convertedFields);
                                else
                                    resultField = resultField.ToNewAliasField(convertedFields);
                            }
                            else
                            {
                                resultField = basicDataSource.RootField;
                            }
                        }
                        else
                        {
                            resultField = new ConstantField(dataContext, body.Type, parameterInfo);
                        }
                    }
                    break;
                    #endregion
            }

            if (resultField == null)
                throw new Exception($"未能从指定的表达式树中提取出有用的字段信息，表达式为：{body}");

            return resultField;
        }

        /// <summary>
        /// 从指定的表达式树中提取出常量值。
        /// </summary>
        /// <param name="body">待提取常量值的表达式树。</param>
        /// <returns>提取到的常量值。</returns>
        private static object ExtractConstant(Expression body)
        {
            try
            {
                return Expression.Lambda(body).Compile().DynamicInvoke();
            }
            catch (Exception)
            {
            }

            if (body.NodeType == ExpressionType.Call)
            {
                MethodCallExpression methodCallExpression = (MethodCallExpression)body;

                object obj = null;
                object[] arguments = new object[methodCallExpression.Arguments == null ? 0 : methodCallExpression.Arguments.Count];

                if (methodCallExpression.Object != null)
                    obj = ExtractConstant(methodCallExpression.Object);

                if (methodCallExpression.Arguments != null)
                {
                    for (int i = 0; i < methodCallExpression.Arguments.Count; i++)
                    {
                        arguments[i] = ExtractConstant(methodCallExpression.Arguments[i]);
                    }
                }

                return methodCallExpression.Method.Invoke(obj, arguments);
            }
            else if (body.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression memberExpression = (MemberExpression)body;

                if (memberExpression.Member.MemberType == MemberTypes.Property || memberExpression.Member.MemberType == MemberTypes.Field)
                {
                    object obj = null;

                    if (memberExpression.Expression != null)
                        obj = ExtractConstant(memberExpression.Expression);

                    if (memberExpression.Member.MemberType == MemberTypes.Property)
                        return ((PropertyInfo)memberExpression.Member).GetValue(obj, null);
                    else
                        return ((FieldInfo)memberExpression.Member).GetValue(obj);
                }
            }

            throw new Exception("未能从指定表达式树中提取出常量值，通常存在子查询、连接查询或 Where 子句中使用到子查询时需要提取常量值，而在提取常量值时不得涉及拉姆达表达式参数，具体表达式为：{body}");
        }

        /// <summary>
        /// 根据指定的实体类型获取该实体类型对应的数据源。
        /// </summary>
        /// <param name="dataContext">数据操作上下文。</param>
        /// <param name="entityType">待获取数据源的实体类型。</param>
        /// <param name="isMappingTable">是否是获取实体类映射到数据库表的数据源。</param>
        /// <returns>该实体类型对应的数据源。</returns>
        internal static OriginalDataSource GetDataSource(IDataContext dataContext, Type entityType, bool isMappingTable)
        {
            var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var fields = entityType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            if ((properties == null || properties.Length < 1) && (fields == null || fields.Length < 1))
                throw new Exception($"未能获取到 {entityType.FullName} 类型所映射到数据库表或视图的字段信息");

            var constructor = entityType.GetConstructor(new Type[0]);

            if (constructor == null)
                throw new Exception($"{entityType.FullName} 实体类没有无参构造函数，不能作为操作数据源的实体类型");

            string mappingName = null;
            MappingType mappingType = MappingType.PublicProperty;

            TableAttribute tableAttribute = entityType.GetCustomAttribute<TableAttribute>(false);
            ViewAttribute viewAttribute = entityType.GetCustomAttribute<ViewAttribute>(false);

            if (isMappingTable && viewAttribute != null)
                throw new Exception($"未能获取 {entityType.FullName} 类型到数据库表的数据源对象，因为该实体类被打上了 View 标记");
            else if (!isMappingTable && tableAttribute != null)
                throw new Exception($"未能获取 {entityType.FullName} 类型到数据库视图的数据源对象，因为该实体类被打上了 Table 标记");

            if (tableAttribute != null)
            {
                mappingName = tableAttribute.Name;
                mappingType = tableAttribute.MappingType;
            }
            else if (viewAttribute != null)
            {
                mappingName = viewAttribute.Name;
                mappingType = viewAttribute.MappingType;
            }

            if (string.IsNullOrWhiteSpace(mappingName))
                mappingName = isMappingTable ? dataContext.SqlFactory.EncodeTableName(entityType.Name) : dataContext.SqlFactory.EncodeViewName(entityType.Name);

            var members = new Dictionary<string, MemberInfo>();
            string dataSourceAlias = dataContext.SqlFactory.GenerateDataSourceAlias(0);
            MemberInfo primaryKey = null;
            MemberInfo autoincrement = null;

            ForEach<System.Reflection.MemberInfo>(item =>
            {
                var fieldInfo = item.GetCustomAttribute<FieldAttribute>(true);

                if ((mappingType == MappingType.IgnoreMarked && fieldInfo != null) || (mappingType == MappingType.OnlyMarked && fieldInfo == null))
                {
                    return;
                }
                else
                {
                    if (fieldInfo == null)
                        fieldInfo = new FieldAttribute();

                    Type memberType = item.MemberType == MemberTypes.Field ? ((FieldInfo)item).FieldType : item.MemberType == MemberTypes.Method ? ((MethodInfo)item).ReturnType : ((PropertyInfo)item).PropertyType;

                    if (!CheckIsBasicType(memberType))
                        throw new Exception($"不支持非基础数据类型的成员映射，若要忽略映射该成员，请配合使用 Attribute.TableAttribute 或 Attribute.ViewAttribute 再加上 Attribute.FieldAttribute 标记进行筛选，具体成员名称为：{entityType.FullName}.{item.Name}");

                    if (fieldInfo.IsNullable == NullableMode.Auto)
                        fieldInfo.IsNullable = memberType.IsValueType && (!memberType.IsGenericType || memberType.GetGenericTypeDefinition() != _NullableType) ? NullableMode.NotNullable : NullableMode.Nullable;

                    if (string.IsNullOrWhiteSpace(fieldInfo.DataType))
                        fieldInfo.DataType = dataContext.NetTypeToDBType(memberType);

                    if (string.IsNullOrWhiteSpace(fieldInfo.Name))
                        fieldInfo.Name = dataContext.SqlFactory.EncodeFieldName(item.Name);

                    MemberInfo memberInfo = new MemberInfo(item, new OriginalField(dataContext, memberType, fieldInfo, dataContext.SqlFactory.GenerateFieldAlias(members.Count)).ModifyDataSourceAlias(dataSourceAlias));

                    if (isMappingTable && fieldInfo.IsPrimaryKey)
                    {
                        if (primaryKey != null)
                            throw new Exception("当前框架暂不支持一张表或视图存在有多个主键的情况");

                        primaryKey = memberInfo;
                    }

                    if (isMappingTable && fieldInfo.IsAutoincrement)
                    {
                        if (autoincrement != null)
                            throw new Exception("当前框架暂不支持一张表或视图存在有多个自增字段的情况");

                        switch (memberInfo.Field.DataType.FullName)
                        {

                            case "System.Int32":
                            case "System.UInt32":
                            case "System.Int64":
                            case "System.UInt64":
                            case "System.Int16":
                            case "System.UInt16":
                            case "System.Byte":
                            case "System.SByte":
                                autoincrement = memberInfo;
                                break;
                            default:
                                throw new Exception($"自增字段只能映射到整数类型的成员上，而获取到的成员 {entityType.FullName}.{item.Name} 的类型为 {memberInfo.Field.DataType.FullName}");
                        }
                    }

                    members.Add(item.Name, memberInfo);
                }
            }, properties, fields);

            if (isMappingTable)
                return new TableDataSource(dataContext, new ObjectField(dataContext, entityType, new ConstructorInfo(constructor, null), members, true), primaryKey, autoincrement, mappingName, 0);
            else
                return new ViewDataSource(dataContext, new ObjectField(dataContext, entityType, new ConstructorInfo(constructor, null), members, true), viewAttribute?.CreateSQL, mappingName, 0);
        }

        /// <summary>
        /// 对指定类型的多个数组执行循环操作。
        /// </summary>
        /// <typeparam name="T">数组中的每一项成员类型。</typeparam>
        /// <param name="callback">循环的回调函数。</param>
        /// <param name="items">所需循环的数组对象。</param>
        private static void ForEach<T>(Action<T> callback, params T[][] items)
        {
            if (items != null)
            {
                foreach (var ary in items)
                {
                    if (ary != null)
                    {
                        foreach (var item in ary)
                        {
                            callback(item);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取应用于指定成员特定类型的自定义特性。
        /// </summary>
        /// <typeparam name="T">需要获取的自定义特性类型。</typeparam>
        /// <param name="memberInfo">待获取自定义特性的成员信息。</param>
        /// <param name="inherit">如果要搜索此成员的继承链以查找属性，则为 true；否则为 false。 会忽略属性和事件的此参数。</param>
        /// <returns>若找到对应的自定义标记对象则返回第一个标记对象，否则返回 null。</returns>
        internal static T GetCustomAttribute<T>(this System.Reflection.MemberInfo memberInfo, bool inherit) where T : System.Attribute
        {
#if NET40
            return GetCustomAttribute<T>(memberInfo.GetCustomAttributes, inherit);
#else
            return (T)memberInfo.GetCustomAttribute(typeof(T), inherit);
#endif
        }

        /// <summary>
        /// 获取应用于指定类型特定类型的自定义特性。
        /// </summary>
        /// <typeparam name="T">需要获取的自定义特性类型。</typeparam>
        /// <param name="type">待获取自定义特性的类型。</param>
        /// <param name="inherit">如果要搜索此成员的继承链以查找属性，则为 true；否则为 false。 会忽略属性和事件的此参数。</param>
        /// <returns>若找到对应的自定义标记对象则返回第一个标记对象，否则返回 null。</returns>
        internal static T GetCustomAttribute<T>(this Type type, bool inherit) where T : System.Attribute
        {
#if NET40
            return GetCustomAttribute<T>(type.GetCustomAttributes, inherit);
#else
            return (T)type.GetCustomAttribute(typeof(T), inherit);
#endif
        }

        /// <summary>
        /// 获取自定义标记。
        /// </summary>
        /// <typeparam name="T">需要获取的自定义标记类型。</typeparam>
        /// <param name="getCustomAttributes">获取所有自定义标记对象的方法。</param>
        /// <param name="inherit">如果要搜索此成员的继承链以查找属性，则为 true；否则为 false。 会忽略属性和事件的此参数。</param>
        /// <returns>若找到对应的自定义标记对象则返回第一个标记对象，否则返回 null。</returns>
        private static T GetCustomAttribute<T>(Func<Type, bool, object[]> getCustomAttributes, bool inherit) where T : System.Attribute
        {
            var attrs = getCustomAttributes(typeof(T), inherit);

            if (attrs != null && attrs.Length > 0)
                return (T)attrs[0];

            return null;
        }

        /// <summary>
        /// 校验类型是否实现了 <see cref="IList{T}"/> 泛型接口。
        /// </summary>
        /// <param name="type">需要校验的类型。</param>
        /// <returns>若该类型实现了 <see cref="IList{T}"/> 泛型接口则返回 true，否则返回 false。</returns>
        internal static bool CheckIsList(this Type type)
        {
            return _IListType.IsAssignableFrom(type);
        }

        /// <summary>
        /// 校验类型是否实现了 <see cref="ICollection{T}"/> 泛型接口。
        /// </summary>
        /// <param name="type">需要校验的类型。</param>
        /// <returns>若该类型实现了 <see cref="ICollection{T}"/> 泛型接口则返回 true，否则返回 false。</returns>
        internal static bool CheckIsCollection(this Type type)
        {
            return _ICollectionType.IsAssignableFrom(type);
        }

        /// <summary>
        /// 校验类型是否为基础数据类型。
        /// </summary>
        /// <param name="type">需要校验的类型。</param>
        /// <returns>若该类型为基础数据类型时返回 true，否则返回 false。</returns>
        public static bool CheckIsBasicType(this Type type)
        {
            return type.IsPrimitive || type.IsEnum || type == _DecimalType || type == _DateTimeOffsetType || type == _TimeSpanType || type == _DateTimeType || type == _StringType || type == _GuidType || (type.IsGenericType && type.GetGenericTypeDefinition() == _NullableType && CheckIsBasicType(type.GetGenericArguments()[0]));
        }

        /// <summary>
        /// 校验指定的对象是否为该类型的默认值。
        /// </summary>
        /// <param name="value">需要校验的值。</param>
        /// <returns>若该值为对应类型的默认值则返回 true，否则返回 false。</returns>
        internal static bool CheckIsDefault(object value)
        {
            if (value == null)
                return true;

            if (value is int intValue)
                return intValue == default;
            else if (value is long longValue)
                return longValue == default;
            else if (value is short shortValue)
                return shortValue == default;
            else if (value is decimal decimalValue)
                return decimalValue == default;
            else if (value is double doubleValue)
                return doubleValue == default;
            else if (value is float floatValue)
                return floatValue == default;
            else if (value is bool boolValue)
                return boolValue == default;
            else if (value is DateTime dateTimeValue)
                return dateTimeValue == default;
            else if (value is Guid guidValue)
                return guidValue == default;
            else if (value is byte byteValue)
                return byteValue == default;
            else if (value is char charValue)
                return charValue == default;
            else if (value is uint uintValue)
                return uintValue == default;
            else if (value is ushort ushortValue)
                return ushortValue == default;
            else if (value is ulong ulongValue)
                return ulongValue == default;
            else if (value is sbyte sbyteValue)
                return sbyteValue == default;
            else if (value is DateTimeOffset dateTimeOffsetValue)
                return dateTimeOffsetValue == default;
            else if (value is TimeSpan timeSpanValue)
                return timeSpanValue == default;

            Type valueType = value.GetType();

            if (valueType.IsValueType)
                return valueType.Equals(Activator.CreateInstance(valueType));

            return false;
        }

        /// <summary>
        /// 校验 <paramref name="type"/> 参数代表的操作是否优先于 <paramref name="verifyType"/> 参数代表的操作。
        /// </summary>
        /// <param name="type">用于比较的操作类型。</param>
        /// <param name="verifyType">被比较的操作类型。</param>
        /// <param name="ignoreEqual">当两者操作优先级相同时是否忽略比较结果直接返回 true。</param>
        /// <returns>若 <paramref name="type"/> 操作优先于 <paramref name="verifyType"/> 操作或两者操作优先级相同且 <paramref name="ignoreEqual"/> 参数为 true 时返回 true，否则返回 false。</returns>
        public static bool CheckIsPriority(OperationType type, OperationType verifyType, bool ignoreEqual)
        {
            int difference = (int)type - (int)verifyType;

            if (difference < 500)
                return false;
            else if (difference > 500)
                return true;
            else
                return ignoreEqual;
        }

        /// <summary>
        /// 校验类型是否实现了 <see cref="IEnumerable{T}"/> 泛型接口。
        /// </summary>
        /// <param name="type">需要校验的类型。</param>
        /// <returns>若该类型实现了 <see cref="IEnumerable{T}"/> 泛型接口则返回 true，否则返回 false。</returns>
        public static bool CheckIsEnumerable(this Type type)
        {
            return _IEnumerableType.IsAssignableFrom(type);
        }

        /// <summary>
        /// 重新设置字段的归属数据源别名。
        /// </summary>
        /// <param name="field">需要设置数据源别名的字段信息。</param>
        /// <param name="aliasIndex">该字段归属数据源别名下标。</param>
        internal static void ResetDataSourceAlias(Field.Field field, int aliasIndex)
        {
            if (field != null)
            {
                if (field.Type == FieldType.Object)
                {
                    ObjectField objectField = (ObjectField)field;

                    foreach (var item in objectField.Members.Values)
                    {
                        ResetDataSourceAlias(item.Field, aliasIndex);
                    }

                    if (objectField.ConstructorInfo.Parameters != null && objectField.ConstructorInfo.Parameters.Count > 0)
                    {
                        foreach (var item in objectField.ConstructorInfo.Parameters)
                        {
                            ResetDataSourceAlias(item, aliasIndex);
                        }
                    }
                }
                else if (field.Type == FieldType.Collection)
                {
                    CollectionField collectionField = (CollectionField)field;

                    if (collectionField.ConstructorInfo.Parameters != null && collectionField.ConstructorInfo.Parameters.Count > 0)
                    {
                        foreach (var item in collectionField.ConstructorInfo.Parameters)
                        {
                            ResetDataSourceAlias(item, aliasIndex);
                        }
                    }

                    foreach (var item in collectionField)
                    {
                        ResetDataSourceAlias(item, aliasIndex);
                    }
                }
                else if (field.Type == FieldType.Binary)
                {
                    BinaryField binaryField = (BinaryField)field;

                    ResetDataSourceAlias(binaryField.Left, aliasIndex);
                    ResetDataSourceAlias(binaryField.Right, aliasIndex);
                }
                else if (field.Type == FieldType.Conditional)
                {
                    ConditionalField conditionalField = (ConditionalField)field;

                    ResetDataSourceAlias(conditionalField.Test, aliasIndex);
                    ResetDataSourceAlias(conditionalField.IfTrue, aliasIndex);
                    ResetDataSourceAlias(conditionalField.IfFalse, aliasIndex);
                }
                else if (field.Type == FieldType.Member)
                {
                    MemberField memberField = (MemberField)field;

                    if (memberField.ObjectField != null)
                        ResetDataSourceAlias(memberField.ObjectField, aliasIndex);
                }
                else if (field.Type == FieldType.Method)
                {
                    MethodField methodField = (MethodField)field;

                    if (methodField.ObjectField != null)
                        ResetDataSourceAlias(methodField.ObjectField, aliasIndex);

                    if (methodField.Parameters != null && methodField.Parameters.Count > 0)
                    {
                        foreach (var item in methodField.Parameters)
                        {
                            ResetDataSourceAlias(item, aliasIndex);
                        }
                    }
                }
                else if (field.Type == FieldType.Unary)
                {
                    UnaryField unaryField = (UnaryField)field;

                    ResetDataSourceAlias(unaryField.Operand, aliasIndex);
                }
                else if (field.Type == FieldType.Switch)
                {
                    SwitchField switchField = (SwitchField)field;

                    ResetDataSourceAlias(switchField.SwitchValue, aliasIndex);

                    if (switchField.Cases != null && switchField.Cases.Count > 0)
                    {
                        foreach (var item in switchField.Cases)
                        {
                            foreach (var testValue in item.TestValues)
                            {
                                ResetDataSourceAlias(testValue, aliasIndex);
                            }

                            ResetDataSourceAlias(item.Body, aliasIndex);
                        }
                    }

                    if (switchField.DefaultBody != null)
                        ResetDataSourceAlias(switchField.DefaultBody, aliasIndex);
                }
                else if (field.Type == FieldType.Original)
                {
                    ((OriginalField)field).ModifyDataSourceAlias(field.DataContext.SqlFactory.GenerateDataSourceAlias(aliasIndex));
                }
                else if (field.Type == FieldType.DefaultOrValue || field.Type == FieldType.NewAlias)
                {
                    ResetDataSourceAlias(((IsolateField)field).InnerField, aliasIndex);
                }
            }
        }
    }
}

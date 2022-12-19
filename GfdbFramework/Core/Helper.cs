using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using GfdbFramework.Attribute;
using GfdbFramework.DataSource;
using GfdbFramework.Enum;
using GfdbFramework.Field;
using GfdbFramework.Interface;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 通用静态帮助类。
    /// </summary>
    public static class Helper
    {
        private static readonly Type _IListType = typeof(IList<>);
        private static readonly Type _Int32Type = typeof(int);
        private static readonly Type _DateTimeType = typeof(DateTime);
        private static readonly Type _DateTimeOffsetType = typeof(DateTimeOffset);
        private static readonly Type _TimeSpanType = typeof(TimeSpan);
        private static readonly Type _GuidType = typeof(Guid);
        private static readonly Type _StringType = typeof(string);
        private static readonly Type _DecimalType = typeof(decimal);
        private static readonly Type _QueryableType = typeof(Queryable);
        private static readonly Type _ModifiableType = typeof(Modifiable<List<int>, int>).GetGenericTypeDefinition();
        private static readonly Type _MultipleJoinType = typeof(MultipleJoin);
        private static readonly Type _ModifiableMultipleJoin = typeof(ModifiableMultipleJoin<int, int, int, int, int>).GetGenericTypeDefinition();
        private static readonly Type _DataSourceType = typeof(DataSourceType);
        private static readonly Type _DBFunType = typeof(DBFun);
        private static readonly Type _NullableType = typeof(int?).GetGenericTypeDefinition();
        private static readonly Type _HelperType = typeof(Helper);
        private static readonly Type _LambdaExpressionType = typeof(LambdaExpression);
        private static readonly Type _ExistentParametersType = typeof(Interface.IReadOnlyDictionary<string, ParameterInfo>);
        private static readonly string _LeftObjectInfoLeftPropName = nameof(LeftObjectInfo<int, int>.Left);
        private static readonly string _LeftObjectInfoRightPropName = nameof(LeftObjectInfo<int, int>.Right);
        private static readonly string _QueryableSelectMethodName = nameof(Queryable.Select);
        private static readonly string _QueryableJoinMethodName = nameof(Queryable.Join);
        private static readonly string _MultipleJoinJoinMethodName = nameof(MultipleJoin.Join);
        private static readonly string _QueryableWhereMethodName = nameof(Queryable.Where);
        private static readonly string _QueryableFirstMethodName = nameof(Queryable<int, int>.First);
        private static readonly string _QueryableFirstOrDefaultMethodName = nameof(Queryable<int, int>.FirstOrDefault);
        private static readonly string _QueryableLastMethodName = nameof(Queryable<int, int>.Last);
        private static readonly string _QueryableTopMethodName = nameof(Queryable<int, int>.Top);
        private static readonly string _QueryableLimitMethodName = nameof(Queryable.Limit);
        private static readonly string _QueryableAscendingMethodName = nameof(Queryable<int, int>.Ascending);
        private static readonly string _QueryableDescendingMethodName = nameof(Queryable<int, int>.Descending);
        private static readonly string _QueryableGroupMethodName = nameof(Queryable.Group);
        private static readonly string _QueryableContainsMethodName = nameof(Queryable<int, int>.Contains);
        private static readonly string _QueryableLeftJoinMethodName = nameof(Queryable<int, int>.LeftJoin);
        private static readonly string _QueryableRightJoinMethodName = nameof(Queryable<int, int>.RightJoin);
        private static readonly string _QueryableInnerJoinMethodName = nameof(Queryable<int, int>.InnerJoin);
        private static readonly string _QueryableFullJoinMethodName = nameof(Queryable<int, int>.FullJoin);
        private static readonly string _QueryableCrossJoinMethodName = nameof(Queryable<int, int>.CrossJoin);
        private static readonly string _QueryableDistinctMethodName = nameof(Queryable<int, int>.Distinct);
        private static readonly string _MultipleJoinLeftJoinMethodName = nameof(MultipleJoin<int, int, int, int>.LeftJoin);
        private static readonly string _MultipleJoinRightJoinMethodName = nameof(MultipleJoin<int, int, int, int>.RightJoin);
        private static readonly string _MultipleJoinInnerJoinMethodName = nameof(MultipleJoin<int, int, int, int>.InnerJoin);
        private static readonly string _MultipleJoinFullJoinMethodName = nameof(MultipleJoin<int, int, int, int>.FullJoin);
        private static readonly string _MultipleJoinCrossJoinMethodName = nameof(MultipleJoin<int, int, int, int>.CrossJoin);
        private static readonly string _MultipleJoinSelectMethodName = nameof(MultipleJoin.Select);
        private static readonly string _DBFunAddMillisecondMethodName = nameof(DBFun.AddMillisecond);
        private static readonly string _HelperToNullMethodName = nameof(ToNull);
        private static readonly string _HelperContainsMethodName = nameof(Contains);
        private static readonly string _HelperLikeMethodName = nameof(Like);
        private static readonly string _IListContainsMethodName = nameof(IList<int>.Contains);
        private static readonly string _QueryableCountPropertyName = nameof(Queryable<int, int>.Count);
        private static MethodInfo _QueryableSelectMethod = null;
        private static MethodInfo _QueryableDistinctMethod = null;
        private static MethodInfo _QueryableJoinMethod1 = null;
        private static MethodInfo _QueryableJoinMethod2 = null;
        private static MethodInfo _MultipleJoinJoinMethod = null;
        private static MethodInfo _MultipleJoinSelectMethod = null;
        private static MethodInfo _DBFunCountMethod = null;
        private static MethodInfo _DBFunAddMillisecondMethod = null;

        /// <summary>
        /// 从指定的表达式树中提取出对应的字段信息。
        /// </summary>
        /// <param name="body">待提取字段信息的表达式树。</param>
        /// <param name="extractType">执行本次提取操作的方式。</param>
        /// <param name="parameters">调用该表达式时所传递的参数集合。</param>
        /// <param name="startAliasIndex">若提取到的字段有需要重置数据源别名时的开始别名下标。</param>
        /// <returns>提取到的字段信息。</returns>
        internal static Field.Field ExtractField(Expression body, ExtractType extractType, Interface.IReadOnlyDictionary<string, ParameterInfo> parameters, ref int startAliasIndex)
        {
            OperationType operationType = OperationType.Default;
            Field.Field resultField = null;

            switch (body.NodeType)
            {
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

                    Field.Field left = ExtractField(binaryExpression.Left, extractType, parameters, ref startAliasIndex);
                    Field.Field right = ExtractField(binaryExpression.Right, extractType, parameters, ref startAliasIndex);

                    if (body.NodeType == ExpressionType.ArrayIndex)
                    {
                        if (right.Type != FieldType.Constant || !(((ConstantField)right).Value is int || ((ConstantField)right).Value is long))
                            throw new Exception("对于从数组集合中引用某个字段时，所使用的索引值必须是运行时常量，而不能是 Sql 中的字段信息");

                        int index = Convert.ToInt32(((ConstantField)right).Value);

                        if (left.Type == FieldType.Constant && left.DataType.IsArray)
                            resultField = new ConstantField(body.Type, ((Array)((ConstantField)left).Value).GetValue(index));
                        else if (left.Type == FieldType.Collection)
                            resultField = ((CollectionField)left)[(int)((ConstantField)right).Value];
                        else
                            throw new Exception("读取某个数组索引处的字段信息时出现错误，被读取的字段并非为 CollectionField 类型");
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

                        resultField = new BinaryField(body.Type, operationType, (BasicField)left, (BasicField)right);
                    }
                    break;
                case ExpressionType.Call:
                    MethodCallExpression methodCallExpression = (MethodCallExpression)body;
                    Field.Field methodExampleField = methodCallExpression.Object == null ? null : ExtractField(methodCallExpression.Object, extractType, parameters, ref startAliasIndex);
                    Field.Field[] methodParameters = null;
                    bool isExistFieldParameter = false;     //是否存在非常量字段类型的参数信息

                    if (methodCallExpression.Arguments != null && methodCallExpression.Arguments.Count > 0)
                    {
                        methodParameters = new Field.Field[methodCallExpression.Arguments.Count];

                        for (int i = 0; i < methodCallExpression.Arguments.Count; i++)
                        {
                            var parameter = ExtractField(methodCallExpression.Arguments[i], extractType, parameters, ref startAliasIndex);

                            if (parameter.Type != FieldType.Constant)
                                isExistFieldParameter = true;

                            methodParameters[i] = parameter;
                        }
                    }

                    //若调用的是 DBFun 类的函数，直接返回 MethodField 字段交友接口实现者处理
                    if (methodCallExpression.Method.ReflectedType.FullName == _DBFunType.FullName)
                    {
                        resultField = new MethodField(null, methodCallExpression.Method, methodParameters);
                    }
                    //若调用实例对象不为 null
                    else if (methodExampleField != null)
                    {
                        //若调用之前的对象类型是 Queryable 类型，则需要对调用方法进行二次处理
                        if (methodCallExpression.Method.ReflectedType.IsSubclassOf(_QueryableType))
                        {
                            if (methodExampleField.Type != FieldType.Constant || !methodExampleField.DataType.IsSubclassOf(_QueryableType))
                                throw new Exception(string.Format("提取子查询字段信息时发现调用实例对象字段不为 ConstantField 或常量值类型不为 Queryable 类型，具体表达式为：{0}", body.ToString()));

                            Queryable queryable = (Queryable)((ConstantField)methodExampleField).Value;

                            //若调用后返回值也是 Queryable 类型，则直接调用相关方法即可
                            if (body.Type.IsSubclassOf(_QueryableType))
                            {
                                //Limit 方法
                                if (methodCallExpression.Method.Name == _QueryableLimitMethodName && methodCallExpression.Arguments != null && methodCallExpression.Arguments.Count == 2 && methodCallExpression.Arguments[0].Type.FullName == _Int32Type.FullName && methodCallExpression.Arguments[1].Type.FullName == _Int32Type.FullName)
                                {
                                    if (methodParameters == null || isExistFieldParameter || methodParameters.Length != 2 || !(((ConstantField)methodParameters[0]).Value is int startIndex) || !(((ConstantField)methodParameters[1]).Value is int count))
                                        throw new Exception(string.Format("对于子查询调用 {0} 方法而言，其使用的方法参数必须是运行时常量，即不允许使用字段作为参数", _QueryableLimitMethodName));

                                    resultField = new ConstantField(body.Type, queryable.Limit(new Limit(startIndex, count)));
                                }
                                //Top 方法
                                else if (methodCallExpression.Method.Name == _QueryableTopMethodName && methodCallExpression.Arguments != null && methodCallExpression.Arguments.Count == 1 && methodCallExpression.Arguments[0].Type.FullName == _Int32Type.FullName)
                                {
                                    if (methodParameters == null || isExistFieldParameter || methodParameters.Length != 1 || !(((ConstantField)methodParameters[0]).Value is int count))
                                        throw new Exception(string.Format("对于子查询调用 {0} 方法而言，其使用的方法参数必须是运行时常量，即不允许使用字段作为参数", _QueryableTopMethodName));

                                    resultField = new ConstantField(body.Type, queryable.Limit(new Limit(count)));
                                }
                                //Distinct 方法
                                else if (methodCallExpression.Method.Name == _QueryableDistinctMethodName && methodParameters == null)
                                {
                                    if (_QueryableDistinctMethod == null)
                                    {
                                        _QueryableDistinctMethod = queryable.GetType().GetMethod(_QueryableDistinctMethodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, Type.DefaultBinder, new Type[] { _LambdaExpressionType, _ExistentParametersType }, null);

                                        if (_QueryableDistinctMethod == null)
                                            throw new Exception("获取 Queryable 类型的 Distinct 方法信息出错");
                                    }

                                    //调用 Distinct 方法并将得到的 Queryable 对象封装成 ConstantField 字段
                                    resultField = new ConstantField(body.Type, _QueryableSelectMethod.Invoke(queryable, null));
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
                                                    throw new Exception("获取 Queryable 类型的 Select 方法信息出错");
                                            }

                                            //修改 Select 泛型方法的类型为待执行 Select 方法的泛型
                                            MethodInfo methodInfo = _QueryableSelectMethod.MakeGenericMethod(methodCallExpression.Method.GetGenericArguments());

                                            //调用 Select 方法并将得到的 Queryable 对象封装成 ConstantField 字段
                                            resultField = new ConstantField(body.Type, methodInfo.Invoke(queryable, new object[] { lambdaExpression, parameters }));
                                        }
                                        //Where 方法
                                        else if (methodCallExpression.Method.Name == _QueryableWhereMethodName)
                                        {
                                            resultField = new ConstantField(body.Type, queryable.Where(lambdaExpression, parameters));
                                        }
                                        //Ascending 方法
                                        else if (methodCallExpression.Method.Name == _QueryableAscendingMethodName && methodCallExpression.Method.IsGenericMethod)
                                        {
                                            resultField = new ConstantField(body.Type, queryable.Sorting(SortType.Ascending, lambdaExpression, parameters));
                                        }
                                        //Descending 方法
                                        else if (methodCallExpression.Method.Name == _QueryableDescendingMethodName && methodCallExpression.Method.IsGenericMethod)
                                        {
                                            resultField = new ConstantField(body.Type, queryable.Sorting(SortType.Descending, lambdaExpression, parameters));
                                        }
                                        //Group 方法
                                        else if (methodCallExpression.Method.Name == _QueryableGroupMethodName && methodCallExpression.Method.IsGenericMethod)
                                        {
                                            resultField = new ConstantField(body.Type, queryable.Group(lambdaExpression, parameters));
                                        }
                                    }
                                    //各种关联查询方法
                                    else if (methodCallExpression.Method.IsGenericMethod && methodCallExpression.Method.GetGenericArguments().Length == 3 && (methodParameters.Length == 2 || methodParameters.Length == 3) && methodParameters[0].Type == FieldType.Constant && methodCallExpression.Arguments[0].Type.IsSubclassOf(_QueryableType) && methodCallExpression.Arguments[1].Type.IsSubclassOf(_LambdaExpressionType) && (methodParameters.Length == 2 || methodCallExpression.Arguments[2].Type.IsSubclassOf(_LambdaExpressionType)))
                                    {
                                        //获取 Queryable 类型的 Join 方法
                                        if (_QueryableJoinMethod1 == null)
                                        {
                                            _QueryableJoinMethod1 = _QueryableType.GetMethod(_QueryableJoinMethodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, Type.DefaultBinder, new Type[] { _DataSourceType, _QueryableType, _LambdaExpressionType, _LambdaExpressionType, _ExistentParametersType }, null);

                                            if (_QueryableJoinMethod1 == null)
                                                throw new Exception("获取 Queryable 类型的 Join 方法信息出错");
                                        }

                                        DataSourceType joinType = DataSourceType.InnerJoin;

                                        if (methodCallExpression.Method.Name == _QueryableInnerJoinMethodName)
                                            joinType = DataSourceType.InnerJoin;
                                        else if (methodCallExpression.Method.Name == _QueryableLeftJoinMethodName)
                                            joinType = DataSourceType.LeftJoin;
                                        else if (methodCallExpression.Method.Name == _QueryableRightJoinMethodName)
                                            joinType = DataSourceType.RightJoin;
                                        else if (methodCallExpression.Method.Name == _QueryableFullJoinMethodName)
                                            joinType = DataSourceType.FullJoin;
                                        else if (methodCallExpression.Method.Name == _QueryableCrossJoinMethodName)
                                            joinType = DataSourceType.CrossJoin;
                                        else
                                            throw new Exception(string.Format("对 {0} 类型中的 {1} 方法提取字段信息时未能正确提取到它的关联查询类型", methodCallExpression.Method.DeclaringType.FullName, methodCallExpression.Method.ReflectedType.Name));

                                        Queryable rightQueryable = (Queryable)((ConstantField)methodParameters[0]).Value;
                                        LambdaExpression selectorLambda = (LambdaExpression)((ConstantField)methodParameters[1]).Value;
                                        LambdaExpression onLambda = methodParameters.Length == 2 ? null : (LambdaExpression)((ConstantField)methodParameters[2]).Value;

                                        //修改 Join 泛型方法的类型为待执行 Join 方法的泛型
                                        MethodInfo methodInfo = _QueryableJoinMethod1.MakeGenericMethod(methodCallExpression.Method.GetGenericArguments()[0]);

                                        //调用 Join 方法并将得到的 Queryable 对象封装成 ConstantField 字段
                                        resultField = new ConstantField(body.Type, methodInfo.Invoke(queryable, new object[] { joinType, rightQueryable, selectorLambda, onLambda, parameters }));
                                    }
                                }
                            }
                            //若调用后返回值为 MultipleJoin 类型，则需要将得到的 MultipleJoin 对象重置别名
                            else if (body.Type.IsSubclassOf(_MultipleJoinType) && methodParameters != null && (methodParameters.Length == 1 || methodParameters.Length == 2) && methodParameters[0].Type == FieldType.Constant)
                            {
                                //获取 Queryable 类型的 Join 方法
                                if (_QueryableJoinMethod2 == null)
                                {
                                    var joinMethods = _QueryableType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);

                                    if (joinMethods != null && joinMethods.Length > 0)
                                    {
                                        foreach (var item in joinMethods)
                                        {
                                            if (item.Name == _QueryableJoinMethodName && item.IsGenericMethod)
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
                                        throw new Exception("获取 Queryable 类型的 Join 方法信息出错（多表关联）");
                                }

                                DataSourceType joinType = DataSourceType.InnerJoin;

                                if (methodCallExpression.Method.Name == _QueryableInnerJoinMethodName)
                                    joinType = DataSourceType.InnerJoin;
                                else if (methodCallExpression.Method.Name == _QueryableLeftJoinMethodName)
                                    joinType = DataSourceType.LeftJoin;
                                else if (methodCallExpression.Method.Name == _QueryableRightJoinMethodName)
                                    joinType = DataSourceType.RightJoin;
                                else if (methodCallExpression.Method.Name == _QueryableFullJoinMethodName)
                                    joinType = DataSourceType.FullJoin;
                                else if (methodCallExpression.Method.Name == _QueryableCrossJoinMethodName)
                                    joinType = DataSourceType.CrossJoin;

                                Queryable rightQueryable = (Queryable)((ConstantField)methodParameters[0]).Value;
                                LambdaExpression onLambda = methodParameters.Length == 1 ? null : (LambdaExpression)((ConstantField)methodParameters[1]).Value;

                                //修改 Join 泛型方法的类型为待执行 Join 方法的泛型
                                MethodInfo methodInfo = _QueryableJoinMethod2.MakeGenericMethod(methodCallExpression.Method.GetGenericArguments());

                                //调用 Join 方法得到 MultipleJoin 对象
                                MultipleJoin multipleJoin = (MultipleJoin)methodInfo.Invoke(queryable, new object[] { joinType, rightQueryable, onLambda, parameters });

                                //将得到的 MultipleJoin 对象重置别名后封装成 ConstantField 字段
                                resultField = new ConstantField(body.Type, multipleJoin.Copy(ref startAliasIndex));
                            }
                            //若调用后返回值不是 Queryable 类型但调用实例类型是 Queryable 类型，则需要对这些方法进行处理后再进行二次转换
                            else
                            {
                                //First 方法，将转换成子查询
                                if (methodCallExpression.Method.Name == _QueryableFirstMethodName && methodParameters == null)
                                {
                                    queryable = queryable.Limit(new Limit(0, 1));

                                    resultField = (queryable.DataSource.SelectField ?? queryable.DataSource.RootField).ToSubquery(queryable.DataContext, queryable.DataSource, new Dictionary<Field.Field, Field.Field>());
                                }
                                //FirstOrDefault 方法，抛出异常
                                else if (methodCallExpression.Method.Name == _QueryableFirstOrDefaultMethodName && methodParameters == null)
                                {
                                    throw new Exception("子查询不支持 FirstOrDefault 方法，请将其改为 First 实现");
                                }
                                //Last 方法，将转换成子查询
                                else if (methodCallExpression.Method.Name == _QueryableLastMethodName && methodParameters == null)
                                {
                                    if (queryable.DataSource.SortItems == null || queryable.DataSource.SortItems.Count < 1)
                                        throw new Exception(string.Format("对于子查询调用 {0} 方法而言必须指定查询时的排序字段，因为子查询的 {0} 方法内部是将排序字段进行方向排序后再取第一条数据", _QueryableLastMethodName));

                                    SortItem[] sortItems = new SortItem[queryable.DataSource.SortItems.Count];

                                    for (int i = 0; i < queryable.DataSource.SortItems.Count; i++)
                                    {
                                        var item = queryable.DataSource.SortItems[i];

                                        sortItems[i] = new SortItem(item.Field, item.Type == SortType.Ascending ? SortType.Descending : SortType.Ascending);
                                    }

                                    BasicDataSource dataSource = queryable.DataSource.Copy().SetSortItems(sortItems).AddLimit(new Limit(1));

                                    resultField = (dataSource.SelectField ?? dataSource.RootField).ToSubquery(queryable.DataContext, dataSource, new Dictionary<Field.Field, Field.Field>());
                                }
                                //Contains 方法，将 queryable 转换成子查询后再换为 ContainField
                                else if (methodCallExpression.Method.Name == _QueryableContainsMethodName && methodParameters != null && methodParameters.Length == 1)
                                {
                                    Type[] argumentTypes = queryable.GetType().GetGenericArguments();

                                    if (!CheckIsBasicType(argumentTypes[1]))
                                        throw new Exception("被用于子查询或 Where 条件的 Contains 方法时，调用对象 Queryable 的查询结果成员必须是基础数据类型");

                                    resultField = new BinaryField(body.Type, OperationType.In, (BasicField)methodParameters[0], (BasicField)(queryable.DataSource.SelectField ?? queryable.DataSource.RootField).ToSubquery(queryable.DataContext, queryable.DataSource, new Dictionary<Field.Field, Field.Field>()));
                                }
                                else
                                {
                                    throw new Exception(string.Format("子查询不支持 {0}.{1} 方法", methodCallExpression.Method.ReflectedType.GetGenericTypeDefinition().FullName == _ModifiableType.FullName ? "Modifiable" : "Queryable", methodCallExpression.Method.Name));
                                }
                            }
                        }
                        //若调用之前的对象类型是 MultipleJoin 类型
                        else if (methodExampleField.Type == FieldType.Constant && methodCallExpression.Method.ReflectedType.IsSubclassOf(_MultipleJoinType))
                        {
                            //且调用后的返回值也是 MultipleJoin 类型，则需要多调用方法进行二次处理（各种关联方法）
                            if (body.Type.IsSubclassOf(_MultipleJoinType) && methodParameters != null && (methodParameters.Length == 2 || methodParameters.Length == 1) && methodParameters[0].Type == FieldType.Constant)
                            {
                                if (_MultipleJoinJoinMethod == null)
                                {
                                    var joinMethods = _MultipleJoinType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);

                                    if (joinMethods != null && joinMethods.Length > 0)
                                    {
                                        foreach (var item in joinMethods)
                                        {
                                            if (item.Name == _MultipleJoinJoinMethodName && item.IsGenericMethod)
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
                                        throw new Exception("获取 MultipleJoin 类型的 Join 方法信息出错（多表关联）");
                                }

                                DataSourceType joinType = DataSourceType.InnerJoin;

                                if (methodCallExpression.Method.Name == _MultipleJoinInnerJoinMethodName)
                                    joinType = DataSourceType.InnerJoin;
                                else if (methodCallExpression.Method.Name == _MultipleJoinLeftJoinMethodName)
                                    joinType = DataSourceType.LeftJoin;
                                else if (methodCallExpression.Method.Name == _MultipleJoinRightJoinMethodName)
                                    joinType = DataSourceType.RightJoin;
                                else if (methodCallExpression.Method.Name == _MultipleJoinFullJoinMethodName)
                                    joinType = DataSourceType.FullJoin;
                                else if (methodCallExpression.Method.Name == _MultipleJoinCrossJoinMethodName)
                                    joinType = DataSourceType.CrossJoin;

                                Queryable rightQueryable = (Queryable)((ConstantField)methodParameters[0]).Value;
                                LambdaExpression onLambda = methodParameters.Length == 1 ? null : (LambdaExpression)((ConstantField)methodParameters[1]).Value;

                                //修改 Join 泛型方法的类型为待执行 Join 方法的泛型
                                MethodInfo methodInfo = _MultipleJoinJoinMethod.MakeGenericMethod(methodCallExpression.Method.GetGenericArguments());

                                //调用 Join 方法得到 MultipleJoin 对象
                                MultipleJoin multipleJoin = (MultipleJoin)methodInfo.Invoke(((ConstantField)methodExampleField).Value, new object[] { rightQueryable, onLambda, joinType, parameters });

                                //将得到的 MultipleJoin 对象封装成 ConstantField 字段
                                resultField = new ConstantField(body.Type, multipleJoin);
                            }
                            //调用之前的对象类型是 MultipleJoin 类型且返回值为 Queryable 类型
                            else if (body.Type.IsSubclassOf(_QueryableType))
                            {
                                if (_MultipleJoinSelectMethod == null)
                                {
                                    _MultipleJoinSelectMethod = _MultipleJoinType.GetMethod(_MultipleJoinSelectMethodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, Type.DefaultBinder, new Type[] { _LambdaExpressionType, _ExistentParametersType }, null);

                                    if (_MultipleJoinSelectMethod == null)
                                        throw new Exception("获取 MultipleJoin 类型的 Select 方法信息出错");
                                }

                                //修改 Select 泛型方法的类型为待执行 Select 方法的泛型
                                MethodInfo methodInfo = _MultipleJoinSelectMethod.MakeGenericMethod(methodCallExpression.Method.GetGenericArguments());

                                //调用 Select 方法并将得到的 MultipleJoin 对象封装成 ConstantField 字段
                                resultField = new ConstantField(body.Type, methodInfo.Invoke(((ConstantField)methodExampleField).Value, new object[] { ((ConstantField)methodParameters[0]).Value, parameters }));
                            }
                            //不支持其他方法作为子查询
                            else
                            {
                                throw new Exception(string.Format("子查询不支持 {0}.{1} 方法", methodCallExpression.Method.ReflectedType.GetGenericTypeDefinition().FullName == _ModifiableMultipleJoin.FullName ? "ModifiableMultipleJoin" : "MultipleJoin", methodCallExpression.Method.Name));
                            }
                        }
                        //若调用之前的对象类型不是 Queryable 类型但返回值为 Queryable 类型
                        else if (body.Type.IsSubclassOf(_QueryableType))
                        {
                            //若包含了非运行时常量参数则抛出异常
                            if (isExistFieldParameter)
                                throw new Exception(string.Format("对于调用方法获取 Queryable 对象时，调用参数不得包含非运行时常量，调用方法为（实例方法）：{0}.{1}", methodCallExpression.Method.ReflectedType.FullName, methodCallExpression.Method.Name));
                            else
                                resultField = new ConstantField(body.Type, ((Queryable)ExtractConstant(body)).Copy(ref startAliasIndex));
                        }
                        //若调用方法实例是非常量字段或调用参数包含非常量字段，则返回 MethodField 类型字段交由接口实现者处理（实例方法）
                        else if (isExistFieldParameter || methodExampleField.Type != FieldType.Constant || methodCallExpression.Method.GetCustomAttribute<DBFunctionAttribute>(true) != null)
                        {
                            //若调用方法为 IList<?>.Contains 方法
                            if (methodCallExpression.Method.Name == _IListContainsMethodName && methodExampleField.Type == FieldType.Constant && CheckIsList(methodCallExpression.Method.DeclaringType) && methodParameters != null && methodParameters.Length == 1 && methodParameters[0] is BasicField basicParameter && CheckIsBasicType(methodParameters[0].DataType))
                                resultField = new BinaryField(body.Type, OperationType.In, basicParameter, (ConstantField)methodExampleField);
                            else
                                resultField = new MethodField(methodExampleField, methodCallExpression.Method, methodParameters);
                        }
                        //若调用实例是常量且参数也都是常量，那么直接调用该方法提取出常量值返回（实例方法）
                        else
                        {
                            resultField = new ConstantField(body.Type, ExtractConstant(body));
                        }
                    }
                    //若调用之前的对象类型不是 Queryable 类型但返回值为 Queryable 类型，则调用方法得到的 Queryable 对象后再重置别名即可（静态方法）
                    else if (body.Type.IsSubclassOf(_QueryableType))
                    {
                        if (isExistFieldParameter)
                            throw new Exception(string.Format("对于调用方法获取 Queryable 对象时，调用参数不得包含非运行时常量，调用方法为（静态方法）：{0}.{1}", methodCallExpression.Method.ReflectedType.FullName, methodCallExpression.Method.Name));

                        resultField = new ConstantField(body.Type, ((Queryable)ExtractConstant(body)).Copy(ref startAliasIndex));
                    }
                    //若调用之前的对象类型不是 Queryable 类型但返回值为 MultipleJoin 类型，则调用方法得到的 MultipleJoin 对象后再重置别名即可（静态方法）
                    else if (body.Type.IsSubclassOf(_MultipleJoinType))
                    {
                        if (isExistFieldParameter)
                            throw new Exception(string.Format("对于调用方法获取 MultipleJoin 对象时，调用参数不得包含非运行时常量，调用方法为（静态方法）：{0}.{1}", methodCallExpression.Method.ReflectedType.FullName, methodCallExpression.Method.Name));

                        resultField = new ConstantField(body.Type, ((MultipleJoin)ExtractConstant(body)).Copy(ref startAliasIndex));
                    }
                    //若调用方法为 ToNull 方法，直接返回函数参数中的第一个参数值即可
                    else if (methodCallExpression.Method.Name == _HelperToNullMethodName && methodParameters != null && methodParameters.Length == 1 && methodCallExpression.Method.DeclaringType.FullName == _HelperType.FullName)
                    {
                        resultField = methodParameters[0];
                    }
                    //若调用方法为 Like 方法，不管调用实例或参数是否是字段信息都直接转换成 Like 的 BinaryField 字段
                    else if (methodCallExpression.Method.Name == _HelperLikeMethodName && methodCallExpression.Method.DeclaringType.FullName == _HelperType.FullName && methodParameters != null && methodParameters.Length == 2 && methodParameters[0] is BasicField basicObject && methodParameters[1] is BasicField basicRule)
                    {
                        resultField = new BinaryField(body.Type, OperationType.Like, basicObject, basicRule);
                    }
                    //若调用参数包含非常量字段
                    else if (isExistFieldParameter || methodCallExpression.Method.GetCustomAttribute<DBFunctionAttribute>(false) != null)
                    {
                        //若调用方法为 Helper.Contains 方法，只有当调用参数是 Sql 字段时才用 Sql 表示，否则直接调用该方法得到布尔常量值
                        if (methodCallExpression.Method.Name == _HelperContainsMethodName && methodCallExpression.Method.ReflectedType.FullName == _HelperType.FullName && methodParameters != null && methodParameters.Length == 2 && methodParameters[0].Type == FieldType.Constant && methodParameters[1] is BasicField basicParameter)
                            resultField = new BinaryField(body.Type, OperationType.In, basicParameter, (ConstantField)methodParameters[0]);
                        //否则返回 MethodField 类型字段交由接口实现者处理（静态方法）
                        else
                            resultField = new MethodField(null, methodCallExpression.Method, methodParameters);
                    }
                    //若调用参数都是常量，那么直接调用该方法提取出常量值返回（静态方法）
                    else
                    {
                        resultField = new ConstantField(body.Type, ExtractConstant(body));
                    }
                    break;
                case ExpressionType.Conditional:
                    ConditionalExpression conditionalExpression = (ConditionalExpression)body;

                    resultField = new ConditionalField(body.Type, OperationType.Conditional, (BasicField)ExtractField(conditionalExpression.Test, extractType, parameters, ref startAliasIndex), (BasicField)ExtractField(conditionalExpression.IfTrue, extractType, parameters, ref startAliasIndex), (BasicField)ExtractField(conditionalExpression.IfFalse, extractType, parameters, ref startAliasIndex));
                    break;
                case ExpressionType.Constant:
                    ConstantExpression constantExpression = (ConstantExpression)body;

                    resultField = new ConstantField(body.Type, constantExpression.Value);
                    break;
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
                        resultField = new ConstantField(body.Type, unaryExpression.Operand);
                    }
                    else
                    {
                        Field.Field operand = ExtractField(unaryExpression.Operand, extractType, parameters, ref startAliasIndex);

                        if (body.NodeType == ExpressionType.UnaryPlus)
                        {
                            resultField = operand;
                        }
                        else if (body.NodeType == ExpressionType.ArrayLength)
                        {
                            if (operand.Type == FieldType.Constant && operand.DataType.IsArray)
                                resultField = new ConstantField(body.Type, ((Array)((ConstantField)operand).Value).Length);
                            else if (operand.Type == FieldType.Collection)
                                resultField = new ConstantField(body.Type, ((CollectionField)operand).Count);
                            else
                                throw new Exception("获取某个对象的数组长度值时发现被获取的对象字段并非为 CollectionField 字段类型");
                        }
                        else
                        {
                            //将结构体转换成可为 null 类型的操作直接返回，不做任何处理
                            if (body.NodeType == ExpressionType.Convert && body.Type.IsGenericType && body.Type.GetGenericArguments().Length == 1 && body.Type.GetGenericTypeDefinition().FullName == _NullableType.FullName)
                            {
                                resultField = operand;
                            }
                            else
                            {
                                switch (body.NodeType)
                                {
                                    case ExpressionType.Convert:
                                    case ExpressionType.ConvertChecked:
                                        resultField = new UnaryField(body.Type, OperationType.Convert, (BasicField)operand);
                                        break;
                                    case ExpressionType.Negate:
                                    case ExpressionType.NegateChecked:
                                        resultField = new UnaryField(body.Type, OperationType.Negate, (BasicField)operand);
                                        break;
                                    case ExpressionType.Not:
                                        if (operand.Type == FieldType.Binary && (((BinaryField)operand).OperationType == OperationType.In || ((BinaryField)operand).OperationType == OperationType.NotIn))
                                        {
                                            BinaryField binaryField = (BinaryField)operand;

                                            resultField = new BinaryField(binaryField.DataType, binaryField.OperationType == OperationType.In ? OperationType.NotIn : OperationType.In, binaryField.Left, binaryField.Right);
                                        }
                                        else if (operand.Type == FieldType.Binary && (((BinaryField)operand).OperationType == OperationType.Like || ((BinaryField)operand).OperationType == OperationType.NotLike))
                                        {
                                            BinaryField binaryField = (BinaryField)operand;

                                            resultField = new BinaryField(binaryField.DataType, binaryField.OperationType == OperationType.Like ? OperationType.NotLike : OperationType.Like, binaryField.Left, binaryField.Right);
                                        }
                                        else if (operand.Type == FieldType.Unary && ((UnaryField)operand).OperationType == OperationType.Not)
                                        {
                                            resultField = ((UnaryField)operand).Operand;
                                        }
                                        else
                                        {
                                            resultField = new UnaryField(body.Type, OperationType.Not, (BasicField)operand);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case ExpressionType.Switch:
                    SwitchExpression switchExpression = (SwitchExpression)body;

                    Field.Field switchValue = ExtractField(switchExpression.SwitchValue, extractType, parameters, ref startAliasIndex);

                    if (switchValue != null && switchValue is BasicField basicSwitchValue)
                    {
                        List<SwitchCase> switchCases = null;
                        BasicField defaultBody = null;

                        if (switchExpression.Cases != null && switchExpression.Cases.Count > 0)
                        {
                            switchCases = new List<SwitchCase>();

                            foreach (var item in switchExpression.Cases)
                            {
                                Field.Field caseBody = ExtractField(item.Body, extractType, parameters, ref startAliasIndex);

                                if (caseBody != null && caseBody is BasicField basicCaseBody)
                                {
                                    List<ConstantField> testValues = new List<ConstantField>();

                                    foreach (var testValueItem in item.TestValues)
                                    {
                                        Field.Field caseTestValue = ExtractField(testValueItem, extractType, parameters, ref startAliasIndex);

                                        if (caseTestValue != null && caseTestValue.Type == FieldType.Constant)
                                            testValues.Add((ConstantField)caseTestValue);
                                        else
                                            throw new Exception(string.Format("获取某个 Switch 语句中某个分支测定值时获取到的字段{0}，表达式为：{0}", caseBody == null ? "为 null" : "类型不是常量字段", testValueItem.ToString()));
                                    }

                                    switchCases.Add(new SwitchCase(basicCaseBody, (Realize.ReadOnlyList<ConstantField>)testValues));
                                }
                                else
                                {
                                    throw new Exception(string.Format("获取某个 Switch 语句中某个分支返回值主体时获取到的字段{0}，表达式为：{0}", caseBody == null ? "为 null" : "类型不是基础数据类型字段，而对于 Sql 而言分支返回值只能是基础数据类型的字段", item.Body.ToString()));
                                }
                            }
                        }

                        if (switchExpression.DefaultBody != null)
                        {
                            Field.Field tempDefaultBody = ExtractField(switchExpression.DefaultBody, extractType, parameters, ref startAliasIndex);

                            if (tempDefaultBody != null && tempDefaultBody is BasicField basicSwitchDefault)
                                defaultBody = basicSwitchDefault;
                            else
                                throw new Exception(string.Format("获取某个 Switch 语句的默认分支返回值时获取到的字段{0}，表达式为：{0}", tempDefaultBody == null ? "为 null" : "类型不是基础数据类型字段，而对于 Sql 而言分支返回值只能是基础数据类型的字段", switchExpression.DefaultBody.ToString()));
                        }

                        resultField = new SwitchField(body.Type, basicSwitchValue, (Realize.ReadOnlyList<SwitchCase>)switchCases, defaultBody);
                    }
                    else
                    {
                        throw new Exception(string.Format("获取某个 Switch 语句的判定值字段时获取到的字段{0}，表达式为：{0}", switchValue == null ? "为 null" : "类型不是基础数据类型字段，而对于 Sql 而言判断值只能是基础数据类型的字段", body.ToString()));
                    }
                    break;
                case ExpressionType.ListInit:
                    ListInitExpression listInitExpression = (ListInitExpression)body;

                    if (listInitExpression.NewExpression != null)
                    {
                        resultField = ExtractField(listInitExpression.NewExpression, extractType, parameters, ref startAliasIndex);

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

                                members.Add(ExtractField(item.Arguments[0], extractType, parameters, ref startAliasIndex));
                            }

                            resultField = new CollectionField(body.Type, collectionField.ConstructorInfo, addMethodInfo, members);
                        }
                    }
                    break;
                case ExpressionType.MemberAccess:
                    MemberExpression memberExpression = (MemberExpression)body;

                    if (memberExpression.Expression != null)
                    {
                        Field.Field exampleField = ExtractField(memberExpression.Expression, extractType, parameters, ref startAliasIndex);

                        if (exampleField.Type == FieldType.Object)
                        {
                            ObjectField objectField = (ObjectField)exampleField;

                            if (objectField.Members == null || objectField.Members.Count < 1 || !objectField.Members.TryGetValue(memberExpression.Member.Name, out MemberInfo memberInfo))
                                throw new Exception(string.Format("未能找到指定 {0} 类型对象中的 {1} 成员所对应的字段信息", memberExpression.Member.DeclaringType.FullName, memberExpression.Member.Name));

                            resultField = memberInfo.Field;
                        }
                        else if (exampleField.Type == FieldType.Constant)
                        {
                            //如果常量对象是 Queryable 类型，则要将其单独处理后转换成子查询字段
                            if (exampleField.DataType.IsSubclassOf(_QueryableType))
                            {
                                //Count 属性
                                if (memberExpression.Member.Name == _QueryableCountPropertyName && memberExpression.Member.MemberType == MemberTypes.Property)
                                {
                                    Queryable queryable = (Queryable)((ConstantField)exampleField).Value;

                                    if (_DBFunCountMethod == null)
                                        _DBFunCountMethod = typeof(DBFun).GetMethod(nameof(DBFun.Count), BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, Type.DefaultBinder, new Type[0], null);

                                    //将查询字段改成数据库的 count 函数
                                    queryable.DataSource.SetSelectField(new MethodField(null, _DBFunCountMethod, null));

                                    resultField = new SubqueryField(body.Type, (BasicField)queryable.DataSource.SelectField, queryable.DataSource);
                                }
                            }
                            else
                            {
                                object constantValue = ((ConstantField)exampleField).Value;

                                //如果是 MultipleJoin 类型，且属性为 LeftObjectInfo.Left 或 LeftObjectInfo.Right 时单独分开处理
                                if (constantValue != null && constantValue is MultipleJoin multipleJoin && (memberExpression.Member.Name == _LeftObjectInfoLeftPropName || memberExpression.Member.Name == _LeftObjectInfoRightPropName))
                                    resultField = ConvertParameter(body.Type, extractType, false, memberExpression.Member.Name == _LeftObjectInfoLeftPropName ? multipleJoin.Left : multipleJoin.Right);
                                //否则直接取值返回
                                else if (memberExpression.Member.MemberType == MemberTypes.Property)
                                    resultField = new ConstantField(body.Type, ((PropertyInfo)memberExpression.Member).GetValue(constantValue, null));
                                else if (memberExpression.Member.MemberType == MemberTypes.Field)
                                    resultField = new ConstantField(body.Type, ((FieldInfo)memberExpression.Member).GetValue(constantValue));
                            }
                        }
                        else if (exampleField.Type == FieldType.Collection && ((memberExpression.Member.Name == "Length" && exampleField.DataType.IsArray) || (memberExpression.Member.Name == "Count" && CheckIsList(exampleField.DataType))))
                        {
                            resultField = new ConstantField(body.Type, ((CollectionField)exampleField).Count);
                        }

                        //若没有正确提取到字段信息则返回 MemberField 类型的字段交由接口实现者处理
                        if (resultField == null)
                            resultField = new MemberField(body.Type, exampleField, memberExpression.Member);
                    }
                    else if (memberExpression.Member.MemberType == MemberTypes.Property)
                    {
                        resultField = new ConstantField(body.Type, ((PropertyInfo)memberExpression.Member).GetValue(null, null));
                    }
                    else if (memberExpression.Member.MemberType == MemberTypes.Field)
                    {
                        resultField = new ConstantField(body.Type, ((FieldInfo)memberExpression.Member).GetValue(null));
                    }
                    else
                    {
                        resultField = new MemberField(body.Type, null, memberExpression.Member);
                    }

                    if (resultField != null && resultField.Type == FieldType.Constant && resultField.DataType.IsGenericType)
                    {
                        object constantvalue = ((ConstantField)resultField).Value;

                        //若得到的是 MultipleJoin 对象，则需要将该对象重置别名后再返回
                        if (constantvalue != null && resultField.DataType.IsSubclassOf(_MultipleJoinType))
                            resultField = new ConstantField(resultField.DataType, ((MultipleJoin)constantvalue).Copy(ref startAliasIndex));
                        //若得到的是 Queryable 对象，也需要将该对象重置别名后再返回
                        else if (constantvalue != null && resultField.DataType.IsSubclassOf(_QueryableType))
                            resultField = new ConstantField(resultField.DataType, ((Queryable)constantvalue).Copy(ref startAliasIndex));
                    }
                    break;
                case ExpressionType.MemberInit:
                    MemberInitExpression memberInitExpression = (MemberInitExpression)body;

                    if (memberInitExpression.NewExpression != null)
                    {
                        Field.Field newField = ExtractField(memberInitExpression.NewExpression, extractType, parameters, ref startAliasIndex);

                        if (newField is ObjectField objectField)
                        {
                            Dictionary<string, MemberInfo> members = new Dictionary<string, MemberInfo>();

                            if (objectField.Members == null || objectField.Members.Count < 1)
                            {
                                foreach (var item in objectField.Members)
                                {
                                    members.Add(item.Key, item.Value);
                                }
                            }

                            foreach (var item in memberInitExpression.Bindings)
                            {
                                if (item.BindingType != MemberBindingType.Assignment)
                                    throw new Exception(string.Format("从某一表达式树中的成员初始化节点中提取初始化成员信息时出错，表达式为：{0}", body.ToString()));

                                Field.Field field = ExtractField(((MemberAssignment)item).Expression, extractType, parameters, ref startAliasIndex);

                                members[item.Member.Name] = new MemberInfo(item.Member, field);
                            }

                            resultField = new ObjectField(body.Type, objectField.ConstructorInfo, members, true);
                        }
                        else if (newField.Type == FieldType.Constant)
                        {
                            foreach (var item in memberInitExpression.Bindings)
                            {
                                if (item.BindingType != MemberBindingType.Assignment)
                                    throw new Exception(string.Format("从某一表达式树中的成员初始化节点中提取初始化成员信息时出错，表达式为：{0}", body.ToString()));

                                Field.Field field = ExtractField(((MemberAssignment)item).Expression, extractType, parameters, ref startAliasIndex);

                                if (field.Type != FieldType.Constant)
                                    throw new Exception(string.Format("常量对象在初始化属性时属性值不能是非运行时常量，表达式为：{0}", body.ToString()));

                                if (item.Member.MemberType == MemberTypes.Property)
                                    ((PropertyInfo)item.Member).SetValue(((ConstantField)newField).Value, ((ConstantField)field).Value, null);
                                else if (item.Member.MemberType == MemberTypes.Field)
                                    ((FieldInfo)item.Member).SetValue(((ConstantField)newField).Value, ((ConstantField)field).Value);
                            }

                            resultField = newField;
                        }
                    }
                    break;
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
                            Field.Field itemParameter = ExtractField(item, extractType, parameters, ref startAliasIndex);

                            arguments.Add(itemParameter);

                            if (itemParameter.Type != FieldType.Constant)
                                isExistFieldParameter = true;
                            else if (!isExistFieldParameter)
                                constructorArgs[i++] = ((ConstantField)itemParameter).Value;
                        }
                    }

                    //若是 new Guid(string)
                    if (resultField == null && body.Type.FullName == _GuidType.FullName && arguments.Count == 1 && arguments[0].DataType.FullName == _StringType.FullName && arguments[0] is BasicField)
                    {
                        resultField = new UnaryField(_GuidType, OperationType.Convert, (BasicField)arguments[0]);
                    }
                    //若是 new DateTime()
                    else if (resultField == null && body.Type.FullName == _DateTimeType.FullName && (arguments.Count == 3 || arguments.Count == 6 || arguments.Count == 7))
                    {
                        bool isNeedConvert = true;

                        foreach (var item in arguments)
                        {
                            if (item.DataType.FullName != _Int32Type.FullName)
                            {
                                isNeedConvert = false;

                                break;
                            }
                        }

                        if (isNeedConvert)
                        {
                            if (isExistFieldParameter)
                            {
                                ConstantField delimiterField = new ConstantField(_StringType, "-");

                                BinaryField parameterField = parameterField = new BinaryField(_StringType, OperationType.Add, new UnaryField(_StringType, OperationType.Convert, (BasicField)arguments[0]), delimiterField);
                                parameterField = new BinaryField(_StringType, OperationType.Add, parameterField, new UnaryField(_StringType, OperationType.Convert, (BasicField)arguments[1]));
                                parameterField = new BinaryField(_StringType, OperationType.Add, parameterField, delimiterField);
                                parameterField = new BinaryField(_StringType, OperationType.Add, parameterField, new UnaryField(_StringType, OperationType.Convert, (BasicField)arguments[2]));

                                if (arguments.Count > 3)
                                {
                                    delimiterField = new ConstantField(_StringType, ":");

                                    parameterField = new BinaryField(_StringType, OperationType.Add, parameterField, new ConstantField(_StringType, " "));
                                    parameterField = new BinaryField(_StringType, OperationType.Add, parameterField, new UnaryField(_StringType, OperationType.Convert, (BasicField)arguments[3]));
                                    parameterField = new BinaryField(_StringType, OperationType.Add, parameterField, delimiterField);
                                    parameterField = new BinaryField(_StringType, OperationType.Add, parameterField, new UnaryField(_StringType, OperationType.Convert, (BasicField)arguments[4]));
                                    parameterField = new BinaryField(_StringType, OperationType.Add, parameterField, delimiterField);
                                    parameterField = new BinaryField(_StringType, OperationType.Add, parameterField, new UnaryField(_StringType, OperationType.Convert, (BasicField)arguments[5]));
                                }

                                resultField = new UnaryField(_DateTimeType, OperationType.Convert, parameterField);

                                if (arguments.Count == 7)
                                {
                                    if (_DBFunAddMillisecondMethod == null)
                                        _DBFunAddMillisecondMethod = _DBFunType.GetMethod(_DBFunAddMillisecondMethodName, BindingFlags.Static | BindingFlags.Public);

                                    resultField = new MethodField(null, _DBFunAddMillisecondMethod, new Realize.ReadOnlyList<Field.Field>(resultField, arguments[6]));
                                }
                            }
                            else
                            {
                                resultField = new ConstantField(body.Type, newExpression.Constructor.Invoke(constructorArgs));
                            }
                        }
                    }

                    if (resultField == null)
                    {
                        //若是数组或集合，则返回 CollectionField 类型的字段，否则返回 ObjectField 类型的字段
                        if (body.Type.IsArray || CheckIsList(body.Type))
                        {
                            if (!body.Type.IsArray && arguments != null && arguments.Count > 0)
                                throw new Exception(string.Format("在提取某一指定表达式树中的字段信息时，初始化 List<T> 实例不能有构造参数，具体表达式为：{0}", body.ToString()));

                            resultField = new CollectionField(body.Type, new ConstructorInfo(newExpression.Constructor, arguments));
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

                            resultField = new ObjectField(body.Type, new ConstructorInfo(newExpression.Constructor, arguments), members);
                        }
                    }
                    break;
                case ExpressionType.NewArrayInit:
                    NewArrayExpression newArrayExpression = (NewArrayExpression)body;

                    System.Reflection.ConstructorInfo[] constructorInfos = body.Type.GetConstructors();

                    if (constructorInfos == null || constructorInfos.Length != 1)
                        throw new Exception("未能获取到 .NET 数组类的构造函数信息");

                    var constructorParameters = constructorInfos[0].GetParameters();

                    if (constructorParameters == null || constructorParameters.Length != 1 || constructorParameters[0].ParameterType.FullName != _Int32Type.FullName)
                        throw new Exception("获取到的 .NET 数组类构造函数参数个数或类型不正确");

                    if (newArrayExpression.Expressions != null && newArrayExpression.Expressions.Count > 0)
                    {
                        List<Field.Field> members = new List<Field.Field>();

                        foreach (var item in newArrayExpression.Expressions)
                        {
                            members.Add(ExtractField(item, extractType, parameters, ref startAliasIndex));
                        }

                        resultField = new CollectionField(body.Type, new ConstructorInfo(constructorInfos[0], null), members);
                    }
                    else
                    {
                        resultField = new CollectionField(body.Type, new ConstructorInfo(constructorInfos[0], null));
                    }
                    break;
                case ExpressionType.Parameter:
                    if (parameters == null)
                        throw new Exception("在提取指定表达式树中的字段信息时需要提取函数的参数信息，但是该函数并未传递任何参数");

                    ParameterExpression parameterExpression = (ParameterExpression)body;

                    if (parameters.TryGetValue(parameterExpression.Name, out ParameterInfo parameterInfo))
                        resultField = ConvertParameter(body.Type, extractType, parameterInfo.IsMain, parameterInfo.Parameter);
                    break;
            }

            if (resultField == null)
                throw new Exception(string.Format("未能从指定的表达式树中提取出有用的字段信息，表达式为：{0}", body.ToString()));

            return resultField;
        }

        /// <summary>
        /// 根据指定的实体类型获取该实体类型对应的数据源。
        /// </summary>
        /// <param name="dataContext">数据操作上下文。</param>
        /// <param name="entityType">待获取数据源的实体类型。</param>
        /// <param name="dataSourceType">需要获取的数据源类型。</param>
        /// <returns>该实体类型对应的数据源。</returns>
        internal static OriginalDataSource GetDataSource(IDataContext dataContext, Type entityType, DataSourceType dataSourceType)
        {
            if (dataSourceType != DataSourceType.Table && dataSourceType != DataSourceType.View)
                throw new Exception(string.Format("不支持获取实体类对应的 {0} 类型数据源", dataSourceType.ToString()));

            var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var fields = entityType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            if ((properties == null || properties.Length < 1) && (fields == null || fields.Length < 1))
                throw new Exception(string.Format("未能获取到 {0} 类型所映射到数据库表或视图的字段信息", entityType.FullName));

            var constructor = entityType.GetConstructor(new Type[0]);

            if (constructor == null)
                throw new Exception(string.Format("{0} 实体类没有无参构造函数，不能作为操作数据源的实体类型", entityType.FullName));

            var mappingInfo = entityType.GetCustomAttribute<MappingAttribute>(false) ?? new MappingAttribute(dataContext.SqlFactory.EncodeName(entityType.Name, dataSourceType == DataSourceType.Table ? NameType.Table : NameType.View), MappingType.PublicProperty);
            var members = new Dictionary<string, MemberInfo>();
            MemberInfo primaryKey = null;
            MemberInfo autoincrement = null;

            ForEach<System.Reflection.MemberInfo>(item =>
            {
                var fieldInfo = item.GetCustomAttribute<FieldAttribute>(true);

                if ((mappingInfo.MappingType == MappingType.IgnoreMarked && fieldInfo != null) || (mappingInfo.MappingType == MappingType.OnlyMarked && fieldInfo == null))
                {
                    return;
                }
                else
                {
                    if (fieldInfo == null)
                        fieldInfo = new FieldAttribute(dataContext.SqlFactory.EncodeName(item.Name, NameType.Field));

                    Type memberType = item.MemberType == MemberTypes.Field ? ((FieldInfo)item).FieldType : item.MemberType == MemberTypes.Method ? ((MethodInfo)item).ReturnType : ((PropertyInfo)item).PropertyType;

                    if (!CheckIsBasicType(memberType))
                        throw new Exception(string.Format("不支持非基础数据类型的成员映射，若要忽略映射该成员，请配合使用 Attribute.MappingAttribute 和 Attribute.FieldAttribute 标记进行筛选，具体成员名称为：{0}.{1}", entityType.FullName, item.Name));

                    if (fieldInfo.IsNullable == FieldNullableMode.Unknown)
                        fieldInfo.IsNullable = memberType.IsValueType && !memberType.IsGenericType ? FieldNullableMode.NotNullable : FieldNullableMode.Nullable;

                    if (string.IsNullOrWhiteSpace(fieldInfo.DataType))
                        fieldInfo.DataType = dataContext.NetTypeToDBType(memberType);

                    if (string.IsNullOrWhiteSpace(fieldInfo.Name))
                        fieldInfo.Name = dataContext.SqlFactory.EncodeName(item.Name, NameType.Field);

                    MemberInfo memberInfo = new MemberInfo(item, new OriginalField(memberType, fieldInfo, dataContext.SqlFactory.GenerateAlias(members.Count, NameType.Field)));

                    if (fieldInfo.IsPrimaryKey)
                    {
                        if (primaryKey != null)
                            throw new Exception("当前框架暂不支持一张表或视图存在有多个主键的情况");

                        primaryKey = memberInfo;
                    }

                    if (fieldInfo.IsAutoincrement)
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
                                throw new Exception(string.Format("自增字段只能映射到整数类型的成员上，具体成员名称为：{0}.{1}", entityType.FullName, item.Name));
                        }
                    }

                    members.Add(item.Name, memberInfo);
                }
            }, properties, fields);

            return new OriginalDataSource(dataContext, dataSourceType, new ObjectField(entityType, new ConstructorInfo(constructor, null), members, true), primaryKey, autoincrement, 0, string.IsNullOrWhiteSpace(mappingInfo.Name) ? dataContext.SqlFactory.EncodeName(entityType.Name, dataSourceType == DataSourceType.Table ? NameType.Table : NameType.View) : mappingInfo.Name);
        }

        /// <summary>
        /// 重新设置指定字段的别名。
        /// </summary>
        /// <param name="dataContext">数据操作上下文。</param>
        /// <param name="setFields">已重新设置过别名的字段集合。</param>
        /// <param name="field">待重置别名的字段信息。</param>
        /// <param name="startAliasIndex">起始别名下标。</param>
        internal static void ResetFieldAlias(IDataContext dataContext, HashSet<Field.Field> setFields, Field.Field field, ref int startAliasIndex)
        {
            if (!setFields.Contains(field))
            {
                if (field.Type == FieldType.Object)
                {
                    ObjectField objectField = (ObjectField)field;

                    if (objectField.ConstructorInfo.Parameters != null && objectField.ConstructorInfo.Parameters.Count > 0)
                    {
                        foreach (var item in objectField.ConstructorInfo.Parameters)
                        {
                            ResetFieldAlias(dataContext, setFields, item, ref startAliasIndex);
                        }
                    }

                    if (objectField.Members != null && objectField.Members.Count > 0)
                    {
                        foreach (var item in objectField.Members)
                        {
                            ResetFieldAlias(dataContext, setFields, item.Value.Field, ref startAliasIndex);
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
                            ResetFieldAlias(dataContext, setFields, item, ref startAliasIndex);
                        }
                    }

                    foreach (var item in collectionField)
                    {
                        ResetFieldAlias(dataContext, setFields, item, ref startAliasIndex);
                    }
                }
                else if (field is BasicField basicField)
                {
                    if (!setFields.Contains(field))
                    {
                        basicField.ModifyAlias(dataContext.SqlFactory.GenerateAlias(startAliasIndex, NameType.Field));

                        setFields.Add(field);

                        startAliasIndex++;
                    }
                }
                else
                {
                    throw new Exception("对字段重置别名时出现未知字段类型");
                }

                setFields.Add(field);
            }
        }

        /// <summary>
        /// 将指定的字段转换成引用字段。
        /// </summary>
        /// <param name="field">待转换的字段信息。</param>
        /// <param name="dataSource">转换后的引用字段归属数据源。</param>
        /// <param name="convertedFields">已转换过的字段集合。</param>
        /// <returns>转换后的字段信息。</returns>
        internal static Field.Field ToQuoteField(Field.Field field, Dictionary<Field.Field, Field.Field> convertedFields, BasicDataSource dataSource)
        {
            if (!convertedFields.TryGetValue(field, out Field.Field convertedField))
            {
                if (field.Type == FieldType.Object)
                {
                    ObjectField objectField = (ObjectField)field;

                    List<Field.Field> arguments = null;
                    Dictionary<string, MemberInfo> members = null;

                    if (objectField.ConstructorInfo != null && objectField.ConstructorInfo.Parameters != null && objectField.ConstructorInfo.Parameters.Count > 0)
                    {
                        arguments = new List<Field.Field>();

                        foreach (var item in objectField.ConstructorInfo.Parameters)
                        {
                            if (!convertedFields.TryGetValue(item, out convertedField))
                                convertedField = ToQuoteField(item, convertedFields, dataSource);

                            arguments.Add(convertedField);
                        }
                    }

                    if (objectField.Members != null && objectField.Members.Count > 0)
                    {
                        members = new Dictionary<string, MemberInfo>();

                        foreach (var item in objectField.Members)
                        {
                            if (!convertedFields.TryGetValue(item.Value.Field, out convertedField))
                                convertedField = ToQuoteField(item.Value.Field, convertedFields, dataSource);

                            members.Add(item.Key, new MemberInfo(item.Value.Member, convertedField));
                        }
                    }

                    convertedField = new ObjectField(objectField.DataType, new ConstructorInfo(objectField.ConstructorInfo.Constructor, arguments), members, objectField.IsNeededInitMembers);
                }
                else if (field.Type == FieldType.Collection)
                {
                    CollectionField collectionField = (CollectionField)field;

                    List<Field.Field> members = null;
                    List<Field.Field> parameters = null;

                    if (collectionField.ConstructorInfo.Parameters != null && collectionField.ConstructorInfo.Parameters.Count > 0)
                    {
                        parameters = new List<Field.Field>();

                        foreach (var item in collectionField.ConstructorInfo.Parameters)
                        {
                            if (!convertedFields.TryGetValue(item, out convertedField))
                                convertedField = ToQuoteField(item, convertedFields, dataSource);

                            parameters.Add(convertedField);
                        }
                    }

                    if (collectionField.Count > 0)
                    {
                        members = new List<Field.Field>();

                        foreach (var item in collectionField)
                        {
                            if (!convertedFields.TryGetValue(item, out convertedField))
                                convertedField = ToQuoteField(item, convertedFields, dataSource);

                            members.Add(convertedField);
                        }
                    }

                    convertedField = new CollectionField(collectionField.DataType, new ConstructorInfo(collectionField.ConstructorInfo.Constructor, parameters), collectionField.AddMethodInfo, members);
                }
                else
                {
                    convertedField = new QuoteField((BasicField)field, dataSource);
                }

                convertedFields[field] = convertedField;
            }

            return convertedField;
        }

        /// <summary>
        /// 校验指定类型是否为基础数据类型。
        /// </summary>
        /// <param name="type">待校验的数据类型。</param>
        /// <returns>若该类型为基础数据类型时返回 true，否则返回 false。</returns>
        public static bool CheckIsBasicType(Type type)
        {
            return type.IsPrimitive || type.IsEnum || type == _DecimalType || type == _DateTimeOffsetType || type == _TimeSpanType || type == _DateTimeType || type == _StringType || type == _GuidType || (type.IsGenericType && type.GetGenericTypeDefinition() == _NullableType && CheckIsBasicType(type.GetGenericArguments()[0]));
        }

        /// <summary>
        /// 将当前不可为 null 值的结构对象转换成可为 null 的类型对象。
        /// </summary>
        /// <typeparam name="T">结构体类型。</typeparam>
        /// <param name="self">待转换的结构体对象。</param>
        /// <returns>转换后的可为 null 的对象。</returns>
        public static T? ToNull<T>(this T self) where T : struct
        {
            return (T?)self;
        }

        /// <summary>
        /// 确认当前集合中是否包含某一指定成员。
        /// </summary>
        /// <typeparam name="TItem">集合中的成员类型。</typeparam>
        /// <param name="self">调用该方法的实例对象。</param>
        /// <param name="item">待确认是否存在于当前集合中的对象。</param>
        /// <returns>若当前集合中存在指定成员则返回 true，否则返回 false。</returns>
        public static bool Contains<TItem>(this IEnumerable<TItem> self, TItem item) where TItem : struct
        {
            if (self != null)
            {
                foreach (var obj in self)
                {
                    if (obj.Equals(item))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 确认当前集合中是否包含某一指定成员。
        /// </summary>
        /// <param name="self">调用该方法的实例对象。</param>
        /// <param name="rule">待确认是否存在于当前集合中的对象。</param>
        /// <returns>若当前集合中存在指定成员则返回 true，否则返回 false。</returns>
        public static bool Contains(this IEnumerable<string> self, string rule)
        {
            if (self != null)
            {
                foreach (var obj in self)
                {
                    if (obj == rule)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 校验当前字符串是否符合指定格式（暂不支持直接在 .Net 框架中调用，后续版本会跟进）。
        /// </summary>
        /// <param name="self">调用该方法的实例对象。</param>
        /// <param name="rule">需要符合的规则字符串。</param>
        /// <returns>若当前字符串满足该规则条件时返回 true，否则返回 false。</returns>
        public static bool Like(this string self, string rule)
        {
            throw new Exception(string.Format("暂不支持直接在 .Net 框架中调用 {0}.{1} 方法，后续版本会跟进", _HelperType.FullName, _HelperLikeMethodName));
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
        /// 获取指定实体类成员信息上的自定义标记信息。
        /// </summary>
        /// <typeparam name="TAttribute">待获取的标记类型。</typeparam>
        /// <param name="memberInfo">需要获取自定义标记的成员信息。</param>
        /// <param name="inherit">是否应当搜索此成员继承链以查找标记信息。</param>
        /// <returns>若找到指定类型的标记信息则返回第一个标记信息，否则返回 null。</returns>
        public static TAttribute GetCustomAttribute<TAttribute>(this System.Reflection.MemberInfo memberInfo, bool inherit) where TAttribute : System.Attribute
        {
            return GetCustomAttribute<TAttribute>(memberInfo.GetCustomAttributes, inherit);
        }

        /// <summary>
        /// 获取指定类型上的自定义标记信息。
        /// </summary>
        /// <typeparam name="TAttribute">待获取的标记类型。</typeparam>
        /// <param name="typeInfo">需要获取标记信息的类型。</param>
        /// <param name="inherit">是否应当搜索此成员继承链以查找标记信息。</param>
        /// <returns>若找到指定类型的标记信息则返回第一个标记信息，否则返回 null。</returns>
        public static TAttribute GetCustomAttribute<TAttribute>(this Type typeInfo, bool inherit) where TAttribute : System.Attribute
        {
            return GetCustomAttribute<TAttribute>(typeInfo.GetCustomAttributes, inherit);
        }

        /// <summary>
        /// 转换提取到的参数信息。
        /// </summary>
        /// <param name="resultType">提取到的参数值类型。</param>
        /// <param name="extractType">提取类型。</param>
        /// <param name="isMainParameter">是否是主参数。</param>
        /// <param name="parameter">参数值。</param>
        /// <returns>转换后的参数信息。</returns>
        private static Field.Field ConvertParameter(Type resultType, ExtractType extractType, bool isMainParameter, object parameter)
        {
            if (parameter is BasicDataSource dataSource)
            {
                if (extractType == ExtractType.Select)
                    return ToQuoteField(dataSource.SelectField ?? dataSource.RootField, new Dictionary<Field.Field, Field.Field>(), dataSource);
                else if (isMainParameter && (extractType == ExtractType.Default || ((dataSource.GroupFields == null || dataSource.GroupFields.Count < 1) && extractType == ExtractType.Group)))
                    return dataSource.RootField;
                else if (extractType == ExtractType.DataSource)
                    return new ConstantField(resultType, dataSource);
                else
                    return ToQuoteField(dataSource.RootField, new Dictionary<Field.Field, Field.Field>(), dataSource);
            }
            else
            {
                return new ConstantField(resultType, (MultipleJoin)parameter);
            }
        }

        /// <summary>
        /// 使用获取到自定义标记的方法筛选出指定类型的标记信息。
        /// </summary>
        /// <typeparam name="TAttribute">目标标记类型。</typeparam>
        /// <param name="getAttributesFunction">获取标记信息的方法。</param>
        /// <param name="inherit">是否应当搜索此成员继承链以查找标记信息。</param>
        /// <returns>若找到指定类型的标记信息则返回第一个标记信息，否则返回 null。</returns>
        private static TAttribute GetCustomAttribute<TAttribute>(Func<Type, bool, object[]> getAttributesFunction, bool inherit) where TAttribute : System.Attribute
        {
            object[] attrs = getAttributesFunction(typeof(TAttribute), inherit);

            if (attrs == null)
            {
                return null;
            }
            else
            {
                foreach (var item in attrs)
                {
                    return (TAttribute)item;
                }
            }

            return null;
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

            throw new Exception(string.Format("未能从指定表达式树中提取出常量值，通常存在子查询、连接查询或 Where 子句中使用到子查询时需要提取常量值，而在提取常量值时不得涉及对拉姆达表达式参数，具体表达式为：{0}", body.ToString()));
        }

        /// <summary>
        /// 校验指定类型是否是 <see cref="IList{T}"/> 类型。
        /// </summary>
        /// <param name="type">需要校验的类型。</param>
        /// <returns>若该类型是 IList 类型则返回 true，否则返回 false。</returns>
        private static bool CheckIsList(Type type)
        {
            if (type.IsGenericType)
            {
                var typeArgs = type.GetGenericArguments();

                return typeArgs.Length == 1 && _IListType.MakeGenericType(typeArgs).IsAssignableFrom(type);
            }

            return false;
        }

        /// <summary>
        /// 校验指定的值是否为默认值。
        /// </summary>
        /// <param name="value">需要校验的值。</param>
        /// <returns>若该值为对应类型的默认值则返回 true，否则返回 false。</returns>
        public static bool CheckIsDefault(object value)
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

            Type valueType = value.GetType();

            if (valueType.IsValueType)
                return valueType.Equals(Activator.CreateInstance(valueType));

            return false;
        }

        /// <summary>
        /// 循环所有枚举器中的成员信息。
        /// </summary>
        /// <typeparam name="TItem">枚举器对象中每个成员的类型。</typeparam>
        /// <param name="items">需要循环的枚举器数组。</param>
        /// <param name="loopFun">循环执行函数。</param>
        public static void ForEach<TItem>(Action<TItem> loopFun, params IEnumerable<TItem>[] items)
        {
            if (items != null)
            {
                foreach (var enumerable in items)
                {
                    if (enumerable != null)
                    {
                        foreach (var item in enumerable)
                        {
                            loopFun(item);
                        }
                    }
                }
            }
        }
    }
}
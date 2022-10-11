using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Enum
{
    /// <summary>
    /// 操作类型枚举。
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 三元运算（条件运算）操作。
        /// </summary>
        Conditional = 03001,
        /// <summary>
        /// 条件或操作。
        /// </summary>
        OrElse = 04001,
        /// <summary>
        /// 条件与操作。
        /// </summary>
        AndAlso = 05001,
        /// <summary>
        /// 按位或操作。
        /// </summary>
        Or = 06001,
        /// <summary>
        /// 按位异或操作。
        /// </summary>
        ExclusiveOr = 07001,
        /// <summary>
        /// 按位与操作。
        /// </summary>
        And = 08001,
        /// <summary>
        /// 条件不等于判断操作。
        /// </summary>
        NotEqual = 09001,
        /// <summary>
        /// 条件等于判断操作。
        /// </summary>
        Equal = 09002,
        /// <summary>
        /// 条件大于判断操作。
        /// </summary>
        GreaterThan = 10001,
        /// <summary>
        /// 条件大于等于判断操作。
        /// </summary>
        GreaterThanOrEqual = 10002,
        /// <summary>
        /// 条件小于判断操作。
        /// </summary>
        LessThan = 10003,
        /// <summary>
        /// 条件小于等于判断操作。
        /// </summary>
        LessThanOrEqual = 10004,
        /// <summary>
        /// 按位右移操作。
        /// </summary>
        RightShift = 11001,
        /// <summary>
        /// 按位左移操作。
        /// </summary>
        LeftShift = 11002,
        /// <summary>
        /// 数学加法操作。
        /// </summary>
        Add = 12001,
        /// <summary>
        /// 数学减法操作。
        /// </summary>
        Subtract = 12002,
        /// <summary>
        /// 数学取余操作。
        /// </summary>
        Modulo = 13001,
        /// <summary>
        /// 数学乘法操作。
        /// </summary>
        Multiply = 13002,
        /// <summary>
        /// 数学除法操作。
        /// </summary>
        Divide = 13003,
        /// <summary>
        /// 条件包含操作。
        /// </summary>
        In = 13004,
        /// <summary>
        /// 条件不包含操作。
        /// </summary>
        NotIn = 13005,
        /// <summary>
        /// 字符串模糊比较操作。
        /// </summary>
        Like = 13006,
        /// <summary>
        /// 字符串模糊比较取反操作。
        /// </summary>
        NotLike = 13007,
        /// <summary>
        /// 数学取反操作。
        /// </summary>
        Negate = 14001,
        /// <summary>
        /// 按位取反操作（对于布尔类型而言则为逻辑取反操作）。
        /// </summary>
        Not = 14002,
        /// <summary>
        /// 一个默认操作（空操作）。
        /// </summary>
        Default = 1000001,
        /// <summary>
        /// 数组长度引用操作。
        /// </summary>
        ArrayLength = 1000002,
        /// <summary>
        /// 数组指定索引处的成员引用操作。
        /// </summary>
        ArrayIndex = 1000003,
        /// <summary>
        /// 方法调用操作。
        /// </summary>
        Call = 1000004,
        /// <summary>
        /// 空值合并操作。
        /// </summary>
        Coalesce = 1000005,
        /// <summary>
        /// 类型转换操作。
        /// </summary>
        Convert = 1000006,
        /// <summary>
        /// 数学幂操作。
        /// </summary>
        Power = 1000007
    }
}

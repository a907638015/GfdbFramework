using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Enum
{
    /// <summary>
    /// 数据源合并类型枚举。
    /// </summary>
    public enum UnionType
    {
        /// <summary>
        /// 对两个结果集进行并集操作，不包括重复行，同时进行默认规则的排序。
        /// </summary>
        Union = 0,
        /// <summary>
        /// 对两个结果集进行并集操作，包括重复行，不进行排序。
        /// </summary>
        UnionALL = 1,
        /// <summary>
        /// 对两个结果集进行交集操作，不包括重复行，同时进行默认规则的排序。
        /// </summary>
        Intersect = 2,
        /// <summary>
        /// 对两个结果集进行差操作，不包括重复行，同时进行默认规则的排序。
        /// </summary>
        Minus = 3
    }
}

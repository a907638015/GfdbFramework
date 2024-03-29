﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Enum
{
    /// <summary>
    /// 
    /// </summary>
    public enum FieldType
    {
        /// <summary>
        /// 原始数据字段（表或视图字段）。
        /// </summary>
        Original = 0,
        /// <summary>
        /// .Net 对象字段（该类型字段作为查询字段时，需查询 ConstructorInfo.Parameters[?] 、 InitMembers[?].Field 、 Members.Values 属性中的所有字段）。
        /// </summary>
        Object = 1,
        /// <summary>
        /// 数组或集合字段（该类型字段作为查询字段时，需查询 ConstructorInfo.Parameters[?] 以及字段本身所含的所有字段）。
        /// </summary>
        Collection = 2,
        /// <summary>
        /// 一元运算字段。
        /// </summary>
        Unary = 3,
        /// <summary>
        /// 二元运算字段。
        /// </summary>
        Binary = 4,
        /// <summary>
        /// 三元运算字段。
        /// </summary>
        Conditional = 5,
        /// <summary>
        /// 对方法进行调用的字段。
        /// </summary>
        Method = 6,
        /// <summary>
        /// 对某对象的成员进行调用的字段。
        /// </summary>
        Member = 7,
        /// <summary>
        /// 对某一数据字段引用后的字段。
        /// </summary>
        Quote = 8,
        /// <summary>
        /// 常量字段。
        /// </summary>
        Constant = 9,
        /// <summary>
        /// 子查询字段。
        /// </summary>
        Subquery = 10,
        /// <summary>
        /// Switch 分支字段。
        /// </summary>
        Switch = 11,
        /// <summary>
        /// 字段值或类型默认值字段。
        /// </summary>
        DefaultOrValue = 12,
        /// <summary>
        /// 需要对字段别名进行隔离的字段。
        /// </summary>
        NewAlias = 13
    }
}

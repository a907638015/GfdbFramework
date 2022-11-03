﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Enum
{
    /// <summary>
    /// 字段允许为空与否的模式枚举。
    /// </summary>
    public enum FieldNullableMode
    {
        /// <summary>
        /// 未知，此模式下将根据实体类的成员类型来自动判断，若类型是值类型则不允许为空，否则允许为空。
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 允许为空。
        /// </summary>
        Nullable = 1,
        /// <summary>
        /// 不允许为空。
        /// </summary>
        NotNullable = 2
    }
}

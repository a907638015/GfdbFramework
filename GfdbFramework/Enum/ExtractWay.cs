using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Enum
{
    /// <summary>
    /// 字段提取方式枚举。
    /// </summary>
    public enum ExtractWay
    {
        /// <summary>
        /// 除查询外的其他字段提取。
        /// </summary>
        Other = 0,
        /// <summary>
        /// 提取查询字段并初始化一个新的可查询对象。
        /// </summary>
        SelectNew = 1,
        /// <summary>
        /// 提取查询字段但只需修改字段的别名。
        /// </summary>
        SelectNewAlias = 2
    }
}

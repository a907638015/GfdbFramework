using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Enum
{
    /// <summary>
    /// 执行提取字段操作时的提取方式。
    /// </summary>
    internal enum ExtractType
    {
        /// <summary>
        /// 默认操作（如：Where、Descending、Update 等无需新开数据源的操作）。
        /// </summary>
        Default = 1,
        /// <summary>
        /// 查询操作。
        /// </summary>
        Select = 2,
        /// <summary>
        /// 分组操作。
        /// </summary>
        Group = 4,
        /// <summary>
        /// 联合操作。
        /// </summary>
        Join = 8,
        /// <summary>
        /// 提取数据源。
        /// </summary>
        DataSource = 9
    }
}

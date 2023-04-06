using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Enum
{
    /// <summary>
    /// 数据库索引类型枚举。
    /// </summary>
    public enum IndexType
    {

        /// <summary>
        /// 常规索引。
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 唯一索引。
        /// </summary>
        Unique = 1,
        /// <summary>
        /// 组合索引。
        /// </summary>
        Composite = 2,
        /// <summary>
        /// 全文索引。
        /// </summary>
        FullText = 3,
        /// <summary>
        /// 哈希索引。
        /// </summary>
        Hash = 4
    }
}

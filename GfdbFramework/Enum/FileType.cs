using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Enum
{
    /// <summary>
    /// 文件类型枚举。
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// 主数据库文件。
        /// </summary>
        Data = 0,
        /// <summary>
        /// 日志文件。
        /// </summary>
        Log = 1,
        /// <summary>
        /// 索引文件。
        /// </summary>
        Index = 2,
        /// <summary>
        /// 其他类型文件。
        /// </summary>
        Other = 3
    }
}

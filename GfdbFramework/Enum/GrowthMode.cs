using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Enum
{
    /// <summary>
    /// 数据库文件增长方式枚举。
    /// </summary>
    public enum GrowthMode
    {
        /// <summary>
        /// 百分比。
        /// </summary>
        Percentage = 0,
        /// <summary>
        /// 绝对大小。
        /// </summary>
        Size = 1
    }
}

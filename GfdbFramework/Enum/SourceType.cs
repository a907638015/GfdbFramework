using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Enum
{
    /// <summary>
    /// 数据源类型枚举。
    /// </summary>
    public enum SourceType
    {
        /// <summary>
        /// 数据库表。
        /// </summary>
        Table = 0,
        /// <summary>
        /// 数据库视图。
        /// </summary>
        View = 1,
        /// <summary>
        /// 查询结果。
        /// </summary>
        Select = 2,
        /// <summary>
        /// 直连接。
        /// </summary>
        InnerJoin = 3,
        /// <summary>
        /// 全连接。
        /// </summary>
        FullJoin = 4,
        /// <summary>
        /// 左连接。
        /// </summary>
        LeftJoin = 5,
        /// <summary>
        /// 右连接。
        /// </summary>
        RightJoin = 6,
        /// <summary>
        /// 交叉连接。
        /// </summary>
        CrossJoin = 7,
        /// <summary>
        /// 合并数据源。
        /// </summary>
        Union = 8
    }
}

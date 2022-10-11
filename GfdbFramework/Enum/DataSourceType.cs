using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Enum
{
    /// <summary>
    /// 数据源类型枚举。
    /// </summary>
    public enum DataSourceType
    {
        /// <summary>
        /// 原始数据库表。
        /// </summary>
        Table = 0,
        /// <summary>
        /// 原始数据库视图。
        /// </summary>
        View = 1,
        /// <summary>
        /// 直连接。
        /// </summary>
        InnerJoin = 2,
        /// <summary>
        /// 全连接。
        /// </summary>
        FullJoin = 3,
        /// <summary>
        /// 左连接。
        /// </summary>
        LeftJoin = 4,
        /// <summary>
        /// 右连接。
        /// </summary>
        RightJoin = 5,
        /// <summary>
        /// 交叉连接。
        /// </summary>
        CrossJoin = 6,
        /// <summary>
        /// 对其他数据源进行查询后的结果集。
        /// </summary>
        QueryResult = 7
    }
}

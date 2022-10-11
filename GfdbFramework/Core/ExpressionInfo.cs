using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.Enum;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 字段表示信息结构体。
    /// </summary>
    public struct ExpressionInfo
    {
        /// <summary>
        /// 使用指定的表示 Sql 以及该表示 Sql 所使用的操作类型初始化一个新的 <see cref="ExpressionInfo"/> 类实例。
        /// </summary>
        /// <param name="sql">表示 Sql 语句。</param>
        /// <param name="type">该表示 Sql 语句所使用的操作类型。</param>
        public ExpressionInfo(string sql, OperationType type)
        {
            SQL = sql;
            Type = type;
        }

        /// <summary>
        /// 获取该字段的表示 Sql。
        /// </summary>
        public string SQL { get; }

        /// <summary>
        /// 获取该字段表示 Sql 所使用的操作类型。
        /// </summary>
        public OperationType Type { get; }
    }
}

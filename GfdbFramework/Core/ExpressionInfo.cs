using GfdbFramework.Enum;
using GfdbFramework.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 对应 Sql 的表示结果结构体类。
    /// </summary>
    public class ExpressionInfo
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
        /// 获取该表示信息中的 Sql 语句。
        /// </summary>
        public string SQL { get; }

        /// <summary>
        /// 获取该表示信息对应 Sql 所使用的操作类型。
        /// </summary>
        public OperationType Type { get; }

        /// <summary>
        /// 获取或设置该表示信息所使用的参数上下文。
        /// </summary>
        internal IParameterContext ParameterContext { get; set; }
    }
}

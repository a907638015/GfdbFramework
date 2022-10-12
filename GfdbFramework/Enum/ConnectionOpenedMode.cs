using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Enum
{
    /// <summary>
    /// 数据库连接打开方式枚举（枚举项的整数值按优先级升序排序）。
    /// </summary>
    public enum ConnectionOpenedMode
    {
        /// <summary>
        /// 自动打开（该方式打开的连接在每次执行完命令后都应当关闭连接）。
        /// </summary>
        Auto = 1,
        /// <summary>
        /// 框架内部打开（该方式打开的连接在框架内部处理完后将会自动关闭连接，主要在一次性插入多条数据时使用）。
        /// </summary>
        Framework = 2,
        /// <summary>
        /// 事务打开（该方式打开的连接应当在 <see cref="Interface.IDatabaseOperation.RollbackTransaction()"/> 或 <see cref="Interface.IDatabaseOperation.CommitTransaction()"/> 后关闭连接）。
        /// </summary>
        Transaction = 3,
        /// <summary>
        /// 手动打开（该方式打开的连接只有手动调用 <see cref="Interface.IDatabaseOperation.CloseConnection()"/> 方法后才能关闭连接）。
        /// </summary>
        Manual = 4
    }
}

using GfdbFramework.Core;
using GfdbFramework.DataSource;
using GfdbFramework.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace GfdbFramework.Interface
{
    /// <summary>
    /// 数据库操作接口类。
    /// </summary>
    public interface IDatabaseOperation : IDisposable
    {
        /// <summary>
        /// 执行指定的 Sql 语句并返回执行该语句所受影响的数据行数。
        /// </summary>
        /// <param name="sql">待执行的 Sql 语句。</param>
        /// <param name="sqlType">待执行语句的命令类型。</param>
        /// <param name="parameters">执行该命令语句所需的参数集合。</param>
        /// <param name="autoincrementValue">执行插入数据命令时最后一条插入语句所插入的自动增长字段值。</param>
        /// <returns>执行 <paramref name="sql"/> 参数对应语句所影响的数据行数。</returns>
        int ExecuteNonQuery(string sql, CommandType sqlType, ReadOnlyList<DbParameter> parameters, out long autoincrementValue);

        /// <summary>
        /// 执行指定的 Sql 语句并返回执行该语句所受影响的数据行数。
        /// </summary>
        /// <param name="sql">待执行的 Sql 语句。</param>
        /// <param name="sqlType">待执行语句的命令类型。</param>
        /// <param name="parameters">执行该命令语句所需的参数集合。</param>
        /// <returns>执行 <paramref name="sql"/> 参数对应语句所影响的数据行数。</returns>
        int ExecuteNonQuery(string sql, CommandType sqlType, ReadOnlyList<DbParameter> parameters);

        /// <summary>
        /// 执行指定的 Sql 语句并返回结果集中的第一行第一列值返回。
        /// </summary>
        /// <param name="sql">待执行的 Sql 语句。</param>
        /// <param name="sqlType">待执行语句的命令类型。</param>
        /// <param name="parameters">执行该命令所需的参数集合。</param>
        /// <returns>执行 <paramref name="sql"/> 参数对应语句得到的结果集中第一行第一列的值。</returns>
        object ExecuteScalar(string sql, CommandType sqlType, ReadOnlyList<DbParameter> parameters);

        /// <summary>
        /// 执行指定命令语句并将结果集中每一行数据转交与处理函数处理。
        /// </summary>
        /// <param name="sql">待执行的 Sql 语句。</param>
        /// <param name="sqlType">待执行语句的命令类型。</param>
        /// <param name="parameters">执行该命令语句所需的参数集合。</param>
        /// <param name="readerHandler">处理结果集中每一行数据的处理函数（若该函数返回 false 则忽略后续的数据行不再回调此处理函数）。</param>
        void ExecuteReader(string sql, CommandType sqlType, ReadOnlyList<DbParameter> parameters, Func<DbDataReader, bool> readerHandler);

        /// <summary>
        /// 打开数据库的连接通道。
        /// </summary>
        /// <returns>打开成功返回 true，否则返回 false。</returns>
        bool OpenConnection();

        /// <summary>
        /// 打开数据库的连接通道（此方法不建议在框架外部调用，外部要手动打开连接直接调用 <see cref="OpenConnection()"/> 方法即可，无需传递连接打开方式）。
        /// </summary>
        /// <param name="openedMode">连接打开方式。</param>
        /// <returns>打开成功返回 true，否则返回 false。</returns>
        bool OpenConnection(OpenedMode openedMode);

        /// <summary>
        /// 关闭数据库的连接通道。
        /// </summary>
        /// <returns>关闭成功返回 true，否则返回 false。</returns>
        bool CloseConnection();

        /// <summary>
        /// 关闭数据库的连接通道（此方法不建议在框架外部调用，外部要手动关闭连接直接调用 <see cref="CloseConnection()"/> 方法即可，无需传递允许关闭的连接打开模式。
        /// </summary>
        /// <param name="openedMode">允许关闭的连接打开模式（当打开模式优先级等于低于允许的打开模式时都应该关闭）。</param>
        /// <returns>关闭成功返回 true，否则返回 false。</returns>
        bool CloseConnection(OpenedMode openedMode);

        /// <summary>
        /// 开启事务执行模式。
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 开启事务执行模式。
        /// </summary>
        /// <param name="level">事务级别。</param>
        void BeginTransaction(IsolationLevel level);

        /// <summary>
        /// 回滚当前事务中的所有操作。
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// 回滚当前事务到指定保存点或回滚指定事务。
        /// </summary>
        /// <param name="pointName">要回滚的保存点名称或事务名称。</param>
        void RollbackTransaction(string pointName);

        /// <summary>
        /// 提交当前事务中的所有操作。
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// 在当前事务模式下保存一个事务回滚点。
        /// </summary>
        /// <param name="pointName">回滚点名称</param>
        void SaveTransaction(string pointName);

        /// <summary>
        /// 获取当前对象中的数据库连接打开方式。
        /// </summary>
        OpenedMode OpenedMode { get; }
    }
}

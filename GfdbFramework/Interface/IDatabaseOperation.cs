using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using GfdbFramework.Enum;
using GfdbFramework.Field;

namespace GfdbFramework.Interface
{
    /// <summary>
    /// 数据库操作接口类。
    /// </summary>
    public interface IDatabaseOperation : IDisposable
    {
        /// <summary>
        /// 执行指定的命令语句并返回执行该语句所受影响的数据行数。
        /// </summary>
        /// <param name="commandText">待执行的命令语句。</param>
        /// <param name="commandType">待执行语句的命令类型。</param>
        /// <param name="parameters">执行该命令语句所需的参数集合。</param>
        /// <param name="autoincrementValue">执行插入数据命令时插入自动增长字段的值。</param>
        /// <returns>执行 <paramref name="commandText"/> 参数对应语句所影响的数据行数。</returns>
        int ExecuteNonQuery(string commandText, CommandType commandType, IReadOnlyList<DbParameter> parameters, out long autoincrementValue);

        /// <summary>
        /// 执行指定的命令语句并返回执行该语句所受影响的数据行数。
        /// </summary>
        /// <param name="commandText">待执行的命令语句。</param>
        /// <param name="commandType">待执行语句的命令类型。</param>
        /// <param name="parameters">执行该命令语句所需的参数集合。</param>
        /// <returns>执行 <paramref name="commandText"/> 参数对应语句所影响的数据行数。</returns>
        int ExecuteNonQuery(string commandText, CommandType commandType, IReadOnlyList<DbParameter> parameters);

        /// <summary>
        /// 执行指定的 Sql 语句并返回执行该语句所受影响的数据行数。
        /// </summary>
        /// <param name="sql">待执行的 Sql 语句。</param>
        /// <param name="parameters">执行该 Sql 语句所需的参数集合。</param>
        /// <returns>执行 <paramref name="sql"/> 参数对应 Sql 语句所影响的数据行数。</returns>
        int ExecuteNonQuery(string sql, IReadOnlyList<DbParameter> parameters);

        /// <summary>
        /// 执行指定的 Sql 语句并返回执行该语句所受影响的数据行数。
        /// </summary>
        /// <param name="sql">待执行的 Sql 语句。</param>
        /// <returns>执行 <paramref name="sql"/> 参数对应 Sql 语句所影响的数据行数。</returns>
        int ExecuteNonQuery(string sql);

        /// <summary>
        /// 执行指定的 Sql 语句并返回结果集中的第一行第一列值返回。
        /// </summary>
        /// <param name="sql">待执行的 Sql 语句。</param>
        /// <returns>执行该 Sql 得到的结果集中第一行第一列的值。</returns>
        object ExecuteScalar(string sql);

        /// <summary>
        /// 执行指定的 Sql 语句并返回结果集中的第一行第一列值返回。
        /// </summary>
        /// <param name="sql">待执行的 Sql 语句。</param>
        /// <param name="parameters">执行该 Sql 语句所需的参数集合。</param>
        /// <returns>执行该 Sql 得到的结果集中第一行第一列的值。</returns>
        object ExecuteScalar(string sql, IReadOnlyList<DbParameter> parameters);

        /// <summary>
        /// 执行指定命令语句并返回结果集中的第一行第一列值返回。
        /// </summary>
        /// <param name="commandText">待执行的命令语句。</param>
        /// <param name="commandType">待执行语句的命令类型。</param>
        /// <param name="parameters">执行该命令所需的参数集合。</param>
        /// <returns>执行该命令得到的结果集中第一行第一列的值。</returns>
        object ExecuteScalar(string commandText, CommandType commandType, IReadOnlyList<DbParameter> parameters);

        /// <summary>
        /// 执行指定的 Sql 语句并将结果集中每一行数据转交与处理函数处理。
        /// </summary>
        /// <param name="sql">待执行的 Sql 语句。</param>
        /// <param name="readerHandler">处理结果集中每一行数据的处理函数（若该函数返回 false 则忽略后续的数据行不再回调此处理函数）。</param>
        void ExecuteReader(string sql, Func<DbDataReader, bool> readerHandler);

        /// <summary>
        /// 执行指定的 Sql 语句并将结果集中每一行数据转交与处理函数处理。
        /// </summary>
        /// <param name="sql">待执行的 Sql 语句。</param>
        /// <param name="parameters">执行该 Sql 语句所需的参数集合。</param>
        /// <param name="readerHandler">处理结果集中每一行数据的处理函数（若该函数返回 false 则忽略后续的数据行不再回调此处理函数）。</param>
        void ExecuteReader(string sql, IReadOnlyList<DbParameter> parameters, Func<DbDataReader, bool> readerHandler);

        /// <summary>
        /// 执行指定命令语句并将结果集中每一行数据转交与处理函数处理。
        /// </summary>
        /// <param name="commandText">待执行的命令语句。</param>
        /// <param name="commandType">待执行语句的命令类型。</param>
        /// <param name="parameters">执行该命令语句所需的参数集合。</param>
        /// <param name="readerHandler">处理结果集中每一行数据的处理函数（若该函数返回 false 则忽略后续的数据行不再回调此处理函数）。</param>
        void ExecuteReader(string commandText, CommandType commandType, IReadOnlyList<DbParameter> parameters, Func<DbDataReader, bool> readerHandler);

        /// <summary>
        /// 创建数据库（该操作为独立操作，不受上下文控制，即不受事务、数据库开关连接等操作影响）。
        /// </summary>
        /// <param name="databaseInfo">待创建数据库的信息。</param>
        /// <returns>创建成功返回 true，否则返回 false。</returns>
        bool CreateDatabase(Core.DatabaseInfo databaseInfo);

        /// <summary>
        /// 校验指定的数据库是否存在。
        /// </summary>
        /// <param name="databaseName">需要确认是否存在的数据库名称。</param>
        /// <returns>若该数据库已存在则返回 true，否则返回 false。</returns>
        bool ExistsDatabase(string databaseName);

        /// <summary>
        /// 创建数据表（该操作为独立操作，不受上下文控制，即不受事务、数据库开关连接等操作影响）。
        /// </summary>
        /// <param name="dataSource">带创建数据表对应的源信息。</param>
        /// <returns>创建成功返回 true，否则返回 false。</returns>
        bool CreateTable(DataSource.OriginalDataSource dataSource);

        /// <summary>
        /// 校验指定的数据表是否存在。
        /// </summary>
        /// <param name="tableName">需要确认是否存在的数据表名称。</param>
        /// <returns>若该数据表已存在则返回 true，否则返回 false。</returns>
        bool ExistsTable(string tableName);

        /// <summary>
        /// 删除指定的数据表。
        /// </summary>
        /// <param name="tableName">待删除的数据表名称。</param>
        /// <returns>删除成功返回 true，否则返回 false。</returns>
        bool DeleteTable(string tableName);

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
        bool OpenConnection(ConnectionOpenedMode openedMode);

        /// <summary>
        /// 关闭数据库的连接通道。
        /// </summary>
        /// <returns>关闭成功返回 true，否则返回 false。</returns>
        bool CloseConnection();

        /// <summary>
        /// 关闭数据库的连接通道（此方法不建议在框架外部调用，外部要手动关闭连接直接调用 <see cref="CloseConnection()"/> 方法即可，无需传递允许关闭的连接打开模式。
        /// </summary>
        /// <param name="openedMode">允许关闭的连接打开模式。</param>
        /// <returns>关闭成功返回 true，否则返回 false。</returns>
        bool CloseConnection(ConnectionOpenedMode openedMode);

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
        ConnectionOpenedMode OpenedMode { get; }
    }
}

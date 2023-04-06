using GfdbFramework.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GfdbFramework.Interface
{
    /// <summary>
    /// 数据操作上下文对象接口类。
    /// </summary>
    public interface IDataContext : IDisposable
    {
        /// <summary>
        /// 获取一个用于执行各种数据库操作的执行对象。
        /// </summary>
        IDatabaseOperation DatabaseOperation { get; }

        /// <summary>
        /// 获取一个用于创建各种 Sql 的工厂实例。
        /// </summary>
        ISqlFactory SqlFactory { get; }

        /// <summary>
        /// 获取指定实体类所映射的数据库表操作对象。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <returns>指定实体类型所映射的数据库表操作对象。</returns>
        Modifiable<TEntity, TEntity> GetTable<TEntity>() where TEntity : class, new();

        /// <summary>
        /// 获取指定实体类所映射的数据库视图操作对象。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <returns>指定实体类型所映射的数据库视图操作对象。</returns>
        Queryable<TEntity, TEntity> GetView<TEntity>() where TEntity : class, new();

        /// <summary>
        /// 校验指定的数据库是否存在。
        /// </summary>
        /// <param name="databaseInfo">需要校验是否存在的数据库信息。</param>
        /// <returns>若存在则返回 true，否则返回 false。</returns>
        bool ExistsDatabase(DatabaseInfo databaseInfo);

        /// <summary>
        /// 删除指定的数据库。
        /// </summary>
        /// <param name="databaseInfo">需要删除的数据库信息。</param>
        /// <returns>删除成功返回 true，否则返回 false。</returns>
        bool DeleteDatabase(DatabaseInfo databaseInfo);

        /// <summary>
        /// 创建指定的数据库。
        /// </summary>
        /// <param name="databaseInfo">需要创建的数据库信息。</param>
        /// <returns>创建成功返回 true，否则返回 false。</returns>
        bool CreateDatabase(DatabaseInfo databaseInfo);

        /// <summary>
        /// 校验指定的数据库表是否存在。
        /// </summary>
        /// <typeparam name="TEntity">该表所映射到的实体类。</typeparam>
        /// <param name="table">所需校验的表操作对象。</param>
        /// <returns>若存在则返回 true，否则返回 false。</returns>
        bool ExistsTable<TEntity>(Modifiable<TEntity, TEntity> table) where TEntity : class, new();

        /// <summary>
        /// 校验指定的数据库视图是否存在。
        /// </summary>
        /// <typeparam name="TEntity">该视图所映射到的实体类。</typeparam>
        /// <param name="view">所需校验的视图操作对象。</param>
        /// <returns>若存在则返回 true，否则返回 false。</returns>
        bool ExistsView<TEntity>(Queryable<TEntity, TEntity> view) where TEntity : class, new();

        /// <summary>
        /// 创建指定的数据库表。
        /// </summary>
        /// <typeparam name="TEntity">该表所映射到的实体类。</typeparam>
        /// <param name="table">需创建的表操作对象。</param>
        /// <returns>创建成功返回 true，否则返回 false。</returns>
        bool CreateTable<TEntity>(Modifiable<TEntity, TEntity> table) where TEntity : class, new();

        /// <summary>
        /// 创建指定的数据库视图。
        /// </summary>
        /// <typeparam name="TEntity">该视图所映射到的实体类。</typeparam>
        /// <param name="view">需创建的视图操作对象。</param>
        /// <returns>创建成功返回 true，否则返回 false。</returns>
        bool CreateView<TEntity>(Queryable<TEntity, TEntity> view) where TEntity : class, new();

        /// <summary>
        /// 删除指定的数据库表。
        /// </summary>
        /// <typeparam name="TEntity">该表所映射到的实体类。</typeparam>
        /// <param name="table">所需删除的表操作对象。</param>
        /// <returns>删除成功返回 true，否则返回 false。</returns>
        bool DeleteTable<TEntity>(Modifiable<TEntity, TEntity> table) where TEntity : class, new();

        /// <summary>
        /// 删除指定的数据库视图。
        /// </summary>
        /// <typeparam name="TEntity">该视图所映射到的实体类。</typeparam>
        /// <param name="view">需删除的视图操作对象。</param>
        /// <returns>删除成功返回 true，否则返回 false。</returns>
        bool DeleteView<TEntity>(Queryable<TEntity, TEntity> view) where TEntity : class, new();

        /// <summary>
        /// 获取所操作数据库中所有的视图名称集合。
        /// </summary>
        /// <returns>当前上下文所操作数据库中所有存在的视图名称集合。</returns>
        ReadOnlyList<string> GetAllViews();

        /// <summary>
        /// 获取所操作数据库中所有的表名称集合。
        /// </summary>
        /// <returns>当前上下文所操作数据库中所有存在的表名称集合。</returns>
        ReadOnlyList<string> GetAllTables();

        /// <summary>
        /// 将指定的 .NET 基础数据类型转换成映射到数据库后的默认数据类型（如：System.Int32 应当返回 int，System.String 可返回 varchar(255)）。
        /// </summary>
        /// <param name="type">待转换成数据库数据类型的框架类型。</param>
        /// <returns>该框架类型映射到数据库的默认数据类型。</returns>
        string NetTypeToDBType(Type type);

        /// <summary>
        /// 获取或设置一个值，该值指示在执行各种 Sql 操作时是否应当区分大小写。
        /// </summary>
        bool IsCaseSensitive { get; set; }

        /// <summary>
        /// 获取当前所操作数据库的内部版本号。
        /// </summary>
        double BuildNumber { get; }

        /// <summary>
        /// 获取当前所操作数据库的版本号。
        /// </summary>
        string Version { get; }

        /// <summary>
        /// 获取当前所操作数据库的发行版本名称。
        /// </summary>
        string ReleaseName { get; }

        /// <summary>
        /// 创建一个新的参数上下文对象。
        /// </summary>
        /// <param name="enableParametric">是否应当启用参数化操作。</param>
        /// <returns>创建好的参数上下文。</returns>
        IParameterContext CreateParameterContext(bool enableParametric);

        /// <summary>
        /// 开启事务执行模式。
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 以指定的事务隔离级别开启事务执行模式。
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
        /// 打开数据库的连接通道（一次性需要操作多条 Sql 语句时建议手动打开连接通道，否则每次执行一次 Sql 都会自动关闭连接）。
        /// </summary>
        /// <returns>打开成功返回 true，否则返回 false。</returns>
        bool OpenConnection();

        /// <summary>
        /// 关闭数据库的连接通道（手动调用 <see cref="OpenConnection"/> 方法打开连接通道时必须手动调用该方法关闭）。
        /// </summary>
        /// <returns>关闭成功返回 true，否则返回 false。</returns>
        bool CloseConnection();
    }
}

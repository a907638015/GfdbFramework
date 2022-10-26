using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using GfdbFramework.Core;

namespace GfdbFramework.Interface
{
    /// <summary>
    /// 数据操作上下文对象接口类。
    /// </summary>
    public interface IDataContext
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
        /// 将指定的 .NET 基础数据类型转换成映射到数据库后的默认数据类型（如：System.Int32 应当返回 int，System.String 可返回 varchar(255)）。
        /// </summary>
        /// <param name="type">待转换成数据库数据类型的框架类型。</param>
        /// <returns>该框架类型映射到数据库的默认数据类型。</returns>
        string NetTypeToDBType(Type type);

        /// <summary>
        /// 获取或设置一个值，该值指示在执行各种 Sql 操作时是否应当区分大小写（为了尽量与 .Net 框架保持一致性，此属性应当默认为 true）。
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
        /// 创建数据库（该操作为独立操作，不受上下文控制，即不受事务、数据库开关连接等操作影响）。
        /// </summary>
        /// <param name="databaseInfo">待创建数据库的信息。</param>
        /// <returns>创建成功返回 true，否则返回 false。</returns>
        bool CreateDatabase(DatabaseInfo databaseInfo);

        /// <summary>
        /// 创建数据表。
        /// </summary>
        /// <typeparam name="TSource">待创建数据表所映射的实体类型。</typeparam>
        /// <param name="modifiable">待创建数据表对应的可修改对象。</param>
        /// <returns>创建成功返回 true，否则返回 false。</returns>
        bool CreateTable<TSource>(Modifiable<TSource, TSource> modifiable) where TSource : class, new();

        /// <summary>
        /// 删除指定的数据表。
        /// </summary>
        /// <param name="modifiable">待删除数据表对应的可修改对象。</param>
        /// <returns>删除成功返回 true，否则返回 false。</returns>
        bool DeleteTable<TSource>(Modifiable<TSource, TSource> modifiable) where TSource : class, new();

        /// <summary>
        /// 校验指定的数据库是否存在。
        /// </summary>
        /// <param name="databaseName">需要确认是否存在的数据库名称。</param>
        /// <returns>若该数据库已存在则返回 true，否则返回 false。</returns>
        bool ExistsDatabase(string databaseName);

        /// <summary>
        /// 校验指定的数据表是否存在。
        /// </summary>
        /// <param name="modifiable">需要确认的数据表所映射的可修改对象。</param>
        /// <returns>若该数据表已存在则返回 true，否则返回 false。</returns>
        bool ExistsTable<TSource>(Modifiable<TSource, TSource> modifiable) where TSource : class, new();

        /// <summary>
        /// 获取当前所操作数据库中所有已存在的表名。
        /// </summary>
        IReadOnlyList<string> TableNames { get; }
    }
}

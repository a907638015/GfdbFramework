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
    }
}

﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using GfdbFramework.Core;
using GfdbFramework.Interface;

namespace GfdbFramework.Realize
{
    /// <summary>
    /// 数据操作上下文基类。
    /// </summary>
    public abstract class DataContext : IDataContext
    {
        private readonly IDatabaseOperation _DatabaseOperation = null;
        private static readonly Type _NullableType = typeof(int?).GetGenericTypeDefinition();

        /// <summary>
        /// 使用指定的数据库操作执行对象以及一个创建各种 Sql 的工厂实例初始化一个新的 <see cref="DataContext"/> 类实例。
        /// </summary>
        /// <param name="databaseOperation">一个用于执行各种数据库操作的执行对象。</param>
        /// <param name="sqlFactory">一个用于创建各种 Sql 的工厂实例。</param>
        public DataContext(IDatabaseOperation databaseOperation, ISqlFactory sqlFactory)
        {
            _DatabaseOperation = databaseOperation;
            SqlFactory = sqlFactory;
            IsCaseSensitive = true;
        }

        /// <summary>
        /// 获取一个用于执行各种数据库操作的执行对象。
        /// </summary>
        IDatabaseOperation IDataContext.DatabaseOperation
        {
            get
            {
                return _DatabaseOperation;
            }
        }

        /// <summary>
        /// 获取一个用于创建各种 Sql 的工厂实例。
        /// </summary>
        public ISqlFactory SqlFactory { get; }

        /// <summary>
        /// 获取当前所操作数据库的内部版本号。
        /// </summary>
        public abstract double BuildNumber { get; }

        /// <summary>
        /// 获取当前所操作数据库的版本号。
        /// </summary>
        public abstract string Version { get; }

        /// <summary>
        /// 获取当前所操作数据库的发行版本名称。
        /// </summary>
        public abstract string ReleaseName { get; }

        /// <summary>
        /// 获取或设置一个值，该值指示在执行各种 Sql 操作时是否应当区分大小写（为了尽量与 .Net 框架保持一致性，此属性应当默认为 true）。
        /// </summary>
        public bool IsCaseSensitive { get; set; }

        /// <summary>
        /// 将指定的 .NET 基础数据类型转换成映射到数据库后的默认数据类型（如：System.Int32 应当返回 int，System.String 可返回 varchar(255)）。
        /// </summary>
        /// <param name="type">待转换成数据库数据类型的框架类型。</param>
        /// <returns>该框架类型映射到数据库的默认数据类型。</returns>
        public virtual string NetTypeToDBType(Type type)
        {
            switch (type.FullName)
            {
                case "System.Int32":
                    return "int";
                case "System.UInt32":
                    return "int unsigned";
                case "System.Int16":
                    return "smallint";
                case "System.UInt16":
                    return "smallint unsigned";
                case "System.Int64":
                    return "bigint";
                case "System.UInt64":
                    return "bigint unsigned";
                case "System.DateTime":
                    return "datetime";
                case "System.Guid":
                    return "uniqueidentifier";
                case "System.Single":
                    return "real";
                case "System.Double":
                    return "float";
                case "System.DateTimeOffset":
                    return "datetimeoffset(7)";
                case "System.TimeSpan":
                    return "time(7)";
                case "System.Decimal":
                    return "decimal(23,5)";
                case "System.Boolean":
                    return "bit";
                case "System.Byte":
                    return "tinyint unsigned";
                case "System.SByte":
                    return "tinyint";
                case "System.String":
                    return "varchar(255)";
                default:
                    if (type.IsArray && type.GetElementType().FullName == "System.Byte")
                        return "varbinary(1024)";
                    break;
            }

            if (type.IsEnum)
                return "int";
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == _NullableType)
                return NetTypeToDBType(type.GetGenericArguments()[0]);
            else
                throw new Exception(string.Format("未能将 .NET 框架中 {0} 类型转换成 Sql Server 对应的数据类型", type.FullName));
        }

        /// <summary>
        /// 获取指定实体类所映射的数据库表操作对象。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <returns>指定实体类型所映射的数据库表操作对象。</returns>
        public Modifiable<TEntity, TEntity> GetTable<TEntity>() where TEntity : class, new()
        {
            Type entityType = typeof(TEntity);

            Queryable queryable = new Modifiable<TEntity, TEntity>(this, Helper.GetDataSource(this, entityType, Enum.DataSourceType.Table));

            return (Modifiable<TEntity, TEntity>)queryable;
        }

        /// <summary>
        /// 获取指定实体类所映射的数据库视图操作对象。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <returns>指定实体类型所映射的数据库视图操作对象。</returns>
        public Queryable<TEntity, TEntity> GetView<TEntity>() where TEntity : class, new()
        {
            Type entityType = typeof(TEntity);

            Queryable queryable = new Queryable<TEntity, TEntity>(this, Helper.GetDataSource(this, entityType, Enum.DataSourceType.Table));

            return (Queryable<TEntity, TEntity>)queryable;
        }

        /// <summary>
        /// 开启事务执行模式。
        /// </summary>
        public virtual void BeginTransaction()
        {
            _DatabaseOperation.BeginTransaction();
        }

        /// <summary>
        /// 开启事务执行模式。
        /// </summary>
        /// <param name="level">事务级别。</param>
        public virtual void BeginTransaction(IsolationLevel level)
        {
            _DatabaseOperation.BeginTransaction(level);
        }

        /// <summary>
        /// 回滚当前事务中的所有操作。
        /// </summary>
        public virtual void RollbackTransaction()
        {
            _DatabaseOperation.RollbackTransaction();
        }

        /// <summary>
        /// 回滚当前事务到指定保存点或回滚指定事务。
        /// </summary>
        /// <param name="pointName">要回滚的保存点名称或事务名称。</param>
        public virtual void RollbackTransaction(string pointName)
        {
            _DatabaseOperation.RollbackTransaction(pointName);
        }

        /// <summary>
        /// 提交当前事务中的所有操作。
        /// </summary>
        public virtual void CommitTransaction()
        {
            _DatabaseOperation.CommitTransaction();
        }

        /// <summary>
        /// 在当前事务模式下保存一个事务回滚点。
        /// </summary>
        /// <param name="pointName">回滚点名称</param>
        public virtual void SaveTransaction(string pointName)
        {
            _DatabaseOperation.SaveTransaction(pointName);
        }
    }
}

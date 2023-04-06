using GfdbFramework.DataSource;
using GfdbFramework.Enum;
using GfdbFramework.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 默认实现的数据操作上下文基类。
    /// </summary>
    public abstract class DataContext : IDataContext
    {
        private static readonly Type _NullableType = typeof(int?).GetGenericTypeDefinition();
        private readonly IDatabaseOperation _DatabaseOperation = null;

        /// <summary>
        /// 使用指定的数据库操作执行对象以及一个创建各种 Sql 的工厂实例初始化一个新的 <see cref="DataContext"/> 类实例。
        /// </summary>
        /// <param name="databaseOperation">一个用于执行各种数据库操作的执行对象。</param>
        /// <param name="sqlFactory">一个用于创建各种 Sql 的工厂实例。</param>
        public DataContext(IDatabaseOperation databaseOperation, ISqlFactory sqlFactory)
        {
            if (databaseOperation == null || sqlFactory == null)
                throw new ArgumentNullException(databaseOperation == null ? nameof(databaseOperation) : nameof(sqlFactory), $"初始化 DataContext 类时参数不能为空");

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
        /// 获取或设置一个值，该值指示在执行各种 Sql 操作时是否应当区分大小写（为了尽量与 .Net 框架保持一致性，此属性应当默认为 true）。
        /// </summary>
        public bool IsCaseSensitive { get; set; }

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
        /// 开启事务执行模式。
        /// </summary>
        public void BeginTransaction()
        {
            _DatabaseOperation.BeginTransaction();
        }

        /// <summary>
        /// 以指定的事务隔离级别开启事务执行模式。
        /// </summary>
        /// <param name="level">事务级别。</param>
        public void BeginTransaction(IsolationLevel level)
        {
            _DatabaseOperation.BeginTransaction(level);
        }

        /// <summary>
        /// 关闭数据库的连接通道（手动调用 <see cref="OpenConnection"/> 方法打开连接通道时必须手动调用该方法关闭）。
        /// </summary>
        /// <returns>关闭成功返回 true，否则返回 false。</returns>
        public bool CloseConnection()
        {
            return _DatabaseOperation.CloseConnection();
        }

        /// <summary>
        /// 提交当前事务中的所有操作。
        /// </summary>
        public void CommitTransaction()
        {
            _DatabaseOperation.CommitTransaction();
        }

        /// <summary>
        /// 释放当前上下文所占用的资源信息。
        /// </summary>
        public void Dispose()
        {
            _DatabaseOperation.Dispose();
        }

        /// <summary>
        /// 获取指定实体类所映射的数据库表操作对象。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <returns>指定实体类型所映射的数据库表操作对象。</returns>
        public Modifiable<TEntity, TEntity> GetTable<TEntity>() where TEntity : class, new()
        {
            return new Modifiable<TEntity, TEntity>(this, (TableDataSource)Helper.GetDataSource(this, typeof(TEntity), true));
        }

        /// <summary>
        /// 获取指定实体类所映射的数据库视图操作对象。
        /// </summary>
        /// <typeparam name="TEntity">实体类型。</typeparam>
        /// <returns>指定实体类型所映射的数据库视图操作对象。</returns>
        public Queryable<TEntity, TEntity> GetView<TEntity>() where TEntity : class, new()
        {
            return new Queryable<TEntity, TEntity>(this, (ViewDataSource)Helper.GetDataSource(this, typeof(TEntity), false));
        }

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
                throw new Exception($"未能将 .NET 框架中 {type.FullName} 类型转换成数据库对应的数据类型");
        }

        /// <summary>
        /// 校验指定的数据库是否存在。
        /// </summary>
        /// <param name="databaseInfo">需要校验是否存在的数据库信息。</param>
        /// <returns>若存在则返回 true，否则返回 false。</returns>
        public abstract bool ExistsDatabase(DatabaseInfo databaseInfo);

        /// <summary>
        /// 删除指定的数据库。
        /// </summary>
        /// <param name="databaseInfo">需要删除的数据库信息。</param>
        /// <returns>删除成功返回 true，否则返回 false。</returns>
        public abstract bool DeleteDatabase(DatabaseInfo databaseInfo);

        /// <summary>
        /// 创建指定的数据库。
        /// </summary>
        /// <param name="databaseInfo">需要创建的数据库信息。</param>
        /// <returns>创建成功返回 true，否则返回 false。</returns>
        public abstract bool CreateDatabase(DatabaseInfo databaseInfo);

        /// <summary>
        /// 校验指定的数据库表是否存在。
        /// </summary>
        /// <typeparam name="TEntity">该表所映射到的实体类。</typeparam>
        /// <param name="table">所需校验的表操作对象。</param>
        /// <returns>若存在则返回 true，否则返回 false。</returns>
        public bool ExistsTable<TEntity>(Modifiable<TEntity, TEntity> table) where TEntity : class, new()
        {
            return ExistsTable((TableDataSource)table.DataSource);
        }

        /// <summary>
        /// 校验指定的数据库表是否存在。
        /// </summary>
        /// <param name="tableSource">所需校验的表数据源。</param>
        /// <returns>若存在则返回 true，否则返回 false。</returns>
        protected abstract bool ExistsTable(TableDataSource tableSource);

        /// <summary>
        /// 校验指定的数据库视图是否存在。
        /// </summary>
        /// <typeparam name="TEntity">该视图所映射到的实体类。</typeparam>
        /// <param name="view">所需校验的视图操作对象。</param>
        /// <returns>若存在则返回 true，否则返回 false。</returns>
        public bool ExistsView<TEntity>(Queryable<TEntity, TEntity> view) where TEntity : class, new()
        {
            if (view.DataSource.Type == SourceType.View)
                return ExistsView((ViewDataSource)view.DataSource);
            else
                throw new ArgumentException("校验指定数据库视图是否存在时，参数传递的操作对象并非是视图操作对象", nameof(view));
        }

        /// <summary>
        /// 校验指定的数据库视图是否存在。
        /// </summary>
        /// <param name="viewSource">所需校验的视图数据源。</param>
        /// <returns>若存在则返回 true，否则返回 false。</returns>
        protected abstract bool ExistsView(ViewDataSource viewSource);

        /// <summary>
        /// 创建指定的数据库表。
        /// </summary>
        /// <typeparam name="TEntity">该表所映射到的实体类。</typeparam>
        /// <param name="table">需创建的表操作对象。</param>
        /// <returns>创建成功返回 true，否则返回 false。</returns>
        public bool CreateTable<TEntity>(Modifiable<TEntity, TEntity> table) where TEntity : class, new()
        {
            return CreateTable((TableDataSource)table.DataSource);
        }

        /// <summary>
        /// 创建指定的数据库表。
        /// </summary>
        /// <param name="tableSource">需创建表的数据源对象。</param>
        /// <returns>创建成功返回 true，否则返回 false。</returns>
        protected abstract bool CreateTable(TableDataSource tableSource);

        /// <summary>
        /// 创建指定的数据库视图。
        /// </summary>
        /// <typeparam name="TEntity">该视图所映射到的实体类。</typeparam>
        /// <param name="view">需创建的视图操作对象。</param>
        /// <returns>创建成功返回 true，否则返回 false。</returns>
        public bool CreateView<TEntity>(Queryable<TEntity, TEntity> view) where TEntity : class, new()
        {
            if (view.DataSource.Type == SourceType.View)
                return CreateView((ViewDataSource)view.DataSource);
            else
                throw new ArgumentException("创建数据库视图时，参数传递的操作对象并非是视图操作对象", nameof(view));
        }

        /// <summary>
        /// 创建指定的数据库视图。
        /// </summary>
        /// <param name="viewSource">需创建视图的数据源。</param>
        /// <returns>创建成功返回 true，否则返回 false。</returns>
        protected abstract bool CreateView(ViewDataSource viewSource);

        /// <summary>
        /// 删除指定的数据库表。
        /// </summary>
        /// <typeparam name="TEntity">该表所映射到的实体类。</typeparam>
        /// <param name="table">所需删除的表操作对象。</param>
        /// <returns>删除成功返回 true，否则返回 false。</returns>
        public bool DeleteTable<TEntity>(Modifiable<TEntity, TEntity> table) where TEntity : class, new()
        {
            return DeleteTable((TableDataSource)table.DataSource);
        }

        /// <summary>
        /// 删除指定的数据库表。
        /// </summary>
        /// <param name="tableSource">需删除表的数据源对象。</param>
        /// <returns>删除成功返回 true，否则返回 false。</returns>
        protected abstract bool DeleteTable(TableDataSource tableSource);

        /// <summary>
        /// 删除指定的数据库视图。
        /// </summary>
        /// <typeparam name="TEntity">该视图所映射到的实体类。</typeparam>
        /// <param name="view">需删除的视图操作对象。</param>
        /// <returns>删除成功返回 true，否则返回 false。</returns>
        public bool DeleteView<TEntity>(Queryable<TEntity, TEntity> view) where TEntity : class, new()
        {
            if (view.DataSource.Type == SourceType.View)
                return DeleteView((ViewDataSource)view.DataSource);
            else
                throw new ArgumentException("删除数据库视图时，参数传递的操作对象并非是视图操作对象", nameof(view));
        }

        /// <summary>
        /// 删除指定的数据库视图。
        /// </summary>
        /// <param name="viewSource">需删除视图的数据源对象。</param>
        /// <returns>删除成功返回 true，否则返回 false。</returns>
        protected abstract bool DeleteView(ViewDataSource viewSource);

        /// <summary>
        /// 获取所操作数据库中所有的视图名称集合。
        /// </summary>
        /// <returns>当前上下文所操作数据库中所有存在的视图名称集合。</returns>
        public abstract ReadOnlyList<string> GetAllViews();

        /// <summary>
        /// 获取所操作数据库中所有的表名称集合。
        /// </summary>
        /// <returns>当前上下文所操作数据库中所有存在的表名称集合。</returns>
        public abstract ReadOnlyList<string> GetAllTables();

        /// <summary>
        /// 创建一个新的参数上下文对象。
        /// </summary>
        /// <param name="enableParametric">是否应当启用参数化操作。</param>
        /// <returns>创建好的参数上下文。</returns>
        public abstract IParameterContext CreateParameterContext(bool enableParametric);

        /// <summary>
        /// 打开数据库的连接通道（一次性需要操作多条 Sql 语句时建议手动打开连接通道，否则每次执行一次 Sql 都会自动关闭连接）。
        /// </summary>
        /// <returns>打开成功返回 true，否则返回 false。</returns>
        public bool OpenConnection()
        {
            return _DatabaseOperation.OpenConnection();
        }

        /// <summary>
        /// 回滚当前事务中的所有操作。
        /// </summary>
        public void RollbackTransaction()
        {
            _DatabaseOperation.RollbackTransaction();
        }

        /// <summary>
        /// 回滚当前事务到指定保存点或回滚指定事务。
        /// </summary>
        /// <param name="pointName">要回滚的保存点名称或事务名称。</param>
        public void RollbackTransaction(string pointName)
        {
            _DatabaseOperation.RollbackTransaction(pointName);
        }

        /// <summary>
        /// 在当前事务模式下保存一个事务回滚点。
        /// </summary>
        /// <param name="pointName">回滚点名称</param>
        public void SaveTransaction(string pointName)
        {
            _DatabaseOperation.SaveTransaction(pointName);
        }
    }
}

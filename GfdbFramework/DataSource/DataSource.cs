using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.Enum;
using GfdbFramework.Interface;

namespace GfdbFramework.DataSource
{
    /// <summary>
    /// 数据源基类。
    /// </summary>
    public abstract class DataSource
    {
        /// <summary>
        /// 使用指定的据操作上下文以及数据源类型初始化一个新的 <see cref="DataSource"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该数据源所使用的数据操作上下文对象。</param>
        /// <param name="type">该数据源的类型。</param>
        internal DataSource(IDataContext dataContext, DataSourceType type)
        {
            DataContext = dataContext;
            Type = type;
        }

        /// <summary>
        /// 获取当前数据源类型。
        /// </summary>
        public DataSourceType Type { get; }

        /// <summary>
        /// 获取当前数据源所使用的数据操作上下文对象。
        /// </summary>
        public IDataContext DataContext { get; }

        /// <summary>
        /// 以当前数据源为蓝本复制出一个一样的数据源信息。
        /// </summary>
        /// <param name="startAliasIndex">复制后新数据源的起始表别名下标。</param>
        /// <returns>复制好的新数据源信息。</returns>
        internal abstract DataSource Copy(ref int startAliasIndex);
    }
}

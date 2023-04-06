using GfdbFramework.Enum;
using GfdbFramework.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.DataSource
{
    /// <summary>
    /// 所有可被操作数据源的抽象基类。
    /// </summary>
    public abstract class DataSource
    {
        /// <summary>
        /// 使用指定的数据操作上下文以及数据源类型初始化一个新的 <see cref="DataSource"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该数据源所使用的数据操作上下文。</param>
        /// <param name="type">当前数据源类型。</param>
        internal DataSource(IDataContext dataContext, SourceType type)
        {
            DataContext = dataContext;
            Type = type;
        }

        /// <summary>
        /// 以当前数据源为蓝本复制出一个一样的数据源信息。
        /// </summary>
        /// <param name="copiedDataSources">已经复制过的数据源集合。</param>
        /// <param name="copiedFields">已经复制好的字段信息。</param>
        /// <param name="startAliasIndex">新数据源的起始别名下标。</param>
        /// <returns>复制好的新数据源信息。</returns>
        public abstract DataSource Copy(Dictionary<DataSource, DataSource> copiedDataSources, Dictionary<Field.Field, Field.Field> copiedFields, ref int startAliasIndex);

        /// <summary>
        /// 获取当前数据源所使用的数据操作上下文。
        /// </summary>
        public IDataContext DataContext { get; }

        /// <summary>
        /// 获取当前数据源的类型。
        /// </summary>
        public SourceType Type { get; }
    }
}

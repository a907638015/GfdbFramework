using GfdbFramework.Enum;
using GfdbFramework.Field;
using GfdbFramework.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.DataSource
{
    /// <summary>
    /// 原始数据库表或视图数据源基类。
    /// </summary>
    public abstract class OriginalDataSource : BasicDataSource
    {
        /// <summary>
        /// 使用指定的数据操作上下文、数据源类型、数据源根字段、对应源名称以及该数据源的别名下标初始化一个新的 <see cref="OriginalDataSource"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该数据源所使用的数据操作上下文。</param>
        /// <param name="type">当前数据源类型。</param>
        /// <param name="rootField">数据源根字段。</param>
        /// <param name="name">数据源对应的视图或表名称。</param>
        /// <param name="aliasIndex">数据源别名下标。</param>
        internal OriginalDataSource(IDataContext dataContext, SourceType type, Field.Field rootField, string name, int aliasIndex)
            : base(dataContext, type, rootField, aliasIndex)
        {
            Name = name;
        }

        /// <summary>
        /// 获取当前数据源对应的视图名或表名。
        /// </summary>
        public string Name { get; }
    }
}

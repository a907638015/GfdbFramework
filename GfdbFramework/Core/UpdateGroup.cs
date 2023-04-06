using GfdbFramework.DataSource;
using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 待更新数据组。
    /// </summary>
    public class UpdateGroup
    {
        /// <summary>
        /// 使用指定的待更新数据源以及所需更新的字段项初始化一个新的 <see cref="UpdateGroup"/> 类实例。
        /// </summary>
        /// <param name="dataSource">本次需要更新的数据源。</param>
        /// <param name="updateFields">本次需要更新的字段项目集合。</param>
        internal UpdateGroup(TableDataSource dataSource, ReadOnlyList<UpdateItem> updateFields)
        {
            UpdateSource = dataSource;
            UpdateFields = updateFields;
        }

        /// <summary>
        /// 获取本次需要更新的数据源。
        /// </summary>
        public TableDataSource UpdateSource { get; }

        /// <summary>
        /// 获取本次需要更新的数据字段信息。
        /// </summary>
        public ReadOnlyList<UpdateItem> UpdateFields { get; }
    }
}

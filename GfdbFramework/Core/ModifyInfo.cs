using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.DataSource;
using GfdbFramework.Field;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 对数据库表数据进行修改时的字段修改信息。
    /// </summary>
    public class ModifyInfo
    {
        /// <summary>
        /// 使用指定的待修改字段信息、待修改字段所归属的数据源以及修改后的值信息初始化一个新的 <see cref="ModifyInfo"/> 类实例。
        /// </summary>
        /// <param name="field">待修改值的字段信息。</param>
        /// <param name="dataSource">待修改字段所归属的数据源信息。</param>
        /// <param name="value">修改后的字段值信息。</param>
        internal ModifyInfo(OriginalField field, OriginalDataSource dataSource, BasicField value)
        {
            DataSource = dataSource;
            Field = field;
            Value = value;
        }

        /// <summary>
        /// 获取带修改值字段所归属的数据源信息。
        /// </summary>
        public OriginalDataSource DataSource { get; }

        /// <summary>
        /// 获取待修改值的字段信息。
        /// </summary>
        public OriginalField Field { get; }

        /// <summary>
        /// 获取修改后的字段值信息。
        /// </summary>
        public BasicField Value { get; }
    }
}

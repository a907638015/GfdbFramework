using GfdbFramework.Field;
using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 待更新字段项。
    /// </summary>
    public class UpdateItem
    {
        /// <summary>
        /// 使用指定的待更新字段以及更新后的值字段初始化一个新的 <see cref="UpdateItem"/> 类实例。
        /// </summary>
        /// <param name="field">待修改值的字段。</param>
        /// <param name="value">修改后的值信息。</param>
        internal UpdateItem(OriginalField field, BasicField value)
        {
            Field = field;
            Value = value;
        }

        /// <summary>
        /// 获取待修改值的字段信息。
        /// </summary>
        public OriginalField Field { get; }

        /// <summary>
        /// 获取待修改字段的目标值。
        /// </summary>
        public BasicField Value { get; }
    }
}

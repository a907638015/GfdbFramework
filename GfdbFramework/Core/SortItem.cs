using GfdbFramework.Enum;
using GfdbFramework.Field;
using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 数据源排序项目类。
    /// </summary>
    public class SortItem
    {
        /// <summary>
        /// 使用指定的排序字段以及排序方式初始化一个新的 <see cref="SortItem"/> 类实例。
        /// </summary>
        /// <param name="field">需要排序的字段信息。</param>
        /// <param name="type">对字段进行排序的方式。</param>
        public SortItem(BasicField field, SortType type)
        {
            Field = field;
            Type = type;
        }

        /// <summary>
        /// 获取需要排序的字段信息。
        /// </summary>
        public BasicField Field { get; }

        /// <summary>
        /// 获取需要排序的方式。
        /// </summary>
        public SortType Type { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.Enum;
using GfdbFramework.Field;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 数据库表索引信息类。
    /// </summary>
    public class IndexInfo
    {
        /// <summary>
        /// 使用指定的索引名称、应用字段集合、索引类型以及排序方式初始化一个新的 <see cref="IndexInfo"/> 类实例。
        /// </summary>
        /// <param name="name">该索引的名称。</param>
        /// <param name="fields">该索引应用的字段集合。</param>
        /// <param name="type">该索引的类型。</param>
        /// <param name="sort">该索引的排序方式。</param>
        internal IndexInfo(string name, Interface.IReadOnlyList<OriginalField> fields, IndexType type, SortType sort)
        {
            Name = name;
            Fields = fields;
            Type = type;
            Sort = sort;
        }

        /// <summary>
        /// 获取该索引应用的字段集合。
        /// </summary>
        public Interface.IReadOnlyList<OriginalField> Fields { get; }

        /// <summary>
        /// 获取该索引的类型。
        /// </summary>
        public IndexType Type { get; }

        /// <summary>
        /// 获取该索引的排序方式。
        /// </summary>
        public SortType Sort { get; }

        /// <summary>
        /// 获取该索引的名称。
        /// </summary>
        public string Name { get; }
    }
}

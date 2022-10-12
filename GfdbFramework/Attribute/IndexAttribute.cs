using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Attribute
{
    /// <summary>
    /// 数据库表索引标记类。
    /// </summary>
    public class IndexAttribute : System.Attribute
    {
        /// <summary>
        /// 使用指定的索引字段集合初始化一个新的 <see cref="IndexAttribute"/> 类实例。
        /// </summary>
        /// <param name="fields">该索引应用的字段集合。</param>
        public IndexAttribute(string[] fields)
            : this(null, fields)
        {
        }

        /// <summary>
        /// 使用指定的索引名称、索引应用的字段集合以及排序方式初始化一个新的 <see cref="IndexAttribute"/> 类实例。
        /// </summary>
        /// <param name="name">该索引名称</param>
        /// <param name="fields">该索引应用的字段集合。</param>
        /// <param name="sort">该索引的排序方式。</param>
        public IndexAttribute(string name, string[] fields, Enum.SortType sort)
            : this(name, fields, Enum.IndexType.Normal, sort)
        {
        }

        /// <summary>
        /// 使用指定的索引名称、索引应用的字段集合以及索引类型初始化一个新的 <see cref="IndexAttribute"/> 类实例。
        /// </summary>
        /// <param name="name">该索引名称</param>
        /// <param name="fields">该索引应用的字段集合。</param>
        /// <param name="type">该索引的类型。</param>
        public IndexAttribute(string name, string[] fields, Enum.IndexType type)
            : this(name, fields, type, Enum.SortType.Ascending)
        {
        }

        /// <summary>
        /// 使用指定的索引名称以及应用字段集合初始化一个新的 <see cref="IndexAttribute"/> 类实例。
        /// </summary>
        /// <param name="name">该索引名称</param>
        /// <param name="fields">该索引应用的字段集合。</param>
        public IndexAttribute(string name, string[] fields)
            : this(name, fields, Enum.IndexType.Normal, Enum.SortType.Ascending)
        {
        }

        /// <summary>
        /// 使用指定的索引名称、索引应用的字段集合、索引类型以及索引排序方式初始化一个新的 <see cref="IndexAttribute"/> 类实例。
        /// </summary>
        /// <param name="name">该索引名称</param>
        /// <param name="fields">该索引应用的字段集合。</param>
        /// <param name="type">该索引的类型。</param>
        /// <param name="sort">该索引的排序方式。</param>
        public IndexAttribute(string name, string[] fields, Enum.IndexType type, Enum.SortType sort)
        {
            Name = name;
            Fields = fields;
            Type = type;
            Sort = sort;
        }

        /// <summary>
        /// 获取或设置该索引应用的字段集合。
        /// </summary>
        public string[] Fields { get; set; }

        /// <summary>
        /// 获取或设置该索引的类型。
        /// </summary>
        public Enum.IndexType Type { get; set; }

        /// <summary>
        /// 获取或设置该索引的排序方式。
        /// </summary>
        public Enum.SortType Sort { get; set; }

        /// <summary>
        /// 获取或设置该索引的名称。
        /// </summary>
        public string Name { get; set; }
    }
}

using GfdbFramework.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Attribute
{
    /// <summary>
    /// 实体成员与数据库表字段或视图字段的映射关系标记类。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class FieldAttribute : System.Attribute
    {
        /// <summary>
        /// 初始化一个新的 <see cref="FieldAttribute"/> 类实例。
        /// </summary>
        public FieldAttribute()
            : this(null)
        {
        }

        /// <summary>
        /// 使用指定的映射目标字段名称初始化一个新的 <see cref="FieldAttribute"/> 类实例。
        /// </summary>
        /// <param name="name">该成员映射到数据库字段的名称。</param>
        public FieldAttribute(string name)
        {
            Name = name;
            IsNullable = NullableMode.Auto;
            SimpleIndex = SortType.Not;
            IncrementSeed = 1;
            IncrementSpeed = 1;
        }

        /// <summary>
        /// 获取或设置该成员所映射到的数据库字段名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置映射字段的数据类型（数据库的字段类型，如：varchar(60)）。
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 获取或设置映射字段是否是主键字段。
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// 获取或设置映射字段是否是唯一字段。
        /// </summary>
        public bool IsUnique { get; set; }

        /// <summary>
        /// 获取或设置映射字段是否是自增字段。
        /// </summary>
        public bool IsAutoincrement { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示在执行自动插入操作时当实体成员的值为默认值时是否将该默认值插入到数据库。
        /// </summary>
        public bool IsInsertForDefault { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示在执行自动更新操作时当实体成员的值为默认值时是否将该默认值更新到数据库。
        /// </summary>
        public bool IsUpdateForDefault { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示当前成员映射的数据库表或视图字段是否允许为空。
        /// </summary>
        public NullableMode IsNullable { get; set; }

        /// <summary>
        /// 获取或设置该字段的索引排序方式（若该值不为 0 时表示需要为该成员加上索引）。
        /// </summary>
        public SortType SimpleIndex { get; set; }

        /// <summary>
        /// 获取或设置该字段的校验约束。
        /// </summary>
        public string CheckConstraint { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值表示映射字段为自增字段时的每次递增量（仅在 <see cref="IsAutoincrement"/> 属性为 true 时有效）。
        /// </summary>
        public int IncrementSpeed { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值表示映射字段为自增字段时的起始值（仅在 <see cref="IsAutoincrement"/> 属性为 true 时有效）。
        /// </summary>
        public int IncrementSeed { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值表示映射字段的默认值。
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// 获取或设置该映射字段的说明文字。
        /// </summary>
        public string Explain { get; set; }
    }
}

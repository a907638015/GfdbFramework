using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Enum
{
    /// <summary>
    /// 实体类到数据库表或视图字段的映射关系枚举类。
    /// </summary>
    public enum MappingType
    {
        /// <summary>
        /// 所有公开属性或字段都将自动映射到数据库表或视图的字段。
        /// </summary>
        PublicProperty = 0,
        /// <summary>
        /// 仅映射被标记上 <see cref="Attribute.FieldAttribute"/> 标签的公开属性或字段。
        /// </summary>
        OnlyMarked = 1,
        /// <summary>
        /// 除去有 <see cref="Attribute.FieldAttribute"/> 标签外的所有公开属性或字段都将自动映射。
        /// </summary>
        IgnoreMarked = 2
    }
}

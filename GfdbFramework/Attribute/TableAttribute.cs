using GfdbFramework.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Attribute
{
    /// <summary>
    /// 实体类到数据库表的映射标记类。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TableAttribute : System.Attribute
    {
        /// <summary>
        /// 使用指定的映射表名初始化一个新的 <see cref="TableAttribute"/> 类实例。
        /// </summary>
        /// <param name="name">实体类所映射的数据库表名称。</param>
        public TableAttribute(string name)
            : this(name, MappingType.PublicProperty)
        {
        }

        /// <summary>
        /// 使用指定的映射方式初始化一个新的 <see cref="TableAttribute"/> 类实例。
        /// </summary>
        /// <param name="mappingType">实体类与数据库字段的映射关系。</param>
        public TableAttribute(MappingType mappingType)
            : this(null, mappingType)
        {
        }

        /// <summary>
        /// 使用指定的映射表名以及映射方式初始化一个新的 <see cref="TableAttribute"/> 类实例。
        /// </summary>
        /// <param name="name">实体类所映射的数据库表名称。</param>
        /// <param name="mappingType">实体类与数据库字段的映射关系。</param>
        public TableAttribute(string name, MappingType mappingType)
        {
            Name = name;
            MappingType = mappingType;
        }

        /// <summary>
        /// 获取或设置实体类所映射到的数据库表或视图名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置该实体类与数据库表字段的映射方式。
        /// </summary>
        public MappingType MappingType { get; set; }
    }
}

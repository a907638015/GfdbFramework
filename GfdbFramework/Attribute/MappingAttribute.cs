using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.Enum;

namespace GfdbFramework.Attribute
{
    /// <summary>
    /// 实体类到数据库表或视图的映射关系标记类。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MappingAttribute : System.Attribute
    {
        /// <summary>
        /// 使用指定映射到的数据库表或视图名称初始化一个新的 <see cref="MappingAttribute"/> 类实例。
        /// </summary>
        /// <param name="name">该实体类所映射到数据库表或视图的名称。</param>
        public MappingAttribute(string name)
            : this(name, MappingType.PublicProperty)
        {
        }

        /// <summary>
        /// 使用指定映射到的数据库表或视图名称以及映射方式初始化一个新的 <see cref="MappingAttribute"/> 类实例。
        /// </summary>
        /// <param name="name">该实体类所映射到数据库表或视图的名称。</param>
        /// <param name="mappingType">该实体类与数据库表或视图的映射方式。</param>
        public MappingAttribute(string name, MappingType mappingType)
        {
            Name = name;
            MappingType = mappingType;
        }

        /// <summary>
        /// 使用指定的映射方式初始化一个新的 <see cref="MappingAttribute"/> 类实例。
        /// </summary>
        /// <param name="mappingType">该实体类与数据库表或视图的映射方式。</param>
        public MappingAttribute(MappingType mappingType)
            : this(null, mappingType)
        {
        }

        /// <summary>
        /// 获取或设置实体类所映射到的数据库表或视图名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置该实体类与数据库表或视图的映射方式。
        /// </summary>
        public MappingType MappingType { get; set; }
    }
}

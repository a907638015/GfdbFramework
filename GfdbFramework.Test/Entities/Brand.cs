using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.Attribute;

namespace GfdbFramework.Test.Entities
{
    /// <summary>
    /// 商品品牌实体类。
    /// </summary>
    [Table("Brands")]
    public class Brand : BaseEntity
    {
        /// <summary>
        /// 获取或设置该商品品牌的名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置该商品品牌的唯一代码。
        /// </summary>
        [Field(IsNullable = Enum.NullableMode.NotNullable, SimpleIndex = Enum.SortType.Ascending)]
        public string Code { get; set; }

        /// <summary>
        /// 获取或设置该商品品牌的上级品牌主键值。
        /// </summary>
        [Field(SimpleIndex = Enum.SortType.Ascending)]
        public int? ParentID { get; set; }
    }
}

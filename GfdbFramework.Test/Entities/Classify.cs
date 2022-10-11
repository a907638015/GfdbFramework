using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.Attribute;

namespace GfdbFramework.Test.Entities
{
    /// <summary>
    /// 商品分类实体类。
    /// </summary>
    [Mapping("Classifies")]
    public class Classify : BaseEntity
    {
        /// <summary>
        /// 获取或设置该商品分类的名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置该商品分类的唯一代码。
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 获取或设置该分类的上级分类主键值。
        /// </summary>
        public int ParentID { get; set; }
    }
}

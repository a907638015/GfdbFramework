using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.Attribute;

namespace GfdbFramework.Test.Entities
{
    /// <summary>
    /// 商品信息实体类。
    /// </summary>
    [Mapping("Commodities")]
    public class Commodity : BaseEntity
    {
        /// <summary>
        /// 获取或设置该商品的名称。
        /// </summary>
        [Field(IsNullable = false)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置该商品的保质期。
        /// </summary>
        public double WarrantyPeriod { get; set; }

        /// <summary>
        /// 获取或设置该商品的保质期单位（0、年；1、月；2、日；3、时）。
        /// </summary>
        public int WarrantyUnit { get; set; }

        /// <summary>
        /// 获取或设置该商品的唯一代码。
        /// </summary>
        [Field(IsNullable = false, SimpleIndex = Enum.SortType.Ascending)]
        public string Code { get; set; }

        /// <summary>
        /// 获取或设置该商品的税率。
        /// </summary>
        public double? TaxRate { get; set; }

        /// <summary>
        /// 获取或设置该商品的分类主键值。
        /// </summary>
        [Field(IsNullable = false, SimpleIndex = Enum.SortType.Ascending)]
        public int ClassifyID { get; set; }

        /// <summary>
        /// 获取或设置该商品的品牌主键值。
        /// </summary>
        [Field(SimpleIndex = Enum.SortType.Ascending)]
        public int? BrandID { get; set; }

        /// <summary>
        /// 获取或设置该商品的成本价。
        /// </summary>
        public decimal CostPrice { get; set; }

        /// <summary>
        /// 获取或设置该商品的零售价。
        /// </summary>
        public decimal SellingPrice { get; set; }

        /// <summary>
        /// 获取或设置该商品运输时的打包单位主键值。
        /// </summary>
        [Field(IsNullable = false, SimpleIndex = Enum.SortType.Ascending)]
        public int PackageUnitID { get; set; }

        /// <summary>
        /// 获取或设置该商品每个运输包裹所含的中包包装单位的主键值。
        /// </summary>
        [Field(IsNullable = false, SimpleIndex = Enum.SortType.Ascending)]
        public int MiddleUnitID { get; set; }

        /// <summary>
        /// 获取或设置该商品每个运输包裹所含的中包包装单位数量。
        /// </summary>
        [Field(IsNullable = false, DefaultValue = 1)]
        public int MiddleQuantity { get; set; }

        /// <summary>
        /// 获取或设置该商品每个中包所含最小包装单位的主键值。
        /// </summary>
        [Field(IsNullable = false, SimpleIndex = Enum.SortType.Ascending)]
        public int MinimumUnitID { get; set; }

        /// <summary>
        /// 获取或设置该商品每个中包所含最小包装单位的数量。
        /// </summary>
        [Field(IsNullable = false)]
        public int MinimumQuantity { get; set; }

        /// <summary>
        /// 获取或设置该商品的运输包裹规格。
        /// </summary>
        [Field(IsNullable = false)]
        public string PackageNorms { get; set; }

        /// <summary>
        /// 获取或设置该商品的中包包装规格。
        /// </summary>
        [Field(IsNullable = false)]
        public string MiddleNorms { get; set; }

        /// <summary>
        /// 获取或设置该商品的最小包装规格。
        /// </summary>
        [Field(IsNullable = false)]
        public string MinimumNorms { get; set; }
    }
}

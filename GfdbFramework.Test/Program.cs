using System;
using System.Collections.Generic;
using GfdbFramework.Attribute;
using GfdbFramework.Core;
using GfdbFramework.Realize;

namespace GfdbFramework.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            DataContext dataContext = new DataContext();

            var commodities = dataContext.Commodities.InnerJoin(dataContext.Users, (left, right) => left, (left, right) => left.CreateUID == right.ID);

            foreach (var item in commodities)
            {
                Console.WriteLine(item.Name);
            }
        }
    }

    /// <summary>
    /// 所有实体类的基类。
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// 获取或设置该数据的主键值。
        /// </summary>
        [Field(IsAutoincrement = true, IsPrimaryKey = true)]
        public int ID { get; set; }
        /// <summary>
        /// 获取或设置该数据是否已被软删除。
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 获取或设置该数据行创建的时间。
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 获取或设置创建该数据的用户主键值。
        /// </summary>
        [Field(IsInsertForDefault = true)]
        public int CreateUID { get; set; }
    }

    /// <summary>
    /// 商品信息实体类。
    /// </summary>
    [Mapping("Commodities")]
    public class Commodity : BaseEntity
    {
        /// <summary>
        /// 获取或设置该商品的名称。
        /// </summary>
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
        public string Code { get; set; }
        /// <summary>
        /// 获取或设置该商品的税率。
        /// </summary>
        public double? TaxRate { get; set; }
        /// <summary>
        /// 获取或设置该商品的分类主键值。
        /// </summary>
        public int ClassifyID { get; set; }
        /// <summary>
        /// 获取或设置该商品的品牌主键值。
        /// </summary>
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
        public int PackageUnitID { get; set; }
        /// <summary>
        /// 获取或设置该商品每个运输包裹所含的中包包装单位的主键值。
        /// </summary>
        public int MiddleUnitID { get; set; }
        /// <summary>
        /// 获取或设置该商品每个运输包裹所含的中包包装单位数量。
        /// </summary>
        public int MiddleQuantity { get; set; }
        /// <summary>
        /// 获取或设置该商品每个中包所含最小包装单位的主键值。
        /// </summary>
        public int MinimumUnitID { get; set; }
        /// <summary>
        /// 获取或设置该商品每个中包所含最小包装单位的数量。
        /// </summary>
        public int MinimumQuantity { get; set; }
        /// <summary>
        /// 获取或设置该商品的运输包裹规格。
        /// </summary>
        public string PackageNorms { get; set; }
        /// <summary>
        /// 获取或设置该商品的中包包装规格。
        /// </summary>
        public string MiddleNorms { get; set; }
        /// <summary>
        /// 获取或设置该商品的最小包装规格。
        /// </summary>
        public string MinimumNorms { get; set; }
    }

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

    /// <summary>
    /// 商品品牌实体类。
    /// </summary>
    [Mapping("Brands")]
    public class Brand : BaseEntity
    {
        /// <summary>
        /// 获取或设置该商品品牌的名称。
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 获取或设置该商品品牌的唯一代码。
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 获取或设置该商品品牌的上级品牌主键值。
        /// </summary>
        public int ParentID { get; set; }
    }

    /// <summary>
    /// 商品单位实体类。
    /// </summary>
    [Mapping("Units")]
    public class Unit : BaseEntity
    {
        /// <summary>
        /// 获取或设置该商品单位的名称。
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 用户信息实体类。
    /// </summary>
    [Mapping("Users")]
    public class User : BaseEntity
    {
        /// <summary>
        /// 获取或设置该用户的登录账号。
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 获取或设置该用户的登录密码。
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 获取或设置该用户的名称。
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 获取或设置该用户的工号。
        /// </summary>
        public string JobNumber { get; set; }
        /// <summary>
        /// 获取或设置该用户的手机号码。
        /// </summary>
        public string Telephone { get; set; }
    }

    /// <summary>
    /// 数据操作上下文对象类。
    /// </summary>
    public class DataContext : SqlServer.DataContext
    {
        private Modifiable<User, User> _Users = null;
        private Modifiable<Commodity, Commodity> _Commodities = null;
        private Modifiable<Classify, Classify> _Classifies = null;
        private Modifiable<Brand, Brand> _Brands = null;
        private Modifiable<Unit, Unit> _Units = null;
        public DataContext()
            : base("Sql Server 2012", "data source=.;database=TestDB;uid=sa;pwd=sasasa")
        {

        }

        /// <summary>
        /// 获取一个用于对数据库 Users 表进行增删改查的操作对象。
        /// </summary>
        public Modifiable<User, User> Users
        {
            get
            {
                if (_Users == null)
                    _Users = base.GetTable<User>();

                return _Users;
            }
        }

        /// <summary>
        /// 获取一个用于对数据库 Commodities 表进行增删改查的操作对象。
        /// </summary>
        public Modifiable<Commodity, Commodity> Commodities
        {
            get
            {
                if (_Commodities == null)
                    _Commodities = base.GetTable<Commodity>();

                return _Commodities;
            }
        }

        /// <summary>
        /// 获取一个用于对数据库 Classifies 表进行增删改查的操作对象。
        /// </summary>
        public Modifiable<Classify, Classify> Classifies
        {
            get
            {
                if (_Classifies == null)
                    _Classifies = base.GetTable<Classify>();

                return _Classifies;
            }
        }

        /// <summary>
        /// 获取一个用于对数据库 Brands 表进行增删改查的操作对象。
        /// </summary>
        public Modifiable<Brand, Brand> Brands
        {
            get
            {
                if (_Brands == null)
                    _Brands = base.GetTable<Brand>();

                return _Brands;
            }
        }

        /// <summary>
        /// 获取一个用于对数据库 Units 表进行增删改查的操作对象。
        /// </summary>
        public Modifiable<Unit, Unit> Units
        {
            get
            {
                if (_Units == null)
                    _Units = base.GetTable<Unit>();

                return _Units;
            }
        }
    }
}

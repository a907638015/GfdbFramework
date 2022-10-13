using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.Core;
using GfdbFramework.Test.Entities;

namespace GfdbFramework.Test
{
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

        /// <summary>
        /// 初始化一个新的 <see cref="DataContext"/> 类实例。
        /// </summary>
        public DataContext()
            : base("Sql Server 2012", "data source=.;database=TestDB2;uid=sa;pwd=sasasa")
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

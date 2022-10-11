using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.Attribute;

namespace GfdbFramework.Test.Entities
{
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
}

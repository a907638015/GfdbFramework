using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.Attribute;

namespace GfdbFramework.Test.Entities
{
    /// <summary>
    /// 用户信息实体类。
    /// </summary>
    [Mapping("Users")]
    public class User : BaseEntity
    {
        /// <summary>
        /// 获取或设置该用户的登录账号。
        /// </summary>
        [Field(IsNullable = false, SimpleIndex = Enum.SortType.Ascending)]
        public string Account { get; set; }

        /// <summary>
        /// 获取或设置该用户的登录密码。
        /// </summary>
        [Field(IsNullable = false)]
        public string Password { get; set; }

        /// <summary>
        /// 获取或设置该用户的名称。
        /// </summary>
        [Field(IsNullable = false)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置该用户的工号。
        /// </summary>
        [Field(IsNullable = false, SimpleIndex = Enum.SortType.Ascending)]
        public string JobNumber { get; set; }

        /// <summary>
        /// 获取或设置该用户的手机号码。
        /// </summary>
        public string Telephone { get; set; }
    }
}

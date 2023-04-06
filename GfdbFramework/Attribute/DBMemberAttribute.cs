using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Attribute
{
    /// <summary>
    /// 数据库成员标记类（在使用 GfdbFramework 框架时若指定成员被打上此标记则会将标记的成员转换成 <see cref="Field.MemberField"/> 字段）。
    /// </summary>
    public class DBMemberAttribute : System.Attribute
    {
    }
}

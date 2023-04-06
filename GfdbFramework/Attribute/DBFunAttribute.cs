using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Attribute
{
    /// <summary>
    /// 数据库函数标记类（在使用 GfdbFramework 框架时若指定方法被打上此标记则会将标记的方法转换成 <see cref="Field.MethodField"/> 字段）。
    /// </summary>
    public class DBFunAttribute : System.Attribute
    {
    }
}

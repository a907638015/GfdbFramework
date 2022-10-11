using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Attribute
{
    /// <summary>
    /// 数据库函数标记类（当某方法标记上此类时将会被强制转成 <see cref="Field.MethodField"/> 字段）。
    /// </summary>
    public class DBFunctionAttribute : System.Attribute
    {
    }
}

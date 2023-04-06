using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace GfdbFramework.Interface
{
    /// <summary>
    /// 参数上下文接口类。
    /// </summary>
    public interface IParameterContext : IDisposable
    {
        /// <summary>
        /// 添加一个常量值到参数上下文中。
        /// </summary>
        /// <param name="value">待添加到参数上下文中的常量值。</param>
        /// <returns>添加到参数中后所使用的参数名。</returns>
        string Add(object value);

        /// <summary>
        /// 将当前参数上下文对象转换成对应的参数集合。
        /// </summary>
        /// <returns>转换后对应的参数集合对象。</returns>
        Core.ReadOnlyList<DbParameter> ToList();

        /// <summary>
        /// 获取一个值，该值指示当前上下文是否应当开启参数化操作。
        /// </summary>
        bool EnableParametric { get; }
    }
}

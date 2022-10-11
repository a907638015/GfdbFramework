using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 通用枚举器类。
    /// </summary>
    /// <typeparam name="T">枚举器中所枚举出的每个成员类型。</typeparam>
    internal class Enumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _Enumerable = null;

        /// <summary>
        /// 使用指定的枚举器初始化一个新的 <see cref="Enumerable{T}"/> 类实例。
        /// </summary>
        /// <param name="enumerable">该枚举器内部所使用的枚举对象。</param>
        internal Enumerable(IEnumerable<T> enumerable)
        {
            _Enumerable = enumerable;
        }

        /// <summary>
        /// 获取一个可以用于循环读取当前枚举器中所有成员信息的计数器。
        /// </summary>
        /// <returns>用于循环读取当前枚举器中所有成员信息的计数器。</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator<T>(_Enumerable?.GetEnumerator());
        }

        /// <summary>
        /// 获取一个可以用于循环读取当前枚举器中所有成员信息的计数器。
        /// </summary>
        /// <returns>用于循环读取当前枚举器中所有成员信息的计数器。</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator<T>(_Enumerable?.GetEnumerator());
        }
    }
}

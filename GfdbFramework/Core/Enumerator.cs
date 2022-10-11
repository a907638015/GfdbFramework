using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 通用循环计数类。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class Enumerator<T> : IEnumerator<T>
    {
        private IEnumerator<T> _Enumerator = null;

        /// <summary>
        /// 使用指定的循环计数对象初始化一个新的 <see cref="Enumerator{T}"/> 类实例。
        /// </summary>
        /// <param name="enumerator">该计数器内部所使用的循环计数对象。</param>
        internal Enumerator(IEnumerator<T> enumerator)
        {
            _Enumerator = enumerator;
        }

        /// <summary>
        /// 获取位于当前索引处的成员信息。
        /// </summary>
        public T Current
        {
            get
            {
                return _Enumerator == null ? default : _Enumerator.Current;
            }
        }

        /// <summary>
        /// 获取位于当前索引处的成员信息。
        /// </summary>
        object System.Collections.IEnumerator.Current
        {
            get
            {
                return _Enumerator == null ? default : _Enumerator.Current;
            }
        }

        /// <summary>
        /// 释放当前对象所占用的资源信息。
        /// </summary>
        public void Dispose()
        {
            _Enumerator?.Dispose();

            _Enumerator = null;
        }

        /// <summary>
        /// 将当前对象中的索引位置往前移一位。
        /// </summary>
        /// <returns>若移动后的索引处还有成员则返回 true，否则返回 false。</returns>
        public bool MoveNext()
        {
            if (_Enumerator == null)
                return false;

            return _Enumerator.MoveNext();
        }

        /// <summary>
        /// 重置当前计数器的索引位置。
        /// </summary>
        public void Reset()
        {
            _Enumerator?.Reset();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Realize
{
    /// <summary>
    /// 只读集合类。
    /// </summary>
    /// <typeparam name="T">集合中每个成员的类型。</typeparam>
    public class ReadOnlyList<T> : Interface.IReadOnlyList<T>
    {
        private readonly IList<T> _List = null;

        /// <summary>
        /// 使用指定的枚举器初始化一个新的 <see cref="ReadOnlyList{T}"/> 类实例。
        /// </summary>
        /// <param name="enumerable">该集合所需包含的成员枚举器。</param>
        public ReadOnlyList(IEnumerable<T> enumerable)
        {
            _List = enumerable == null ? null : new List<T>(enumerable);
        }

        /// <summary>
        /// 使用指定的成员数组初始化一个新的 <see cref="ReadOnlyList{T}"/> 类实例。
        /// </summary>
        /// <param name="items">该集合所需包含的成员数组。</param>
        public ReadOnlyList(params T[] items)
        {
            _List = items == null ? null : new List<T>(items);
        }

        /// <summary>
        /// 使用指定的集合初始化一个新的 <see cref="ReadOnlyList{T}"/> 类实例（隐式转换构造函数，隐式转换时将直接引用 <paramref name="list"/> 参数而不会从新构造一个 <see cref="IList{T}"/>）。
        /// </summary>
        /// <param name="list">成员集合。</param>
        private ReadOnlyList(IList<T> list)
        {
            _List = list;
        }

        /// <summary>
        /// 获取集合中位于指定索引处的成员。
        /// </summary>
        /// <param name="index">待获取成员所在的索引位置。</param>
        /// <returns>指定索引出的成员信息。</returns>
        public T this[int index]
        {
            get
            {
                if (_List == null)
                    throw new ArgumentOutOfRangeException("索引超出集合中的成员数量范围");

                return _List[index];
            }
        }

        /// <summary>
        /// 获取当前集合中所有成员的个数。
        /// </summary>
        public int Count
        {
            get
            {
                return _List == null ? 0 : _List.Count;
            }
        }

        /// <summary>
        /// 获取一个用于循环读取当前集合中所有成员的计数器。
        /// </summary>
        /// <returns>一个用于循环读取当前集合中所有成员的计数器。</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new Core.Enumerator<T>(_List?.GetEnumerator());
        }

        /// <summary>
        /// 获取一个用于循环读取当前集合中所有成员的计数器。
        /// </summary>
        /// <returns>一个用于循环读取当前集合中所有成员的计数器。</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Core.Enumerator<T>(_List?.GetEnumerator());
        }

        /// <summary>
        /// 将 <see cref="List{T}"/> 类型对象隐式转换成 <see cref="ReadOnlyList{T}"/> 类型。
        /// </summary>
        /// <param name="list">待转换的集合对象。</param>
        public static implicit operator ReadOnlyList<T>(List<T> list)
        {
            return new ReadOnlyList<T>(list);
        }
    }
}

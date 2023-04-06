using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 只读集合类。
    /// </summary>
    /// <typeparam name="T">集合中每个成员的数据类型。</typeparam>
    public class ReadOnlyList<T> : IList<T>
    {
        private readonly IList<T> _List = null;

        /// <summary>
        /// 使用指定的集合初始化一个新的 <see cref="ReadOnlyList{T}"/> 类实例。
        /// </summary>
        /// <param name="list">当前只读对象所需包含的成员集合列表。</param>
        public ReadOnlyList(IList<T> list)
        {
            if (list is ReadOnlyList<T> readOnlyList)
                _List = readOnlyList._List;
            else
                _List = list;
        }

        /// <summary>
        /// 使用指定的成员数组初始化一个新的 <see cref="ReadOnlyList{T}"/> 类实例。
        /// </summary>
        /// <param name="args">当前只读对象所需包含的成员数组。</param>
        public ReadOnlyList(params T[] args)
        {
            if (args != null)
                _List = new List<T>(args);
        }

        /// <summary>
        /// 使用指定的成员枚举器初始化一个新的 <see cref="ReadOnlyList{T}"/> 类实例。
        /// </summary>
        /// <param name="enumerable">当前只读对象所需包含的成员枚举器。</param>
        public ReadOnlyList(IEnumerable<T> enumerable)
        {
            if (enumerable != null)
                _List = new List<T>(enumerable);
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
                if (_List == null || index < 0 || index >= _List.Count)
                    throw new ArgumentOutOfRangeException(nameof(index), "获取只读集合中指定索引处的成员时索引位置超出集合范围");

                return _List[index];
            }
            set
            {
                throw new Exception("只读集合不能修改指定索引处的成员值");
            }
        }

        /// <summary>
        /// 获取该集合中所有成员的个数。
        /// </summary>
        public int Count
        {
            get
            {
                return _List == null ? 0 : _List.Count;
            }
        }

        /// <summary>
        /// 获取一个值，该值指示当前集合是否是只读集合（就当前对象而言，该属性始终返回 true 值）。
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 判断当前集合中是否包含有指定的成员信息。
        /// </summary>
        /// <param name="item">需要判断是否存在于当前集合的成员信息。</param>
        /// <returns>若存在该成员时返回 true，否则返回 false。</returns>
        public bool Contains(T item)
        {
            return _List == null ? false : _List.Contains(item);
        }

        /// <summary>
        /// 从指定索引处开始复制成员到另外一个数组中。
        /// </summary>
        /// <param name="array">用于接收的数组对象。</param>
        /// <param name="arrayIndex">开始复制的索引位置。</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (_List != null)
                _List.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 获取指定成员在当前集合中的索引位置。
        /// </summary>
        /// <param name="item">待获取索引位置的成员对象。</param>
        /// <returns>若在集合中找到该成员则返回第一次出现该成员的索引位置，否则返回 -1。</returns>
        public int IndexOf(T item)
        {
            return _List == null ? -1 : _List.IndexOf(item);
        }

        /// <summary>
        /// 将 <see cref="List{T}"/> 类型对象隐式转换成 <see cref="ReadOnlyList{T}"/> 类型。
        /// </summary>
        /// <param name="list">待转换的集合对象。</param>
        public static implicit operator ReadOnlyList<T>(List<T> list)
        {
            return new ReadOnlyList<T>(list);
        }

        /// <summary>
        /// 将 <typeparamref name="T"/> 类型的数组对象隐式转换成 <see cref="ReadOnlyList{T}"/> 类型。
        /// </summary>
        /// <param name="list">待转换的数组对象。</param>
        public static implicit operator ReadOnlyList<T>(T[] list)
        {
            return new ReadOnlyList<T>(list);
        }

        #region 隐藏不必要的接口方法

        void ICollection<T>.Add(T item)
        {
            throw new Exception("只读集合不能添加新的成员");
        }

        void ICollection<T>.Clear()
        {
            throw new Exception("只读集合不能清空已有的成员");
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new Exception("只读集合不能移除已有的成员");
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new Exception("只读集合不能在指定索引位置插入新的成员");
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new Exception("只读集合不能移除指定索引处已有的成员");
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new Enumerator<T>(_List?.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator<T>(_List?.GetEnumerator());
        }

        #endregion
    }
}

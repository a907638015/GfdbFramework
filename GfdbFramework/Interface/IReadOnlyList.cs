using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Interface
{
    /// <summary>
    /// 只读集合接口。
    /// </summary>
    /// <typeparam name="T">集合中每个成员的类型。</typeparam>
    public interface IReadOnlyList<out T> : IEnumerable<T>
    {
        /// <summary>
        /// 获取集合中位于指定索引处的成员。
        /// </summary>
        /// <param name="index">待获取成员所在的索引位置。</param>
        /// <returns>指定索引出的成员信息。</returns>
        T this[int index] { get; }

        /// <summary>
        /// 获取当前集合中所有成员的个数。
        /// </summary>
        int Count { get; }
    }
}

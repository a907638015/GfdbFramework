using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Interface
{
    /// <summary>
    /// 只读键值对集合接口。
    /// </summary>
    /// <typeparam name="TKey">集合中的键类型。</typeparam>
    /// <typeparam name="TValue">集合中的值类型。</typeparam>
    public interface IReadOnlyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        /// 获取指定键对应的值。
        /// </summary>
        /// <param name="key">待获取对应值的键。</param>
        /// <returns>若找到对应键的值则返回该值，否则抛出异常。</returns>
        TValue this[TKey key] { get; }

        /// <summary>
        /// 获取一个可以用于枚举出当前集合中所有值的枚举器。
        /// </summary>
        IEnumerable<TValue> Values { get; }

        /// <summary>
        /// 获取一个可以用于枚举出当前集合中所有键的枚举器。
        /// </summary>
        IEnumerable<TKey> Keys { get; }

        /// <summary>
        /// 获取当前集合中所有键值对的数量。
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 校验当前集合中是否包含指定键。
        /// </summary>
        /// <param name="key">待校验是否存在于当前集合中的键。</param>
        /// <returns>若该集合包含有该键值时返回 true，否则返回 false。</returns>
        bool ContainsKey(TKey key);

        /// <summary>
        /// 尝试从集合中读取指定键对应的值。
        /// </summary>
        /// <param name="key">待获取对应值的键。</param>
        /// <param name="value">获取到对应键的值。</param>
        /// <returns>若在集合中找到该键对应的值时 true，否则返回 false。</returns>
        bool TryGetValue(TKey key, out TValue value);
    }
}

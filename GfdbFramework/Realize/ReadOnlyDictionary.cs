using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Realize
{
    /// <summary>
    /// 只读键值对集合类。
    /// </summary>
    /// <typeparam name="TKey">集合中的键类型。</typeparam>
    /// <typeparam name="TValue">集合中的值类型。</typeparam>
    public class ReadOnlyDictionary<TKey, TValue> : Interface.IReadOnlyDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _Dictionary = null;

        /// <summary>
        /// 使用指定的键值对枚举器初始化一个新的 <see cref="ReadOnlyDictionary{TKey, TValue}"/> 类实例。
        /// </summary>
        /// <param name="keyValuePairs">该集合中所需包含的键值对枚举器。</param>
        public ReadOnlyDictionary(IDictionary<TKey, TValue> keyValuePairs)
        {
            _Dictionary = keyValuePairs == null ? null : new Dictionary<TKey, TValue>(keyValuePairs);
        }

        /// <summary>
        /// 使用指定的键值对枚举器初始化一个新的 <see cref="ReadOnlyDictionary{TKey, TValue}"/> 类实例。
        /// </summary>
        /// <param name="keyValuePairs">该集合中所需包含的键值对枚举器。</param>
        public ReadOnlyDictionary(Interface.IReadOnlyDictionary<TKey, TValue> keyValuePairs)
        {
            if (keyValuePairs != null)
            {
                if (keyValuePairs is ReadOnlyDictionary<TKey, TValue> readOnlyDictionary)
                {
                    _Dictionary = new Dictionary<TKey, TValue>(readOnlyDictionary._Dictionary);
                }
                else
                {
                    _Dictionary = new Dictionary<TKey, TValue>();

                    foreach (var item in keyValuePairs)
                    {
                        _Dictionary.Add(item.Key, item.Value);
                    }
                }
            }
        }

        /// <summary>
        /// 使用指定的键值对数组初始化一个新的 <see cref="ReadOnlyDictionary{TKey, TValue}"/> 类实例。
        /// </summary>
        /// <param name="keyValuePairs">该集合中所需包含的键值对数组。</param>
        public ReadOnlyDictionary(params KeyValuePair<TKey, TValue>[] keyValuePairs)
        {
            if (keyValuePairs != null)
            {
                _Dictionary = new Dictionary<TKey, TValue>();

                foreach (var item in keyValuePairs)
                {
                    _Dictionary.Add(item.Key, item.Value);
                }
            }
        }

        /// <summary>
        /// 使用指定的键值对枚举器初始化一个新的 <see cref="ReadOnlyDictionary{TKey, TValue}"/> 类实例（隐式转换构造函数，隐式转换时将直接引用 <paramref name="keyValuePairs"/> 参数而不会从新构造一个 <see cref="Dictionary{TKey, TValue}"/>）。
        /// </summary>
        /// <param name="keyValuePairs">该集合中所需包含的键值对枚举器。</param>
        private ReadOnlyDictionary(Dictionary<TKey, TValue> keyValuePairs)
        {
            _Dictionary = keyValuePairs;
        }

        /// <summary>
        /// 获取指定键对应的值。
        /// </summary>
        /// <param name="key">待获取对应值的键。</param>
        /// <returns>若找到对应键的值则返回该值，否则抛出异常。</returns>
        public TValue this[TKey key]
        {
            get
            {
                if (_Dictionary == null)
                    throw new ArgumentOutOfRangeException("未在当前键值对集合中找到指定键所对应的值");

                return _Dictionary[key];
            }
        }

        /// <summary>
        /// 获取一个可以用于枚举出当前集合中所有值的枚举器。
        /// </summary>
        public IEnumerable<TValue> Values
        {
            get
            {
                return new Core.Enumerable<TValue>(_Dictionary?.Values);
            }
        }

        /// <summary>
        /// 获取一个可以用于枚举出当前集合中所有键的枚举器。
        /// </summary>
        public IEnumerable<TKey> Keys
        {
            get
            {
                return new Core.Enumerable<TKey>(_Dictionary?.Keys);
            }
        }

        /// <summary>
        /// 获取当前集合中所有键值对的数量。
        /// </summary>
        public int Count
        {
            get
            {
                return _Dictionary == null ? 0 : _Dictionary.Count;
            }
        }

        /// <summary>
        /// 校验当前集合中是否包含指定键。
        /// </summary>
        /// <param name="key">待校验是否存在于当前集合中的键。</param>
        /// <returns>若该集合包含有该键值时返回 true，否则返回 false。</returns>
        public bool ContainsKey(TKey key)
        {
            return _Dictionary != null && _Dictionary.ContainsKey(key);
        }

        /// <summary>
        /// 尝试从集合中读取指定键对应的值。
        /// </summary>
        /// <param name="key">待获取对应值的键。</param>
        /// <param name="value">获取到对应键的值。</param>
        /// <returns>若在集合中找到该键对应的值时 true，否则返回 false。</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (_Dictionary == null)
            {
                value = default;

                return false;
            }
            else
            {
                return _Dictionary.TryGetValue(key, out value);
            }
        }

        /// <summary>
        /// 获取一个用于循环读取当前集合中所有键值对的计数器。
        /// </summary>
        /// <returns>一个用于循环读取当前集合中所有键值对的计数器。</returns>
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return new Core.Enumerator<KeyValuePair<TKey, TValue>>(_Dictionary?.GetEnumerator());
        }

        /// <summary>
        /// 获取一个用于循环读取当前集合中所有键值对的计数器。
        /// </summary>
        /// <returns>一个用于循环读取当前集合中所有键值对的计数器。</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Core.Enumerator<KeyValuePair<TKey, TValue>>(_Dictionary?.GetEnumerator());
        }

        /// <summary>
        /// 将 <see cref="List{T}"/> 类型对象隐式转换成 <see cref="ReadOnlyList{T}"/> 类型。
        /// </summary>
        /// <param name="dictionary">待转换的集合对象。</param>
        public static implicit operator ReadOnlyDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }
    }
}

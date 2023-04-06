using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 只读键值对类。
    /// </summary>
    /// <typeparam name="TKey">键值对中的键类型。</typeparam>
    /// <typeparam name="TValue">键值对中的值类型。</typeparam>
    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _KeyValues = null;

        /// <summary>
        /// 使用指定的键值对初始化一个新的 <see cref="ReadOnlyDictionary{TKey, TValue}"/> 类实例。
        /// </summary>
        /// <param name="keyValues">该键值对所需包含的键值集合。</param>
        public ReadOnlyDictionary(IDictionary<TKey, TValue> keyValues)
        {
            if (keyValues is ReadOnlyDictionary<TKey, TValue> readOnlyKeyValues)
                _KeyValues = readOnlyKeyValues._KeyValues;
            else
                _KeyValues = keyValues;
        }

        /// <summary>
        /// 获取键值对中指定键所对应的值。
        /// </summary>
        /// <param name="key">待获取对应值的键。</param>
        /// <returns>指定键所对应的值。</returns>
        public TValue this[TKey key]
        {
            get
            {
                if (_KeyValues == null)
                    throw new ArgumentOutOfRangeException(nameof(key), "获取只读键值对中指定键所对应的值时键值超出范围");

                return _KeyValues[key];
            }
            set
            {
                throw new Exception("只读键值对不能修改指定键所对应的值");
            }
        }

        /// <summary>
        /// 获取当前键值对中所有的键集合。
        /// </summary>
        public ICollection<TKey> Keys
        {
            get
            {
                if (_KeyValues == null)
                    return new ReadOnlyList<TKey>();

                return _KeyValues.Keys;
            }
        }

        /// <summary>
        /// 获取当前键值对中所有的值集合。
        /// </summary>
        public ICollection<TValue> Values
        {
            get
            {
                if (_KeyValues == null)
                    return new ReadOnlyList<TValue>();

                return _KeyValues.Values;
            }
        }

        /// <summary>
        /// 获取当前键值对中所有键值的数量。
        /// </summary>
        public int Count
        {
            get
            {
                return _KeyValues == null ? 0 : _KeyValues.Count;
            }
        }

        /// <summary>
        /// 获取一个值，该值指示当前键值对是否是只读的（就当前对象而言，该属性始终返回 true 值）。
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 确认当前键值对中是否含有指定的键。
        /// </summary>
        /// <param name="key">需要确认是否包含的键。</param>
        /// <returns>若键值对中含有该键时返回 true，否则返回 false。</returns>
        public bool ContainsKey(TKey key)
        {
            return _KeyValues != null && _KeyValues.ContainsKey(key);
        }

        /// <summary>
        /// 尝试从当前键值对中获取指定键所对应的值。
        /// </summary>
        /// <param name="key">待获取对应值的键。</param>
        /// <param name="value">获取到的值。</param>
        /// <returns>若该键值对中包含有指定的键时返回 true，否则返回 false。</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (_KeyValues == null)
            {
                value = default;

                return false;
            }

            return _KeyValues.TryGetValue(key, out value);
        }

        /// <summary>
        /// 将 <see cref="Dictionary{TKey, TValue}"/> 类型对象隐式转换成 <see cref="ReadOnlyDictionary{TKey, TValue}"/> 类型。
        /// </summary>
        /// <param name="keyValues">待转换的键值对。</param>
        public static implicit operator ReadOnlyDictionary<TKey, TValue>(Dictionary<TKey, TValue> keyValues)
        {
            return new ReadOnlyDictionary<TKey, TValue>(keyValues);
        }

        #region 隐藏不必要的接口方法

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            throw new Exception("只读键值对不能添加新的键值");
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new Exception("只读键值对不能添加新的键值");
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            throw new Exception("只读集合不能清空已有的键值");
        }
        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            throw new Exception("只读集合不能移除已有键值");
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new Exception("只读集合不能移除已有键值");
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            if (_KeyValues == null)
                return false;

            return ((ICollection<KeyValuePair<TKey, TValue>>)_KeyValues).Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (_KeyValues != null)
                ((ICollection<KeyValuePair<TKey, TValue>>)_KeyValues).CopyTo(array, arrayIndex);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return new Enumerator<KeyValuePair<TKey, TValue>>(_KeyValues?.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator<KeyValuePair<TKey, TValue>>(_KeyValues?.GetEnumerator());
        }

        #endregion
    }
}

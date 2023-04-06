using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 与数据库操作相关的静态扩展方法。
    /// </summary>
    public static class Extend
    {
        private static readonly Type _ExtendType = typeof(Extend);
        private static readonly string _ExtendLikeMethodName = nameof(Extend.Like);

        /// <summary>
        /// 将当前不可为 null 值的结构对象转换成可为 null 的类型对象。
        /// </summary>
        /// <typeparam name="T">结构体类型。</typeparam>
        /// <param name="self">待转换的结构体对象。</param>
        /// <returns>转换后的可为 null 的对象。</returns>
        public static T? ToNull<T>(this T self) where T : struct
        {
            return (T?)self;
        }

        /// <summary>
        /// 确认当前集合中是否包含某一指定成员。
        /// </summary>
        /// <typeparam name="TItem">集合中的成员类型。</typeparam>
        /// <param name="self">调用该方法的实例对象。</param>
        /// <param name="item">待确认是否存在于当前集合中的对象。</param>
        /// <returns>若当前集合中存在指定成员则返回 true，否则返回 false。</returns>
        public static bool Contains<TItem>(this IEnumerable<TItem> self, TItem item) where TItem : struct
        {
            if (self != null)
            {
                foreach (var obj in self)
                {
                    if (obj.Equals(item))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 确认当前集合中是否包含某一指定成员。
        /// </summary>
        /// <param name="self">调用该方法的实例对象。</param>
        /// <param name="rule">待确认是否存在于当前集合中的对象。</param>
        /// <returns>若当前集合中存在指定成员则返回 true，否则返回 false。</returns>
        public static bool Contains(this IEnumerable<string> self, string rule)
        {
            if (self != null)
            {
                foreach (var obj in self)
                {
                    if (obj == rule)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 校验当前字符串是否符合指定格式（暂不支持直接在 .Net 框架中调用，后续版本会跟进）。
        /// </summary>
        /// <param name="self">调用该方法的实例对象。</param>
        /// <param name="rule">需要符合的规则字符串。</param>
        /// <returns>若当前字符串满足该规则条件时返回 true，否则返回 false。</returns>
        public static bool Like(this string self, string rule)
        {
            throw new Exception($"暂不支持直接在 .Net 框架中调用 {_ExtendType.FullName}.{_ExtendLikeMethodName} 方法，后续版本会跟进");
        }
    }
}

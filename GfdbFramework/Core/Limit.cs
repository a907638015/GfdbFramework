using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 对查询数据结果进行数据行限定的结构体。
    /// </summary>
    public struct Limit
    {
        /// <summary>
        /// 使用指定的前几行数据值初始化一个新的 <see cref="Limit"/> 结构对象。
        /// </summary>
        /// <param name="topCount">指示应当返回的前几行数据。</param>
        internal Limit(int topCount)
            : this(0, topCount)
        {
        }

        /// <summary>
        /// 使用指定开始数据行下标以及所需返回的数据行数初始化一个新的 <see cref="Limit"/> 结构对象。
        /// </summary>
        /// <param name="start">指示结果集中应当返回的起始数据行下标。</param>
        /// <param name="count">指示结果集中应当返回的数据行数。</param>
        internal Limit(int start, int count)
        {
            Start = start;
            Count = count < 0 ? 0 : count;
        }

        /// <summary>
        /// 获取一个值，该值指示应当从查询结果集中的第几行开始返回数据（包含该下标所在行的数据）。
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// 获取一个值，该值指示返回数据应当到第几行就截止返回（不包含该下标所在行的数据）。
        /// </summary>
        public int Count { get; }
    }
}

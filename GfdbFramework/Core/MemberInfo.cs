using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Core
{
    /// <summary>
    /// .Net 对象字段成员信息类。
    /// </summary>
    public class MemberInfo
    {
        /// <summary>
        /// 使用指定的成员以及成员所对应的框架字段初始化一个新的 <see cref="MemberInfo"/> 类实例。
        /// </summary>
        /// <param name="member">该对象字段的成员信息。</param>
        /// <param name="field">该对象字段成员所对应的框架字段信息</param>
        public MemberInfo(System.Reflection.MemberInfo member, Field.Field field)
        {
            Member = member;
            Field = field;
        }

        /// <summary>
        /// 获取该对象字段成员的信息。
        /// </summary>
        public System.Reflection.MemberInfo Member { get; }

        /// <summary>
        /// 获取该对象字段成员对应的框架字段信息。
        /// </summary>
        public Field.Field Field { get; }
    }
}

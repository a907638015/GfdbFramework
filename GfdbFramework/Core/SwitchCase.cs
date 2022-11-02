using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.Field;

namespace GfdbFramework.Core
{
    /// <summary>
    /// Switch 分支字段中的某个分支信息类。
    /// </summary>
    public class SwitchCase
    {
        /// <summary>
        /// 使用指定的返回值字段以及条件判定值集合初始化一个新的 <see cref="SwitchCase"/> 类实例。
        /// </summary>
        /// <param name="body">该分支的返回值字段。</param>
        /// <param name="testValues">该分支的条件判定值集合。</param>
        internal SwitchCase(BasicField body, Interface.IReadOnlyList<ConstantField> testValues)
        {
            Body = body;
            TestValues = testValues;
        }

        /// <summary>
        /// 获取该分支的返回值字段。
        /// </summary>
        public BasicField Body { get; }

        /// <summary>
        /// 获取该分支的条件判定值。
        /// </summary>
        public Interface.IReadOnlyList<ConstantField> TestValues { get; }
    }
}

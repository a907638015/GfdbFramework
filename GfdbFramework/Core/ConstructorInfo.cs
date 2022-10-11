using System;
using System.Text;
using System.Collections.Generic;
using GfdbFramework.Interface;
using GfdbFramework.Realize;

namespace GfdbFramework.Core
{
    /// <summary>
    /// .Net 对象字段所需的构造函数信息类。
    /// </summary>
    public class ConstructorInfo
    {
        /// <summary>
        /// 使用指定的构造函数以及构造参数初始化一个新的 <see cref="ConstructorInfo"/> 类实例。
        /// </summary>
        /// <param name="constructor">该对象字段对应类的构造函数。</param>
        /// <param name="parameters">该对象字段对应类的构造参数枚举器。</param>
        internal ConstructorInfo(System.Reflection.ConstructorInfo constructor, IEnumerable<Field.Field> parameters)
        {
            Constructor = constructor;
            Parameters = new ReadOnlyList<Field.Field>(parameters);
        }

        /// <summary>
        /// 获取该对象字段对应类的构造函数信息。
        /// </summary>
        public System.Reflection.ConstructorInfo Constructor { get; }

        /// <summary>
        /// 获取构建该对象字段对应类实例时所需的构造参数集合。
        /// </summary>
        public Interface.IReadOnlyList<Field.Field> Parameters { get; }
    }
}

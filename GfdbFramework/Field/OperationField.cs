using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.Enum;

namespace GfdbFramework.Field
{
    /// <summary>
    /// 带有具体操作的字段类。
    /// </summary>
    public abstract class OperationField : BasicField
    {
        /// <summary>
        /// 使用指定的字段类型、字段返回值的数据类型以及字段的操作类型初始化一个新的 <see cref="OperationField"/> 类实例。
        /// </summary>
        /// <param name="type">该字段的类型。</param>
        /// <param name="dataType">该字段返回值的数据类型。</param>
        /// <param name="operationType">该字段的操作类型。</param>
        internal OperationField(FieldType type, Type dataType, OperationType operationType)
            : base(type, dataType)
        {
            OperationType = operationType;
        }

        /// <summary>
        /// 获取当前字段的操作类型。
        /// </summary>
        public OperationType OperationType { get; }
    }
}

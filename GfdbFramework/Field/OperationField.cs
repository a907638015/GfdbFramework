using GfdbFramework.Enum;
using GfdbFramework.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Field
{
    /// <summary>
    /// 带有具体操作的基础字段类。
    /// </summary>
    public abstract class OperationField : BasicField
    {
        /// <summary>
        /// 使用指定的数据操作上下文、字段类型、字段返回数据类型以及该字段的操作类型初始化一个新的 <see cref="OperationField"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该字段所使用的数据操作上下文。</param>
        /// <param name="fieldType">该字段的类型。</param>
        /// <param name="dataType">该字段所返回的数据类型。</param>
        /// <param name="operationType">该字段的操作类型。</param>
        internal OperationField(IDataContext dataContext, FieldType fieldType, Type dataType, OperationType operationType)
            : base(dataContext, fieldType, dataType)
        {
            OperationType = operationType;
        }

        /// <summary>
        /// 获取当前字段的操作类型。
        /// </summary>
        public OperationType OperationType { get; }
    }
}

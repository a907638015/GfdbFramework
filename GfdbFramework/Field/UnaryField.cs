using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.Enum;
using GfdbFramework.Interface;

namespace GfdbFramework.Field
{
    /// <summary>
    /// 一元操作字段类。
    /// </summary>
    public class UnaryField : OperationField
    {
        /// <summary>
        /// 使用指定的字段返回值的数据类型、字段的操作类型以及字段的操作对象初始化一个新的 <see cref="UnaryField"/> 类实例。
        /// </summary>
        /// <param name="dataType">该字段返回值的数据类型。</param>
        /// <param name="operationType">该字段的操作类型。</param>
        /// <param name="operand">该字段的操作对象。</param>
        internal UnaryField(Type dataType, OperationType operationType, BasicField operand)
            : base(FieldType.Unary, dataType, operationType)
        {
            Operand = operand;
        }

        /// <summary>
        /// 获取当前一元字段的操作对象。
        /// </summary>
        public BasicField Operand { get; }

        /// <summary>
        /// 以当前字段为蓝本复制出一个一样的字段信息。
        /// </summary>
        /// <param name="dataContext">数据操作上下文对象。</param>
        /// <param name="isDeepCopy">是否深度复制（深度复制下 <see cref="QuoteField"/> 类型字段也将对 <see cref="QuoteField.UsingDataSource"/> 进行复制）。</param>
        /// <param name="copiedDataSources">已经复制过的数据源集合。</param>
        /// <param name="copiedFields">已经复制过的字段集合。</param>
        /// <param name="startTableAliasIndex">复制字段时若有复制数据源操作时的表别名起始下标。</param>
        /// <returns>复制好的新字段信息。</returns>
        internal override Field Copy(IDataContext dataContext, bool isDeepCopy, Dictionary<DataSource.DataSource, DataSource.DataSource> copiedDataSources, Dictionary<Field, Field> copiedFields, ref int startTableAliasIndex)
        {
            if (!copiedFields.TryGetValue(this, out Field self))
            {
                self = new UnaryField(DataType, OperationType, (BasicField)Operand.Copy(dataContext, isDeepCopy, copiedDataSources, copiedFields, ref startTableAliasIndex)).ModifyAlias(Alias);

                copiedFields[this] = self;
            }

            return self;
        }
    }
}

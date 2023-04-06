using GfdbFramework.Core;
using GfdbFramework.DataSource;
using GfdbFramework.Enum;
using GfdbFramework.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Field
{
    /// <summary>
    /// 一元操作基础字段类。
    /// </summary>
    public class UnaryField : OperationField
    {
        /// <summary>
        /// 使用指定的数据操作上下文、字段返回数据类型、操作类型以及该字段所操作的基础字段初始化一个新的 <see cref="UnaryField"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该字段所使用的数据操作上下文。</param>
        /// <param name="dataType">该字段所返回的数据类型。</param>
        /// <param name="operationType">该字段的操作类型。</param>
        /// <param name="operand">该字段所操作的基础字段。</param>
        internal UnaryField(IDataContext dataContext, Type dataType, OperationType operationType, BasicField operand)
            : base(dataContext, FieldType.Unary, dataType, operationType)
        {
            Operand = operand;
        }

        /// <summary>
        /// 以当前字段为蓝本复制一个新的字段信息。
        /// </summary>
        /// <param name="copiedDataSources">已经复制好的数据源信息。</param>
        /// <param name="copiedFields">已经复制好的字段信息。</param>
        /// <param name="startDataSourceAliasIndex">若复制该字段还需复制数据源时新复制数据源的起始别名下标。</param>
        /// <returns>复制好的新字段。</returns>
        internal override Field Copy(Dictionary<DataSource.DataSource, DataSource.DataSource> copiedDataSources, Dictionary<Field, Field> copiedFields, ref int startDataSourceAliasIndex)
        {
            if (!copiedFields.TryGetValue(this, out Field self))
            {
                self = new UnaryField(DataContext, DataType, OperationType, (BasicField)Operand.Copy(copiedDataSources, copiedFields, ref startDataSourceAliasIndex)).ModifyAlias(Alias);

                copiedFields[this] = self;
            }

            return self;
        }

        /// <summary>
        /// 将当前字段与指定的字段对齐。
        /// </summary>
        /// <param name="field">对齐的目标字段。</param>
        /// <param name="alignedFields">已对齐过的字段。</param>
        /// <returns>对齐后的字段。</returns>
        internal override Field AlignField(Field field, Dictionary<Field, Field> alignedFields)
        {
            if (DataType == field.DataType)
            {
                if (!alignedFields.TryGetValue(this, out Field self))
                {
                    self = new UnaryField(DataContext, DataType, OperationType, Operand).ModifyAlias(((BasicField)field).Alias);

                    alignedFields[this] = self;
                }

                return self;
            }
            else
            {
                throw new Exception($"对齐到另外一个 {Type} 类型的字段时发现两个字段的返回数据类型不一致");
            }
        }

        /// <summary>
        /// 获取当前一元字段的操作对象。
        /// </summary>
        public BasicField Operand { get; }
    }
}

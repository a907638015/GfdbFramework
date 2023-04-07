using GfdbFramework.Core;
using GfdbFramework.Enum;
using GfdbFramework.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Field
{
    /// <summary>
    /// 二元操作基础字段类。
    /// </summary>
    public class BinaryField : OperationField
    {
        /// <summary>
        /// 使用指定的数据操作上下文、字段返回数据类型、操作类型以及左右两侧的操作字段初始化一个新的 <see cref="BinaryField"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该字段所使用的数据操作上下文。</param>
        /// <param name="dataType">该字段返回值的数据类型。</param>
        /// <param name="operationType">该字段的操作类型。</param>
        /// <param name="left">左侧的操作数据字段。</param>
        /// <param name="right">右侧的操作数据字段。</param>
        internal BinaryField(IDataContext dataContext, Type dataType, OperationType operationType, BasicField left, BasicField right)
            : base(dataContext, FieldType.Binary, dataType, operationType)
        {
            Left = left;
            Right = right;
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
                self = new BinaryField(DataContext, DataType, OperationType, (BasicField)Left.Copy(copiedDataSources, copiedFields, ref startDataSourceAliasIndex), (BasicField)Right.Copy(copiedDataSources, copiedFields, ref startDataSourceAliasIndex)).ModifyAlias(Alias);

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
            var result = base.AlignField(field, alignedFields);

            if (result == null)
            {
                if (!alignedFields.TryGetValue(this, out result))
                {
                    result = new BinaryField(DataContext, DataType, OperationType, Left, Right).ModifyAlias(((BasicField)field).Alias);

                    alignedFields[this] = result;
                }
            }

            return result;
        }

        /// <summary>
        /// 获取当前二元字段左侧的操作对象。
        /// </summary>
        public BasicField Left { get; }

        /// <summary>
        /// 获取当前二元字段右侧的操作对象。
        /// </summary>
        public BasicField Right { get; }
    }
}

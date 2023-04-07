using GfdbFramework.Core;
using GfdbFramework.Enum;
using GfdbFramework.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Field
{
    /// <summary>
    /// 三元操作（条件判断）基础字段类。
    /// </summary>
    public class ConditionalField : BasicField
    {
        /// <summary>
        /// 使用指定的数据操作上下文、字段返回数据类型、字段的判定操作对象以及判定为真或假时的返回字段初始化一个新的 <see cref="ConditionalField"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该字段所使用的数据操作上下文。</param>
        /// <param name="dataType">该字段返回值的数据类型。</param>
        /// <param name="test">该字段的判定对象。</param>
        /// <param name="ifTrue">该字段判定对象值为真时的返回值。</param>
        /// <param name="ifFalse">该字段判定对象值为假时的返回值。</param>
        internal ConditionalField(IDataContext dataContext, Type dataType, BasicField test, BasicField ifTrue, BasicField ifFalse)
            : base(dataContext, FieldType.Conditional, dataType)
        {
            Test = test;
            IfTrue = ifTrue;
            IfFalse = ifFalse;
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
                self = new ConditionalField(DataContext, DataType, (BasicField)Test.Copy(copiedDataSources, copiedFields, ref startDataSourceAliasIndex), (BasicField)IfTrue.Copy(copiedDataSources, copiedFields, ref startDataSourceAliasIndex), (BasicField)IfFalse.Copy(copiedDataSources, copiedFields, ref startDataSourceAliasIndex)).ModifyAlias(Alias);

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
                    result = new ConditionalField(DataContext, DataType, Test, IfTrue, IfFalse).ModifyAlias(((BasicField)field).Alias);

                    alignedFields[this] = result;
                }
            }

            return result;
        }

        /// <summary>
        /// 获取当前三元字段的判定对象。
        /// </summary>
        public BasicField Test { get; }

        /// <summary>
        /// 获取当前三元字段判定对象值为真时的返回值。
        /// </summary>
        public BasicField IfTrue { get; }

        /// <summary>
        /// 获取当前三元字段判定对象值为假时的返回值。
        /// </summary>
        public BasicField IfFalse { get; }
    }
}

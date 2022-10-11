using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.Enum;
using GfdbFramework.Interface;

namespace GfdbFramework.Field
{
    /// <summary>
    /// 三元操作（条件判断操作）字段类。
    /// </summary>
    public class ConditionalField : OperationField
    {
        /// <summary>
        /// 使用指定的字段返回值的数据类型、字段的操作类型、字段的判定操作对象、字段判定对象值为真时的返回值以及判定对象值为假时的返回值初始化一个新的 <see cref="ConditionalField"/> 类实例。
        /// </summary>
        /// <param name="dataType">该字段返回值的数据类型。</param>
        /// <param name="operationType">该字段的操作类型。</param>
        /// <param name="test">该字段的判定对象。</param>
        /// <param name="ifTrue">该字段判定对象值为真时的返回值。</param>
        /// <param name="ifFalse">该字段判定对象值为假时的返回值。</param>
        internal ConditionalField(Type dataType, OperationType operationType, BasicField test, BasicField ifTrue, BasicField ifFalse)
            : base(FieldType.Conditional, dataType, operationType)
        {
            Test = test;
            IfTrue = ifTrue;
            IfFalse = ifFalse;
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
                self = new ConditionalField(DataType, OperationType, (BasicField)Test.Copy(dataContext, isDeepCopy, copiedDataSources, copiedFields, ref startTableAliasIndex), (BasicField)IfTrue.Copy(dataContext, isDeepCopy, copiedDataSources, copiedFields, ref startTableAliasIndex), (BasicField)IfFalse.Copy(dataContext, isDeepCopy, copiedDataSources, copiedFields, ref startTableAliasIndex)).ModifyAlias(Alias);

                copiedFields[this] = self;
            }
            
            return self;
        }
    }
}

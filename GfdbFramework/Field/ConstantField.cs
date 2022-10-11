using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.Enum;
using GfdbFramework.Interface;

namespace GfdbFramework.Field
{
    /// <summary>
    /// 常量字段类。
    /// </summary>
    public class ConstantField : BasicField
    {
        /// <summary>
        /// 使用指定的字段返回值的数据类型以及常量值初始化一个新的 <see cref="ConstantField"/> 类实例。
        /// </summary>
        /// <param name="dataType">该字段返回值的数据类型。</param>
        /// <param name="value">该字段的常量值。</param>
        public ConstantField(Type dataType, object value) 
            : base(FieldType.Constant, dataType)
        {
            Value = value;
        }

        /// <summary>
        /// 获取当前常量字段的值。
        /// </summary>
        public object Value { get; }

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
                self = new ConstantField(DataType, Value);

                copiedFields[this] = self;
            }

            return self;
        }
    }
}

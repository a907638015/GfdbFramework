using GfdbFramework.Enum;
using GfdbFramework.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Field
{
    /// <summary>
    /// 对真实数据进行隔离的字段类。
    /// </summary>
    public class IsolateField : BasicField
    {
        /// <summary>
        /// 使用指定的数据操作上下文、字段类型以及内部所需隔离的基础数据字段初始化一个新的 <see cref="IsolateField"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该字段所使用的数据操作上下文。</param>
        /// <param name="type">字段类型。</param>
        /// <param name="field">该字段返回值的数据类型。</param>
        public IsolateField(IDataContext dataContext, FieldType type, BasicField field)
            : base(dataContext, type, field.DataType)
        {
            InnerField = field;
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
                self = new IsolateField(DataContext, Type, (BasicField)InnerField.Copy(copiedDataSources, copiedFields, ref startDataSourceAliasIndex)).ModifyAlias(Alias);

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
                    result = new IsolateField(DataContext, Type, InnerField).ModifyAlias(((BasicField)field).Alias);

                    alignedFields[this] = result;
                }
            }

            return result;
        }

        /// <summary>
        /// 获取当前所需隔离的字段。
        /// </summary>
        public BasicField InnerField { get; }
    }
}

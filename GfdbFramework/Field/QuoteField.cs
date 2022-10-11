using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.DataSource;
using GfdbFramework.Enum;
using GfdbFramework.Interface;

namespace GfdbFramework.Field
{
    /// <summary>
    /// 对某一成员引用的字段类。
    /// </summary>
    public class QuoteField : BasicField
    {
        /// <summary>
        /// 使用指定引用字段以及引用字段所归属的数据源初始化一个新的 <see cref="QuoteField"/> 类实例。
        /// </summary>
        /// <param name="field">该字段所引用的字段信息。</param>
        /// <param name="dataSource">该字段所引用字段归属的数据源信息。</param>
        internal QuoteField(BasicField field, BasicDataSource dataSource)
            : this(field.Type == FieldType.Original ? ((OriginalField)field).FieldName : field.Alias, field.DataType, dataSource)
        {
        }

        /// <summary>
        /// 使用指定引用字段名称、字段返回值的数据类型以及引用字段所归属的数据源初始化一个新的 <see cref="QuoteField"/> 类实例。
        /// </summary>
        /// <param name="fieldName">该字段所引用的字段名称。</param>
        /// <param name="dataType">该字段返回值的数据类型。</param>
        /// <param name="dataSource">该字段所引用字段归属的数据源信息。</param>
        private QuoteField(string fieldName, Type dataType, BasicDataSource dataSource)
            : base(FieldType.Quote, dataType)
        {
            UsingFieldName = fieldName;
            UsingDataSource = dataSource;
        }

        /// <summary>
        /// 获取被引用的字段名称。
        /// </summary>
        public string UsingFieldName { get; }

        /// <summary>
        /// 获取被引用字段所归属的数据源信息。
        /// </summary>
        public BasicDataSource UsingDataSource { get; }

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
                if (isDeepCopy)
                {
                    if (!copiedDataSources.TryGetValue(UsingDataSource, out DataSource.DataSource dataSource))
                    {
                        dataSource = (BasicDataSource)UsingDataSource.Copy(ref startTableAliasIndex);

                        copiedDataSources[UsingDataSource] = dataSource;
                    }

                    self = new QuoteField(UsingFieldName, DataType, (BasicDataSource)dataSource).ModifyAlias(Alias);
                }
                else
                {
                    self = new QuoteField(UsingFieldName, DataType, UsingDataSource).ModifyAlias(Alias);
                }

                copiedFields[this] = self;
            }

            return self;
        }
    }
}

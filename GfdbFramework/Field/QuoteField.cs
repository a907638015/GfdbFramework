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
    /// 对其他数据源字段进行引用的字段类。
    /// </summary>
    public class QuoteField : BasicField
    {
        private readonly BasicDataSource _QuoteDataSource = null;
        private readonly string _QuoteDataSourceAlias = null;

        /// <summary>
        /// 使用一个被引用的字段以及该字段所属的数据源初始化一个新的 <see cref="QuoteField"/> 类实例。
        /// </summary>
        /// <param name="usingField">被引用的字段名称。</param>
        /// <param name="quoteDataSource">被引用字段所属的数据源。</param>
        internal QuoteField(BasicField usingField, BasicDataSource quoteDataSource)
            : this(usingField.DataContext, usingField.DataType, usingField.Type == FieldType.Original ? ((OriginalField)usingField).FieldName : usingField.Alias, quoteDataSource, null)
        {
        }

        /// <summary>
        /// 使用一个被引用的字段以及该字段所属的数据源别名初始化一个新的 <see cref="QuoteField"/> 类实例。
        /// </summary>
        /// <param name="usingField">被引用的字段名称。</param>
        /// <param name="quoteDataSourceAlias">被引用字段所属的数据源别名。</param>
        internal QuoteField(BasicField usingField, string quoteDataSourceAlias)
            : this(usingField.DataContext, usingField.DataType, usingField.Type == FieldType.Original ? ((OriginalField)usingField).FieldName : usingField.Alias, null, quoteDataSourceAlias)
        {
        }

        /// <summary>
        /// 使用指定的数据操作上下文、字段返回数据类型、被引用的字段名、该字段所属的数据源、该字段所属数据源的别名初始化一个新的 <see cref="QuoteField"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该字段所使用的数据操作上下文。</param>
        /// <param name="dataType">该字段所返回的数据类型。</param>
        /// <param name="usingFieldName">被引用的字段名称。</param>
        /// <param name="quoteDataSource">被引用字段所属的数据源。</param>
        /// <param name="quoteDataSourceAlias">被引用字段所属的数据源别名。</param>
        private QuoteField(IDataContext dataContext, Type dataType, string usingFieldName, BasicDataSource quoteDataSource, string quoteDataSourceAlias)
            : base(dataContext, FieldType.Quote, dataType)
        {
            _QuoteDataSource = quoteDataSource;
            _QuoteDataSourceAlias = quoteDataSourceAlias;

            QuoteFieldName = usingFieldName;
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
                if (_QuoteDataSource == null)
                {
                    self = new QuoteField(DataContext, DataType, QuoteFieldName, null, QuoteDataSourceAlias).ModifyAlias(Alias);
                }
                else
                {
                    if (!copiedDataSources.TryGetValue(_QuoteDataSource, out DataSource.DataSource dataSource))
                        dataSource = (BasicDataSource)_QuoteDataSource.Copy(copiedDataSources, copiedFields, ref startDataSourceAliasIndex);

                    self = new QuoteField(DataContext, DataType, QuoteFieldName, (BasicDataSource)dataSource, null).ModifyAlias(Alias);

                    copiedFields[this] = self;
                }
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
                    if (_QuoteDataSource == null)
                        self = new QuoteField(DataContext, DataType, QuoteFieldName, null, QuoteDataSourceAlias).ModifyAlias(((BasicField)field).Alias);
                    else
                        self = new QuoteField(DataContext, DataType, QuoteFieldName, _QuoteDataSource, null).ModifyAlias(((BasicField)field).Alias);

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
        /// 获取被引用的字段名称。
        /// </summary>
        public string QuoteFieldName { get; }

        /// <summary>
        /// 获取被引用字段所归属的数据源别名。
        /// </summary>
        public string QuoteDataSourceAlias
        {
            get
            {
                if (_QuoteDataSource == null)
                    return _QuoteDataSourceAlias;

                return _QuoteDataSource.Alias;
            }
        }
    }
}

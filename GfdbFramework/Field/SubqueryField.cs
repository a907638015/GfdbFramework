using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.Core;
using GfdbFramework.DataSource;
using GfdbFramework.Enum;
using GfdbFramework.Interface;

namespace GfdbFramework.Field
{
    /// <summary>
    /// 子查询字段类。
    /// </summary>
    public class SubqueryField : BasicField
    {
        /// <summary>
        /// 使用指定的字段返回值数据类型以及一个作为子查询的数据源初始化一个新的 <see cref="SubqueryField"/> 类实例。
        /// </summary>
        /// <param name="dataType">该字段返回值的数据类型。</param>
        /// <param name="dataSource">作为子查询的数据源。</param>
        internal SubqueryField(Type dataType, BasicField field, BasicDataSource dataSource)
            : base(FieldType.Subquery, dataType)
        {
            QueryField = field;
            QueryDataSource = dataSource;
        }

        /// <summary>
        /// 获取待查询的字段信息。
        /// </summary>
        public BasicField QueryField { get; }

        /// <summary>
        /// 获取作为子查询的数据源。
        /// </summary>
        public BasicDataSource QueryDataSource { get; }

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
                if (!copiedDataSources.TryGetValue(QueryDataSource, out DataSource.DataSource dataSource))
                {
                    dataSource = QueryDataSource.Copy(ref startTableAliasIndex);

                    copiedDataSources[QueryDataSource] = dataSource;
                }

                BasicDataSource queryDataSource = (BasicDataSource)dataSource;

                var queryField = GetQueryField(queryDataSource.SelectField ?? queryDataSource.RootField);

                if (queryField == null)
                    throw new Exception("未能复制当前子查询字段信息，获取到的待查询字段数据为 null");

                self = new SubqueryField(DataType, queryField, queryDataSource);

                copiedFields[this] = self;
            }

            return self;
        }

        /// <summary>
        /// 校验指定字段是否是与 <see cref="QueryField"/> 是同一字段（此处的同一字段与字面的同一字段意思不一样，此处代表引用字段是否是复制后数据源中的新字段）。
        /// </summary>
        /// <param name="field">待校验的字段信息。</param>
        /// <returns>若相同时返回 true，否则返回 false。</returns>
        private bool CheckIsSame(BasicField field)
        {
            return field.Type == QueryField.Type && ((QueryField.Type == FieldType.Original && ((OriginalField)field).FieldName == ((OriginalField)QueryField).FieldName) || (QueryField.Type != FieldType.Original && field.Alias == QueryField.Alias));
        }

        /// <summary>
        /// 从复制出的新数据源查询字段中获取待查询的字段信息。
        /// </summary>
        /// <param name="selectField">复制好的数据源查询字段。</param>
        /// <returns>若找到待查询的字段信息则返回待查询字段信息，否则返回 null。</returns>
        private BasicField GetQueryField(Field selectField)
        {
            if (selectField.Type == FieldType.Object)
            {
                ObjectField objectField = (ObjectField)selectField;

                if (objectField.Members != null && objectField.Members.TryGetValue(QueryField.Type == FieldType.Original ? ((OriginalField)QueryField).FieldName : QueryField.Alias, out MemberInfo memberInfo) && memberInfo.Field.Type == QueryField.Type && CheckIsSame((BasicField)memberInfo.Field))
                {
                    return (BasicField)memberInfo.Field;
                }
                else
                {
                    if (objectField.ConstructorInfo.Parameters != null && objectField.ConstructorInfo.Parameters.Count > 0)
                    {
                        foreach (var item in objectField.ConstructorInfo.Parameters)
                        {
                            var queryField = GetQueryField(item);

                            if (queryField != null)
                                return queryField;
                        }
                    }
                }
            }
            else if (selectField.Type == FieldType.Collection)
            {
                CollectionField collectionField = (CollectionField)selectField;

                if (collectionField.ConstructorInfo.Parameters != null && collectionField.ConstructorInfo.Parameters.Count > 0)
                {
                    foreach (var item in collectionField.ConstructorInfo.Parameters)
                    {
                        BasicField queryField = GetQueryField(item);

                        if (queryField != null)
                            return queryField;
                    }
                }

                foreach (var item in collectionField)
                {
                    BasicField queryField = GetQueryField(item);

                    if (queryField != null)
                        return queryField;
                }
            }
            else if (CheckIsSame((BasicField)selectField))
            {
                return (BasicField)selectField;
            }

            return null;
        }

        /// <summary>
        /// 将当前字段转换成子查询字段。
        /// </summary>
        /// <param name="dataContext">数据操作上下文对象。</param>
        /// <param name="dataSource">该字段所归属的数据源。</param>
        /// <param name="convertedFields">已转换过的字段信息集合。</param>
        /// <returns>转换后的子查询字段。</returns>
        internal override Field ToSubquery(IDataContext dataContext, BasicDataSource dataSource, Dictionary<Field, Field> convertedFields)
        {
            return this;
        }
    }
}

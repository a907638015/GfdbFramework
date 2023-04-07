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
    /// 子查询字段类。
    /// </summary>
    public class SubqueryField : OperationField
    {
        /// <summary>
        /// 使用指定的数据操作上下文、作为子查询的字段信息以及该字段所属的数据源初始化一个新的 <see cref="SubqueryField"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该字段所使用的数据操作上下文。</param>
        /// <param name="field">作为子查询的字段。</param>
        /// <param name="dataSource">子查询字段所属的数据源。</param>
        internal SubqueryField(IDataContext dataContext, BasicField field, BasicDataSource dataSource)
            : base(dataContext, FieldType.Subquery, field.DataType, OperationType.Subquery)
        {
            SelectField = field;
            BelongDataSource = dataSource;
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
                if (!copiedDataSources.TryGetValue(BelongDataSource, out DataSource.DataSource dataSource))
                    dataSource = BelongDataSource.Copy(copiedDataSources, copiedFields, ref startDataSourceAliasIndex);

                BasicDataSource belongDataSource = (BasicDataSource)dataSource;

                if (!copiedFields.TryGetValue(SelectField, out Field selectField))
                    selectField = FindField(belongDataSource.SelectField ?? belongDataSource.RootField);

                if (selectField == null)
                    throw new Exception("未能复制当前子查询字段信息，获取到的待查询字段数据为 null");

                self = new SubqueryField(DataContext, (BasicField)selectField, belongDataSource).ModifyAlias(Alias);

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
                    result = new SubqueryField(DataContext, SelectField, BelongDataSource).ModifyAlias(((BasicField)field).Alias);

                    alignedFields[this] = result;
                }
            }

            return result;
        }

        /// <summary>
        /// 校验指定字段是否是与 <see cref="SelectField"/> 是同一字段（此处的同一字段与字面的同一字段意思不一样，此处代表引用字段是否是复制后数据源中的新字段）。
        /// </summary>
        /// <param name="field">待校验的字段信息。</param>
        /// <returns>若相同时返回 true，否则返回 false。</returns>
        private bool CheckIsSame(BasicField field)
        {
            return field.Type == SelectField.Type && ((SelectField.Type == FieldType.Original && ((OriginalField)field).FieldName == ((OriginalField)SelectField).FieldName) || (SelectField.Type != FieldType.Original && field.Alias == SelectField.Alias));
        }

        /// <summary>
        /// 从指定字段中获取待查询的字段信息。
        /// </summary>
        /// <param name="field">包含待查询字段的实例字段。</param>
        /// <returns>若找到待查询的字段信息则返回待查询字段信息，否则返回 null。</returns>
        private BasicField FindField(Field field)
        {
            if (field.Type == FieldType.Object)
            {
                ObjectField objectField = (ObjectField)field;

                if (objectField.Members != null && objectField.Members.TryGetValue(SelectField.Type == FieldType.Original ? ((OriginalField)SelectField).FieldName : SelectField.Alias, out MemberInfo memberInfo) && memberInfo.Field is BasicField basicField && CheckIsSame(basicField))
                {
                    return (BasicField)memberInfo.Field;
                }
                else
                {
                    if (objectField.ConstructorInfo.Parameters != null && objectField.ConstructorInfo.Parameters.Count > 0)
                    {
                        foreach (var item in objectField.ConstructorInfo.Parameters)
                        {
                            var selectField = FindField(item);

                            if (selectField != null)
                                return selectField;
                        }
                    }
                }
            }
            else if (field.Type == FieldType.Collection)
            {
                CollectionField collectionField = (CollectionField)field;

                if (collectionField.ConstructorInfo.Parameters != null && collectionField.ConstructorInfo.Parameters.Count > 0)
                {
                    foreach (var item in collectionField.ConstructorInfo.Parameters)
                    {
                        BasicField selectField = FindField(item);

                        if (selectField != null)
                            return selectField;
                    }
                }

                foreach (var item in collectionField)
                {
                    BasicField selectField = FindField(item);

                    if (selectField != null)
                        return selectField;
                }
            }
            else if (CheckIsSame((BasicField)field))
            {
                return (BasicField)field;
            }

            return null;
        }

        /// <summary>
        /// 获取待查询的字段信息。
        /// </summary>
        public BasicField SelectField { get; }

        /// <summary>
        /// 获取待查询字段所归属的数据源。
        /// </summary>
        public BasicDataSource BelongDataSource { get; }
    }
}

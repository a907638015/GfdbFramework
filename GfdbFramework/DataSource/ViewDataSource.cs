using GfdbFramework.Core;
using GfdbFramework.Enum;
using GfdbFramework.Field;
using GfdbFramework.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.DataSource
{
    /// <summary>
    /// 原始数据库视图对应的数据源类。
    /// </summary>
    public class ViewDataSource : OriginalDataSource
    {
        /// <summary>
        /// 使用指定的数据操作上下文、视图根字段、视图创建 Sql 语句、视图名称以及该数据源的别名下标初始化一个新的 <see cref="ViewDataSource"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该数据源所使用的数据操作上下文。</param>
        /// <param name="rootField">数据源根字段。</param>
        /// <param name="createSQL">该数据源对应视图的创建语句。</param>
        /// <param name="name">数据源对应的视图名称。</param>
        /// <param name="aliasIndex">数据源别名下标。</param>
        internal ViewDataSource(IDataContext dataContext, Field.Field rootField, string createSQL, string name, int aliasIndex)
            : base(dataContext, SourceType.View, rootField, name, aliasIndex)
        {
            if (RootField.Type != FieldType.Object)
                throw new ArgumentException($"实例化 {typeof(ViewDataSource).FullName} 数据源类的实例时，根字段必须是 {FieldType.Object} 类型的字段", nameof(rootField));

            CreateSQL = createSQL;
        }

        /// <summary>
        /// 以当前数据源为蓝本复制出一个一样的数据源信息。
        /// </summary>
        /// <param name="copiedDataSources">已经复制过的数据源集合。</param>
        /// <param name="copiedFields">已经复制好的字段信息。</param>
        /// <param name="startAliasIndex">新数据源的起始别名下标。</param>
        /// <returns>复制好的新数据源信息。</returns>
        public override DataSource Copy(Dictionary<DataSource, DataSource> copiedDataSources, Dictionary<Field.Field, Field.Field> copiedFields, ref int startAliasIndex)
        {
            if (!copiedDataSources.TryGetValue(this, out DataSource self))
            {
                Field.Field rootField = RootField.Copy(copiedDataSources, copiedFields, ref startAliasIndex);
                Field.Field selectField = SelectField?.Copy(copiedDataSources, copiedFields, ref startAliasIndex);
                BasicField whereField = (BasicField)Where?.Copy(copiedDataSources, copiedFields, ref startAliasIndex);
                List<SortItem> sortItems = null;
                List<BasicField> groupFields = null;

                if (SortItems != null && SortItems.Count > 0)
                {
                    sortItems = new List<SortItem>();

                    foreach (var item in SortItems)
                    {
                        sortItems.Add(new SortItem((BasicField)item.Field.Copy(copiedDataSources, copiedFields, ref startAliasIndex), item.Type));
                    }
                }

                if (GroupFields != null && GroupFields.Count > 0)
                {
                    groupFields = new List<BasicField>();

                    foreach (var item in GroupFields)
                    {
                        groupFields.Add((BasicField)item.Copy(copiedDataSources, copiedFields, ref startAliasIndex));
                    }
                }

                if (startAliasIndex != AliasIndex)
                {
                    Helper.ResetDataSourceAlias(rootField, startAliasIndex);
                    Helper.ResetDataSourceAlias(selectField, startAliasIndex);
                    Helper.ResetDataSourceAlias(whereField, startAliasIndex);

                    if (sortItems != null)
                    {
                        foreach (var item in sortItems)
                        {
                            Helper.ResetDataSourceAlias(item.Field, startAliasIndex);
                        }
                    }

                    if (groupFields != null)
                    {
                        foreach (var item in groupFields)
                        {
                            Helper.ResetDataSourceAlias(item, startAliasIndex);
                        }
                    }
                }

                self = new ViewDataSource(DataContext, rootField, CreateSQL, Name, startAliasIndex++)
                    .AddLimit(Limit)
                    .AddWhere(whereField)
                    .SetSelectField(selectField)
                    .SetSortItems(sortItems)
                    .SetGroupFields(groupFields)
                    .SetDistinctly(IsDistinctly);

                copiedDataSources[this] = self;
            }

            return self;
        }

        /// <summary>
        /// 将当前数据源对齐到合并数据源中的主数据源。
        /// </summary>
        /// <param name="mainSource">合并数据源中主数据源。</param>
        /// <returns>对齐后的数据源。</returns>
        internal protected override BasicDataSource AlignUnionSource(BasicDataSource mainSource)
        {
            var alignedField = new Dictionary<Field.Field, Field.Field>();

            var returnField = (SelectField ?? RootField).AlignField(mainSource.SelectField ?? mainSource.RootField, alignedField);

            ViewDataSource dataSource = new ViewDataSource(DataContext, SelectField == null ? returnField : RootField, CreateSQL, Name, AliasIndex);

            dataSource.AddLimit(Limit)
                .AddWhere(Where)
                .SetDistinctly(IsDistinctly)
                .SetSortItems(SortItems)
                .SetGroupFields(GroupFields);

            if (SelectField != null)
                dataSource.SetSelectField(returnField);

            return dataSource;
        }

        /// <summary>
        /// 以当前数据源为蓝本复制出一个一样的数据源信息（浅复制，内部所引用的信息不会复制）。
        /// </summary>
        /// <returns>复制好的新数据源信息。</returns>
        internal protected override BasicDataSource ShallowCopy()
        {
            ViewDataSource dataSource = new ViewDataSource(DataContext, RootField, CreateSQL, Name, AliasIndex);

            dataSource.AddLimit(Limit)
                .AddWhere(Where)
                .SetDistinctly(IsDistinctly)
                .SetSortItems(SortItems)
                .SetGroupFields(GroupFields)
                .SetSelectField(SelectField);

            return dataSource;
        }

        /// <summary>
        /// 获取该数据源对应视图的创建 Sql 语句。
        /// </summary>
        public string CreateSQL { get; }
    }
}

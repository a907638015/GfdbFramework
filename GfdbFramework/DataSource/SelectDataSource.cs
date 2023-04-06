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
    /// 查询结果数据源类。
    /// </summary>
    public class SelectDataSource : BasicDataSource
    {
        /// <summary>
        /// 使用指定的数据操作上下文、当前查询的字段、数据源根字段、被查询的数据源以及该数据源的别名下标初始化一个新的 <see cref="SelectDataSource"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该数据源所使用的数据操作上下文。</param>
        /// <param name="selectField">被查询的字段。</param>
        /// <param name="rootField">数据源根字段。</param>
        /// <param name="queryDataSource">被查询的数据源实例。</param>
        /// <param name="aliasIndex">数据源别名下标。</param>
        internal SelectDataSource(IDataContext dataContext, Field.Field selectField, Field.Field rootField, DataSource queryDataSource, int aliasIndex)
            : base(dataContext, SourceType.Select, rootField, aliasIndex)
        {
            SetSelectField(selectField);

            QueryDataSource = queryDataSource;
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

                self = new SelectDataSource(DataContext, SelectField?.Copy(copiedDataSources, copiedFields, ref startAliasIndex), RootField.Copy(copiedDataSources, copiedFields, ref startAliasIndex), QueryDataSource.Copy(copiedDataSources, copiedFields, ref startAliasIndex), startAliasIndex++)
                    .AddLimit(Limit)
                    .AddWhere((BasicField)Where?.Copy(copiedDataSources, copiedFields, ref startAliasIndex))
                    .SetDistinctly(IsDistinctly)
                    .SetSortItems(sortItems)
                    .SetGroupFields(groupFields);

                copiedDataSources[this] = self;
            }

            return self;
        }

        /// <summary>
        /// 以当前数据源为蓝本复制出一个一样的数据源信息（浅复制，内部所引用的信息不会复制）。
        /// </summary>
        /// <returns>复制好的新数据源信息。</returns>
        internal protected override BasicDataSource ShallowCopy()
        {
            SelectDataSource dataSource = new SelectDataSource(DataContext, SelectField, RootField, QueryDataSource, AliasIndex);

            dataSource.AddLimit(Limit)
                .AddWhere(Where)
                .SetDistinctly(IsDistinctly)
                .SetSortItems(SortItems)
                .SetGroupFields(GroupFields);

            return dataSource;
        }

        /// <summary>
        /// 将当前数据源对齐到合并数据源中的主数据源。
        /// </summary>
        /// <param name="mainSource">合并数据源中主数据源。</param>
        /// <returns>对齐后的数据源。</returns>
        internal protected override BasicDataSource AlignUnionSource(BasicDataSource mainSource)
        {
            var alignedField = new Dictionary<Field.Field, Field.Field>();

            var selectField = SelectField.AlignField(mainSource.SelectField ?? mainSource.RootField, alignedField);

            SelectDataSource dataSource = new SelectDataSource(DataContext, selectField, RootField, QueryDataSource, AliasIndex);

            dataSource.AddLimit(Limit)
                .AddWhere(Where)
                .SetDistinctly(IsDistinctly)
                .SetSortItems(SortItems)
                .SetGroupFields(GroupFields);

            return dataSource;
        }

        /// <summary>
        /// 获取待查询的数据源实例。
        /// </summary>
        public DataSource QueryDataSource { get; }
    }
}

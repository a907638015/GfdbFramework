using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.Core;
using GfdbFramework.Enum;
using GfdbFramework.Field;
using GfdbFramework.Interface;

namespace GfdbFramework.DataSource
{
    /// <summary>
    /// 查询结果集数据源。
    /// </summary>
    public class ResultDataSource : BasicDataSource
    {
        /// <summary>
        /// 使用指定的数据操作上下文、数据源类型、数据源根字段、数据源别名下标以及来源数据源初始化一个新的 <see cref="ResultDataSource"/> 类实例。
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="type">该数据源的类型。</param>
        /// <param name="rootField">该数据源的根字段信息。</param>
        /// <param name="aliasIndex">该数据源之后的源别名下标。</param>
        /// <param name="fromDataSource">该数据源的数据来源数据源。</param>
        internal ResultDataSource(IDataContext dataContext, DataSourceType type, Field.Field rootField, int aliasIndex, DataSource fromDataSource)
            : base(dataContext, type, rootField, aliasIndex)
        {
            FromDataSource = fromDataSource;
        }

        /// <summary>
        /// 获取该数据源的来源数据源。
        /// </summary>
        public DataSource FromDataSource { get; }

        /// <summary>
        /// 以当前数据源为蓝本复制出一个一样的数据源信息（浅复制，内部所引用的信息不会复制）。
        /// </summary>
        /// <returns>复制好的新数据源信息。</returns>
        internal override BasicDataSource Copy()
        {
            return new ResultDataSource(DataContext, Type, RootField, AliasIndex, FromDataSource)
                .AddLimit(Limit)
                .AddWhere(Where)
                .SetSortItems(SortItems)
                .SetGroupFields(GroupFields)
                .SetSelectField(SelectField)
                .SetDistinctly(IsDistinctly);
        }

        /// <summary>
        /// 以当前数据源为蓝本复制出一个一样的数据源信息。
        /// </summary>
        /// <param name="startAliasIndex">复制后新数据源的起始表别名下标。</param>
        /// <returns>复制好的新数据源信息。</returns>
        internal override DataSource Copy(ref int startAliasIndex)
        {
            var copiedFields = new Dictionary<Field.Field, Field.Field>();
            var fromDataSource = FromDataSource.Copy(ref startAliasIndex);
            var copiedDataSources = new Dictionary<DataSource, DataSource>() { { FromDataSource, fromDataSource } };
            var rootField = RootField.Copy(DataContext, true, copiedDataSources, copiedFields, ref startAliasIndex);
            var whereField = (BasicField)Where?.Copy(DataContext, true, copiedDataSources, copiedFields, ref startAliasIndex);
            var selectField = SelectField?.Copy(DataContext, true, copiedDataSources, copiedFields, ref startAliasIndex);
            var aliasIndex = startAliasIndex;
            List<SortItem> sortItems = null;
            List<BasicField> groupFields = null;

            if (SortItems != null && SortItems.Count > 0)
            {
                sortItems = new List<SortItem>();

                foreach (var item in SortItems)
                {
                    sortItems.Add(new SortItem((BasicField)item.Field.Copy(DataContext, true, copiedDataSources, copiedFields, ref startAliasIndex), item.Type));
                }
            }

            if (GroupFields != null && GroupFields.Count > 0)
            {
                groupFields = new List<BasicField>();

                foreach (var item in GroupFields)
                {
                    groupFields.Add((BasicField)item.Copy(DataContext, true, copiedDataSources, copiedFields, ref startAliasIndex));
                }
            }

            startAliasIndex++;

            return new ResultDataSource(DataContext, Type, rootField, aliasIndex, fromDataSource)
                .AddLimit(Limit)
                .AddWhere(whereField)
                .SetSortItems(sortItems)
                .SetGroupFields(groupFields)
                .SetSelectField(selectField)
                .SetDistinctly(IsDistinctly);
        }
    }
}

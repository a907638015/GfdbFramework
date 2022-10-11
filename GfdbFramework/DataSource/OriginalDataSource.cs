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
    /// 原始数据库表或视图数据源。
    /// </summary>
    public class OriginalDataSource : BasicDataSource
    {
        /// <summary>
        /// 使用指定的数据操作上下文、数据源类型、数据源根字段、数据源主键成员信息、数据源自增成员信息、数据源别名下标以及数据源对应的表或视图名称初始化一个新的 <see cref="OriginalDataSource"/> 类实例。
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="type">该数据源的类型。</param>
        /// <param name="rootField">该数据源的根字段信息。</param>
        /// <param name="primaryKey">该数据源的主键成员信息。</param>
        /// <param name="autoincrement">该数据源的自增成员信息。</param>
        /// <param name="aliasIndex">该数据源之后的源别名下标。</param>
        /// <param name="name">该数据源所对应的数据库表或视图名称。</param>
        internal OriginalDataSource(IDataContext dataContext, DataSourceType type, ObjectField rootField, MemberInfo primaryKey, MemberInfo autoincrement, int aliasIndex, string name)
            : base(dataContext, type, rootField, aliasIndex)
        {
            Name = name;
            PrimaryKey = primaryKey;
            Autoincrement = autoincrement;
        }

        /// <summary>
        /// 获取当前数据源对应的数据库表或视图名称。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 获取该原始数据源中的主键成员信息。
        /// </summary>
        public MemberInfo PrimaryKey { get; }

        /// <summary>
        /// 获取该原始数据源中的自增成员信息。
        /// </summary>
        public MemberInfo Autoincrement { get; }

        /// <summary>
        /// 以当前数据源为蓝本复制出一个一样的数据源信息（浅复制，内部所引用的信息不会复制）。
        /// </summary>
        /// <returns>复制好的新数据源信息。</returns>
        internal override BasicDataSource Copy()
        {
            return new OriginalDataSource(DataContext, Type, (ObjectField)RootField, PrimaryKey, Autoincrement, AliasIndex, Name)
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
            int aliasIndex = startAliasIndex;

            startAliasIndex++;

            var copiedDataSources = new Dictionary<DataSource, DataSource>();
            var copiedFields = new Dictionary<Field.Field, Field.Field>();
            var rootField = RootField.Copy(DataContext, true, copiedDataSources, copiedFields, ref startAliasIndex);
            var whereField = (BasicField)Where?.Copy(DataContext, true, copiedDataSources, copiedFields, ref startAliasIndex);
            var selectField = SelectField?.Copy(DataContext, true, copiedDataSources, copiedFields, ref startAliasIndex);
            List<SortItem> sortItems = null;
            List<BasicField> groupFields = null;
            MemberInfo primaryKey = null;
            MemberInfo autoincrement = null;

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

            if (PrimaryKey != null)
                primaryKey = new MemberInfo(PrimaryKey.Member, copiedFields[PrimaryKey.Field]);

            if (Autoincrement != null)
                autoincrement = new MemberInfo(Autoincrement.Member, copiedFields[Autoincrement.Field]);

            return new OriginalDataSource(DataContext, Type, (ObjectField)rootField, primaryKey, autoincrement, aliasIndex, Name)
                .AddLimit(Limit)
                .AddWhere(whereField)
                .SetSortItems(sortItems)
                .SetGroupFields(groupFields)
                .SetSelectField(selectField)
                .SetDistinctly(IsDistinctly);
        }
    }
}

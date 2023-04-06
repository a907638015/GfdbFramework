using GfdbFramework.Core;
using GfdbFramework.Enum;
using GfdbFramework.Field;
using GfdbFramework.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GfdbFramework.DataSource
{
    /// <summary>
    /// 原始数据库表对应的数据源类。
    /// </summary>
    public class TableDataSource : OriginalDataSource
    {
        private ReadOnlyList<IndexInfo> _Indices = null;

        /// <summary>
        /// 使用指定的数据操作上下文、表根字段、表主键信息、表自增字段信息、表名称以及该数据源的别名下标初始化一个新的 <see cref="TableDataSource"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该数据源所使用的数据操作上下文。</param>
        /// <param name="rootField">数据源根字段。</param>
        /// <param name="primaryKey">数据源主键字段信息。</param>
        /// <param name="autoincrement">数据源自增字段信息。</param>
        /// <param name="name">数据源对应的表名称。</param>
        /// <param name="aliasIndex">数据源别名下标。</param>
        internal TableDataSource(IDataContext dataContext, Field.Field rootField, MemberInfo primaryKey, MemberInfo autoincrement, string name, int aliasIndex)
            : base(dataContext, SourceType.Table, rootField, name, aliasIndex)
        {
            if (RootField.Type != FieldType.Object)
                throw new ArgumentException($"实例化 {typeof(TableDataSource).FullName} 数据源类的实例时，根字段必须是 {FieldType.Object} 类型的字段", nameof(rootField));

            PrimaryKey = primaryKey;
            Autoincrement = autoincrement;
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
                MemberInfo primaryKey = PrimaryKey == null ? null : new MemberInfo(PrimaryKey.Member, PrimaryKey.Field.Copy(copiedDataSources, copiedFields, ref startAliasIndex));
                MemberInfo autoincrement = Autoincrement == null ? null : new MemberInfo(Autoincrement.Member, Autoincrement.Field.Copy(copiedDataSources, copiedFields, ref startAliasIndex));
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
                    Helper.ResetDataSourceAlias(primaryKey?.Field, startAliasIndex);
                    Helper.ResetDataSourceAlias(autoincrement?.Field, startAliasIndex);

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

                self = new TableDataSource(DataContext, rootField, primaryKey, autoincrement, Name, startAliasIndex++)
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

            TableDataSource dataSource = new TableDataSource(DataContext, SelectField == null ? returnField : RootField, PrimaryKey, Autoincrement, Name, AliasIndex);

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
            TableDataSource dataSource = new TableDataSource(DataContext, RootField, PrimaryKey, Autoincrement, Name, AliasIndex);

            dataSource.AddLimit(Limit)
                .AddWhere(Where)
                .SetDistinctly(IsDistinctly)
                .SetSortItems(SortItems)
                .SetGroupFields(GroupFields)
                .SetSelectField(SelectField);

            return dataSource;
        }

        /// <summary>
        /// 获取该原始数据源中的主键成员信息。
        /// </summary>
        public MemberInfo PrimaryKey { get; }

        /// <summary>
        /// 获取该原始数据源中的自增成员信息。
        /// </summary>
        public MemberInfo Autoincrement { get; }

        /// <summary>
        /// 获取当前原始数据源中的索引集合。
        /// </summary>
        public ReadOnlyList<IndexInfo> Indices
        {
            get
            {
                if (_Indices == null)
                {
                    ObjectField rootField = (ObjectField)RootField;

                    if (rootField.Members != null && rootField.Members.Count > 0)
                    {
                        HashSet<string> names = new HashSet<string>();

                        List<IndexInfo> indices = new List<IndexInfo>();

                        object[] attrs = rootField.DataType.GetCustomAttributes(true);

                        var matchResult = new Regex("[a-zA-Z0-9_]+").Match(Name);
                        var indexPrefix = matchResult == null || matchResult.Groups == null || matchResult.Groups.Count < 1 ? Guid.NewGuid().ToString().Substring(0, 8) : matchResult.Groups[0].Value;

                        if (attrs != null)
                        {
                            foreach (var item in attrs)
                            {
                                if (item is Attribute.IndexAttribute indexAttribute)
                                {
                                    if (indexAttribute.Fields == null || indexAttribute.Fields.Length < 1)
                                        throw new Exception($"在获取 {RootField.DataType.FullName} 类映射到数据库表的索引信息时发现一个字段信息不明确的索引");

                                    List<OriginalField> fields = new List<OriginalField>();

                                    foreach (var fieldName in indexAttribute.Fields)
                                    {
                                        if (rootField.Members.TryGetValue(fieldName, out MemberInfo memberInfo) && memberInfo.Field.Type == FieldType.Original)
                                            fields.Add((OriginalField)memberInfo.Field);
                                        else
                                            throw new Exception($"在获取 {RootField.DataType.FullName} 类映射到数据库表的索引信息时至少有一个索引应用的字段名未能在映射成员中找到");
                                    }

                                    string indexName = indexAttribute.Name;

                                    if (string.IsNullOrWhiteSpace(indexName))
                                    {
                                        int i = 0;

                                        do
                                        {
                                            indexName = string.Format("{0}_AutoIndex_{1}", indexPrefix, i++);
                                        } while (names.Contains(indexName));
                                    }

                                    if (names.Contains(indexName))
                                        throw new Exception($"{indexName} 索引名与另外一个索引重名");

                                    names.Add(indexName);

                                    indices.Add(new IndexInfo(indexName, fields, indexAttribute.Type, indexAttribute.Sort));
                                }
                            }
                        }

                        foreach (var item in rootField.Members)
                        {
                            if (item.Value.Field.Type == FieldType.Original)
                            {
                                OriginalField field = (OriginalField)item.Value.Field;

                                if (field.SimpleIndex != SortType.Not)
                                {
                                    string indexName;

                                    int i = 0;

                                    do
                                    {
                                        indexName = $"{indexPrefix}_SimpleIndex_{i++}";
                                    } while (names.Contains(indexName));

                                    names.Add(indexName);

                                    indices.Add(new IndexInfo(indexName, new ReadOnlyList<OriginalField>(field), IndexType.Normal, field.SimpleIndex));
                                }
                            }
                        }

                        _Indices = indices;
                    }
                    else
                    {
                        _Indices = new ReadOnlyList<IndexInfo>((IEnumerable<IndexInfo>)null);
                    }
                }

                return _Indices;
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using GfdbFramework.Core;
using GfdbFramework.Enum;
using GfdbFramework.Field;
using GfdbFramework.Interface;

namespace GfdbFramework.DataSource
{
    /// <summary>
    /// 基本数据源类。
    /// </summary>
    public abstract class BasicDataSource : DataSource
    {
        private static readonly Type _BOOL_TYPE = typeof(bool);
        private Limit? _Limit = null;
        private BasicField _Where = null;
        private Interface.IReadOnlyList<SortItem> _SortItems = null;
        private Interface.IReadOnlyList<BasicField> _GroupFields = null;
        private Field.Field _SelectField = null;
        private bool _IsDistinctly = false;

        /// <summary>
        /// 使用指定的数据操作上下文、数据源类型、数据源根字段以及数据源别名下标初始化一个新的 <see cref="BasicDataSource"/> 类实例。
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="type">该数据源的类型。</param>
        /// <param name="rootField">该数据源的根字段信息。</param>
        /// <param name="aliasIndex">该数据源之后的源别名下标。</param>
        internal BasicDataSource(IDataContext dataContext, DataSourceType type, Field.Field rootField, int aliasIndex)
            : base(dataContext, type)
        {
            AliasIndex = aliasIndex;
            RootField = rootField;
            Alias = dataContext.SqlFactory.GenerateAlias(aliasIndex, NameType.Table);
        }

        /// <summary>
        /// 获取一个值，该值指示在获取该数据源对应数据时是否应当做去重处理。
        /// </summary>
        public bool IsDistinctly
        {
            get
            {
                return _IsDistinctly;
            }
        }

        /// <summary>
        /// 获取当前数据源的别名。
        /// </summary>
        public string Alias { get; }

        /// <summary>
        /// 获取一个值，该值指示当前数据源所使用的别名下标。
        /// </summary>
        internal int AliasIndex { get; }

        /// <summary>
        /// 获取该数据源的根字段信息。
        /// </summary>
        public Field.Field RootField { get; }

        /// <summary>
        /// 获取一个字段，该字段表示需要被查询的字段信息（若此字段为 null，则应当使用 <see cref="RootField"/> 字段作为查询字段，即查询所有的字段信息）。
        /// </summary>
        public Field.Field SelectField
        {
            get
            {
                return _SelectField;
            }
        }

        /// <summary>
        /// 获取一个结构对象，该对象指定了应当返回数据源中第几行到第几行的成员信息。
        /// </summary>
        public Limit? Limit
        {
            get
            {
                return _Limit;
            }
        }

        /// <summary>
        /// 获取一个字段，该字段指示如何对当前数据源中的成员进行条件筛选。
        /// </summary>
        public BasicField Where
        {
            get
            {
                return _Where;
            }
        }

        /// <summary>
        /// 获取一个集合，该集合指示如何对当前数据源中的成员进行排序。
        /// </summary>
        public Interface.IReadOnlyList<SortItem> SortItems
        {
            get
            {
                return _SortItems;
            }
        }

        /// <summary>
        /// 获取一个集合，该集合指示应当如何对当前数据源中的成员进行分组。
        /// </summary>
        public Interface.IReadOnlyList<BasicField> GroupFields
        {
            get
            {
                return _GroupFields;
            }
        }

        /// <summary>
        /// 添加一个条件限定字段到当前数据源。
        /// </summary>
        /// <param name="where">待添加的条件筛选字段。</param>
        /// <returns>当前数据源。</returns>
        internal BasicDataSource AddWhere(BasicField where)
        {
            if (where != null)
            {
                if (where.DataType.FullName != _BOOL_TYPE.FullName)
                    throw new Exception("无法将非布尔类型的字段添加到条件限定字段中");

                if (_Where != null)
                    _Where = new BinaryField(_BOOL_TYPE, OperationType.AndAlso, _Where, where);
                else
                    _Where = where;
            }

            return this;
        }

        /// <summary>
        /// 对当前数据源添加一个返回行数限定信息。
        /// </summary>
        /// <param name="limit">对返回数据行数进行限定的结构体。</param>
        /// <returns>当前数据源。</returns>
        internal BasicDataSource AddLimit(Limit? limit)
        {
            if (limit != null && limit.HasValue)
            {
                if (_Limit != null && _Limit.HasValue)
                    _Limit = new Limit(_Limit.Value.Start + limit.Value.Start, (limit.Value.Count + limit.Value.Start) > _Limit.Value.Count - limit.Value.Start ? _Limit.Value.Count - limit.Value.Start : limit.Value.Count);
                else
                    _Limit = limit;
            }

            return this;
        }

        /// <summary>
        /// 对当前数据源添多个排序项。
        /// </summary>
        /// <param name="sortItems">待添加的排序项集合。</param>
        /// <returns>当前数据源。</returns>
        internal BasicDataSource AddSortItems(IList<SortItem> sortItems)
        {
            if (sortItems != null && sortItems.Count > 0)
            {
                var sortItemList = new List<SortItem>();

                if (_SortItems != null && _SortItems.Count > 0)
                    sortItemList.AddRange(_SortItems);

                sortItemList.AddRange(sortItems);

                _SortItems = new Realize.ReadOnlyList<SortItem>(sortItemList);
            }

            return this;
        }

        /// <summary>
        /// 为当前数据源设置指定的排序项。
        /// </summary>
        /// <param name="sortItems">新的排序项目枚举器。</param>
        /// <returns>当前数据源。</returns>
        internal BasicDataSource SetSortItems(IEnumerable<SortItem> sortItems)
        {
            if (sortItems != null)
                _SortItems = new Realize.ReadOnlyList<SortItem>(sortItems);

            return this;
        }

        /// <summary>
        /// 为当前数据源设置指定的分组字段。
        /// </summary>
        /// <param name="groupFields">新的分组字段枚举器。</param>
        /// <returns>当前数据源。</returns>
        internal BasicDataSource SetGroupFields(IEnumerable<BasicField> groupFields)
        {
            if (groupFields != null)
                _GroupFields = new Realize.ReadOnlyList<BasicField>(groupFields);

            return this;
        }

        /// <summary>
        /// 为当前数据源设置指定的查询字段信息。
        /// </summary>
        /// <param name="selectField">待查询的字段信息。</param>
        /// <returns>当前数据源。</returns>
        internal BasicDataSource SetSelectField(Field.Field selectField)
        {
            _SelectField = selectField;

            return this;
        }

        /// <summary>
        /// 指定当前数据源是否应当做去重处理。
        /// </summary>
        /// <param name="isDistinctly">指示是否应当在查询该数据源数据时做去重处理。</param>
        /// <returns>当前数据源。</returns>
        internal BasicDataSource SetDistinctly(bool isDistinctly)
        {
            _IsDistinctly = isDistinctly;

            return this;
        }

        /// <summary>
        /// 以当前数据源为蓝本复制出一个一样的数据源信息（浅复制，内部所引用的信息不会复制）。
        /// </summary>
        /// <returns>复制好的新数据源信息。</returns>
        internal abstract BasicDataSource Copy();
    }
}

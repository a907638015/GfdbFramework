using GfdbFramework.Attribute;
using GfdbFramework.Core;
using GfdbFramework.Enum;
using GfdbFramework.Interface;
using System;
using System.Collections.Generic;

namespace GfdbFramework.Field
{
    /// <summary>
    /// 原始数据库表或视图字段类。
    /// </summary>
    public class OriginalField : BasicField
    {
        private string _DataSourceAlias = null;

        /// <summary>
        /// 使用指定的数据操作上下文、字段返回数据类型、字段标记信息以及该字段返回的数据类型初始化一个新的 <see cref="OriginalField"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该字段所使用的数据操作上下文。</param>
        /// <param name="dataType">该字段所返回的数据类型。</param>
        /// <param name="fieldAttribute">该字段的标记信息。</param>
        /// <param name="alias">该字段所使用的别名。</param>
        internal OriginalField(IDataContext dataContext, Type dataType, FieldAttribute fieldAttribute, string alias)
            : base(dataContext, Enum.FieldType.Original, dataType)
        {
            ModifyAlias(alias);

            IsPrimaryKey = fieldAttribute.IsPrimaryKey;
            IsUnique = fieldAttribute.IsUnique;
            IsAutoincrement = fieldAttribute.IsAutoincrement;
            IsInsertForDefault = fieldAttribute.IsInsertForDefault;
            IsUpdateForDefault = fieldAttribute.IsUpdateForDefault;
            IsNullable = fieldAttribute.IsNullable == NullableMode.Nullable;
            SimpleIndex = fieldAttribute.SimpleIndex;
            IncrementSpeed = fieldAttribute.IncrementSpeed;
            IncrementSeed = fieldAttribute.IncrementSeed;
            DefaultValue = fieldAttribute.DefaultValue;
            FieldType = fieldAttribute.DataType;
            FieldName = fieldAttribute.Name;
            Explain = fieldAttribute.Explain;
            CheckConstraint = fieldAttribute.CheckConstraint;
        }

        /// <summary>
        /// 使用指定的原始字段初始化一个新的 <see cref="OriginalField"/> 类实例。
        /// </summary>
        /// <param name="originalField">用于参考的原始字段信息。</param>
        private OriginalField(OriginalField originalField)
            : base(originalField.DataContext, Enum.FieldType.Original, originalField.DataType)
        {
            ModifyAlias(originalField.Alias);

            IsPrimaryKey = originalField.IsPrimaryKey;
            IsUnique = originalField.IsUnique;
            IsAutoincrement = originalField.IsAutoincrement;
            IsInsertForDefault = originalField.IsInsertForDefault;
            IsUpdateForDefault = originalField.IsUpdateForDefault;
            IncrementSpeed = originalField.IncrementSpeed;
            IncrementSeed = originalField.IncrementSeed;
            DefaultValue = originalField.DefaultValue;
            FieldType = originalField.FieldType;
            FieldName = originalField.FieldName;
            IsNullable = originalField.IsNullable;
            SimpleIndex = originalField.SimpleIndex;
            Explain = originalField.Explain;
            CheckConstraint = originalField.CheckConstraint;
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
            if (!copiedFields.TryGetValue(this, out Field copiedField))
            {
                copiedField = new OriginalField(this).ModifyDataSourceAlias(DataSourceAlias).ModifyAlias(Alias);

                copiedFields[this] = copiedField;
            }

            return copiedField;
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
                    result = new OriginalField(this).ModifyDataSourceAlias(DataSourceAlias).ModifyAlias(((BasicField)field).Alias);

                    alignedFields[this] = result;
                }
            }

            return result;
        }

        /// <summary>
        /// 修改当前字段归属的数据源名称。
        /// </summary>
        /// <param name="dataSourceAlias">该字段归属数据源新的别名。</param>
        /// <returns>当前字段。</returns>
        internal virtual OriginalField ModifyDataSourceAlias(string dataSourceAlias)
        {
            if (dataSourceAlias != _DataSourceAlias)
                _DataSourceAlias = dataSourceAlias;

            return this;
        }

        /// <summary>
        /// 获取当前字段所归属数据源的别名。
        /// </summary>
        public string DataSourceAlias
        {
            get
            {
                return _DataSourceAlias;
            }
        }

        /// <summary>
        /// 获取一个值，该值指示当前字段是否是主键字段。
        /// </summary>
        public bool IsPrimaryKey { get; }

        /// <summary>
        /// 获取一个值，该值指示当前字段是否是唯一字段。
        /// </summary>
        public bool IsUnique { get; }

        /// <summary>
        /// 获取一个值，该值指示当前字段是否是自增字段。
        /// </summary>
        public bool IsAutoincrement { get; }

        /// <summary>
        /// 获取一个值，该值指示在执行自动插入操作时当该字段所映射实体成员的值为默认值时是否将该默认值插入到数据库。
        /// </summary>
        public bool IsInsertForDefault { get; }

        /// <summary>
        /// 获取一个值，该值指示在执行自动更新操作时当该字段所映射实体成员的值为默认值时是否将该默认值更新到数据库。
        /// </summary>
        public bool IsUpdateForDefault { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示当前字段是否允许为空值。
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// 获取或设置该字段的索引排序方式（默认为 <see cref="SortType.Not"/>，且简单索引的索引类型都为 <see cref="IndexType.Normal"/>）。
        /// </summary>
        public SortType SimpleIndex { get; set; }

        /// <summary>
        /// 获取或设置该字段的校验约束。
        /// </summary>
        public string CheckConstraint { get; set; }

        /// <summary>
        /// 获取一个值，该值表示当前字段为自增字段时的每次递增量（仅在 <see cref="IsAutoincrement"/> 属性为 true 时有效）。
        /// </summary>
        public int IncrementSpeed { get; }

        /// <summary>
        /// 获取一个值，该值表示当前字段为自增字段时的起始值（仅在 <see cref="IsAutoincrement"/> 属性为 true 时有效）。
        /// </summary>
        public int IncrementSeed { get; }

        /// <summary>
        /// 获取一个值，该值指示当前字段的默认值。
        /// </summary>
        public object DefaultValue { get; }

        /// <summary>
        /// 获取该字段的原始数据类型（即数据库中的数据类型）。
        /// </summary>
        public string FieldType { get; }

        /// <summary>
        /// 获取当前数据字段的名称。
        /// </summary>
        public string FieldName { get; }

        /// <summary>
        /// 获取当前数据字段的描述说明文字。
        /// </summary>
        public string Explain { get; }
    }
}

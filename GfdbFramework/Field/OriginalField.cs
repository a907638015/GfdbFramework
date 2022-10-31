using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.Attribute;
using GfdbFramework.DataSource;
using GfdbFramework.Interface;

namespace GfdbFramework.Field
{
    /// <summary>
    /// 原始数据字段（数据库表或视图字段）类型。
    /// </summary>
    public sealed class OriginalField : BasicField
    {
        /// <summary>
        /// 使用指定的字段数据类型、字段标记信息以及该字段所使用的别名初始化一个新的 <see cref="OriginalField"/> 类实例。
        /// </summary>
        /// <param name="dataType">该字段的数据类型。</param>
        /// <param name="fieldAttribute">该字段的标记信息。</param>
        /// <param name="alias">该字段所使用的别名。</param>
        internal OriginalField(Type dataType, FieldAttribute fieldAttribute, string alias)
            : base(Enum.FieldType.Original, dataType)
        {
            ModifyAlias(alias);

            IsPrimaryKey = fieldAttribute.IsPrimaryKey;
            IsUnique = fieldAttribute.IsUnique;
            IsAutoincrement = fieldAttribute.IsAutoincrement;
            IsInsertForDefault = fieldAttribute.IsInsertForDefault;
            IsUpdateForDefault = fieldAttribute.IsUpdateForDefault;
            IsNullable = fieldAttribute.IsNullable;
            SimpleIndex = fieldAttribute.SimpleIndex == 0 ? null : (Enum.SortType?)fieldAttribute.SimpleIndex;
            IncrementSpeed = fieldAttribute.IncrementSpeed;
            IncrementSeed = fieldAttribute.IncrementSeed;
            DefaultValue = fieldAttribute.DefaultValue;
            FieldType = fieldAttribute.DataType;
            FieldName = fieldAttribute.Name;
            Explain = fieldAttribute.Explain;
            CheckConstraint = fieldAttribute.CheckConstraint;
        }

        /// <summary>
        /// 使用指定的原始数据字段复制一个新的 <see cref="OriginalField"/> 类实例。
        /// </summary>
        /// <param name="originalField">需要复制的原始数据字段。</param>
        private OriginalField(OriginalField originalField)
            : base(Enum.FieldType.Original, originalField.DataType)
        {
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
        /// 获取或设置该字段的索引排序方式（默认为 null，且简单索引的索引类型都为 <see cref="Enum.IndexType.Normal"/>）。
        /// </summary>
        public Enum.SortType? SimpleIndex { get; set; }

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
                self = new OriginalField(this).ModifyAlias(Alias);

                copiedFields[this] = self;
            }

            return self;
        }
    }
}

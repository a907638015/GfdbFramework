using GfdbFramework.Core;
using GfdbFramework.Enum;
using GfdbFramework.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GfdbFramework.Field
{
    /// <summary>
    /// 成员调用字段类。
    /// </summary>
    public class MemberField : BasicField
    {
        /// <summary>
        /// 使用指定的数据操作上下文、字段返回数据类型、成员字段实例以及所调用的成员信息初始化一个新的 <see cref="MemberField"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该字段所使用的数据操作上下文。</param>
        /// <param name="dataType">该字段返回值的数据类型。</param>
        /// <param name="objectField">成员实例的字段对象。</param>
        /// <param name="memberInfo">被调用的成员信息。</param>
        internal MemberField(IDataContext dataContext, Type dataType, Field objectField, System.Reflection.MemberInfo memberInfo)
            : base(dataContext, FieldType.Member, dataType)
        {
            MemberInfo = memberInfo;
            ObjectField = objectField;
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
                self = new MemberField(DataContext, DataType, ObjectField?.Copy(copiedDataSources, copiedFields, ref startDataSourceAliasIndex), MemberInfo).ModifyAlias(Alias);

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
            if (DataType == field.DataType)
            {
                if (!alignedFields.TryGetValue(this, out Field self))
                {
                    self = new MemberField(DataContext, DataType, ObjectField, MemberInfo).ModifyAlias(((BasicField)field).Alias);

                    alignedFields[this] = self;
                }

                return self;
            }
            else
            {
                throw new Exception($"对齐到另外一个 {Type} 类型的字段时发现两个字段的返回数据类型不一致");
            }
        }

        /// <summary>
        /// 获取一个对象字段，该字段代表包含被调用成员的对象（为 null 时代表静态成员调用）。
        /// </summary>
        public Field ObjectField { get; }

        /// <summary>
        /// 获取被调用的成员信息。
        /// </summary>
        public System.Reflection.MemberInfo MemberInfo { get; }
    }
}

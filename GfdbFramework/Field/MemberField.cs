using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using GfdbFramework.Enum;
using GfdbFramework.Interface;

namespace GfdbFramework.Field
{
    /// <summary>
    /// 对某字段成员进行调用的字段类。
    /// </summary>
    public class MemberField : BasicField
    {
        /// <summary>
        /// 使用指定的字段返回值的数据类型、包含引用成员的对象字段以及所引用的成员信息初始化一个新的 <see cref="BinaryField"/> 类实例。
        /// </summary>
        /// <param name="dataType">该字段返回值的数据类型。</param>
        /// <param name="objectField">包含调用成员的对象字段。</param>
        /// <param name="memberInfo">被调用的成员信息。</param>
        internal MemberField(Type dataType, Field objectField, MemberInfo memberInfo)
            : base(FieldType.Member, dataType)
        {
            MemberInfo = memberInfo;
            ObjectField = objectField;
        }

        /// <summary>
        /// 获取一个对象字段，该字段代表包含被调用成员的对象（为 null 时代表静态成员调用）。
        /// </summary>
        public Field ObjectField { get; }

        /// <summary>
        /// 获取被调用的成员信息。
        /// </summary>
        public MemberInfo MemberInfo { get; }

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
                self = new MemberField(DataType, ObjectField?.Copy(dataContext, isDeepCopy, copiedDataSources, copiedFields, ref startTableAliasIndex), MemberInfo);

                copiedFields[this] = self;
            }

            return self;
        }
    }
}

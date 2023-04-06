using GfdbFramework.Core;
using GfdbFramework.DataSource;
using GfdbFramework.Enum;
using GfdbFramework.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GfdbFramework.Field
{
    /// <summary>
    /// .Net 框架类字段。
    /// </summary>
    public class ObjectField : Field
    {
        /// <summary>
        /// 使用指定的数据操作上下文、字段返回数据类型、对象字段对应类的构造信息、字段所包含的成员集合以及一个标识是否需要初始化该对象中所有成员信息的一个值初始化一个新的 <see cref="ObjectField"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该字段所使用的数据操作上下文。</param>
        /// <param name="dataType">该字段所返回的数据类型。</param>
        /// <param name="constructorInfo">该字段对应 .Net 对象类的构造信息。</param>
        /// <param name="members">该字段所有的成员集合。</param>
        /// <param name="whetherNecessaryInit">标识在实例化 .NET 类后是否需要初始化所有成员值的一个标识。</param>
        internal ObjectField(IDataContext dataContext, Type dataType, ConstructorInfo constructorInfo, ReadOnlyDictionary<string, MemberInfo> members, bool whetherNecessaryInit)
            : base(dataContext, FieldType.Object, dataType)
        {
            Members = members;
            ConstructorInfo = constructorInfo;
            WhetherNecessaryInit = whetherNecessaryInit;
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
                Dictionary<string, MemberInfo> members = new Dictionary<string, MemberInfo>();
                List<Field> constructorParameters = null;

                if (ConstructorInfo.Parameters != null && ConstructorInfo.Parameters.Count > 0)
                {
                    constructorParameters = new List<Field>();

                    foreach (var item in ConstructorInfo.Parameters)
                    {
                        if (!copiedFields.TryGetValue(item, out Field copiedField))
                            copiedField = item.Copy(copiedDataSources, copiedFields, ref startDataSourceAliasIndex);

                        constructorParameters.Add(copiedField);
                    }
                }

                foreach (var item in Members)
                {
                    if (!copiedFields.TryGetValue(item.Value.Field, out Field copiedField))
                        copiedField = item.Value.Field.Copy(copiedDataSources, copiedFields, ref startDataSourceAliasIndex);

                    members.Add(item.Key, new MemberInfo(item.Value.Member, copiedField));
                }

                self = new ObjectField(DataContext, DataType, new ConstructorInfo(ConstructorInfo.Constructor, constructorParameters), members, WhetherNecessaryInit);

                copiedFields[this] = self;
            }

            return self;
        }

        /// <summary>
        /// 将当前字段转换成子查询字段。
        /// </summary>
        /// <param name="dataSource">该字段所归属的数据源信息。</param>
        /// <param name="convertedFields">已转换的字段信息。</param>
        /// <returns>转换后的新字段。</returns>
        internal override Field ToSubqueryField(BasicDataSource dataSource, Dictionary<Field, Field> convertedFields)
        {
            if (!convertedFields.TryGetValue(this, out Field self))
            {
                Dictionary<string, MemberInfo> members = new Dictionary<string, MemberInfo>();
                List<Field> constructorParameters = null;

                if (ConstructorInfo.Parameters != null && ConstructorInfo.Parameters.Count > 0)
                {
                    constructorParameters = new List<Field>();

                    foreach (var item in ConstructorInfo.Parameters)
                    {
                        if (!convertedFields.TryGetValue(item, out Field convertedField))
                            convertedField = item.ToSubqueryField(dataSource, convertedFields);

                        constructorParameters.Add(convertedField);
                    }
                }

                foreach (var item in Members)
                {
                    if (!convertedFields.TryGetValue(item.Value.Field, out Field convertedField))
                        convertedField = item.Value.Field.ToSubqueryField(dataSource, convertedFields);

                    members.Add(item.Key, new MemberInfo(item.Value.Member, convertedField));
                }

                self = new ObjectField(DataContext, DataType, new ConstructorInfo(ConstructorInfo.Constructor, constructorParameters), members, WhetherNecessaryInit);

                convertedFields[this] = self;
            }

            return self;
        }

        /// <summary>
        /// 将当前字段转换成引用字段。
        /// </summary>
        /// <param name="dataSource">该字段所归属的数据源。</param>
        /// <param name="convertedFields">已转换的字段信息。</param>
        /// <returns>转换后的新字段。</returns>
        internal override Field ToQuoteField(BasicDataSource dataSource, Dictionary<Field, Field> convertedFields)
        {
            if (!convertedFields.TryGetValue(this, out Field self))
            {
                Dictionary<string, MemberInfo> members = new Dictionary<string, MemberInfo>();
                List<Field> constructorParameters = null;

                if (ConstructorInfo.Parameters != null && ConstructorInfo.Parameters.Count > 0)
                {
                    constructorParameters = new List<Field>();

                    foreach (var item in ConstructorInfo.Parameters)
                    {
                        if (!convertedFields.TryGetValue(item, out Field convertedField))
                            convertedField = item.ToQuoteField(dataSource, convertedFields);

                        constructorParameters.Add(convertedField);
                    }
                }

                foreach (var item in Members)
                {
                    if (!convertedFields.TryGetValue(item.Value.Field, out Field convertedField))
                        convertedField = item.Value.Field.ToQuoteField(dataSource, convertedFields);

                    members.Add(item.Key, new MemberInfo(item.Value.Member, convertedField));
                }

                self = new ObjectField(DataContext, DataType, new ConstructorInfo(ConstructorInfo.Constructor, constructorParameters), members, WhetherNecessaryInit);

                convertedFields[this] = self;
            }

            return self;
        }

        /// <summary>
        /// 将当前字段转换成引用字段。
        /// </summary>
        /// <param name="sourceAlias">该字段所归属的数据源别名。</param>
        /// <param name="convertedFields">已转换的字段信息。</param>
        /// <returns>转换后的新字段。</returns>
        internal override Field ToQuoteField(string sourceAlias, Dictionary<Field, Field> convertedFields)
        {
            if (!convertedFields.TryGetValue(this, out Field self))
            {
                Dictionary<string, MemberInfo> members = new Dictionary<string, MemberInfo>();
                List<Field> constructorParameters = null;

                if (ConstructorInfo.Parameters != null && ConstructorInfo.Parameters.Count > 0)
                {
                    constructorParameters = new List<Field>();

                    foreach (var item in ConstructorInfo.Parameters)
                    {
                        constructorParameters.Add(item.ToQuoteField(sourceAlias, convertedFields));
                    }
                }

                foreach (var item in Members)
                {
                    members.Add(item.Key, new MemberInfo(item.Value.Member, item.Value.Field.ToQuoteField(sourceAlias, convertedFields)));
                }

                self = new ObjectField(DataContext, DataType, new ConstructorInfo(ConstructorInfo.Constructor, constructorParameters), members, WhetherNecessaryInit);

                convertedFields[this] = self;
            }

            return self;
        }

        /// <summary>
        /// 将当前字段转换成查询后隔离的新别名字段。
        /// </summary>
        /// <param name="convertedFields">已转换的字段信息。</param>
        /// <returns>转换后的新字段。</returns>
        internal override Field ToNewAliasField(Dictionary<Field, Field> convertedFields)
        {
            if (!convertedFields.TryGetValue(this, out Field self))
            {
                Dictionary<string, MemberInfo> members = new Dictionary<string, MemberInfo>();
                List<Field> constructorParameters = null;

                if (ConstructorInfo.Parameters != null && ConstructorInfo.Parameters.Count > 0)
                {
                    constructorParameters = new List<Field>();

                    foreach (var item in ConstructorInfo.Parameters)
                    {
                        if (!convertedFields.TryGetValue(item, out Field convertedField))
                            convertedField = item.ToNewAliasField(convertedFields);

                        constructorParameters.Add(convertedField);
                    }
                }

                foreach (var item in Members)
                {
                    if (!convertedFields.TryGetValue(item.Value.Field, out Field convertedField))
                        convertedField = item.Value.Field.ToNewAliasField(convertedFields);

                    members.Add(item.Key, new MemberInfo(item.Value.Member, convertedField));
                }

                self = new ObjectField(DataContext, DataType, new ConstructorInfo(ConstructorInfo.Constructor, constructorParameters), members, WhetherNecessaryInit);

                convertedFields[this] = self;
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
            if (field.Type == FieldType.Object)
            {
                if (DataType == field.DataType)
                {
                    if (!alignedFields.TryGetValue(this, out Field self))
                    {
                        ObjectField objectField = (ObjectField)field;

                        if (Members?.Count != objectField.Members?.Count || ConstructorInfo.Parameters?.Count != objectField.ConstructorInfo.Parameters?.Count)
                            throw new Exception($"对齐到另外一个 {Type} 类型的字段时发现目标字段成员数量或构造参数数量不一致");

                        Dictionary<string, MemberInfo> members = new Dictionary<string, MemberInfo>();
                        List<Field> constructorParameters = null;

                        if (ConstructorInfo.Parameters != null && ConstructorInfo.Parameters.Count > 0)
                        {
                            constructorParameters = new List<Field>();

                            for (int i = 0; i < ConstructorInfo.Parameters.Count; i++)
                            {
                                constructorParameters.Add(ConstructorInfo.Parameters[i].AlignField(objectField.ConstructorInfo.Parameters[i], alignedFields));
                            }
                        }

                        if (objectField.Members != null && objectField.Members.Count > 0)
                        {
                            foreach (var item in objectField.Members)
                            {
                                if (Members.TryGetValue(item.Key, out MemberInfo memberInfo))
                                    members.Add(item.Key, new MemberInfo(item.Value.Member, memberInfo.Field.AlignField(item.Value.Field, alignedFields)));
                                else
                                    throw new Exception($"对齐到另外一个 {Type} 类型的字段时发现字段不包含目标字段中名为 {item.Key} 的成员");
                            }
                        }

                        self = new ObjectField(DataContext, DataType, new ConstructorInfo(ConstructorInfo.Constructor, constructorParameters), members, WhetherNecessaryInit);

                        alignedFields[this] = self;
                    }

                    return self;
                }
                else
                {
                    throw new Exception($"对齐到另外一个 {Type} 类型的字段时发现两个字段的返回数据类型不一致");
                }
            }
            else
            {
                throw new Exception($"未能将 {Type} 类型的字段与 {field.Type} 类型的字段对齐");
            }
        }

        /// <summary>
        /// 重新设置当前字段的别名。
        /// </summary>
        /// <param name="startAliasIndex">字段别名的起始下标。</param>
        internal override Field ResetAlias(ref int startAliasIndex)
        {
            if (ConstructorInfo != null && ConstructorInfo.Parameters != null && ConstructorInfo.Parameters.Count > 0)
            {
                foreach (var item in ConstructorInfo.Parameters)
                {
                    item.ResetAlias(ref startAliasIndex);
                }
            }

            if (Members != null && Members.Count > 0)
            {
                foreach (var item in Members)
                {
                    item.Value.Field.ResetAlias(ref startAliasIndex);
                }
            }

            return this;
        }

        /// <summary>
        /// 获取该对象字段的调试显示结果。
        /// </summary>
        /// <param name="parameterContext">获取显示结果所使用的参数上下文对象。</param>
        /// <returns>获取到的显示结果。</returns>
        internal override string GetDebugResult(IParameterContext parameterContext)
        {
            StringBuilder result = new StringBuilder("{");

            foreach (var item in Members)
            {
                if (result.Length > 1)
                    result.Append(",");

                result.Append(item.Value.Field.GetDebugResult(parameterContext));
            }

            result.Append("}");

            return result.ToString();
        }

        /// <summary>
        /// 获取一个值，该值指示在实例化该字段所对应的 .Net 框架类后是否需要初始化 <see cref="Members"/> 属性中所有的成员值。
        /// </summary>
        public bool WhetherNecessaryInit { get; }

        /// <summary>
        /// 获取该对象字段对应 .NET 框架类的构造函数信息。
        /// </summary>
        public ConstructorInfo ConstructorInfo { get; }

        /// <summary>
        /// 获取该对象字段中所有的成员信息。
        /// </summary>
        public ReadOnlyDictionary<string, MemberInfo> Members { get; }

        /// <summary>
        /// 获取当前对象字段中指定名称所对应字段的信息。
        /// </summary>
        /// <param name="memberName">待获取的成员名称。</param>
        /// <returns>指定名称对应的字段信息。</returns>
        public Field this[string memberName]
        {
            get
            {
                return Members[memberName].Field;
            }
        }
    }
}

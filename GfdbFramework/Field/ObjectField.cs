using System;
using System.Collections.Generic;
using GfdbFramework.Core;
using GfdbFramework.DataSource;
using GfdbFramework.Enum;
using GfdbFramework.Interface;

namespace GfdbFramework.Field
{
    /// <summary>
    /// .Net 对象字段类。
    /// </summary>
    public class ObjectField : Field
    {
        private readonly Interface.IReadOnlyDictionary<string, MemberInfo> _Members = null;
        private readonly ConstructorInfo _ConstructorInfo = null;

        /// <summary>
        /// 使用指定的字段类型、对象字段对应类的构造信息以及字段所包含的成员集合初始化一个新的 <see cref="ObjectField"/> 类实例。
        /// </summary>
        /// <param name="dataType">该字段返回值的数据类型。</param>
        /// <param name="constructorInfo">该字段对应 .Net 对象类的构造信息。</param>
        /// <param name="members">该字段所有的成员集合。</param>
        internal ObjectField(Type dataType, ConstructorInfo constructorInfo, IDictionary<string, MemberInfo> members)
            : this(dataType, constructorInfo, members, false)
        {
        }

        /// <summary>
        /// 使用指定的字段类型、对象字段对应类的构造信息、字段所包含的成员集合以及一个标识是否需要初始化该对象中所有成员信息的一个值初始化一个新的 <see cref="ObjectField"/> 类实例。
        /// </summary>
        /// <param name="dataType">该字段返回值的数据类型。</param>
        /// <param name="constructorInfo">该字段对应 .Net 对象类的构造信息。</param>
        /// <param name="members">该字段所有的成员集合。</param>
        /// <param name="neededInitMembers">标识在实例化 .NET 类后是否需要初始化所有成员值的一个标识。</param>
        internal ObjectField(Type dataType, ConstructorInfo constructorInfo, IDictionary<string, MemberInfo> members, bool neededInitMembers)
            : base(FieldType.Object, dataType)
        {
            _Members = new Realize.ReadOnlyDictionary<string, MemberInfo>(members);
            _ConstructorInfo = constructorInfo;
            IsNeededInitMembers = neededInitMembers;
        }

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
                Dictionary<string, MemberInfo> members = new Dictionary<string, MemberInfo>();
                List<Field> constructorParameters = null;

                if (_ConstructorInfo.Parameters != null && _ConstructorInfo.Parameters.Count > 0)
                {
                    constructorParameters = new List<Field>();

                    foreach (var item in _ConstructorInfo.Parameters)
                    {
                        if (!copiedFields.TryGetValue(item, out Field copiedField))
                            copiedField = item.Copy(dataContext, isDeepCopy, copiedDataSources, copiedFields, ref startTableAliasIndex);

                        constructorParameters.Add(copiedField);
                    }
                }

                foreach (var item in _Members)
                {
                    if (!copiedFields.TryGetValue(item.Value.Field, out Field copiedField))
                        copiedField = item.Value.Field.Copy(dataContext, isDeepCopy, copiedDataSources, copiedFields, ref startTableAliasIndex);

                    members.Add(item.Key, new MemberInfo(item.Value.Member, copiedField));
                }

                self = new ObjectField(DataType, new ConstructorInfo(_ConstructorInfo.Constructor, constructorParameters), members);
            }

            copiedFields[this] = self;

            return self;
        }

        /// <summary>
        /// 将当前字段转换成子查询字段。
        /// </summary>
        /// <param name="dataContext">数据操作上下文对象。</param>
        /// <param name="dataSource">该字段所归属的数据源。</param>
        /// <param name="convertedFields">已转换过的字段信息集合。</param>
        /// <returns>转换后的子查询字段。</returns>
        internal override Field ToSubquery(IDataContext dataContext, BasicDataSource dataSource, Dictionary<Field, Field> convertedFields)
        {
            if (!convertedFields.TryGetValue(this, out Field self))
            {
                Dictionary<string, MemberInfo> members = new Dictionary<string, MemberInfo>();
                List<Field> constructorParameters = null;

                if (_ConstructorInfo.Parameters != null && _ConstructorInfo.Parameters.Count > 0)
                {
                    constructorParameters = new List<Field>();

                    foreach (var item in _ConstructorInfo.Parameters)
                    {
                        if (!convertedFields.TryGetValue(item, out Field convertedField))
                            convertedField = item.ToSubquery(dataContext, dataSource, convertedFields);

                        constructorParameters.Add(convertedField);
                    }
                }

                foreach (var item in _Members)
                {
                    if (!convertedFields.TryGetValue(item.Value.Field, out Field copiedField))
                        copiedField = item.Value.Field.ToSubquery(dataContext, dataSource, convertedFields);

                    members.Add(item.Key, new MemberInfo(item.Value.Member, copiedField));
                }

                self = new ObjectField(DataType, new ConstructorInfo(_ConstructorInfo.Constructor, constructorParameters), members);
            }

            convertedFields.Add(this, self);

            return self;
        }

        /// <summary>
        /// 获取该对象字段对应 .NET 框架类的构造函数信息。
        /// </summary>
        public ConstructorInfo ConstructorInfo
        {
            get
            {
                return _ConstructorInfo;
            }
        }

        /// <summary>
        /// 获取该对象字段中所有的成员信息。
        /// </summary>
        public Interface.IReadOnlyDictionary<string, MemberInfo> Members
        {
            get
            {
                return _Members;
            }
        }

        /// <summary>
        /// 获取当前对象字段中指定名称的成员所对应的框架字段信息。
        /// </summary>
        /// <param name="memberName">待获取的成员名称。</param>
        /// <returns>指定名称对应的框架字段信息。</returns>
        public Field this[string memberName]
        {
            get
            {
                return _Members[memberName].Field;
            }
        }

        /// <summary>
        /// 获取一个值，该值指示在实例化该字段所对应的 .NET 框架类后是否需要初始化 <see cref="Members"/> 属性中所有的成员值。
        /// </summary>
        public bool IsNeededInitMembers { get; }
    }
}

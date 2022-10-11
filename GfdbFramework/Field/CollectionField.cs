using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using GfdbFramework.Core;
using GfdbFramework.DataSource;
using GfdbFramework.Enum;
using GfdbFramework.Interface;

namespace GfdbFramework.Field
{
    /// <summary>
    /// .Net 集合或数组字段类。
    /// </summary>
    public class CollectionField : Field, Interface.IReadOnlyList<Field>
    {
        private readonly Interface.IReadOnlyList<Field> _Members = null;

        /// <summary>
        /// 使用指定的字段类型、字段对应 .NET 框架类的构造信息、新增成员的方法信息以及该字段所包含的成员集合初始化一个新的 <see cref="CollectionField"/> 类实例。
        /// </summary>
        /// <param name="dataType">该字段返回值的数据类型。</param>
        /// <param name="constructorInfo">该对象字段对应 .NET 框架类的构造函数信息。</param>
        /// <param name="addMethodInfo">该对象字段对应 .NET 框架类的新增成员函数信息。</param>
        /// <param name="members">该字段所包含的成员集合。</param>
        internal CollectionField(Type dataType, Core.ConstructorInfo constructorInfo, MethodInfo addMethodInfo, IEnumerable<Field> members)
            : base(FieldType.Collection, dataType)
        {
            ConstructorInfo = constructorInfo;
            AddMethodInfo = addMethodInfo;
            _Members = new Realize.ReadOnlyList<Field>(members);
        }

        /// <summary>
        /// 使用指定的字段类型、字段对应 .NET 框架类的构造信息以及该字段所包含的成员集合初始化一个新的 <see cref="CollectionField"/> 类实例。
        /// </summary>
        /// <param name="dataType">该字段返回值的数据类型。</param>
        /// <param name="constructorInfo">该对象字段对应 .NET 框架类的构造函数信息。</param>
        /// <param name="members">该字段所包含的成员集合。</param>
        internal CollectionField(Type dataType, Core.ConstructorInfo constructorInfo, IEnumerable<Field> members)
            : this(dataType, constructorInfo, null, members)
        {
        }

        /// <summary>
        /// 使用指定的字段类型以及该字段对应 .NET 框架类的构造信息初始化一个新的 <see cref="CollectionField"/> 类实例。
        /// </summary>
        /// <param name="dataType">该字段返回值的数据类型。</param>
        /// <param name="constructorInfo">该对象字段对应 .NET 框架类的构造函数信息。</param>
        internal CollectionField(Type dataType, Core.ConstructorInfo constructorInfo)
            : this(dataType, constructorInfo, null, null)
        {
        }

        /// <summary>
        /// 获取该集合字段中所有成员的个数。
        /// </summary>
        public int Count
        {
            get
            {
                return _Members == null ? 0 : _Members.Count;
            }
        }

        /// <summary>
        /// 获取集合字段指定索引处的成员信息。
        /// </summary>
        /// <param name="index">待获取成员信息所在的下标。</param>
        /// <returns>指定索引处的成员信息。</returns>
        public Field this[int index]
        {
            get
            {
                return _Members[index];
            }
        }

        /// <summary>
        /// 获取该对象字段对应 .NET 框架类的构造函数信息。
        /// </summary>
        public Core.ConstructorInfo ConstructorInfo { get; }

        /// <summary>
        /// 获取该对象字段对应 .NET 框架类的新增成员函数信息。
        /// </summary>
        public MethodInfo AddMethodInfo { get; }

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
                List<Field> members = null;
                List<Field> parameters = null;

                if (_Members != null && _Members.Count > 0)
                {
                    members = new List<Field>();

                    foreach (var item in _Members)
                    {
                        if (!copiedFields.TryGetValue(item, out Field copiedField))
                            copiedField = item.Copy(dataContext, isDeepCopy, copiedDataSources, copiedFields, ref startTableAliasIndex);

                        members.Add(copiedField);
                    }
                }

                if (ConstructorInfo.Parameters != null && ConstructorInfo.Parameters.Count > 0)
                {
                    parameters = new List<Field>();

                    foreach (var item in ConstructorInfo.Parameters)
                    {
                        if (!copiedFields.TryGetValue(item, out Field copiedField))
                            copiedField = item.Copy(dataContext, isDeepCopy, copiedDataSources, copiedFields, ref startTableAliasIndex);

                        parameters.Add(copiedField);
                    }
                }

                self = new CollectionField(DataType, new Core.ConstructorInfo(ConstructorInfo.Constructor, parameters), AddMethodInfo, members);

                copiedFields.Add(this, self);
            }

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
                List<Field> members = null;
                List<Field> parameters = null;

                if (_Members != null && _Members.Count > 0)
                {
                    members = new List<Field>();

                    foreach (var item in _Members)
                    {
                        if (!convertedFields.TryGetValue(item, out Field convertField))
                            convertField = item.ToSubquery(dataContext, dataSource, convertedFields);

                        members.Add(convertField);
                    }
                }

                if (ConstructorInfo.Parameters != null && ConstructorInfo.Parameters.Count > 0)
                {
                    parameters = new List<Field>();

                    foreach (var item in ConstructorInfo.Parameters)
                    {
                        if (!convertedFields.TryGetValue(item, out Field convertField))
                            convertField = item.ToSubquery(dataContext, dataSource, convertedFields);

                        parameters.Add(convertField);
                    }
                }

                self = new CollectionField(DataType, new Core.ConstructorInfo(ConstructorInfo.Constructor, parameters), AddMethodInfo, members);

                convertedFields[this] = self;
            }

            return self;
        }

        /// <summary>
        /// 获取一个用于枚举当前集合字段中所有成员的枚举器。
        /// </summary>
        /// <returns>用于枚举当前集合字段中所有成员的枚举器。</returns>
        IEnumerator<Field> IEnumerable<Field>.GetEnumerator()
        {
            return new Enumerator<Field>(_Members?.GetEnumerator());
        }

        /// <summary>
        /// 获取一个用于枚举当前集合字段中所有成员的枚举器。
        /// </summary>
        /// <returns>用于枚举当前集合字段中所有成员的枚举器。</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator<Field>(_Members?.GetEnumerator());
        }
    }
}

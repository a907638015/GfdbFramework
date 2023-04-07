using GfdbFramework.Core;
using GfdbFramework.DataSource;
using GfdbFramework.Enum;
using GfdbFramework.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Field
{
    /// <summary>
    /// .Net 集合或数组字段类。
    /// </summary>
    public class CollectionField : Field, IEnumerable<Field>
    {
        private readonly ReadOnlyList<Field> _Fields = null;

        /// <summary>
        /// 使用指定的数据操作上下文、字段返回数据类型、对象字段对应类的构造信息、新增成员的方法信息以及该字段所包含的成员集合初始化一个新的 <see cref="CollectionField"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该字段所使用的数据操作上下文。</param>
        /// <param name="dataType">该字段返回值的数据类型。</param>
        /// <param name="constructorInfo">该对象字段对应 .NET 框架类的构造函数信息。</param>
        /// <param name="addMethodInfo">该对象字段对应 .NET 框架类的新增成员函数信息。</param>
        /// <param name="fields">该字段所包含的成员集合。</param>
        internal CollectionField(IDataContext dataContext, Type dataType, ConstructorInfo constructorInfo, System.Reflection.MethodInfo addMethodInfo, IEnumerable<Field> fields)
            : base(dataContext, FieldType.Collection, dataType)
        {
            _Fields = new ReadOnlyList<Field>(fields);
            AddMethodInfo = addMethodInfo;
            ConstructorInfo = constructorInfo;
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
                List<Field> fields = null;
                List<Field> parameters = null;

                if (_Fields != null && _Fields.Count > 0)
                {
                    fields = new List<Field>();

                    foreach (var item in _Fields)
                    {
                        fields.Add(item.Copy(copiedDataSources, copiedFields, ref startDataSourceAliasIndex));
                    }
                }

                if (ConstructorInfo.Parameters != null && ConstructorInfo.Parameters.Count > 0)
                {
                    parameters = new List<Field>();

                    foreach (var item in ConstructorInfo.Parameters)
                    {
                        parameters.Add(item.Copy(copiedDataSources, copiedFields, ref startDataSourceAliasIndex));
                    }
                }

                self = new CollectionField(DataContext, DataType, new ConstructorInfo(ConstructorInfo.Constructor, parameters), AddMethodInfo, fields);

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
                List<Field> fields = null;
                List<Field> parameters = null;

                if (_Fields != null && _Fields.Count > 0)
                {
                    fields = new List<Field>();

                    foreach (var item in _Fields)
                    {
                        fields.Add(item.ToSubqueryField(dataSource, convertedFields));
                    }
                }

                if (ConstructorInfo.Parameters != null && ConstructorInfo.Parameters.Count > 0)
                {
                    parameters = new List<Field>();

                    foreach (var item in ConstructorInfo.Parameters)
                    {
                        parameters.Add(item.ToSubqueryField(dataSource, convertedFields));
                    }
                }

                self = new CollectionField(DataContext, DataType, new ConstructorInfo(ConstructorInfo.Constructor, parameters), AddMethodInfo, fields);

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
                List<Field> fields = null;
                List<Field> parameters = null;

                if (_Fields != null && _Fields.Count > 0)
                {
                    fields = new List<Field>();

                    foreach (var item in _Fields)
                    {
                        fields.Add(item.ToQuoteField(dataSource, convertedFields));
                    }
                }

                if (ConstructorInfo.Parameters != null && ConstructorInfo.Parameters.Count > 0)
                {
                    parameters = new List<Field>();

                    foreach (var item in ConstructorInfo.Parameters)
                    {
                        parameters.Add(item.ToQuoteField(dataSource, convertedFields));
                    }
                }

                self = new CollectionField(DataContext, DataType, new ConstructorInfo(ConstructorInfo.Constructor, parameters), AddMethodInfo, fields);

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
                List<Field> fields = null;
                List<Field> parameters = null;

                if (_Fields != null && _Fields.Count > 0)
                {
                    fields = new List<Field>();

                    foreach (var item in _Fields)
                    {
                        fields.Add(item.ToQuoteField(sourceAlias, convertedFields));
                    }
                }

                if (ConstructorInfo.Parameters != null && ConstructorInfo.Parameters.Count > 0)
                {
                    parameters = new List<Field>();

                    foreach (var item in ConstructorInfo.Parameters)
                    {
                        parameters.Add(item.ToQuoteField(sourceAlias, convertedFields));
                    }
                }

                self = new CollectionField(DataContext, DataType, new ConstructorInfo(ConstructorInfo.Constructor, parameters), AddMethodInfo, fields);

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
                List<Field> fields = null;
                List<Field> parameters = null;

                if (_Fields != null && _Fields.Count > 0)
                {
                    fields = new List<Field>();

                    foreach (var item in _Fields)
                    {
                        fields.Add(item.ToNewAliasField(convertedFields));
                    }
                }

                if (ConstructorInfo.Parameters != null && ConstructorInfo.Parameters.Count > 0)
                {
                    parameters = new List<Field>();

                    foreach (var item in ConstructorInfo.Parameters)
                    {
                        parameters.Add(item.ToNewAliasField(convertedFields));
                    }
                }

                self = new CollectionField(DataContext, DataType, new ConstructorInfo(ConstructorInfo.Constructor, parameters), AddMethodInfo, fields);

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
            if (field.Type == FieldType.Collection)
            {
                if (DataType == field.DataType)
                {
                    if (!alignedFields.TryGetValue(this, out Field self))
                    {
                        CollectionField collectionField = (CollectionField)field;

                        if (Count != collectionField.Count || ConstructorInfo.Parameters?.Count != collectionField.ConstructorInfo.Parameters?.Count)
                            throw new Exception($"对齐到另外一个 {Type} 类型的字段时发现目标字段的数组(集合)长度或构造参数数量不一致");

                        List<Field> fields = null;
                        List<Field> constructorParameters = null;

                        if (ConstructorInfo.Parameters != null && ConstructorInfo.Parameters.Count > 0)
                        {
                            constructorParameters = new List<Field>();

                            for (int i = 0; i < ConstructorInfo.Parameters.Count; i++)
                            {
                                constructorParameters.Add(ConstructorInfo.Parameters[i].AlignField(collectionField.ConstructorInfo.Parameters[i], alignedFields));
                            }
                        }

                        if (collectionField.Count > 0)
                        {
                            for (int i = 0; i < collectionField.Count; i++)
                            {
                                fields.Add(this[i].AlignField(collectionField[i], alignedFields));
                            }
                        }

                        self = new CollectionField(DataContext, DataType, new ConstructorInfo(ConstructorInfo.Constructor, constructorParameters), AddMethodInfo, fields);

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

            foreach (var item in this)
            {
                item.ResetAlias(ref startAliasIndex);
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
            StringBuilder result = new StringBuilder("[");

            foreach (var item in _Fields)
            {
                if (result.Length > 1)
                    result.Append(",");

                result.Append(item.GetDebugResult(parameterContext));
            }

            result.Append("]");

            return result.ToString();
        }

        /// <summary>
        /// 获取一个用于用于枚举所包含成员的枚举器对象。
        /// </summary>
        /// <returns>用于枚举所包含成员的枚举器对象。</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Field>)_Fields).GetEnumerator();
        }

        /// <summary>
        /// 获取一个用于用于枚举所包含成员的枚举器对象。
        /// </summary>
        /// <returns>用于枚举所包含成员的枚举器对象。</returns>
        IEnumerator<Field> IEnumerable<Field>.GetEnumerator()
        {
            return ((IEnumerable<Field>)_Fields).GetEnumerator();
        }

        /// <summary>
        /// 获取该字段指定索引处的子字段信息。
        /// </summary>
        /// <param name="index">待获取子字段位置的索引值。</param>
        /// <returns>指定索引处的字段。</returns>
        public Field this[int index]
        {
            get
            {
                return _Fields[index];
            }
        }

        /// <summary>
        /// 获取该对象字段对应 .NET 框架类的构造函数信息。
        /// </summary>
        public ConstructorInfo ConstructorInfo { get; }

        /// <summary>
        /// 获取一个值，若当前字段为集合字段时可用于添加成员信息。
        /// </summary>
        internal System.Reflection.MethodInfo AddMethodInfo { get; }

        /// <summary>
        /// 获取该字段中所有成员的个数。
        /// </summary>
        public int Count
        {
            get
            {
                return _Fields.Count;
            }
        }
    }
}

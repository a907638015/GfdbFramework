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
    /// 方法调用字段类。
    /// </summary>
    public class MethodField : BasicField
    {
        /// <summary>
        /// 使用指定的数据操作上下文、调用实例、调用参数集合以及所调用的方法信息初始化一个新的 <see cref="MethodField"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该字段所使用的数据操作上下文。</param>
        /// <param name="objectField">调用实例的字段对象。</param>
        /// <param name="parameters">调用方法所需的参数字段集合。</param>
        /// <param name="methodInfo">被调用的方法信息。</param>
        internal MethodField(IDataContext dataContext, Field objectField, ReadOnlyList<Field> parameters, MethodInfo methodInfo)
            : base(dataContext, FieldType.Method, methodInfo.ReturnType)
        {
            MethodInfo = methodInfo;
            ObjectField = objectField;
            Parameters = parameters;
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
                Field objectField = ObjectField?.Copy(copiedDataSources, copiedFields, ref startDataSourceAliasIndex);

                List<Field> parameters = null;

                if (Parameters != null && Parameters.Count > 0)
                {
                    parameters = new List<Field>();

                    foreach (var item in Parameters)
                    {
                        parameters.Add(item.Copy(copiedDataSources, copiedFields, ref startDataSourceAliasIndex));
                    }
                }

                self = new MethodField(DataContext, objectField, parameters, MethodInfo).ModifyAlias(Alias);

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
                    self = new MethodField(DataContext, ObjectField, Parameters, MethodInfo).ModifyAlias(((BasicField)field).Alias); ;

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
        /// 获取当前字段所调用的方法信息。
        /// </summary>
        public MethodInfo MethodInfo { get; }

        /// <summary>
        /// 获取一个对象字段，该字段表示调用方法的实例对象字段（为 null 时代表为静态方法调用）。
        /// </summary>
        public Field ObjectField { get; }

        /// <summary>
        /// 获取调用方法所需的参数集合。
        /// </summary>
        public ReadOnlyList<Field> Parameters { get; }
    }
}

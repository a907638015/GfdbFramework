using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using GfdbFramework.Enum;
using GfdbFramework.Interface;

namespace GfdbFramework.Field
{
    /// <summary>
    /// 对某方法进行调用的字段类。
    /// </summary>
    public class MethodField : BasicField
    {
        /// <summary>
        /// 使用指定一个调用方法的实例对象字段、被调用的方法信息以及调用该方法所需的参数集合初始化一个新的 <see cref="MethodField"/> 类实例。
        /// </summary>
        /// <param name="objectField">调用当前方法的实例对象字段信息。</param>
        /// <param name="methodInfo">被调用的方法信息。</param>
        /// <param name="parameters">调用该方法所需的参数集合。</param>
        internal MethodField(Field objectField, MethodInfo methodInfo, IEnumerable<Field> parameters)
            : base(FieldType.Method, methodInfo.ReturnType)
        {
            MethodInfo = methodInfo;
            ObjectField = objectField;
            Parameters = new Realize.ReadOnlyList<Field>(parameters);
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
        /// 获取调用该方法所需的参数集合。
        /// </summary>
        public Interface.IReadOnlyList<Field> Parameters { get; }

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
                List<Field> parameters = null;

                if (Parameters != null && Parameters.Count > 0)
                {
                    parameters = new List<Field>();

                    foreach (var item in Parameters)
                    {
                        parameters.Add(item.Copy(dataContext, isDeepCopy, copiedDataSources, copiedFields, ref startTableAliasIndex));
                    }
                }

                self = new MethodField(ObjectField?.Copy(dataContext, isDeepCopy, copiedDataSources, copiedFields, ref startTableAliasIndex), MethodInfo, parameters);

                copiedFields[this] = self;
            }

            return self;
        }
    }
}

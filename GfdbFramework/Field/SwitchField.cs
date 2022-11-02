using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.Core;
using GfdbFramework.Interface;

namespace GfdbFramework.Field
{
    /// <summary>
    /// Switch 分支语句字段。
    /// </summary>
    public class SwitchField : BasicField
    {
        /// <summary>
        /// 使用指定的字段返回值的数据类型、判断字段、分支集合以及默认的分支主体初始化一个新的 <see cref="SwitchField"/> 类实例。
        /// </summary>
        /// <param name="dataType">该字段返回值的数据类型。</param>
        /// <param name="switchValue">该字段的判断值字段。</param>
        /// <param name="cases">除去默认分支外的所有分支信息。</param>
        /// <param name="defaultBody">该字段的默认分支信息。</param>
        internal SwitchField(Type dataType, BasicField switchValue, Interface.IReadOnlyList<SwitchCase> cases, BasicField defaultBody)
            : base(Enum.FieldType.Switch, dataType)
        {
            SwitchValue = switchValue;
            Cases = cases;
            DefaultBody = defaultBody;
        }

        /// <summary>
        /// 获取该判断分支语句中的判断值字段。
        /// </summary>
        public BasicField SwitchValue { get; }

        /// <summary>
        /// 获取该判断分支语句中所有的分支信息。
        /// </summary>
        public Interface.IReadOnlyList<SwitchCase> Cases { get; }

        /// <summary>
        /// 获取该判断分支语句中的默认主体字段。
        /// </summary>
        public BasicField DefaultBody { get; }

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
                BasicField switchValue = (BasicField)SwitchValue.Copy(dataContext, isDeepCopy, copiedDataSources, copiedFields, ref startTableAliasIndex);

                List<SwitchCase> switchCases = null;

                if (Cases != null && Cases.Count > 0)
                {
                    switchCases = new List<SwitchCase>();

                    foreach (var item in Cases)
                    {
                        List<ConstantField> testValues = new List<ConstantField>();

                        foreach (var testValueItem in item.TestValues)
                        {
                            testValues.Add((ConstantField)testValueItem.Copy(dataContext, isDeepCopy, copiedDataSources, copiedFields, ref startTableAliasIndex));
                        }

                        switchCases.Add(new SwitchCase((BasicField)item.Body.Copy(dataContext, isDeepCopy, copiedDataSources, copiedFields, ref startTableAliasIndex), (Realize.ReadOnlyList<ConstantField>)testValues));
                    }
                }

                BasicField defaultBody = (BasicField)DefaultBody?.Copy(dataContext, isDeepCopy, copiedDataSources, copiedFields, ref startTableAliasIndex);

                self = new SwitchField(DataType, switchValue, (Realize.ReadOnlyList<SwitchCase>)switchCases, defaultBody).ModifyAlias(Alias);

                copiedFields[this] = self;
            }

            return self;
        }
    }
}

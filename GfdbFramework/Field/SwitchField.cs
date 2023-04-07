using GfdbFramework.Core;
using GfdbFramework.Enum;
using GfdbFramework.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Field
{
    /// <summary>
    /// Switch 分支语句字段类。
    /// </summary>
    public class SwitchField : BasicField
    {
        /// <summary>
        /// 使用指定的数据操作上下文、字段返回数据类型、判断字段、分支集合以及默认的分支主体初始化一个新的 <see cref="SwitchField"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该字段所使用的数据操作上下文。</param>
        /// <param name="dataType">该字段返回值的数据类型。</param>
        /// <param name="switchValue">该字段的判断值字段。</param>
        /// <param name="cases">除去默认分支外的所有分支信息。</param>
        /// <param name="defaultBody">该字段的默认分支返回值字段。</param>
        internal SwitchField(IDataContext dataContext, Type dataType, BasicField switchValue, ReadOnlyList<SwitchCase> cases, BasicField defaultBody)
            : base(dataContext, FieldType.Switch, dataType)
        {
            SwitchValue = switchValue;
            Cases = cases;
            DefaultBody = defaultBody;
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
                BasicField switchValue = (BasicField)SwitchValue.Copy(copiedDataSources, copiedFields, ref startDataSourceAliasIndex);

                List<SwitchCase> switchCases = null;

                if (Cases != null && Cases.Count > 0)
                {
                    switchCases = new List<SwitchCase>();

                    foreach (var item in Cases)
                    {
                        List<ConstantField> testValues = new List<ConstantField>();

                        foreach (var testValueItem in item.TestValues)
                        {
                            testValues.Add((ConstantField)testValueItem.Copy(copiedDataSources, copiedFields, ref startDataSourceAliasIndex));
                        }

                        switchCases.Add(new SwitchCase((BasicField)item.Body.Copy(copiedDataSources, copiedFields, ref startDataSourceAliasIndex), testValues));
                    }
                }

                BasicField defaultBody = (BasicField)DefaultBody?.Copy(copiedDataSources, copiedFields, ref startDataSourceAliasIndex);

                self = new SwitchField(DataContext, DataType, switchValue, switchCases, defaultBody).ModifyAlias(Alias);

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
            var result = base.AlignField(field, alignedFields);

            if (result == null)
            {
                if (!alignedFields.TryGetValue(this, out result))
                {
                    result = new SwitchField(DataContext, DataType, SwitchValue, Cases, DefaultBody).ModifyAlias(((BasicField)field).Alias);

                    alignedFields[this] = result;
                }
            }

            return result;
        }

        /// <summary>
        /// 获取该判断分支语句中的判断值字段。
        /// </summary>
        public BasicField SwitchValue { get; }

        /// <summary>
        /// 获取该判断分支语句中所有的分支信息。
        /// </summary>
        public ReadOnlyList<SwitchCase> Cases { get; }

        /// <summary>
        /// 获取该判断分支语句中的默认主体字段。
        /// </summary>
        public BasicField DefaultBody { get; }
    }
}

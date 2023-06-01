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
    /// 所有数据字段的基础抽象类。
    /// </summary>
    [DebuggerDisplay("{GetDebugResult(DataContext.CreateParameterContext(false))}")]
    public abstract class Field
    {
        /// <summary>
        /// 使用指定的数据操作上下文、字段类型以及该字段返回的数据类型初始化一个新的 <see cref="Field"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该字段所使用的数据操作上下文。</param>
        /// <param name="fieldType">该字段的类型。</param>
        /// <param name="dataType">该字段所返回的数据类型。</param>
        internal Field(IDataContext dataContext, FieldType fieldType, Type dataType)
        {
            DataContext = dataContext;
            Type = fieldType;
            DataType = dataType;
        }

        /// <summary>
        /// 以当前字段为蓝本复制一个新的字段信息。
        /// </summary>
        /// <param name="copiedDataSources">已经复制好的数据源信息。</param>
        /// <param name="copiedFields">已经复制好的字段信息。</param>
        /// <param name="startDataSourceAliasIndex">若复制该字段还需复制数据源时新复制数据源的起始别名下标。</param>
        /// <returns>复制好的新字段。</returns>
        internal abstract Field Copy(Dictionary<DataSource.DataSource, DataSource.DataSource> copiedDataSources, Dictionary<Field, Field> copiedFields, ref int startDataSourceAliasIndex);

        /// <summary>
        /// 将当前字段转换成子查询字段。
        /// </summary>
        /// <param name="dataSource">该字段所归属的数据源信息。</param>
        /// <param name="convertedFields">已转换的字段信息。</param>
        /// <returns>转换后的新字段。</returns>
        internal abstract Field ToSubqueryField(BasicDataSource dataSource, Dictionary<Field, Field> convertedFields);

        /// <summary>
        /// 获取该对象字段的调试显示结果。
        /// </summary>
        /// <param name="parameterContext">获取显示结果所使用的参数上下文对象。</param>
        /// <returns>获取到的显示结果。</returns>
        internal abstract string GetDebugResult(IParameterContext parameterContext);

        /// <summary>
        /// 将当前字段转换成引用字段。
        /// </summary>
        /// <param name="dataSource">该字段所归属的数据源。</param>
        /// <param name="convertedFields">已转换的字段信息。</param>
        /// <returns>转换后的新字段。</returns>
        internal abstract Field ToQuoteField(BasicDataSource dataSource, Dictionary<Field, Field> convertedFields);

        /// <summary>
        /// 将当前字段转换成查询后隔离的新别名字段。
        /// </summary>
        /// <param name="convertedFields">已转换的字段信息。</param>
        /// <returns>转换后的新字段。</returns>
        internal abstract Field ToNewAliasField(Dictionary<Field, Field> convertedFields);

        /// <summary>
        /// 将当前字段转换成引用字段。
        /// </summary>
        /// <param name="sourceAlias">该字段所归属的数据源别名。</param>
        /// <param name="convertedFields">已转换的字段信息。</param>
        /// <param name="needCopyAalias">转成引用字段后是否需要复制别名。</param>
        /// <returns>转换后的新字段。</returns>
        internal abstract Field ToQuoteField(string sourceAlias, Dictionary<Field, Field> convertedFields, bool needCopyAalias = false);

        /// <summary>
        /// 将当前字段与指定的字段对齐。
        /// </summary>
        /// <param name="field">对齐的目标字段。</param>
        /// <param name="alignedFields">已对齐过的字段。</param>
        /// <returns>对齐后的字段。</returns>
        internal abstract Field AlignField(Field field, Dictionary<Field, Field> alignedFields);

        /// <summary>
        /// 重新设置当前字段的别名。
        /// </summary>
        /// <param name="startAliasIndex">字段别名的起始下标。</param>
        internal abstract Field ResetAlias(ref int startAliasIndex);

        /// <summary>
        /// 获取当前字段所使用的数据操作上下文。
        /// </summary>
        public IDataContext DataContext { get; }

        /// <summary>
        /// 获取当前字段的类型。
        /// </summary>
        public FieldType Type { get; }

        /// <summary>
        /// 获取当前字段的返回数据类型。
        /// </summary>
        public Type DataType { get; }
    }
}

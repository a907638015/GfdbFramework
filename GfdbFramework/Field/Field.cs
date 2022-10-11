using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.DataSource;
using GfdbFramework.Enum;
using GfdbFramework.Interface;

namespace GfdbFramework.Field
{
    /// <summary>
    /// 所有数据字段的基类。
    /// </summary>
    public abstract class Field
    {
        /// <summary>
        /// 使用指定的字段类型以及字段返回值的数据类型初始化一个新的 <see cref="Field"/> 类实例。
        /// </summary>
        /// <param name="type">该字段的类型。</param>
        /// <param name="dataType">该字段返回值的数据类型。</param>
        internal Field(FieldType type, Type dataType)
        {
            Type = type;
            DataType = dataType;
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
        internal abstract Field Copy(IDataContext dataContext, bool isDeepCopy, Dictionary<DataSource.DataSource, DataSource.DataSource> copiedDataSources, Dictionary<Field, Field> copiedFields, ref int startTableAliasIndex);

        /// <summary>
        /// 将当前字段转换成子查询字段。
        /// </summary>
        /// <param name="dataContext">数据操作上下文对象。</param>
        /// <param name="dataSource">该字段所归属的数据源。</param>
        /// <param name="convertedFields">已转换过的字段信息集合。</param>
        /// <returns>转换后的子查询字段。</returns>
        internal abstract Field ToSubquery(IDataContext dataContext, BasicDataSource dataSource, Dictionary<Field, Field> convertedFields);

        /// <summary>
        /// 获取当前数据字段的类型。
        /// </summary>
        public FieldType Type { get; }

        /// <summary>
        /// 获取当前字段的数据类型。
        /// </summary>
        public Type DataType { get; }
    }
}

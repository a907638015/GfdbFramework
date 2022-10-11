using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.DataSource;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 提取字段信息时的参数信息。
    /// </summary>
    internal class ParameterInfo
    {
        /// <summary>
        /// 使用一个标识当前参数是否是主参数的值以及该参数对应的数据源信息初始化一个新的 <see cref="ParameterInfo"/> 类实例。
        /// </summary>
        /// <param name="isMain">该参数是否为主参数。</param>
        /// <param name="dataSource">该参数对应的数据源信息。</param>
        internal ParameterInfo(bool isMain, BasicDataSource dataSource)
        {
            IsMain = isMain;
            Parameter = dataSource;
        }
        /// <summary>
        /// 使用一个标识当前参数是否是主参数的值以及该参数对应的多表关联操作对象初始化一个新的 <see cref="ParameterInfo"/> 类实例。
        /// </summary>
        /// <param name="isMain">该参数是否为主参数。</param>
        /// <param name="multipleJoin">该参数对应的多表关联操作对象。</param>
        internal ParameterInfo(bool isMain, MultipleJoin multipleJoin)
        {
            IsMain = isMain;
            Parameter = multipleJoin;
        }

        /// <summary>
        /// 使用一个对应该参数的数据源信息初始化一个新的 <see cref="ParameterInfo"/> 类实例。
        /// </summary>
        /// <param name="dataSource">该参数对应的数据源信息。</param>
        internal ParameterInfo(BasicDataSource dataSource)
            : this(false, dataSource)
        {
        }

        /// <summary>
        /// 使用一个对应该参数的多表关联操作对象初始化一个新的 <see cref="ParameterInfo"/> 类实例。
        /// </summary>
        /// <param name="multipleJoin">该参数对应的多表关联操作对象。</param>
        internal ParameterInfo(MultipleJoin multipleJoin)
        {
            IsMain = false;
            Parameter = multipleJoin;
        }

        /// <summary>
        /// 使用指定的参数值初始化一个新的 <see cref="ParameterInfo"/> 类实例。
        /// </summary>
        /// <param name="multipleJoin">该参数对应的参数值。</param>
        private ParameterInfo(object parameter)
        {
            IsMain = false;
            Parameter = parameter;
        }

        /// <summary>
        /// 以当前参数为蓝本复制一个新的参数并将该参数的 <see cref="IsMain"/> 属性值改为 false。
        /// </summary>
        /// <returns>复制后的参数信息</returns>
        internal ParameterInfo Copy()
        {
            return new ParameterInfo(Parameter);
        }

        /// <summary>
        /// 获取一个值，该值指示当前参数是否是主参数。
        /// </summary>
        internal bool IsMain { get; }

        /// <summary>
        /// 获取当前参数对应的数据源信息。
        /// </summary>
        internal object Parameter { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using GfdbFramework.Core;
using GfdbFramework.DataSource;
using GfdbFramework.Enum;
using GfdbFramework.Interface;

namespace GfdbFramework.Field
{
    /// <summary>
    /// 基础数据类型字段。
    /// </summary>
    public abstract class BasicField : Field
    {
        private static readonly string _BOOL_TYPE_NAME = typeof(bool).FullName;
        private string _Alias = null;
        private ExpressionInfo? _ExpressionSQL = null;
        private ExpressionInfo? _BooleanInfo = null;

        /// <summary>
        /// 使用指定的字段类型以及字段返回值的数据类型初始化一个新的 <see cref="BasicField"/> 类实例。
        /// </summary>
        /// <param name="type">该字段的类型。</param>
        /// <param name="dataType">该字段返回值的数据类型。</param>
        internal BasicField(FieldType type, Type dataType)
            :base(type, dataType)
        {
        }

        /// <summary>
        /// 获取当前数据字段所使用的别名。
        /// </summary>
        public string Alias
        {
            get
            {
                return _Alias;
            }
        }

        /// <summary>
        /// 获取当前基础数据类型字段的 Sql 表示信息。
        /// </summary>
        public virtual ExpressionInfo ExpressionInfo
        {
            get
            {
                if (_ExpressionSQL == null || !_ExpressionSQL.HasValue)
                    throw new Exception("未能获取当前字段的表示 Sql 信息");

                return _ExpressionSQL.Value;
            }
        }

        /// <summary>
        /// 获取当前基础数据类型字段被直接用于 Where、On、Case 等条件操作时的 Sql 表示信息。
        /// </summary>
        public virtual ExpressionInfo BooleanInfo
        {
            get
            {
                if (_BooleanInfo == null || !_BooleanInfo.HasValue)
                    throw new Exception("未能生成当前字段被直接用于 Where、On、Case 等条件操作时的表示 Sql 信息");

                return _BooleanInfo.Value;
            }
        }

        /// <summary>
        /// 初始化当前字段的表示 Sql 语句。
        /// </summary>
        /// <param name="dataContext">数据操作上下文。</param>
        /// <param name="dataSource">该字段归属的数据源。</param>
        /// <param name="addParameter">添加 Sql 所需使用的参数方法。</param>
        public virtual void InitExpressionSQL(IDataContext dataContext, DataSource.DataSource dataSource, Func<object, string> addParameter)
        {
            InitExpressionSQL(dataContext, dataSource, true, addParameter);
        }

        /// <summary>
        /// 初始化当前字段的表示 Sql 语句。
        /// </summary>
        /// <param name="dataContext">数据操作上下文。</param>
        /// <param name="dataSource">该字段归属的数据源。</param>
        /// <param name="isInitBooleanInfo">是否初始化条件表示方式。</param>
        /// <param name="addParameter">添加 Sql 所需使用的参数方法。</param>
        public virtual void InitExpressionSQL(IDataContext dataContext, DataSource.DataSource dataSource, bool isInitBooleanInfo, Func<object, string> addParameter)
        {
            if (_ExpressionSQL == null)
            {
                if (isInitBooleanInfo && DataType.FullName == _BOOL_TYPE_NAME)
                    _BooleanInfo = dataContext.SqlFactory.InitFieldWhere(dataContext, dataSource, this, addParameter);

                switch (Type)
                {
                    case FieldType.Original:
                        _ExpressionSQL = dataContext.SqlFactory.InitOriginalField(dataContext, dataSource, (OriginalField)this, addParameter);
                        break;
                    case FieldType.Unary:
                        _ExpressionSQL = dataContext.SqlFactory.InitUnaryField(dataContext, dataSource, (UnaryField)this, addParameter);
                        break;
                    case FieldType.Binary:
                        _ExpressionSQL = dataContext.SqlFactory.InitBinaryField(dataContext, dataSource, (BinaryField)this, addParameter);
                        break;
                    case FieldType.Conditional:
                        _ExpressionSQL = dataContext.SqlFactory.InitConditionalField(dataContext, dataSource, (ConditionalField)this, addParameter);
                        break;
                    case FieldType.Method:
                        _ExpressionSQL = dataContext.SqlFactory.InitMethodField(dataContext, dataSource, (MethodField)this, addParameter);
                        break;
                    case FieldType.Member:
                        _ExpressionSQL = dataContext.SqlFactory.InitMemberField(dataContext, dataSource, (MemberField)this, addParameter);
                        break;
                    case FieldType.Quote:
                        _ExpressionSQL = dataContext.SqlFactory.InitQuoteField(dataContext, dataSource, (QuoteField)this, addParameter);
                        break;
                    case FieldType.Subquery:
                        _ExpressionSQL = dataContext.SqlFactory.InitSubqueryField(dataContext, dataSource, (SubqueryField)this, addParameter);
                        break;
                    case FieldType.Constant:
                        _ExpressionSQL = dataContext.SqlFactory.InitConstantField(dataContext, dataSource, (ConstantField)this, addParameter);
                        break;
                }
            }
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
                self = new SubqueryField(DataType, this, dataSource);

                convertedFields.Add(this, self);
            }

            return self;
        }

        /// <summary>
        /// 修改当前数据字段的别名。
        /// </summary>
        /// <param name="alias">新的数据字段别名。</param>
        /// <returns>当前字段。</returns>
        internal BasicField ModifyAlias(string alias)
        {
            _Alias = alias;

            return this;
        }
    }
}

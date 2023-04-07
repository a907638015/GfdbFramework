using GfdbFramework.Core;
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
    /// 所有基础数据类型字段的抽象基类。
    /// </summary>
    public abstract class BasicField : Field
    {
        private readonly Type _BoolType = typeof(bool);
        private string _Alias = null;
        private ExpressionInfo _BasicExpression = null;
        private ExpressionInfo _BoolExpression = null;

        /// <summary>
        /// 使用指定的数据操作上下文、字段类型以及该字段返回的数据类型初始化一个新的 <see cref="BasicField"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该字段所使用的数据操作上下文。</param>
        /// <param name="fieldType">该字段的类型。</param>
        /// <param name="dataType">该字段所返回的数据类型。</param>
        internal BasicField(IDataContext dataContext, FieldType fieldType, Type dataType)
            : base(dataContext, fieldType, dataType)
        {
            IsBoolDataType = dataType == _BoolType;
        }

        /// <summary>
        /// 修改当前字段的别名。
        /// </summary>
        /// <param name="alias">新字段别名。</param>
        /// <returns>当前字段。</returns>
        internal virtual BasicField ModifyAlias(string alias)
        {
            _Alias = alias;

            return this;
        }

        /// <summary>
        /// 重新设置当前字段的别名。
        /// </summary>
        /// <param name="startAliasIndex">字段别名的起始下标。</param>
        internal override Field ResetAlias(ref int startAliasIndex)
        {
            if (string.IsNullOrWhiteSpace(Alias))
            {
                ModifyAlias(DataContext.SqlFactory.GenerateFieldAlias(startAliasIndex));

                startAliasIndex++;
            }

            return this;
        }

        /// <summary>
        /// 将当前字段转换成子查询字段。
        /// </summary>
        /// <param name="dataSource">该字段所归属的数据源信息。</param>
        /// <param name="convertedFields">已转换的字段信息。</param>
        /// <returns>转换后的新字段。</returns>
        internal override Field ToSubqueryField(BasicDataSource dataSource, Dictionary<Field, Field> convertedFields)
        {
            if (!convertedFields.TryGetValue(this, out Field convertedField))
            {
                convertedField = new SubqueryField(DataContext, this, dataSource);

                convertedFields[this] = convertedField;
            }

            return convertedField;
        }

        /// <summary>
        /// 将当前字段转换成引用字段。
        /// </summary>
        /// <param name="dataSource">该字段所归属的数据源。</param>
        /// <param name="convertedFields">已转换的字段信息。</param>
        /// <returns>转换后的新字段。</returns>
        internal override Field ToQuoteField(BasicDataSource dataSource, Dictionary<Field, Field> convertedFields)
        {
            if (!convertedFields.TryGetValue(this, out Field convertedField))
            {
                convertedField = new QuoteField(this, dataSource).ModifyAlias(Alias);

                convertedFields[this] = convertedField;
            }

            return convertedField;
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
                self = new IsolateField(DataContext, FieldType.NewAlias, this);

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
            if (!convertedFields.TryGetValue(this, out Field convertedField))
            {
                convertedField = new QuoteField(this, sourceAlias).ModifyAlias(Alias);

                convertedFields[this] = convertedField;
            }

            return convertedField;
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
                if (Alias == ((BasicField)field).Alias)
                    return this;
                else
                    return null;
            }
            else
            {
                throw new Exception($"对齐到另外一个 {Type} 类型的字段时发现两个字段的返回数据类型不一致");
            }
        }

        /// <summary>
        /// 获取该对象字段的调试显示结果。
        /// </summary>
        /// <param name="parameterContext">获取显示结果所使用的参数上下文对象。</param>
        /// <returns>获取到的显示结果。</returns>
        internal override string GetDebugResult(IParameterContext parameterContext)
        {
            if (IsBoolDataType)
                return GetBoolExpressionInfo(parameterContext).SQL;
            else
                return GetBasicExpressionInfo(parameterContext).SQL;
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
        /// 获取一个值，该值指示当前基础数据类型字段的返回值是否是布尔类型。
        /// </summary>
        public bool IsBoolDataType { get; }

        /// <summary>
        /// 使用指定的参数上下文对象获取该字段的基础表示信息。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <returns>创建好的表示信息对象。</returns>
        public virtual ExpressionInfo GetBasicExpression(IParameterContext parameterContext)
        {
            if (parameterContext.EnableParametric)
            {
                if (_BasicExpression == null || _BasicExpression.ParameterContext != parameterContext)
                {
                    _BasicExpression = GetBasicExpressionInfo(parameterContext);

                    if (parameterContext.EnableParametric)
                        _BasicExpression.ParameterContext = parameterContext;
                }

                return _BasicExpression;
            }
            else
            {
                return GetBasicExpressionInfo(parameterContext);
            }
        }

        /// <summary>
        /// 使用指定的参数上下文对象获取该字段在用作布尔操作时的表示信息（如：where、on、case when 等操作时的判断语句表示方式）。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <returns>创建好的表示信息对象。</returns>
        public virtual ExpressionInfo GetBoolExpression(IParameterContext parameterContext)
        {
            if (IsBoolDataType)
            {
                if (parameterContext.EnableParametric)
                {
                    if (_BoolExpression == null || _BoolExpression.ParameterContext != parameterContext)
                    {
                        _BoolExpression = GetBoolExpressionInfo(parameterContext);

                        _BoolExpression.ParameterContext = parameterContext;
                    }

                    return _BoolExpression;
                }
                else
                {
                    return GetBoolExpressionInfo(parameterContext);
                }
            }

            throw new Exception("对于非布尔类型值的字段不能生成对应的布尔表示信息");
        }

        /// <summary>
        /// 使用指定的参数上下文对象获取该字段的基础表示信息。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <returns>创建好的表示信息对象。</returns>
        private ExpressionInfo GetBasicExpressionInfo(IParameterContext parameterContext)
        {
            switch (Type)
            {
                case FieldType.Original:
                    return DataContext.SqlFactory.CreateOriginalBasicSql(parameterContext, (OriginalField)this);
                case FieldType.Unary:
                    return DataContext.SqlFactory.CreateUnaryBasicSql(parameterContext, (UnaryField)this);
                case FieldType.Binary:
                    return DataContext.SqlFactory.CreateBinaryBasicSql(parameterContext, (BinaryField)this);
                case FieldType.Conditional:
                    return DataContext.SqlFactory.CreateConditionalBasicSql(parameterContext, (ConditionalField)this);
                case FieldType.Method:
                    return DataContext.SqlFactory.CreateMethodBasicSql(parameterContext, (MethodField)this);
                case FieldType.Member:
                    return DataContext.SqlFactory.CreateMemberBasicSql(parameterContext, (MemberField)this);
                case FieldType.Quote:
                    return DataContext.SqlFactory.CreateQuoteBasicSql(parameterContext, (QuoteField)this);
                case FieldType.Constant:
                    return DataContext.SqlFactory.CreateConstantBasicSql(parameterContext, (ConstantField)this);
                case FieldType.Subquery:
                    return DataContext.SqlFactory.CreateSubqueryBasicSql(parameterContext, (SubqueryField)this);
                case FieldType.Switch:
                    return DataContext.SqlFactory.CreateSwitchBasicSql(parameterContext, (SwitchField)this);
                case FieldType.DefaultOrValue:
                case FieldType.NewAlias:
                    return ((IsolateField)this).InnerField.GetBasicExpressionInfo(parameterContext);
                default:
                    throw new Exception($"未能创建 {GetType().Name} 字段类型对应的 SQL 基础表示方式信息");
            }
        }

        /// <summary>
        /// 使用指定的参数上下文对象获取该字段在用作布尔操作时的表示信息（如：where、on、case when 等操作时的判断语句表示方式）。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <returns>创建好的表示信息对象。</returns>
        private ExpressionInfo GetBoolExpressionInfo(IParameterContext parameterContext)
        {
            switch (Type)
            {
                case FieldType.Original:
                    return DataContext.SqlFactory.CreateOriginalBoolSql(parameterContext, (OriginalField)this);
                case FieldType.Unary:
                    return DataContext.SqlFactory.CreateUnaryBoolSql(parameterContext, (UnaryField)this);
                case FieldType.Binary:
                    return DataContext.SqlFactory.CreateBinaryBoolSql(parameterContext, (BinaryField)this);
                case FieldType.Conditional:
                    return DataContext.SqlFactory.CreateConditionalBoolSql(parameterContext, (ConditionalField)this);
                case FieldType.Method:
                    return DataContext.SqlFactory.CreateMethodBoolSql(parameterContext, (MethodField)this);
                case FieldType.Member:
                    return DataContext.SqlFactory.CreateMemberBoolSql(parameterContext, (MemberField)this);
                case FieldType.Quote:
                    return DataContext.SqlFactory.CreateQuoteBoolSql(parameterContext, (QuoteField)this);
                case FieldType.Constant:
                    return DataContext.SqlFactory.CreateConstantBoolSql(parameterContext, (ConstantField)this);
                case FieldType.Subquery:
                    return DataContext.SqlFactory.CreateSubqueryBoolSql(parameterContext, (SubqueryField)this);
                case FieldType.Switch:
                    return DataContext.SqlFactory.CreateSwitchBoolSql(parameterContext, (SwitchField)this);
                case FieldType.DefaultOrValue:
                case FieldType.NewAlias:
                    return ((IsolateField)this).InnerField.GetBoolExpression(parameterContext);
                default:
                    throw new Exception($"未能创建 {GetType().Name} 字段对应的基础表示信息");
            }
        }
    }
}

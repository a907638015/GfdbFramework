using GfdbFramework.Core;
using GfdbFramework.DataSource;
using GfdbFramework.Field;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace GfdbFramework.Interface
{
    /// <summary>
    /// 各种 Sql 语句的创建工厂接口类。
    /// </summary>
    public interface ISqlFactory
    {
        /// <summary>
        /// 创建一元操作字段的基础表示 Sql 信息。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="field">待创建表示 Sql 信息的一元操作字段。</param>
        /// <returns>该一元操作字段对应的基础 Sql 表示结果。</returns>
        ExpressionInfo CreateUnaryBasicSql(IParameterContext parameterContext, UnaryField field);

        /// <summary>
        /// 创建一元操作字段的布尔表示 Sql 信息。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="field">待创建表示 Sql 信息的一元操作字段。</param>
        /// <returns>该一元操作字段对应的布尔 Sql 表示结果。</returns>
        ExpressionInfo CreateUnaryBoolSql(IParameterContext parameterContext, UnaryField field);

        /// <summary>
        /// 创建原始数据库表或视图字段的基础表示 Sql 信息。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="field">待创建表示 Sql 信息的原始字段。</param>
        /// <returns>该原始数据库表或视图字段对应的基础 Sql 表示结果。</returns>
        ExpressionInfo CreateOriginalBasicSql(IParameterContext parameterContext, OriginalField field);

        /// <summary>
        /// 创建原始数据库表或视图字段的布尔表示 Sql 信息。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="field">待创建表示 Sql 信息的原始字段。</param>
        /// <returns>该原始数据库表或视图字段对应的布尔 Sql 表示结果。</returns>
        ExpressionInfo CreateOriginalBoolSql(IParameterContext parameterContext, OriginalField field);

        /// <summary>
        /// 创建子查询字段的基础表示 Sql 信息。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="field">待创建表示 Sql 信息的子查询字段。</param>
        /// <returns>该子查询字段对应的基础 Sql 表示结果。</returns>
        ExpressionInfo CreateSubqueryBasicSql(IParameterContext parameterContext, SubqueryField field);

        /// <summary>
        /// 创建子查询字段的布尔表示 Sql 信息。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="field">待创建表示 Sql 信息的子查询字段。</param>
        /// <returns>该子查询字段对应的布尔 Sql 表示结果。</returns>
        ExpressionInfo CreateSubqueryBoolSql(IParameterContext parameterContext, SubqueryField field);

        /// <summary>
        /// 创建二元操作字段的基础表示 Sql 信息。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="field">待创建表示 Sql 信息的二元操作字段。</param>
        /// <returns>该二元操作字段对应的基础 Sql 表示结果。</returns>
        ExpressionInfo CreateBinaryBasicSql(IParameterContext parameterContext, BinaryField field);

        /// <summary>
        /// 创建二元操作字段的布尔表示 Sql 信息。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="field">待创建表示 Sql 信息的二元操作字段。</param>
        /// <returns>该二元操作字段对应的布尔 Sql 表示结果。</returns>
        ExpressionInfo CreateBinaryBoolSql(IParameterContext parameterContext, BinaryField field);

        /// <summary>
        /// 创建三元操作字段的基础表示 Sql 信息。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="field">待创建表示 Sql 信息的三元操作字段。</param>
        /// <returns>该三元操作字段对应的基础 Sql 表示结果。</returns>
        ExpressionInfo CreateConditionalBasicSql(IParameterContext parameterContext, ConditionalField field);

        /// <summary>
        /// 创建三元操作字段的布尔表示 Sql 信息。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="field">待创建表示 Sql 信息的三元操作字段。</param>
        /// <returns>该三元操作字段对应的布尔 Sql 表示结果。</returns>
        ExpressionInfo CreateConditionalBoolSql(IParameterContext parameterContext, ConditionalField field);

        /// <summary>
        /// 创建常量字段的基础表示 Sql 信息。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="field">待创建表示 Sql 信息的常量字段。</param>
        /// <returns>该常量字段对应的基础 Sql 表示结果。</returns>
        ExpressionInfo CreateConstantBasicSql(IParameterContext parameterContext, ConstantField field);

        /// <summary>
        /// 创建常量字段的布尔表示 Sql 信息。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="field">待创建表示 Sql 信息的常量字段。</param>
        /// <returns>该常量字段对应的布尔 Sql 表示结果。</returns>
        ExpressionInfo CreateConstantBoolSql(IParameterContext parameterContext, ConstantField field);

        /// <summary>
        /// 创建成员调用字段的基础表示 Sql 信息。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="field">待创建表示 Sql 信息的成员调用字段。</param>
        /// <returns>该成员调用字段对应的基础 Sql 表示结果。</returns>
        ExpressionInfo CreateMemberBasicSql(IParameterContext parameterContext, MemberField field);

        /// <summary>
        /// 创建成员调用字段的布尔表示 Sql 信息。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="field">待创建表示 Sql 信息的成员调用字段。</param>
        /// <returns>该成员调用字段对应的布尔 Sql 表示结果。</returns>
        ExpressionInfo CreateMemberBoolSql(IParameterContext parameterContext, MemberField field);

        /// <summary>
        /// 创建方法调用字段的基础表示 Sql 信息。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="field">待创建表示 Sql 信息的方法调用字段。</param>
        /// <returns>该方法调用字段对应的基础 Sql 表示结果。</returns>
        ExpressionInfo CreateMethodBasicSql(IParameterContext parameterContext, MethodField field);

        /// <summary>
        /// 创建方法调用字段的布尔表示 Sql 信息。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="field">待创建表示 Sql 信息的方法调用字段。</param>
        /// <returns>该方法调用字段对应的布尔 Sql 表示结果。</returns>
        ExpressionInfo CreateMethodBoolSql(IParameterContext parameterContext, MethodField field);

        /// <summary>
        /// 创建引用字段的基础表示 Sql 信息。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="field">待创建表示 Sql 信息的引用字段。</param>
        /// <returns>该引用字段对应的基础 Sql 表示结果。</returns>
        ExpressionInfo CreateQuoteBasicSql(IParameterContext parameterContext, QuoteField field);

        /// <summary>
        /// 创建引用字段的布尔表示 Sql 信息。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="field">待创建表示 Sql 信息的引用字段。</param>
        /// <returns>该引用字段对应的布尔 Sql 表示结果。</returns>
        ExpressionInfo CreateQuoteBoolSql(IParameterContext parameterContext, QuoteField field);

        /// <summary>
        /// 创建 Switch 分支字段的布尔表示 Sql 信息。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="field">待创建表示 Sql 信息的 Switch 分支字段。</param>
        /// <returns>该 Switch 分支字段对应的布尔 Sql 表示结果。</returns>
        ExpressionInfo CreateSwitchBoolSql(IParameterContext parameterContext, SwitchField field);

        /// <summary>
        /// 创建 Switch 分支字段的基础表示 Sql 信息。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="field">待创建表示 Sql 信息的 Switch 分支字段。</param>
        /// <returns>该 Switch 分支字段对应的基础 Sql 表示结果。</returns>
        ExpressionInfo CreateSwitchBasicSql(IParameterContext parameterContext, SwitchField field);

        /// <summary>
        /// 生成指定数据源对应的查询 Sql 语句。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="dataSource">待生成语句的数据源对象。</param>
        /// <returns>生成好的查询 Sql 语句。</returns>
        string GenerateSelectSql(IParameterContext parameterContext, BasicDataSource dataSource);

        /// <summary>
        /// 生成指定数据源对应的插入 Sql 语句。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="dataSource">待生成语句的数据源对象。</param>
        /// <param name="fields">需要插入的字段集合。</param>
        /// <param name="args">需要插入字段对应的参数集合。</param>
        /// <returns>生成好的插入 Sql 语句。</returns>
        string GenerateInsertSql(IParameterContext parameterContext, TableDataSource dataSource, ReadOnlyList<OriginalField> fields, ReadOnlyList<BasicField> args);

        /// <summary>
        /// 生成指定数据源对应的插入 Sql 语句。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="dataSource">待生成语句的数据源对象。</param>
        /// <param name="insertDataSource">需要插入到数据表的数据源。</param>
        /// <returns>生成好的插入 Sql 语句。</returns>
        string GenerateInsertSql(IParameterContext parameterContext, TableDataSource dataSource, BasicDataSource insertDataSource);

        /// <summary>
        /// 生成指定数据源对应的删除 Sql 语句。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="deletingDataSources">需要删除数据行的源数组。</param>
        /// <param name="fromDataSource">待删除数据行的目标来源。</param>
        /// <param name="where">删除数据行时的条件字段。</param>
        /// <returns>生成好的删除 Sql 语句。</returns>
        string GenerateDeleteSql(IParameterContext parameterContext, TableDataSource[] deletingDataSources, DataSource.DataSource fromDataSource, BasicField where);

        /// <summary>
        /// 生成指定数据源对应的更新 Sql 语句。
        /// </summary>
        /// <param name="parameterContext">创建表示 Sql 时用于参数化操作的上下文对象。</param>
        /// <param name="updateGroups">所有待更新的数据组。</param>
        /// <param name="fromDataSource">待更新数据的目标来源。</param>
        /// <param name="where">更新数据行时的条件字段。</param>
        /// <returns>生成好的更新 Sql 语句。</returns>
        string GenerateUpdateSql(IParameterContext parameterContext, ReadOnlyList<UpdateGroup> updateGroups, DataSource.DataSource fromDataSource, BasicField where);

        /// <summary>
        /// 使用指定的别名下标生成一个字段别名（必须保证不同下标生成的别名不同，相同下标生成的别名相同，且所生成的别名不得是数据库中的关键字）。
        /// </summary>
        /// <param name="aliasIndex">生成别名所使用的下标。</param>
        /// <returns>使用指定别名下标生成好的别名。</returns>
        string GenerateFieldAlias(int aliasIndex);

        /// <summary>
        /// 使用指定的别名下标生成一个数据源别名（必须保证不同下标生成的别名不同，相同下标生成的别名相同，且所生成的别名不得是数据库中的关键字）。
        /// </summary>
        /// <param name="aliasIndex">生成别名所使用的下标。</param>
        /// <returns>使用指定别名下标生成好的别名。</returns>
        string GenerateDataSourceAlias(int aliasIndex);

        /// <summary>
        /// 对指定的原始字段名进行编码。
        /// </summary>
        /// <param name="name">需要编码的原始字段名。</param>
        /// <returns>编码后的名称。</returns>
        string EncodeFieldName(string name);

        /// <summary>
        /// 对指定的原始表名进行编码。
        /// </summary>
        /// <param name="name">需要编码的原始表名。</param>
        /// <returns>编码后的名称。</returns>
        string EncodeTableName(string name);

        /// <summary>
        /// 对指定的原始视图名进行编码。
        /// </summary>
        /// <param name="name">需要编码的原始视图名。</param>
        /// <returns>编码后的名称。</returns>
        string EncodeViewName(string name);
    }
}

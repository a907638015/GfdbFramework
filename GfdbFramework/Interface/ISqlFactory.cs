using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using GfdbFramework.Core;
using GfdbFramework.DataSource;
using GfdbFramework.Enum;
using GfdbFramework.Field;

namespace GfdbFramework.Interface
{
    /// <summary>
    /// 各种 Sql 创建工厂接口类。
    /// </summary>
    public interface ISqlFactory
    {
        /// <summary>
        /// 初始化指定原始数据字段的 Sql 表示信息。
        /// </summary>
        /// <param name="dataContext">数据操作上下文对象。</param>
        /// <param name="dataSource">待生成 Sql 表示信息字段所归属的数据源信息。</param>
        /// <param name="field">待生成 Sql 表示信息的字段。</param>
        /// <param name="addParameter">添加 Sql 所需的参数方法（参数为需要添加的参数，返回值代表该参数的变量名）。</param>
        /// <returns>生成好的表示 Sql 信息。</returns>
        ExpressionInfo InitOriginalField(IDataContext dataContext, DataSource.DataSource dataSource, OriginalField field, Func<object, string> addParameter);

        /// <summary>
        /// 初始化指定基础数据类型字段被直接用做 Where、On、Case 等条件判定时的 Sql 表示信息（如原始 Bit 类型字段直接用作 Where 条件时需要写成 Table.FieldName = 1）。
        /// </summary>
        /// <param name="dataContext">数据操作上下文对象。</param>
        /// <param name="dataSource">待生成 Sql 表示信息字段所归属的数据源信息。</param>
        /// <param name="field">需要生成 Where、On、Case 等条件判定表示 Sql 信息的字段。</param>
        /// <param name="addParameter">添加 Sql 所需的参数方法（参数为需要添加的参数，返回值代表该参数的变量名）。</param>
        /// <returns>生成好用于在 Where、On、Case 等条件判定时的 Sql 表示信息。</returns>
        ExpressionInfo InitFieldWhere(IDataContext dataContext, DataSource.DataSource dataSource, BasicField field, Func<object, string> addParameter);

        /// <summary>
        /// 初始化指定一元操作字段的 Sql 表示信息。
        /// </summary>
        /// <param name="dataContext">数据操作上下文对象。</param>
        /// <param name="dataSource">待生成 Sql 表示信息字段所归属的数据源信息。</param>
        /// <param name="field">待生成 Sql 表示信息的字段。</param>
        /// <param name="addParameter">添加 Sql 所需的参数方法（参数为需要添加的参数，返回值代表该参数的变量名）。</param>
        /// <returns>生成好的表示 Sql 信息。</returns>
        ExpressionInfo InitUnaryField(IDataContext dataContext, DataSource.DataSource dataSource, UnaryField field, Func<object, string> addParameter);

        /// <summary>
        /// 初始化指定二元操作字段的 Sql 表示信息。
        /// </summary>
        /// <param name="dataContext">数据操作上下文对象。</param>
        /// <param name="dataSource">待生成 Sql 表示信息字段所归属的数据源信息。</param>
        /// <param name="field">待生成 Sql 表示信息的字段。</param>
        /// <param name="addParameter">添加 Sql 所需的参数方法（参数为需要添加的参数，返回值代表该参数的变量名）。</param>
        /// <returns>生成好的表示 Sql 信息。</returns>
        ExpressionInfo InitBinaryField(IDataContext dataContext, DataSource.DataSource dataSource, BinaryField field, Func<object, string> addParameter);

        /// <summary>
        /// 初始化指定三元操作（条件操作）字段的 Sql 表示信息。
        /// </summary>
        /// <param name="dataContext">数据操作上下文对象。</param>
        /// <param name="dataSource">待生成 Sql 表示信息字段所归属的数据源信息。</param>
        /// <param name="field">待生成 Sql 表示信息的字段。</param>
        /// <param name="addParameter">添加 Sql 所需的参数方法（参数为需要添加的参数，返回值代表该参数的变量名）。</param>
        /// <returns>生成好的表示 Sql 信息。</returns>
        ExpressionInfo InitConditionalField(IDataContext dataContext, DataSource.DataSource dataSource, ConditionalField field, Func<object, string> addParameter);

        /// <summary>
        /// 初始化指定方法调用字段的 Sql 表示信息。
        /// </summary>
        /// <param name="dataContext">数据操作上下文对象。</param>
        /// <param name="dataSource">待生成 Sql 表示信息字段所归属的数据源信息。</param>
        /// <param name="field">待生成 Sql 表示信息的字段。</param>
        /// <param name="addParameter">添加 Sql 所需的参数方法（参数为需要添加的参数，返回值代表该参数的变量名）。</param>
        /// <returns>生成好的表示 Sql 信息。</returns>
        ExpressionInfo InitMethodField(IDataContext dataContext, DataSource.DataSource dataSource, MethodField field, Func<object, string> addParameter);

        /// <summary>
        /// 初始化指定成员调用字段的 Sql 表示信息。
        /// </summary>
        /// <param name="dataContext">数据操作上下文对象。</param>
        /// <param name="dataSource">待生成 Sql 表示信息字段所归属的数据源信息。</param>
        /// <param name="field">待生成 Sql 表示信息的字段。</param>
        /// <param name="addParameter">添加 Sql 所需的参数方法（参数为需要添加的参数，返回值代表该参数的变量名）。</param>
        /// <returns>生成好的表示 Sql 信息。</returns>
        ExpressionInfo InitMemberField(IDataContext dataContext, DataSource.DataSource dataSource, MemberField field, Func<object, string> addParameter);

        /// <summary>
        /// 初始化指定引用字段的 Sql 表示信息。
        /// </summary>
        /// <param name="dataContext">数据操作上下文对象。</param>
        /// <param name="dataSource">待生成 Sql 表示信息字段所归属的数据源信息。</param>
        /// <param name="field">待生成 Sql 表示信息的字段。</param>
        /// <param name="addParameter">添加 Sql 所需的参数方法（参数为需要添加的参数，返回值代表该参数的变量名）。</param>
        /// <returns>生成好的表示 Sql 信息。</returns>
        ExpressionInfo InitQuoteField(IDataContext dataContext, DataSource.DataSource dataSource, QuoteField field, Func<object, string> addParameter);

        /// <summary>
        /// 初始化指定子查询字段的 Sql 表示信息。
        /// </summary>
        /// <param name="dataContext">数据操作上下文对象。</param>
        /// <param name="dataSource">待生成 Sql 表示信息字段所归属的数据源信息。</param>
        /// <param name="field">待生成 Sql 表示信息的字段。</param>
        /// <param name="addParameter">添加 Sql 所需的参数方法（参数为需要添加的参数，返回值代表该参数的变量名）。</param>
        /// <returns>生成好的表示 Sql 信息。</returns>
        ExpressionInfo InitSubqueryField(IDataContext dataContext, DataSource.DataSource dataSource, SubqueryField field, Func<object, string> addParameter);

        /// <summary>
        /// 初始化指定常量字段的 Sql 表示信息。
        /// </summary>
        /// <param name="dataContext">数据操作上下文对象。</param>
        /// <param name="dataSource">待生成 Sql 表示信息字段所归属的数据源信息。</param>
        /// <param name="field">待生成 Sql 表示信息的字段。</param>
        /// <param name="addParameter">添加 Sql 所需的参数方法（参数为需要添加的参数，返回值代表该参数的变量名）。</param>
        /// <returns>生成好的表示 Sql 信息。</returns>
        ExpressionInfo InitConstantField(IDataContext dataContext, DataSource.DataSource dataSource, ConstantField field, Func<object, string> addParameter);

        /// <summary>
        /// 使用指定的别名下标生成一个别名（必须保证不同下标生成的别名不同，相同下标生成的别名相同，且所生成的别名不得是数据库中的关键字）。
        /// </summary>
        /// <param name="aliasIndex">生成别名时的下标。</param>
        /// <param name="type">需要生成别名的名称类型。</param>
        /// <returns>使用指定别名下标生成好的别名。</returns>
        string GenerateAlias(int aliasIndex, NameType type);

        /// <summary>
        /// 对指定的原始字段名、表名或视图名称进行编码。
        /// </summary>
        /// <param name="name">需要编码的原始字段名、表名或视图名称名称。</param>
        /// <param name="type">名称类型。</param>
        /// <returns>编码后的名称。</returns>
        string EncodeName(string name, NameType type);

        /// <summary>
        /// 生成指定数据源所对应的 Sql 查询语句。
        /// </summary>
        /// <param name="dataContext">数据操作上下文对象。</param>
        /// <param name="dataSource">待生成查询 Sql 的数据源信息。</param>
        /// <param name="parameters">执行生成 Sql 所需使用的参数集合。</param>
        /// <returns>生成好的 Sql 查询语句。</returns>
        string GenerateQuerySql(IDataContext dataContext, BasicDataSource dataSource, out IReadOnlyList<DbParameter> parameters);

        /// <summary>
        /// 生成指定数据表的插入 Sql 语句。
        /// </summary>
        /// <param name="dataContext">数据操作上下文对象。</param>
        /// <param name="dataSource">待生成插入 Sql 语句的原始数据源对象。</param>
        /// <param name="fields">需要插入的字段集合。</param>
        /// <param name="values">需要插入字段对应的值集合。</param>
        /// <param name="parameters">执行生成 Sql 所需使用的参数集合。</param>
        /// <returns>生成好的插入 Sql 语句。</returns>
        string GenerateInsertSql(IDataContext dataContext, OriginalDataSource dataSource, IReadOnlyList<OriginalField> fields, IReadOnlyList<BasicField> values, out IReadOnlyList<DbParameter> parameters);

        /// <summary>
        /// 生成指定数据表的插入 Sql 语句。
        /// </summary>
        /// <param name="dataContext">数据操作上下文对象。</param>
        /// <param name="dataSource">待生成插入 Sql 语句的原始数据源对象。</param>
        /// <param name="fields">需要插入的字段集合。</param>
        /// <param name="entitys">需要插入的查询结果数据源。</param>
        /// <param name="parameters">执行生成 Sql 所需使用的参数集合。</param>
        /// <returns>生成好的插入 Sql 语句。</returns>
        string GenerateInsertSql(IDataContext dataContext, OriginalDataSource dataSource, IReadOnlyList<OriginalField> fields, BasicDataSource entitys, out IReadOnlyList<DbParameter> parameters);

        /// <summary>
        /// 生成指定数据表的数据删除 Sql 语句。
        /// </summary>
        /// <param name="dataContext">数据操作上下文对象。</param>
        /// <param name="deleteSources">待删除数据行的源数组。</param>
        /// <param name="fromSource">待删除数据行的来源数据。</param>
        /// <param name="where">删除时的条件限定字段信息。</param>
        /// <param name="parameters">执行生成 Sql 所需使用的参数集合。</param>
        /// <returns>生成好的数据删除 Sql 语句。</returns>
        string GenerateDeleteSql(IDataContext dataContext, OriginalDataSource[] deleteSources, DataSource.DataSource fromSource, BasicField where, out IReadOnlyList<DbParameter> parameters);

        /// <summary>
        /// 生成指定数据表的数据更新 Sql 语句。
        /// </summary>
        /// <param name="dataContext">数据操作上下文对象。</param>
        /// <param name="modifyFields">需要修改的字段信息集合。</param>
        /// <param name="dataSource">待修改数据的来源数据。</param>
        /// <param name="where">修改时的条件限定字段信息。</param>
        /// <param name="parameters">执行生成 Sql 所需使用的参数集合。</param>
        /// <returns>生成好的数据更新 Sql 语句。</returns>
        string GenerateUpdateSql(IDataContext dataContext, IReadOnlyList<ModifyInfo> modifyFields, DataSource.DataSource dataSource, BasicField where, out IReadOnlyList<DbParameter> parameters);
    }
}

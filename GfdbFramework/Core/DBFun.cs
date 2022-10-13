using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 通用数据库函数（仅包含常见函数，其他非常见函数请自定义方法然后打上 <see cref="Attribute.DBFunctionAttribute"/> 标记，再实现 <see cref="Interface.ISqlFactory"/> 即可）。
    /// </summary>
    public static class DBFun
    {
        private const string _CALL_EXCEPTION_MESSAGE = "不能直接在 .NET 框架中直接调用数据库函数";

        /// <summary>
        /// 聚合函数，用于统计查询结果中某列具体值所出现的次数。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>查询结果中指定列具体值所出现的次数。</returns>
        public static int Count(object cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 获取数据库服务器的当前时间。
        /// </summary>
        /// <returns>数据库服务器的当前时间。</returns>
        public static DateTime NowTime()
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 生成一个随机的全球唯一码。
        /// </summary>
        /// <returns>生成好的全球唯一码。</returns>
        public static Guid NewID()
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计查询结果集中的数据条数。
        /// </summary>
        /// <returns>查询结果集中的数据条数。</returns>
        public static int Count()
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最大值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最大值。</returns>
        public static int Max(int cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最大值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最大值。</returns>
        public static uint Max(uint cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最大值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最大值。</returns>
        public static long Max(long cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最大值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最大值。</returns>
        public static ulong Max(ulong cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最大值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最大值。</returns>
        public static double Max(double cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最大值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最大值。</returns>
        public static float Max(float cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最大值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最大值。</returns>
        public static decimal Max(decimal cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最大值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最大值。</returns>
        public static short Max(short cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最大值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最大值。</returns>
        public static ushort Max(ushort cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最大值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最大值。</returns>
        public static byte Max(byte cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最大值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最大值。</returns>
        public static sbyte Max(sbyte cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最小值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最小值。</returns>
        public static DateTime Max(DateTime cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最小值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最小值。</returns>
        public static int Min(int cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最小值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最小值。</returns>
        public static uint Min(uint cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最小值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最小值。</returns>
        public static long Min(long cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最小值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最小值。</returns>
        public static ulong Min(ulong cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最小值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最小值。</returns>
        public static double Min(double cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最小值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最小值。</returns>
        public static float Min(float cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最小值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最小值。</returns>
        public static decimal Min(decimal cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最小值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最小值。</returns>
        public static short Min(short cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最小值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最小值。</returns>
        public static ushort Min(ushort cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最小值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最小值。</returns>
        public static byte Min(byte cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最小值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最小值。</returns>
        public static sbyte Min(sbyte cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于获取某列所有值中的最小值。
        /// </summary>
        /// <param name="cellValue">待获取列中的值。</param>
        /// <returns>该列所有值中的最小值。</returns>
        public static DateTime Min(DateTime cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的和。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的和。</returns>
        public static int Sum(int cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的和。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的和。</returns>
        public static uint Sum(uint cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的和。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的和。</returns>
        public static long Sum(long cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的和。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的和。</returns>
        public static ulong Sum(ulong cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的和。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的和。</returns>
        public static double Sum(double cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的和。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的和。</returns>
        public static float Sum(float cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的和。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的和。</returns>
        public static decimal Sum(decimal cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的和。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的和。</returns>
        public static short Sum(short cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的和。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的和。</returns>
        public static ushort Sum(ushort cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的和。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的和。</returns>
        public static byte Sum(byte cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的和。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的和。</returns>
        public static sbyte Sum(sbyte cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的均值。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的均值。</returns>
        public static int Avg(int cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的均值。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的均值。</returns>
        public static uint Avg(uint cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的均值。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的均值。</returns>
        public static long Avg(long cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的均值。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的均值。</returns>
        public static ulong Avg(ulong cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的均值。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的均值。</returns>
        public static double Avg(double cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的均值。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的均值。</returns>
        public static float Avg(float cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的均值。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的均值。</returns>
        public static decimal Avg(decimal cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的均值。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的均值。</returns>
        public static short Avg(short cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的均值。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的均值。</returns>
        public static ushort Avg(ushort cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的均值。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的均值。</returns>
        public static byte Avg(byte cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的均值。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的均值。</returns>
        public static sbyte Avg(sbyte cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的标准偏差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的标准偏差。</returns>
        public static double STDev(int cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的标准偏差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的标准偏差。</returns>
        public static double STDev(uint cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的标准偏差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的标准偏差。</returns>
        public static double STDev(long cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的标准偏差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的标准偏差。</returns>
        public static double STDev(ulong cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的标准偏差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的标准偏差。</returns>
        public static double STDev(double cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的标准偏差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的标准偏差。</returns>
        public static double STDev(float cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的标准偏差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的标准偏差。</returns>
        public static double STDev(decimal cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的标准偏差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的标准偏差。</returns>
        public static double STDev(short cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的标准偏差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的标准偏差。</returns>
        public static double STDev(ushort cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的标准偏差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的标准偏差。</returns>
        public static double STDev(byte cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的标准偏差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的标准偏差。</returns>
        public static double STDev(sbyte cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的标准差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的标准差。</returns>
        public static double STDevP(int cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的标准差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的标准差。</returns>
        public static double STDevP(uint cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的标准差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的标准差。</returns>
        public static double STDevP(long cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的标准差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的标准差。</returns>
        public static double STDevP(ulong cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的标准差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的标准差。</returns>
        public static double STDevP(double cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的标准差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的标准差。</returns>
        public static double STDevP(float cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的标准差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的标准差。</returns>
        public static double STDevP(decimal cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的标准差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的标准差。</returns>
        public static double STDevP(short cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的标准差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的标准差。</returns>
        public static double STDevP(ushort cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的标准差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的标准差。</returns>
        public static double STDevP(byte cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的标准差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的标准差。</returns>
        public static double STDevP(sbyte cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的统计方差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的统计方差。</returns>
        public static double VarP(int cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的统计方差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的统计方差。</returns>
        public static double VarP(uint cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的统计方差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的统计方差。</returns>
        public static double VarP(long cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的统计方差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的统计方差。</returns>
        public static double VarP(ulong cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的统计方差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的统计方差。</returns>
        public static double VarP(double cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的统计方差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的统计方差。</returns>
        public static double VarP(float cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的统计方差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的统计方差。</returns>
        public static double VarP(decimal cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的统计方差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的统计方差。</returns>
        public static double VarP(short cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的统计方差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的统计方差。</returns>
        public static double VarP(ushort cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的统计方差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的统计方差。</returns>
        public static double VarP(byte cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的统计方差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的统计方差。</returns>
        public static double VarP(sbyte cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的填充统计方差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的填充统计方差。</returns>
        public static double Var(int cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的填充统计方差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的填充统计方差。</returns>
        public static double Var(uint cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的填充统计方差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的填充统计方差。</returns>
        public static double Var(long cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的填充统计方差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的填充统计方差。</returns>
        public static double Var(ulong cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的填充统计方差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的填充统计方差。</returns>
        public static double Var(double cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的填充统计方差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的填充统计方差。</returns>
        public static double Var(float cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的填充统计方差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的填充统计方差。</returns>
        public static double Var(decimal cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的填充统计方差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的填充统计方差。</returns>
        public static double Var(short cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的填充统计方差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的填充统计方差。</returns>
        public static double Var(ushort cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的填充统计方差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的填充统计方差。</returns>
        public static double Var(byte cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计某列中所有值的填充统计方差。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>该列中所有值的填充统计方差。</returns>
        public static double Var(sbyte cellValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }
    }
}

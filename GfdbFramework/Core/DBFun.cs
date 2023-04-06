using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 通用数据库函数（仅包含常见函数，其他非常见函数请自定义方法然后打上 <see cref="Attribute.DBFunAttribute"/> 标记）。
    /// </summary>
    public static class DBFun
    {
        private const string _CALL_EXCEPTION_MESSAGE = "不能直接在 .NET 框架中直接调用数据库函数";

        /// <summary>
        /// 聚合函数，用于统计查询结果中某列具体值所出现的次数。
        /// </summary>
        /// <typeparam name="T">需要获取出现次数的列数据类型。</typeparam>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>查询结果中指定列具体值所出现的次数。</returns>
        public static int Count<T>(T cellValue) where T : struct
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 聚合函数，用于统计查询结果中某列具体值所出现的次数。
        /// </summary>
        /// <param name="cellValue">待统计列中的值。</param>
        /// <returns>查询结果中指定列具体值所出现的次数。</returns>
        public static int Count(string cellValue)
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
        /// 获取数据库服务器的当前时间。
        /// </summary>
        /// <returns>数据库服务器的当前时间。</returns>
        public static DateTime NowTime()
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 计算指定日期时间比当前日期时间大多少天。
        /// </summary>
        /// <param name="self">当前日期时间。</param>
        /// <param name="compareValue">比较时间参数。</param>
        /// <returns>指定日期时间大于当前日期时间的天数。</returns>
        public static int DiffDay(this DateTime self, DateTime compareValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 计算指定日期时间比当前日期时间大多少年。
        /// </summary>
        /// <param name="self">当前日期时间。</param>
        /// <param name="compareValue">比较时间参数。</param>
        /// <returns>指定日期时间大于当前日期时间的年数。</returns>
        public static int DiffYear(this DateTime self, DateTime compareValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 计算指定日期时间比当前日期时间大多少月。
        /// </summary>
        /// <param name="self">当前日期时间。</param>
        /// <param name="compareValue">比较时间参数。</param>
        /// <returns>指定日期时间大于当前日期时间的月数。</returns>
        public static int DiffMonth(this DateTime self, DateTime compareValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 计算指定日期时间比当前日期时间大多少小时。
        /// </summary>
        /// <param name="self">当前日期时间。</param>
        /// <param name="compareValue">比较时间参数。</param>
        /// <returns>指定日期时间大于当前日期时间的小时数。</returns>
        public static int DiffHour(this DateTime self, DateTime compareValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 计算指定日期时间比当前日期时间大多少分钟。
        /// </summary>
        /// <param name="self">当前日期时间。</param>
        /// <param name="compareValue">比较时间参数。</param>
        /// <returns>指定日期时间大于当前日期时间的分钟数。</returns>
        public static int DiffMinute(this DateTime self, DateTime compareValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 计算指定日期时间比当前日期时间大多少秒。
        /// </summary>
        /// <param name="self">当前日期时间。</param>
        /// <param name="compareValue">比较时间参数。</param>
        /// <returns>指定日期时间大于当前日期时间的秒数。</returns>
        public static int DiffSecond(this DateTime self, DateTime compareValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 计算指定日期时间比当前日期时间大多少毫秒。
        /// </summary>
        /// <param name="self">当前日期时间。</param>
        /// <param name="compareValue">比较时间参数。</param>
        /// <returns>指定日期时间大于当前日期时间的毫秒数。</returns>
        public static int DiffMillisecond(this DateTime self, DateTime compareValue)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 在当前日期时间上添加指定天数。
        /// </summary>
        /// <param name="self">当前日期时间。</param>
        /// <param name="value">需要添加的天数。</param>
        /// <returns>在当前时间上添加了指定天数后的一个新时间。</returns>
        public static DateTime AddDay(this DateTime self, int value)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 在当前日期时间上添加指定年数。
        /// </summary>
        /// <param name="self">当前日期时间。</param>
        /// <param name="value">需要添加的年数。</param>
        /// <returns>在当前时间上添加了指定年数后的一个新时间。</returns>
        public static DateTime AddYear(this DateTime self, int value)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 在当前日期时间上添加指定月数。
        /// </summary>
        /// <param name="self">当前日期时间。</param>
        /// <param name="value">需要添加的月数。</param>
        /// <returns>在当前时间上添加了指定月数后的一个新时间。</returns>
        public static DateTime AddMonth(this DateTime self, int value)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 在当前日期时间上添加指定小时数。
        /// </summary>
        /// <param name="self">当前日期时间。</param>
        /// <param name="value">需要添加的小时数。</param>
        /// <returns>在当前时间上添加了指定小时数后的一个新时间。</returns>
        public static DateTime AddHour(this DateTime self, int value)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 在当前日期时间上添加指定分钟数。
        /// </summary>
        /// <param name="self">当前日期时间。</param>
        /// <param name="value">需要添加的分钟数。</param>
        /// <returns>在当前时间上添加了指定分钟数后的一个新时间。</returns>
        public static DateTime AddMinute(this DateTime self, int value)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 在当前日期时间上添加指定秒数。
        /// </summary>
        /// <param name="self">当前日期时间。</param>
        /// <param name="value">需要添加的秒数。</param>
        /// <returns>在当前时间上添加了指定秒数后的一个新时间。</returns>
        public static DateTime AddSecond(this DateTime self, int value)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 在当前日期时间上添加指定毫秒数。
        /// </summary>
        /// <param name="self">当前日期时间。</param>
        /// <param name="value">需要添加的毫秒数。</param>
        /// <returns>在当前时间上添加了指定毫秒数后的一个新时间。</returns>
        public static DateTime AddMillisecond(this DateTime self, int value)
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 生成一个随机的全球唯一码。
        /// </summary>
        /// <returns>生成好的全球唯一码。</returns>
        public static Guid NewGuid()
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 生成一个随机的 32 位整数值。
        /// </summary>
        /// <returns>生成好的随机 32 位整数值。</returns>
        public static Guid NewInt()
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 生成一个随机的 64 位整数值。
        /// </summary>
        /// <returns>生成好的随机 64 位整数值。</returns>
        public static Guid NewLong()
        {
            throw new Exception(_CALL_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// 生成一个指定范围内的随机 32 位整数值（最小值为 <paramref name="min"/>，最大值为 <paramref name="max"/> - 1）。
        /// </summary>
        /// <returns>生成好指定范围内的随机 32 位整数值。</returns>
        public static Guid NewInt(int min, int max)
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

using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.Enum;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 文件信息类。
    /// </summary>
    public class FileInfo
    {
        /// <summary>
        /// 获取或设置文件路径。
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 获取或设置文件增长速度（当 <see cref="GrowthMode"/> 属性为 <see cref="GrowthMode.Percentage"/> 时代表增长百分比，否则为每次增长的大小值，单位：MB）。
        /// </summary>
        public double Growth { get; set; }

        /// <summary>
        /// 获取或设置文件增长模式。
        /// </summary>
        public GrowthMode GrowthMode { get; set; }

        /// <summary>
        /// 获取或设置文件最大允许的大小值（单位：byte）。
        /// </summary>
        public long MaxSize { get; set; }

        /// <summary>
        /// 获取或设置文件初始的大小值（单位：byte）。
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 获取或设置该文件类型。
        /// </summary>
        public Enum.FileType Type { get; set; }
    }
}

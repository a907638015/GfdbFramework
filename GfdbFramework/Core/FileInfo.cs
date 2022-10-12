using System;
using System.Collections.Generic;
using System.Text;

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
        /// 获取或设置文件增长速度。
        /// </summary>
        public double Growth { get; set; }

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

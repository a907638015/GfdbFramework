using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 数据库信息类。
    /// </summary>
    public class DatabaseInfo
    {
        /// <summary>
        /// 获取或设置该数据库的名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置保存该数据库的文件信息。
        /// </summary>
        public IList<FileInfo> Files { get; set; }

        /// <summary>
        /// 获取或设置该数据库额外的配置信息。
        /// </summary>
        public object ExtraInfo { get; set; }
    }
}

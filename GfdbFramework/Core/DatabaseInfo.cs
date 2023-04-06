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
        /// 使用指定的数据库名称初始化一个新的 <see cref="DatabaseInfo"/> 类实例。
        /// </summary>
        /// <param name="name">数据库名称。</param>
        public DatabaseInfo(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "初始化数据库信息时数据库名称不能为空");

            Name = name;
        }

        /// <summary>
        /// 获取或设置该数据库的名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置保存该数据库的文件信息。
        /// </summary>
        public IList<DatabaseFile> Files { get; set; }

        /// <summary>
        /// 获取或设置该数据库额外的配置信息。
        /// </summary>
        public object ExtraInfo { get; set; }
    }
}

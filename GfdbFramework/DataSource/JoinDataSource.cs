using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.Enum;
using GfdbFramework.Field;
using GfdbFramework.Interface;

namespace GfdbFramework.DataSource
{
    /// <summary>
    /// 各种连接查询的数据源。
    /// </summary>
    public class JoinDataSource : DataSource
    {
        /// <summary>
        /// 使用指定的数据操作上下文、数据源类型、连接查询中的左侧数据源、连接查询中的右侧数据源以及左右数据源的关联条件初始化一个新的 <see cref="JoinDataSource"/> 类实例。
        /// </summary>
        /// <param name="dataContext">数据操作上下文。</param>
        /// <param name="type">数据源类型。</param>
        /// <param name="left">连接查询中的左侧数据源。</param>
        /// <param name="right">连接查询中的右侧数据源。</param>
        /// <param name="on">左右数据源的关联条件字段信息。</param>
        public JoinDataSource(IDataContext dataContext, DataSourceType type, DataSource left, DataSource right, BasicField on)
            : base(dataContext, type)
        {
            Left = left;
            Right = right;
            On = on;
        }

        /// <summary>
        /// 以当前数据源为蓝本复制出一个一样的数据源信息。
        /// </summary>
        /// <param name="startAliasIndex">复制后新数据源的起始表别名下标。</param>
        /// <returns>复制好的新数据源信息。</returns>
        internal override DataSource Copy(ref int startAliasIndex)
        {
            DataSource left = Left.Copy(ref startAliasIndex);
            DataSource right = Right.Copy(ref startAliasIndex);

            Dictionary<DataSource, DataSource> copiedDataSources = new Dictionary<DataSource, DataSource>()
            {
                { Left, left },
                { Right, right }
            };

            BasicField on = (BasicField)On.Copy(DataContext, true, copiedDataSources, new Dictionary<Field.Field, Field.Field>(), ref startAliasIndex);

            return new JoinDataSource(DataContext, Type, left, right, on);
        }

        /// <summary>
        /// 获取该连接查询中的左侧数据源信息。
        /// </summary>
        public DataSource Left { get; }

        /// <summary>
        /// 获取该连接查询中的右侧数据源信息。
        /// </summary>
        public DataSource Right { get; }

        /// <summary>
        /// 获取该连接查询中左右数据源的关联条件。
        /// </summary>
        public BasicField On { get; }
    }
}

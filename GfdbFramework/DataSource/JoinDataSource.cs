using GfdbFramework.Enum;
using GfdbFramework.Field;
using GfdbFramework.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.DataSource
{
    /// <summary>
    /// 各种连接数据源类。
    /// </summary>
    public class JoinDataSource : DataSource
    {
        /// <summary>
        /// 使用指定的数据操作上下文、数据源类型、关联左侧数据源、关联右侧数据源以及关联条件字段初始化一个新的 <see cref="JoinDataSource"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该数据源所使用的数据操作上下文。</param>
        /// <param name="type">数据源的关联类型。</param>
        /// <param name="left">关联左侧的数据源实例。</param>
        /// <param name="right">关联右侧的数据源实例。</param>
        /// <param name="on">关联条件字段实例。</param>
        public JoinDataSource(IDataContext dataContext, SourceType type, DataSource left, DataSource right, BasicField on)
            : base(dataContext, type)
        {
            Left = left;
            Right = right;
            On = on;
        }

        /// <summary>
        /// 以当前数据源为蓝本复制出一个一样的数据源信息。
        /// </summary>
        /// <param name="copiedDataSources">已经复制过的数据源集合。</param>
        /// <param name="copiedFields">已经复制好的字段信息。</param>
        /// <param name="startAliasIndex">新数据源的起始别名下标。</param>
        /// <returns>复制好的新数据源信息。</returns>
        public override DataSource Copy(Dictionary<DataSource, DataSource> copiedDataSources, Dictionary<Field.Field, Field.Field> copiedFields, ref int startAliasIndex)
        {
            if (!copiedDataSources.TryGetValue(this, out DataSource self))
            {
                DataSource left = Left.Copy(copiedDataSources, copiedFields, ref startAliasIndex);
                DataSource right = Right.Copy(copiedDataSources, copiedFields, ref startAliasIndex);
                Field.Field on = On?.Copy(copiedDataSources, copiedFields, ref startAliasIndex);

                self = new JoinDataSource(DataContext, Type, left, right, (BasicField)on);

                copiedDataSources[this] = self;
            }

            return self;
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

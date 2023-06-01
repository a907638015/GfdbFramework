using GfdbFramework.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.DataSource
{
    /// <summary>
    /// 合并数据源类型。
    /// </summary>
    public class UnionDataSource : BasicDataSource
    {
        /// <summary>
        /// 使用指定的数据操作上下文、主数据源、从属数据源、合并类型以及该数据源的别名下标初始化一个新的 <see cref="UnionDataSource"/> 类实例。
        /// </summary>
        /// <param name="dataContext">该数据源所使用的数据操作上下文。</param>
        /// <param name="main">合并时的主数据源。</param>
        /// <param name="affiliation">合并时的从属数据源。</param>
        /// <param name="unionType">合并类型。</param>
        /// <param name="aliasIndex">数据源别名下标。</param>
        internal UnionDataSource(IDataContext dataContext, BasicDataSource main, BasicDataSource affiliation,Enum.UnionType unionType, int aliasIndex)
            : base(dataContext, Enum.SourceType.Union, (main.SelectField ?? main.RootField).ToQuoteField(dataContext.SqlFactory.GenerateDataSourceAlias(aliasIndex), new Dictionary<Field.Field, Field.Field>(), true), aliasIndex)
        {
            Main = main;
            Affiliation = affiliation;
            UnionType = unionType;
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
                self = new UnionDataSource(DataContext, (BasicDataSource)Main.Copy(copiedDataSources, copiedFields, ref startAliasIndex), (BasicDataSource)Affiliation.Copy(copiedDataSources, copiedFields, ref startAliasIndex), UnionType, startAliasIndex++);

                copiedDataSources[this] = self;
            }

            return self;
        }

        /// <summary>
        /// 将当前数据源对齐到合并数据源中的主数据源。
        /// </summary>
        /// <param name="mainSource">合并数据源中主数据源。</param>
        /// <returns>对齐后的数据源。</returns>
        internal protected override BasicDataSource AlignUnionSource(BasicDataSource mainSource)
        {
            return new UnionDataSource(DataContext, Main.AlignUnionSource(mainSource), Affiliation.AlignUnionSource(mainSource), UnionType, AliasIndex);
        }

        /// <summary>
        /// 以当前数据源为蓝本复制出一个一样的数据源信息（浅复制，内部所引用的信息不会复制）。
        /// </summary>
        /// <returns>复制好的新数据源信息。</returns>
        internal protected override BasicDataSource ShallowCopy()
        {
            return new UnionDataSource(DataContext, Main, Affiliation, UnionType, AliasIndex);
        }

        /// <summary>
        /// 获取合并结果中的主数据源（即第一个数据源）。
        /// </summary>
        public BasicDataSource Main { get; }

        /// <summary>
        /// 获取合并结果中的从属数据源（即第二个数据源）。
        /// </summary>
        public BasicDataSource Affiliation { get; }

        /// <summary>
        /// 获取合并类型。
        /// </summary>
        public Enum.UnionType UnionType { get; }
    }
}

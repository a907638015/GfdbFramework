﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GfdbFramework.Core
{
    /// <summary>
    /// 关联对象项目类。
    /// </summary>
    public class JoinItem
    {
    }

    /// <summary>
    /// 关联对象项目类。
    /// </summary>
    /// <typeparam name="TLeft">对象左侧关联信息实体类型。</typeparam>
    /// <typeparam name="TRight">对象左侧关联信息实体类型。</typeparam>
    public class JoinItem<TLeft, TRight> : JoinItem
    {
        /// <summary>
        /// 获取当前关联对象的左侧操作源。
        /// </summary>
        public TLeft Left { get; }

        /// <summary>
        /// 获取当前关联对象的右侧操作源。
        /// </summary>
        public TRight Right { get; }
    }
}

/// -------------------------------------------------------------------------------
/// Copyright (C) 2024 - 2025, Hurley, Independent Studio.
/// Copyright (C) 2025 - 2026, Hainan Yuanyou Information Technology Co., Ltd. Guangzhou Branch
///
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
///
/// The above copyright notice and this permission notice shall be included in
/// all copies or substantial portions of the Software.
///
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
/// THE SOFTWARE.
/// -------------------------------------------------------------------------------

using System;

using UnityEngine;

namespace NovaFramework
{
    /// <summary>
    /// 框架加载的程序库的标签类型的枚举定义
    /// </summary>
    [Flags]
    public enum LibraryTag : int
    {
        /// <summary>
        /// 无效
        /// </summary>
        [Header("未知")]
        Unknown = 0x0000,

        /// <summary>
        /// 核心库，框架启动的固定配置，是框架正常运行的必需模块
        /// </summary>
        [Header("核心库")]
        Core = 0x0001,
        /// <summary>
        /// 模块库，平台提供的对特定功能的公共支持库，根据业务需求选择配置加载
        /// </summary>
        [Header("模块库")]
        Module = 0x0002,
        /// <summary>
        /// 业务库，由业务人员自定义配置，并进行实际业务的开发
        /// </summary>
        [Header("业务库")]
        Game = 0x0004,

        /// <summary>
        /// 共享模组（编辑器环境下需要使用），一般由配置文件，协议文件等公共文件组成
        /// </summary>
        [Header("共享模组")]
        Shared = 0x0010,
        /// <summary>
        /// 编译模组，主要针对业务模块，是否需要在启动时预解析加载的标识
        /// </summary>
        [Header("编译模组")]
        Compile = 0x0020,
        /// <summary>
        /// 重载模组（支持热重载），标识该模块属于纯逻辑模组，可进行热重载处理
        /// </summary>
        [Header("重载模组")]
        Hotfix = 0x0040,

        /// <summary>
        /// 演示库，用于进行模块的功能演示，仅在调试模式下加载，发布模式将自动屏蔽
        /// </summary>
        [Header("教程库")]
        Tutorial = 0x0100,
        /// <summary>
        /// 测试库，专用于模块测试，仅在调试模式下加载，发布模式将自动屏蔽
        /// </summary>
        [Header("测试库")]
        Test = 0x0200,
    }
}

/// -------------------------------------------------------------------------------
/// CoreEngine Framework
///
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

namespace CoreEngine
{
    /// <summary>
    /// 框架加载的程序库的标签类型的枚举定义
    /// </summary>
    [System.Flags]
    public enum LibraryTag : int
    {
        /// <summary>
        /// 无效
        /// </summary>
        Unknown = 0x0000,

        /// <summary>
        /// 核心库
        /// </summary>
        Core = 0x0001,
        /// <summary>
        /// 模块库
        /// </summary>
        Module = 0x0002,
        /// <summary>
        /// 业务库
        /// </summary>
        Game = 0x0004,

        /// <summary>
        /// 共享模组（编辑器环境下需要使用）
        /// </summary>
        Shared = 0x0010,
        /// <summary>
        /// 编译模组
        /// </summary>
        Compile = 0x0020,
        /// <summary>
        /// 重载模组（支持热重载）
        /// </summary>
        Hotfix = 0x0040,

        /// <summary>
        /// 演示库
        /// </summary>
        Tutorial = 0x0100,
        /// <summary>
        /// 测试库
        /// </summary>
        Test = 0x0200,
    }
}

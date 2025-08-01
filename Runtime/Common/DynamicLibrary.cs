/// -------------------------------------------------------------------------------
/// CoreEngine Framework
///
/// Copyright (C) 2024 - 2025, Hurley, Independent Studio.
/// Copyright (C) 2025, Hainan Yuanyou Information Tecdhnology Co., Ltd. Guangzhou Branch
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

using System.Collections.Generic;

namespace CoreEngine
{
    /// <summary>
    /// 动态库管理类，负责管理当前运行上下文中涉及到的全部需要动态链接的程序库
    /// </summary>
    internal static class DynamicLibrary
    {
        /**
         * 核心库，程序启动必须装载，所以为默认配置
         */

        const string NovaLibraryName = @"Nova.Library";
        const string NovaEngineName  = @"Nova.Engine";
        const string NovaBasicName   = @"Nova.Basic";
        const string NovaImportName  = @"Nova.Import";
        const string NovaSampleName  = @"Nova.Sample";

        /// <summary>
        /// 外部控制入口，固定设置
        /// </summary>
        public const string ExternalControlEntranceName = NovaBasicName;

        /// <summary>
        /// 核心库列表
        /// </summary>
        static readonly IList<LibraryInfo> _coreLibraries = new List<LibraryInfo>()
        {
            new LibraryInfo() { order = 1, name = NovaLibraryName, tutorial = false, reloadable = false },
            new LibraryInfo() { order = 2, name = NovaEngineName,  tutorial = false, reloadable = false },
            new LibraryInfo() { order = 3, name = NovaBasicName,   tutorial = false, reloadable = false },
            new LibraryInfo() { order = 4, name = NovaImportName,  tutorial = false, reloadable = false },
            new LibraryInfo() { order = 5, name = NovaSampleName,  tutorial = true,  reloadable = false },
        };

        /// <summary>
        /// 业务库列表
        /// </summary>
        static readonly IList<LibraryInfo> _gameLibraries = new List<LibraryInfo>()
        {
            new LibraryInfo() { order = 11, name = "Agen", tutorial = false, reloadable = false },
            new LibraryInfo() { order = 12, name = "Game", tutorial = false, reloadable = false },
            new LibraryInfo() { order = 13, name = "GameHotfix", tutorial = false, reloadable = true },
        };

        /// <summary>
        /// 获取当前系统注册的全部程序集名称<br/>
        /// 若指定是否开启教程，则将根据教程开启状态返回带有教程库的名称列表
        /// </summary>
        /// <param name="tutorial">教程状态标识</param>
        /// <returns>返回全部程序集的名称列表</returns>
        public static IList<string> GetAllAssemblyNames(bool tutorial = false)
        {
            List<string> assemblyNames = new ();

            // 核心库
            for (int n = 0; n < _coreLibraries.Count; ++n)
            {
                LibraryInfo info = _coreLibraries[n];
                if (!tutorial && info.tutorial)
                    continue;

                assemblyNames.Add(info.name);
            }

            // 业务库
            for (int n = 0; n < _gameLibraries.Count; ++n)
            {
                LibraryInfo info = _gameLibraries[n];
                if (!tutorial && info.tutorial)
                    continue;

                assemblyNames.Add(info.name);
            }

            return assemblyNames;
        }

        /// <summary>
        /// 获取当前系统注册的全部可重载程序集名称<br/>
        /// 若指定是否开启教程，则将根据教程开启状态返回带有教程库的名称列表
        /// </summary>
        /// <param name="tutorial">教程状态标识</param>
        /// <returns>返回全部可重载程序集的名称列表</returns>
        public static IList<string> GetAllReloadableAssemblyNames(bool tutorial = false)
        {
            List<string> assemblyNames = new();

            // 跳过所有核心库
            // 因为核心库不可进行重载操作

            // 业务库
            for (int n = 0; n < _gameLibraries.Count; ++n)
            {
                LibraryInfo info = _gameLibraries[n];
                if (!tutorial && info.tutorial)
                    continue;

                if (info.reloadable)
                    assemblyNames.Add(info.name);
            }

            return assemblyNames;
        }
    }
}

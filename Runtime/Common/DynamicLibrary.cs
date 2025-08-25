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
        /// <summary>
        /// 程序库许可通过检测的回调句柄定义
        /// </summary>
        /// <param name="info">库信息</param>
        /// <returns>程序库通过许可返回true，否则返回false</returns>
        public delegate bool LibraryInfoLicenseApprovedCallback(LibraryInfo info);

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
            new LibraryInfo() { order =  1, name = NovaLibraryName, source_path = null, tags = LibraryTag.Core },
            new LibraryInfo() { order =  2, name = NovaEngineName,  source_path = null, tags = LibraryTag.Core },
            new LibraryInfo() { order =  3, name = NovaBasicName,   source_path = null, tags = LibraryTag.Core },
            new LibraryInfo() { order =  4, name = NovaImportName,  source_path = null, tags = LibraryTag.Core },
            new LibraryInfo() { order = 11, name = NovaSampleName,  source_path = null, tags = LibraryTag.Core | LibraryTag.Tutorial },
        };

        /// <summary>
        /// 业务库列表
        /// </summary>
        static readonly IList<LibraryInfo> _gameLibraries = new List<LibraryInfo>()
        {
            new LibraryInfo() { order = 101, name = "Agen",       source_path = null, tags = LibraryTag.Game | LibraryTag.Shared },
            new LibraryInfo() { order = 102, name = "Game",       source_path = null, tags = LibraryTag.Game },
            new LibraryInfo() { order = 103, name = "GameHotfix", source_path = null, tags = LibraryTag.Game | LibraryTag.Hotfix },
        };

        /// <summary>
        /// AOT元数据列表
        /// </summary>
        static readonly IList<string> _aotLibraries = new List<string>()
        {
            "System.Core.dll",
            "System.dll",
            "mscorlib.dll",
            "UnityEngine.CoreModule.dll",
        };

        /// <summary>
        /// 获取当前系统注册的全部程序集名称<br/>
        /// 若指定是否开启检测回调，若开启则将根据检测结果过滤程序库的名称列表
        /// </summary>
        /// <param name="callback">过滤回调</param>
        /// <returns>返回全部程序集的名称列表</returns>
        public static IList<string> GetAllAssemblyNames(LibraryInfoLicenseApprovedCallback callback = null)
        {
            List<string> assemblyNames = new ();

            // 核心库
            for (int n = 0; n < _coreLibraries.Count; ++n)
            {
                LibraryInfo info = _coreLibraries[n];
                if (null == callback || false == callback(info))
                    continue;

                assemblyNames.Add(info.name);
            }

            // 业务库
            for (int n = 0; n < _gameLibraries.Count; ++n)
            {
                LibraryInfo info = _gameLibraries[n];
                if (null == callback || false == callback(info))
                    continue;

                assemblyNames.Add(info.name);
            }

            return assemblyNames;
        }

        /// <summary>
        /// 获取当前系统注册的全部可重载程序集名称<br/>
        /// 若指定是否开启检测回调，若开启则将根据检测结果过滤程序库的名称列表
        /// </summary>
        /// <param name="callback">过滤回调</param>
        /// <returns>返回全部可重载程序集的名称列表</returns>
        public static IList<string> GetAllReloadableAssemblyNames(LibraryInfoLicenseApprovedCallback callback = null)
        {
            List<string> assemblyNames = new();

            // 跳过所有核心库
            // 因为核心库不可进行重载操作

            // 业务库
            for (int n = 0; n < _gameLibraries.Count; ++n)
            {
                LibraryInfo info = _gameLibraries[n];
                if (null == callback || false == callback(info))
                    continue;

                if (info.IsContainsTag(LibraryTag.Hotfix))
                    assemblyNames.Add(info.name);
            }

            return assemblyNames;
        }

        /// <summary>
        /// 获取当前系统注册的全部元数据链接库名称
        /// </summary>
        /// <returns>返回全部元数据链接库的名称列表</returns>
        public static IList<string> GetAllGenericAotNames()
        {
            return _aotLibraries;
        }

        /// <summary>
        /// 通过程序集名称查找对应源码路径
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns>返回源码路径，若查找失败则返回null</returns>
        /// <exception cref="System.IO.FileNotFoundException"></exception>
        public static string GetSourcePathByAssemblyName(string assemblyName)
        {
            LibraryInfo info = GetLibraryInfoByAssemblyName(assemblyName);
            if (null == info)
            {
                throw new System.IO.FileNotFoundException($"unknown assembly name \"{assemblyName}\".");
            }

            if (string.IsNullOrEmpty(info.source_path))
            {
                // 核心库不提供源码路径
                if (_coreLibraries.Contains(info))
                {
                    return null;
                }

                string source_dir = SystemPath.GetPath(ResourcePathType.SourceCodePath);
                return System.IO.Path.Combine(source_dir, assemblyName);
            }

            return info.source_path;
        }

        /// <summary>
        /// 通过程序集名称获取库文件路径
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns>返回库文件路径</returns>
        public static string GetLibraryFilePathByAssemblyName(string assemblyName)
        {
            LibraryInfo info = GetLibraryInfoByAssemblyName(assemblyName);
            if (null == info)
            {
                throw new System.IO.FileNotFoundException($"unknown assembly name \"{assemblyName}\".");
            }

            string library_dir = SystemPath.GetPath(ResourcePathType.LinkLibraryPath);
            return System.IO.Path.Combine(library_dir, $"{assemblyName}.dll");
        }

        /// <summary>
        /// 通过程序集名称获取二进制库文件路径
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns>返回二进制库文件路径</returns>
        public static string GetBinaryLibraryFilePathByAssemblyName(string assemblyName)
        {
            LibraryInfo info = GetLibraryInfoByAssemblyName(assemblyName);
            if (null == info)
            {
                throw new System.IO.FileNotFoundException($"unknown assembly name \"{assemblyName}\".");
            }

            string library_dir = SystemPath.GetPath(ResourcePathType.LinkLibraryPath);
            return System.IO.Path.Combine(library_dir, $"{assemblyName}.dll.bytes");
        }

        /// <summary>
        /// 通过程序集名称查找程序库对象实例
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns>返回程序库实例，若查找失败则返回null</returns>
        public static LibraryInfo GetLibraryInfoByAssemblyName(string assemblyName)
        {
            for (int n = 0; n < _gameLibraries.Count; ++n)
            {
                LibraryInfo info = _gameLibraries[n];
                if (assemblyName == info.name)
                {
                    return info;
                }
            }

            return null;
        }
    }
}

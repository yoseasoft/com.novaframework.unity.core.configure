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
using System.Collections.Generic;
using System.IO;

namespace NovaFramework
{
    /// <summary>
    /// 动态库管理类，负责管理当前运行上下文中涉及到的全部需要动态链接的程序库
    /// </summary>
    internal static class DynamicLibrary
    {
        /// <summary>
        /// 程序库条件过滤的回调句柄定义
        /// </summary>
        /// <param name="info">库信息</param>
        /// <returns>程序库过滤成果返回true，否则返回false</returns>
        public delegate bool LibraryInfoConditionalFilteringCallback(LibraryInfo info);

        /**
         * 核心库，程序启动必须装载，所以为默认配置
         */

        const string NovaLibraryName = @"Nova.Library";
        const string NovaEngineName  = @"Nova.Engine";
        const string NovaBasicName   = @"Nova.Basic";
        const string NovaImportName  = @"Nova.Import";

        /// <summary>
        /// 外部控制入口，固定设置
        /// </summary>
        public const string ExternalControlEntranceName = NovaBasicName;

        /// <summary>
        /// 核心库列表
        /// </summary>
        static readonly IList<LibraryInfo> _coreLibraries = new List<LibraryInfo>()
        {
            new () { order = 1, name = NovaLibraryName, tags = LibraryTag.Core },
            new () { order = 2, name = NovaEngineName,  tags = LibraryTag.Core },
            new () { order = 3, name = NovaBasicName,   tags = LibraryTag.Core },
            new () { order = 4, name = NovaImportName,  tags = LibraryTag.Core },
        };

        /// <summary>
        /// 模块库列表
        /// </summary>
        static readonly IList<LibraryInfo> _moduleLibraries = new List<LibraryInfo>();

        /// <summary>
        /// 业务库列表
        /// </summary>
        static readonly IList<LibraryInfo> _gameLibraries = new List<LibraryInfo>();

        /// <summary>
        /// AOT元数据列表
        /// </summary>
        static readonly IList<string> _aotLibraries = new List<string>();

        /// <summary>
        /// 获取当前系统注册的全部程序集名称<br/>
        /// 若指定是否开启检测回调，若开启则将根据检测结果过滤程序库的名称列表
        /// </summary>
        /// <param name="callback">过滤回调</param>
        /// <returns>返回全部程序集的名称列表</returns>
        public static IList<string> GetAllAssemblyNames(LibraryInfoConditionalFilteringCallback callback = null)
        {
            IList<string> assemblyNames = new List<string>();

            // 核心库
            for (int n = 0; n < _coreLibraries.Count; ++n)
            {
                LibraryInfo info = _coreLibraries[n];
                if (null == callback || false == callback(info))
                    continue;

                assemblyNames.Add(info.name);
            }

            // 模块库
            for (int n = 0; n < _moduleLibraries.Count; ++n)
            {
                LibraryInfo info = _moduleLibraries[n];
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
        /// 获取当前系统注册的全部可加工程序集名称<br/>
        /// 若指定是否开启检测回调，若开启则将根据检测结果过滤程序库的名称列表
        /// </summary>
        /// <param name="callback">过滤回调</param>
        /// <returns>返回全部可加工程序集的名称列表</returns>
        public static IList<string> GetAllPlayableAssemblyNames(LibraryInfoConditionalFilteringCallback callback = null)
        {
            IList<string> assemblyNames = new List<string>();

            // 跳过所有核心库
            // 因为核心库不可进行加工操作

            // 模块库
            for (int n = 0; n < _moduleLibraries.Count; ++n)
            {
                LibraryInfo info = _moduleLibraries[n];
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
        /// 获取当前系统注册的全部元数据链接库名称
        /// </summary>
        /// <returns>返回全部元数据链接库的名称列表</returns>
        public static IList<string> GetAllGenericAotNames()
        {
            return _aotLibraries;
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
                throw new FileNotFoundException($"unknown assembly name \"{assemblyName}\".");
            }

            string library_dir = EnvironmentPath.GetPath(ResourcePathType.LinkLibraryPath);
            return Path.Combine(library_dir, $"{assemblyName}.dll");
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
                throw new FileNotFoundException($"unknown assembly name \"{assemblyName}\".");
            }

            string library_dir = EnvironmentPath.GetPath(ResourcePathType.LinkLibraryPath);
            return Path.Combine(library_dir, $"{assemblyName}.dll.bytes");
        }

        /// <summary>
        /// 通过程序集名称查找程序库对象实例
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns>返回程序库实例，若查找失败则返回null</returns>
        public static LibraryInfo GetLibraryInfoByAssemblyName(string assemblyName)
        {
            for (int n = 0; n < _coreLibraries.Count; ++n)
            {
                LibraryInfo info = _coreLibraries[n];
                if (assemblyName == info.name)
                {
                    return info;
                }
            }

            for (int n = 0; n < _moduleLibraries.Count; ++n)
            {
                LibraryInfo info = _moduleLibraries[n];
                if (assemblyName == info.name)
                {
                    return info;
                }
            }

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

        #region 动态程序库注册绑定相关接口函数

        /// <summary>
        /// 注册新的程序库信息
        /// </summary>
        /// <param name="order">程序库标签序号</param>
        /// <param name="name">程序库标签名称</param>
        /// <param name="tags">程序库标签</param>
        public static void RegisterLibraryInfo(int order, string name, IList<string> tags)
        {
            LibraryTag tag = LibraryTag.Unknown;

            for (int n = 0; null != tags && n < tags.Count; ++n)
            {
                tag |= (LibraryTag) Enum.Parse(typeof(LibraryTag), tags[n]);
            }

            LibraryTag tagForPackType = (LibraryTag) Enum.ToObject(typeof(LibraryTag), (int) tag & 0x0f);
            IList<LibraryInfo> container;

            switch (tagForPackType)
            {
                case LibraryTag.Core:
                    throw new ArgumentException("Cannot register core library.");
                case LibraryTag.Module:
                    container = _moduleLibraries;
                    break;
                case LibraryTag.Game:
                    container = _gameLibraries;
                    break;
                default:
                    throw new ArgumentException("Invalid library tag.");
            }

            // 重复注册检查
            for (int n = 0; n < container.Count; ++n)
            {
                LibraryInfo tmp = container[n];
                if (tmp.order == order || tmp.name == name)
                {
                    throw new ArgumentException("Library name is already registered.");
                }
            }

            LibraryInfo info = new LibraryInfo() { order = order, name = name, tags = tag };
            container.Add(info);
        }

        /// <summary>
        /// 注销所有的程序库信息
        /// </summary>
        public static void UnregisterAllLibraryInfos()
        {
            _moduleLibraries.Clear();
            _gameLibraries.Clear();
        }

        /// <summary>
        /// 注册新的预编译库名称
        /// </summary>
        /// <param name="name">预编译库名称</param>
        public static void RegisterAotLibraryName(string name)
        {
            if (_aotLibraries.Contains(name))
            {
                throw new ArgumentException("AOT library name is already registered.");
            }

            _aotLibraries.Add(name);
        }

        /// <summary>
        /// 注销所有的预编译库名称
        /// </summary>
        public static void UnregisterAllAotLibraryNames()
        {
            _aotLibraries.Clear();
        }

        #endregion
    }
}

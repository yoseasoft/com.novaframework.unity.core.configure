/// -------------------------------------------------------------------------------
/// NovaFramework By UnityEngine
///
/// Copyright (C) 2024 - 2025, Hurley, Independent Studio.
/// Copyright (C) 2025 - 2026, Hainan Yuanyou Information Technology Co., Ltd. Guangzhou Branch
/// Copyright (C) 2026, Hurley, Independent Studio.
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

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NovaFramework
{
    /// <summary>
    /// 动态模块库管理服务类，负责管理当前运行上下文中涉及到的全部需要动态链接的模块库
    /// </summary>
    internal sealed class ExtensionModuleService : Singleton<ExtensionModuleService>
    {
        /// <summary>
        /// 程序库条件过滤的回调句柄定义
        /// </summary>
        /// <param name="info">库信息</param>
        /// <returns>程序库过滤成果返回true，否则返回false</returns>
        public delegate bool LibraryInfoConditionalFilteringCallback(ExtensionModuleInfo info);

        /**
         * 核心库，程序启动必须装载，所以为默认配置
         */

        const string NovaEngineLibraryName = @"NovaEngine.Library";
        const string NovaEngineKernelName  = @"NovaEngine.Kernel";
        const string NovaEngineBasicName   = @"NovaEngine.Basic";
        const string NovaEngineImportName  = @"NovaEngine.Import";

        /// <summary>
        /// 外部控制入口，固定设置
        /// </summary>
        public const string ExternalControlEntranceName = NovaEngineBasicName;

        /// <summary>
        /// 核心库列表
        /// </summary>
        readonly IList<ExtensionModuleInfo> _coreLibraries = new List<ExtensionModuleInfo>()
        {
            new () { order = 1, name = NovaEngineLibraryName, tags = ExtensionModuleTag.Core },
            new () { order = 2, name = NovaEngineKernelName,  tags = ExtensionModuleTag.Core },
            new () { order = 3, name = NovaEngineBasicName,   tags = ExtensionModuleTag.Core },
            new () { order = 4, name = NovaEngineImportName,  tags = ExtensionModuleTag.Core },
        };

        /// <summary>
        /// 模块库列表
        /// </summary>
        readonly IList<ExtensionModuleInfo> _moduleLibraries = new List<ExtensionModuleInfo>();

        /// <summary>
        /// 业务库列表
        /// </summary>
        readonly IList<ExtensionModuleInfo> _gameLibraries = new List<ExtensionModuleInfo>();

        /// <summary>
        /// AOT元数据列表
        /// </summary>
        readonly IList<string> _aotLibraries = new List<string>();

        /// <summary>
        /// 动态库初始化回调接口
        /// </summary>
        protected override sealed void OnInitialize()
        {
#if UNITY_EDITOR
            // 编辑器模式，且未运行状态
            if (!EditorApplication.isPlaying)
            {
                Serialization.EnvironmentConfigures environmentConfigures = Serialization.EnvironmentConfigures.Instance;
                AutoloadConfigurationModuleObjects(environmentConfigures.modules);
                AutoloadConfigurationAotFileNames(environmentConfigures.aots);
            }
#endif
        }

        /// <summary>
        /// 动态库清理回调接口
        /// </summary>
        protected override sealed void OnCleanup()
        {
            UnregisterAllModuleInfos();
            UnregisterAllAotFileNames();
        }

        /// <summary>
        /// 获取当前系统注册的全部程序集名称<br/>
        /// 若指定是否开启检测回调，若开启则将根据检测结果过滤程序库的名称列表
        /// </summary>
        /// <param name="callback">过滤回调</param>
        /// <returns>返回全部程序集的名称列表</returns>
        public IReadOnlyList<string> GetAllAssemblyNames(LibraryInfoConditionalFilteringCallback callback = null)
        {
            List<string> assemblyNames = new ();

            // 核心库
            for (int n = 0; n < _coreLibraries.Count; ++n)
            {
                ExtensionModuleInfo info = _coreLibraries[n];
                if (null == callback || false == callback(info))
                    continue;

                assemblyNames.Add(info.name);
            }

            // 模块库
            for (int n = 0; n < _moduleLibraries.Count; ++n)
            {
                ExtensionModuleInfo info = _moduleLibraries[n];
                if (null == callback || false == callback(info))
                    continue;

                assemblyNames.Add(info.name);
            }

            // 业务库
            for (int n = 0; n < _gameLibraries.Count; ++n)
            {
                ExtensionModuleInfo info = _gameLibraries[n];
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
        public IReadOnlyList<string> GetAllPlayableAssemblyNames(LibraryInfoConditionalFilteringCallback callback = null)
        {
            List<string> assemblyNames = new ();

            // 跳过所有核心库
            // 因为核心库不可进行加工操作

            // 模块库
            for (int n = 0; n < _moduleLibraries.Count; ++n)
            {
                ExtensionModuleInfo info = _moduleLibraries[n];
                if (null == callback || false == callback(info))
                    continue;

                assemblyNames.Add(info.name);
            }

            // 业务库
            for (int n = 0; n < _gameLibraries.Count; ++n)
            {
                ExtensionModuleInfo info = _gameLibraries[n];
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
        public IReadOnlyList<string> GetAllGenericAotNames()
        {
            return (List<string>) _aotLibraries;
        }

        /// <summary>
        /// 通过程序集名称获取库文件路径
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns>返回库文件路径</returns>
        public string GetLibraryFilePathByAssemblyName(string assemblyName)
        {
            ExtensionModuleInfo info = GetModuleInfoByAssemblyName(assemblyName);
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
        public string GetBinaryLibraryFilePathByAssemblyName(string assemblyName)
        {
            ExtensionModuleInfo info = GetModuleInfoByAssemblyName(assemblyName);
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
        public ExtensionModuleInfo GetModuleInfoByAssemblyName(string assemblyName)
        {
            for (int n = 0; n < _coreLibraries.Count; ++n)
            {
                ExtensionModuleInfo info = _coreLibraries[n];
                if (assemblyName == info.name)
                {
                    return info;
                }
            }

            for (int n = 0; n < _moduleLibraries.Count; ++n)
            {
                ExtensionModuleInfo info = _moduleLibraries[n];
                if (assemblyName == info.name)
                {
                    return info;
                }
            }

            for (int n = 0; n < _gameLibraries.Count; ++n)
            {
                ExtensionModuleInfo info = _gameLibraries[n];
                if (assemblyName == info.name)
                {
                    return info;
                }
            }

            return null;
        }

        #region 动态模块库注册绑定相关接口函数

        /// <summary>
        /// 自动加载配置的模块信息
        /// </summary>
        /// <param name="moduleObjects">信息列表</param>
        public void AutoloadConfigurationModuleObjects(IReadOnlyList<Serialization.SerializedModuleObject> moduleObjects)
        {
            for (int n = 0; null != moduleObjects && n < moduleObjects.Count; ++n)
            {
                Serialization.SerializedModuleObject libraryObject = moduleObjects[n];
                RegisterModuleInfo(libraryObject.order, libraryObject.name, libraryObject.tags);
            }
        }

        /// <summary>
        /// 注册新的模块信息
        /// </summary>
        /// <param name="order">模块标签序号</param>
        /// <param name="name">模块标签名称</param>
        /// <param name="tags">模块标签</param>
        public void RegisterModuleInfo(int order, string name, IList<string> tags)
        {
            ExtensionModuleTag tag = ExtensionModuleTag.Unknown;

            for (int n = 0; null != tags && n < tags.Count; ++n)
            {
                tag |= Enum.Parse<ExtensionModuleTag>(tags[n]);
            }

            ExtensionModuleTag tagForPackType = (ExtensionModuleTag) Enum.ToObject(typeof(ExtensionModuleTag), (int) tag & 0x0f);
            IList<ExtensionModuleInfo> container;

            switch (tagForPackType)
            {
                case ExtensionModuleTag.Core:
                    throw new ArgumentException("Cannot register core library.");
                case ExtensionModuleTag.Module:
                    container = _moduleLibraries;
                    break;
                case ExtensionModuleTag.Game:
                    container = _gameLibraries;
                    break;
                default:
                    throw new ArgumentException("Invalid module tag.");
            }

            // 重复注册检查
            for (int n = 0; n < container.Count; ++n)
            {
                ExtensionModuleInfo tmp = container[n];
                if (tmp.order == order || tmp.name == name)
                {
                    throw new ArgumentException("Module name is already registered.");
                }
            }

            ExtensionModuleInfo info = new () { order = order, name = name, tags = tag };
            container.Add(info);
        }

        /// <summary>
        /// 注销所有的程序库信息
        /// </summary>
        public void UnregisterAllModuleInfos()
        {
            _moduleLibraries.Clear();
            _gameLibraries.Clear();
        }

        /// <summary>
        /// 自动加载配置的预编译库名称
        /// </summary>
        /// <param name="libraryNames">库名称列表</param>
        public void AutoloadConfigurationAotFileNames(IReadOnlyList<string> libraryNames)
        {
            for (int n = 0; null != libraryNames && n < libraryNames.Count; ++n)
            {
                RegisterAotFileName(libraryNames[n]);
            }
        }

        /// <summary>
        /// 注册新的预编译库名称
        /// </summary>
        /// <param name="name">预编译库名称</param>
        public void RegisterAotFileName(string name)
        {
            if (_aotLibraries.Contains(name))
            {
                throw new ArgumentException("AOT file name is already registered.");
            }

            _aotLibraries.Add(name);
        }

        /// <summary>
        /// 注销所有的预编译库名称
        /// </summary>
        public void UnregisterAllAotFileNames()
        {
            _aotLibraries.Clear();
        }

        #endregion
    }
}

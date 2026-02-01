/// -------------------------------------------------------------------------------
/// Copyright (C) 2026, Hainan Yuanyou Information Technology Co., Ltd. Guangzhou Branch
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NovaFramework.Serialization
{
    /// <summary>
    /// 环境上下文的基础配置，用于应用启动的初始环境部署
    /// </summary>
    // [CreateAssetMenu(fileName = "EnvironmentConfigures", menuName = "Nova Framework/Environment Configures")] // 创建后可以不再显示在右键菜单
    public sealed class EnvironmentConfigures : ScriptableObject
    {
        // ----------------------------------------------------------------------------------------------------
        [Header("变量配置")]

        public List<SerializedVariableObject> variables = new();

        // ----------------------------------------------------------------------------------------------------
        [Header("模组配置")]

        public List<SerializedLibraryObject> modules = new();

        // ----------------------------------------------------------------------------------------------------
        [Header("AOT配置")]

        public List<string> aots = new();

        /// <summary>
        /// EnvironmentConfigures实例
        /// </summary>
        public static EnvironmentConfigures Instance
        {
            get
            {
                EnvironmentConfigures settings = Resources.Load<EnvironmentConfigures>(nameof(EnvironmentConfigures));
                if (settings == null)
                {
                    settings = CreateInstance<EnvironmentConfigures>();
                    Logger.Error("Could not found any EnvironmentConfigures assets, please create one instance in resources directory.");
                }

                return settings;
            }
        }
    }
}

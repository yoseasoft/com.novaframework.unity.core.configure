/// -------------------------------------------------------------------------------
/// Copyright (C) 2024 - 2025, Hurley, Independent Studio.
/// Copyright (C) 2025, Hainan Yuanyou Information Technology Co., Ltd. Guangzhou Branch
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

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NovaFramework
{
    /// <summary>
    /// 环境变量定义类，应用于整个项目的本地变量配置管理
    /// </summary>
    internal sealed class EnvironmentVariables : Singleton<EnvironmentVariables>
    {
        readonly IDictionary<string, string> _variables = new Dictionary<string, string>();

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
                AutoloadConfigurationVariableObjects(environmentConfigures.variables);
            }
#endif
        }

        /// <summary>
        /// 动态库清理回调接口
        /// </summary>
        protected override sealed void OnCleanup()
        {
            _variables.Clear();
        }

        /// <summary>
        /// 自动加载配置的变量信息
        /// </summary>
        /// <param name="variableObjects">信息列表</param>
        public void AutoloadConfigurationVariableObjects(IReadOnlyList<Serialization.SerializedVariableObject> variableObjects)
        {
            for (int n = 0; null != variableObjects && n < variableObjects.Count; ++n)
            {
                Serialization.SerializedVariableObject variableObject = variableObjects[n];
                SetValue(variableObject.key, variableObject.value);
            }
        }

        /// <summary>
        /// 设置环境变量
        /// </summary>
        /// <param name="key">变量键</param>
        /// <param name="value">变量值</param>
        public void SetValue(string key, string value)
        {
            if (_variables.ContainsKey(key))
            {
                Logger.Warn("当前系统环境变量中已存在给定的键“{0}”，重复设置将覆盖旧值！", key);
                _variables.Remove(key);
            }

            _variables.Add(key, value);
        }

        /// <summary>
        /// 设置环境变量
        /// </summary>
        /// <param name="vars">字典对象</param>
        public void SetValue(IReadOnlyDictionary<string, string> vars)
        {
            foreach (KeyValuePair<string, string> pair in vars)
            {
                SetValue(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// 获取环境变量
        /// </summary>
        /// <param name="key">变量键</param>
        /// <returns>返回环境变量的值</returns>
        public string GetValue(string key)
        {
            if (_variables.TryGetValue(key, out string value))
                return value;

            return null;
        }
    }
}

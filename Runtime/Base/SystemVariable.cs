/// -------------------------------------------------------------------------------
/// CoreEngine Framework
///
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

namespace CoreEngine
{
    /// <summary>
    /// 系统变量定义类，应用于整个项目的环境变量配置管理
    /// </summary>
    internal static class SystemVariables
    {
        static readonly IDictionary<string, string> _variables = new Dictionary<string, string>()
        {
            {"ORIGINAL_RESOURCE_PATH", "Assets/_Resources"},
            {"SOURCE_CODE_PATH", "Assets/Sources"},
            {"SCRIPT_FILE_PATH", "Assets/LuaScripts"},
            {"AOT_LIBRARY_PATH", "Assets/_Resources/Aot"},
            {"LINK_LIBRARY_PATH", "Assets/_Resources/Code"},
            {"CONTEXT_PATH", "Assets/_Resources/Context"},
            {"CONFIG_PATH", "Assets/_Resources/Config"},
            {"GUI_PATH", "Assets/_Resources/Gui/"},
        };

        /// <summary>
        /// 设置系统变量
        /// </summary>
        /// <param name="key">变量键</param>
        /// <param name="value">变量值</param>
        public static void SetValue(string key, string value)
        {
            if (_variables.ContainsKey(key))
            {
                UnityEngine.Debug.LogWarningFormat("当前系统环境变量中已存在给定的键“{0}”，重复设置将覆盖旧值！", key);
                _variables.Remove(key);
            }

            _variables.Add(key, value);
        }

        /// <summary>
        /// 获取系统变量
        /// </summary>
        /// <param name="key">变量键</param>
        /// <returns>返回系统变量的值</returns>
        public static string GetValue(string key)
        {
            if (_variables.TryGetValue(key, out string value))
                return value;

            return null;
        }
    }
}

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
using System.IO;
using System.Text;
using System.Runtime.CompilerServices;

namespace NovaFramework
{
    /// <summary>
    /// 系统环境路径管理类，用于对整个项目内的路径访问提供统一的接口函数
    /// </summary>
    internal static class EnvironmentPath
    {
        static readonly IDictionary<ResourcePathType, string> _cachePaths = new Dictionary<ResourcePathType, string>();

        /// <summary>
        /// 通过指定的路径类型，获取路径值
        /// </summary>
        /// <param name="type">路径类型</param>
        /// <returns>返回路径值</returns>
        public static string GetPath(ResourcePathType type)
        {
            if (_cachePaths.TryGetValue(type, out string path))
            {
                return path;
            }

            string name = ConvertPathTypeToName(type.ToString());
            path = GetPath(name);

            if (false == string.IsNullOrEmpty(path))
                _cachePaths.Add(type, path);

            return path;
        }

        /// <summary>
        /// 通过指定的路径名称，获取路径值
        /// </summary>
        /// <param name="key">路径名称</param>
        /// <returns>返回路径值</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetPath(string key)
        {
            return EnvironmentVariables.GetValue(key);
        }

        /// <summary>
        /// 通过指定的路径类型，获取路径值
        /// </summary>
        /// <param name="type">路径类型</param>
        /// <param name="paths">路径其它部分</param>
        /// <returns>返回路径值</returns>
        public static string GetFilePath(ResourcePathType type, params string[] paths)
        {
            string path = GetPath(type);
            if (string.IsNullOrEmpty(path))
                return path;

            return Path.Combine(path, Path.Combine(paths)).Replace('\\', '/');
        }

        /// <summary>
        /// 通过指定的路径名称，获取路径值
        /// </summary>
        /// <param name="key">路径名称</param>
        /// <param name="paths">路径其它部分</param>
        /// <returns>返回路径值</returns>
        public static string GetFilePath(string key, params string[] paths)
        {
            string path = GetPath(key);
            if (string.IsNullOrEmpty(path))
                return path;

            return Path.Combine(path, Path.Combine(paths)).Replace('\\', '/');
        }

        /// <summary>
        /// 将大小写混合的成员名称转换为大写加下划线形式的名称
        /// 例如：“CoreEngineClassType”格式的名称将转换为“CORE_ENGINE_CLASS_TYPE”
        /// </summary>
        /// <param name="memberName">成员名称</param>
        /// <returns>返回转换后的成员名称</returns>
        private static string ConvertPathTypeToName(string memberName)
        {
            if (string.IsNullOrEmpty(memberName))
            {
                return memberName;
            }

            StringBuilder sb = new StringBuilder();
            int start = 0;
            int pos = 1;

            string sub_name;
            do
            {
                // 每个大写字符判定为一个新的单词的开始
                // 从这里截取出上一个完整的单词
                // 单词之间用‘_’进行连接
                if (System.Char.IsUpper(memberName[pos]))
                {
                    sub_name = memberName.Substring(start, pos - start);
                    if (sb.Length > 0) sb.Append('_');
                    sb.Append(sub_name.ToUpper());

                    start = pos;
                }

                ++pos;
            } while (pos < memberName.Length);

            // 处理最后一个单词
            sub_name = memberName.Substring(start, pos - start);
            if (sb.Length > 0) sb.Append('_');
            sb.Append(sub_name.ToUpper());

            return sb.ToString();
        }
    }
}

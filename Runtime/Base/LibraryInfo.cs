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

namespace CoreEngine
{
    /// <summary>
    /// 框架加载的程序库的信息对象类，用于描述程序库的基础属性
    /// </summary>
    public class LibraryInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int order { get; internal set; }

        /// <summary>
        /// 程序库名称
        /// </summary>
        public string name { get; internal set; }

        /// <summary>
        /// 源码路径
        /// </summary>
        public string source_path { get; internal set; }

        /// <summary>
        /// 标签集合
        /// </summary>
        public LibraryTag tags { get; internal set; }

        /// <summary>
        /// 检测当前程序库的标签中是否存在指定标签类型
        /// </summary>
        /// <param name="tag">标签类型</param>
        /// <returns>若存在指定标签则返回true，否则返回false</returns>
        public bool IsContainsTag(LibraryTag tag)
        {
            if ((tags & tag) == tag)
            {
                return true;
            }

            return false;
        }
    }
}

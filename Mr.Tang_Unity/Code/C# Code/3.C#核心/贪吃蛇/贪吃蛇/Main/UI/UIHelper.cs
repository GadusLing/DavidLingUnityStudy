using System;

namespace 贪吃蛇.Main.UI
{
    /// <summary>
    /// UI辅助工具类 - 提供UI相关的通用工具方法
    /// 职责：处理UI渲染中的常见计算问题
    /// 设计模式：工具类模式（静态方法集合）
    /// </summary>
    public static class UIHelper
    {
        /// <summary>
        /// 计算文本显示宽度（支持中文字符）
        /// 算法：中文字符宽度为2，英文字符宽度为1
        /// 用途：实现文本的精确居中对齐
        /// </summary>
        /// <param name="text">要计算宽度的文本</param>
        /// <returns>文本的显示宽度（字符单位）</returns>
        public static int GetDisplayWidth(string text)
        {
            int width = 0;
            foreach (char c in text)
            {
                // 判断是否为中文字符（Unicode范围：4E00-9FFF）
                if (c >= 0x4E00 && c <= 0x9FFF) // 中文字符
                    width += 2;
                else                             // 英文字符
                    width += 1;
            }
            return width;
        }
    }
}
using System;

namespace 贪吃蛇.Main.UI
{
    /// <summary>
    /// 文本内容UI元素 - 用于显示多行文本内容
    /// 职责：
    /// 1. 支持多行文本的居中显示
    /// 2. 自动计算所需的垂直空间
    /// 3. 提供信息展示功能
    /// 特点：
    /// - 支持可变参数构造（可传入任意行数的文本）
    /// - 每行文本独立居中对齐
    /// - 纯展示型元素，不支持交互
    /// 用途：制作人员名单、游戏说明、多行提示信息等
    /// 实现：IUIElement接口，支持多态渲染
    /// </summary>
    public class TextContentElement : IUIElement
    {
        #region 私有字段
        /// <summary>
        /// 文本行数组 - 存储所有要显示的文本行
        /// 每个元素代表一行文本内容
        /// </summary>
        private string[] _lines;
        #endregion

        #region 构造函数
        /// <summary>
        /// 文本内容元素构造函数
        /// 使用可变参数，支持传入任意数量的文本行
        /// </summary>
        /// <param name="lines">要显示的文本行（可变参数）</param>
        /// <example>
        /// 使用示例：
        /// new TextContentElement("第一行", "第二行", "第三行");
        /// new TextContentElement("策划:David", "程序:David");
        /// </example>
        public TextContentElement(params string[] lines)
        {
            _lines = lines;
        }
        #endregion

        #region IUIElement接口实现
        /// <summary>
        /// 渲染多行文本内容到控制台
        /// 核心功能：
        /// 1. 设置白色文本颜色（固定颜色，不受选中状态影响）
        /// 2. 逐行渲染，每行独立计算居中位置
        /// 3. 自动处理垂直位置递增
        /// 算法：
        /// - 每行文本都基于centerX进行居中计算
        /// - Y坐标从startY开始，每行递增1
        /// - 使用UIHelper.GetDisplayWidth确保中英文混合文本的精确居中
        /// </summary>
        /// <param name="centerX">屏幕中心X坐标，用于每行的居中对齐</param>
        /// <param name="startY">第一行文本的Y坐标位置</param>
        /// <param name="isSelected">选中状态（文本内容不支持选中，此参数被忽略）</param>
        public void Render(int centerX, int startY, bool isSelected)
        {
            // 文本内容固定使用白色，不受选中状态影响
            // 保持一致的视觉效果
            Console.ForegroundColor = ConsoleColor.White;
            
            // 逐行渲染文本内容
            for (int i = 0; i < _lines.Length; i++)
            {
                // 为每行独立计算居中位置
                // centerX - (文本显示宽度 / 2) = 文本起始X坐标
                // startY + i = 当前行的Y坐标
                Console.SetCursorPosition(centerX - UIHelper.GetDisplayWidth(_lines[i]) / 2, startY + i);
                
                // 输出当前行文本
                Console.WriteLine(_lines[i]);
            }
        }

        /// <summary>
        /// 获取文本内容的总高度
        /// 高度计算：文本行数 = 占用的显示行数
        /// 用于后续UI元素的位置计算
        /// </summary>
        /// <returns>文本内容的总行数</returns>
        public int GetHeight() => _lines.Length;
        #endregion
    }
}
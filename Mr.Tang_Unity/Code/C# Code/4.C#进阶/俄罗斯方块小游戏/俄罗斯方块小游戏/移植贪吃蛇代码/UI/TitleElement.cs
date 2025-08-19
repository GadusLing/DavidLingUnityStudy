using System;

namespace 俄罗斯方块.Main.UI
{
    /// <summary>
    /// 标题UI元素 - 用于显示场景标题或重要文本
    /// 职责：
    /// 1. 居中显示标题文本
    /// 2. 使用固定的白色文本颜色
    /// 3. 提供良好的视觉层次感
    /// 特点：纯展示型元素，不支持交互
    /// 用途：场景标题、重要信息展示
    /// </summary>
    public class TitleElement : IUIElement
    {
        #region 私有字段
        /// <summary>
        /// 标题文本内容 - 要显示的标题文字
        /// </summary>
        private string _title;
        #endregion
        
        #region 构造函数
        /// <summary>
        /// 标题元素构造函数
        /// </summary>
        /// <param name="title">要显示的标题文本</param>
        public TitleElement(string title)
        {
            _title = title;
        }
        #endregion

        #region IUIElement接口实现
        /// <summary>
        /// 渲染标题到控制台
        /// 核心功能：
        /// 1. 设置白色文本颜色（标题固定为白色）
        /// 2. 计算居中位置
        /// 3. 输出标题文本
        /// 注意：标题不响应选中状态，isSelected参数被忽略
        /// </summary>
        /// <param name="centerX">屏幕中心X坐标，用于居中对齐</param>
        /// <param name="startY">标题的Y坐标位置</param>
        /// <param name="isSelected">选中状态（标题不支持选中，此参数被忽略）</param>
        public void Render(int centerX, int startY, bool isSelected)
        {
            // 标题固定使用白色，不受选中状态影响
            Console.ForegroundColor = ConsoleColor.White;
            
            // 计算居中位置：使用UIHelper确保中英文混合标题的精确居中
            Console.SetCursorPosition(centerX - UIHelper.GetDisplayWidth(_title) / 2, startY);
            
            // 输出标题文本
            Console.WriteLine(_title);
        }

        /// <summary>
        /// 获取标题高度
        /// 标题固定占用1行高度
        /// </summary>
        /// <returns>标题高度（1行）</returns>
        public int GetHeight() => 1;
        #endregion
    }
}
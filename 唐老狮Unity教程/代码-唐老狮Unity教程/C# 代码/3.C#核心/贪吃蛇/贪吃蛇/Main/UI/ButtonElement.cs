using System;

namespace 贪吃蛇.Main.UI
{
    /// <summary>
    /// 按钮UI元素 - 可交互的按钮组件
    /// 职责：
    /// 1. 显示按钮文本（支持选中状态的颜色变化）
    /// 2. 执行用户定义的操作（通过Action委托）
    /// 3. 支持键盘导航和交互
    /// 特点：唯一可执行操作的UI元素
    /// 实现：IUIElement接口，支持多态渲染
    /// </summary>
    public class ButtonElement : IUIElement
    {
        #region 私有字段
        /// <summary>
        /// 按钮显示文本 - 用户看到的按钮标签
        /// </summary>
        private string _text;
        
        /// <summary>
        /// 按钮点击时执行的操作 - 使用委托实现回调机制
        /// 支持任意无参数的操作，如场景切换、游戏退出等
        /// </summary>
        private Action _action;
        #endregion

        #region 构造函数
        /// <summary>
        /// 按钮元素构造函数
        /// </summary>
        /// <param name="text">按钮显示的文本内容</param>
        /// <param name="action">按钮被激活时执行的操作（委托）</param>
        public ButtonElement(string text, Action action)
        {
            _text = text;
            _action = action;
        }
        #endregion

        #region IUIElement接口实现
        /// <summary>
        /// 渲染按钮到控制台
        /// 核心功能：
        /// 1. 根据选中状态设置文本颜色（选中=红色，未选中=白色）
        /// 2. 计算居中位置并设置光标坐标
        /// 3. 输出按钮文本
        /// </summary>
        /// <param name="centerX">屏幕中心X坐标，用于居中对齐</param>
        /// <param name="startY">按钮的Y坐标位置</param>
        /// <param name="isSelected">是否为当前选中的按钮</param>
        public void Render(int centerX, int startY, bool isSelected)
        {
            // 设置文本颜色：选中状态为红色，普通状态为白色
            Console.ForegroundColor = isSelected ? ConsoleColor.Red : ConsoleColor.White;
            
            // 计算居中位置：中心X坐标减去文本宽度的一半
            // 使用UIHelper.GetDisplayWidth确保中英文混合文本的精确居中
            Console.SetCursorPosition(centerX - UIHelper.GetDisplayWidth(_text) / 2, startY);
            
            // 输出按钮文本
            Console.WriteLine(_text);
        }

        /// <summary>
        /// 获取按钮高度
        /// 按钮固定占用1行高度
        /// </summary>
        /// <returns>按钮高度（1行）</returns>
        public int GetHeight() => 1;
        #endregion

        #region 按钮特有功能
        /// <summary>
        /// 执行按钮操作 - 按钮被激活时调用
        /// 使用?.操作符进行空值检查，防止空引用异常
        /// 调用时机：用户按下Enter键且当前按钮被选中时
        /// </summary>
        public void Execute() => _action?.Invoke();
        #endregion
    }
}
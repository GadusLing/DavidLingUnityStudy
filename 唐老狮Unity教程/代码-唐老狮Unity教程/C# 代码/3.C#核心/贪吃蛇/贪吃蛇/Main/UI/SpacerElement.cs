using System;

namespace 贪吃蛇.Main.UI
{
    /// <summary>
    /// 空白间距UI元素 - 用于控制UI元素之间的间距
    /// 职责：
    /// 1. 在UI元素之间创建可控的空白空间
    /// 2. 提供灵活的布局控制能力
    /// 3. 解决固定间距布局的限制问题
    /// 特点：
    /// - 不渲染任何可见内容
    /// - 可自定义占用的行数
    /// - 最小高度为1行（防止无效间距）
    /// 设计意义：实现了"间距即组件"的设计理念
    /// </summary>
    public class SpacerElement : IUIElement
    {
        #region 私有字段
        /// <summary>
        /// 间距高度 - 该间距元素占用的行数
        /// 最小值为1，确保间距有效性
        /// </summary>
        private int _height;
        #endregion

        #region 构造函数
        /// <summary>
        /// 间距元素构造函数
        /// </summary>
        /// <param name="height">间距高度（行数），默认为1行，最小值为1</param>
        public SpacerElement(int height = 1)
        {
            // 确保间距至少为1行，防止无效间距
            _height = Math.Max(1, height);
        }
        #endregion

        #region IUIElement接口实现
        /// <summary>
        /// 渲染空白间距（实际上不渲染任何内容）
        /// 核心理念：通过"不渲染"来实现空白效果
        /// 间距的作用通过GetHeight()方法影响后续元素的位置实现
        /// </summary>
        /// <param name="centerX">中心X坐标（间距元素不使用）</param>
        /// <param name="startY">起始Y坐标（间距元素不使用）</param>
        /// <param name="isSelected">选中状态（间距元素不支持选中）</param>
        public void Render(int centerX, int startY, bool isSelected)
        {
            // 间距元素的核心特点：不渲染任何可见内容
            // 通过"空实现"达到创建空白空间的目的
        }

        /// <summary>
        /// 获取间距高度
        /// 返回构造时设置的行数，用于布局计算
        /// </summary>
        /// <returns>间距占用的行数</returns>
        public int GetHeight() => _height;
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 贪吃蛇.Main.UI;

namespace 贪吃蛇.Main
{
    /// <summary>
    /// 抽象场景基类 - 使用模板方法模式
    /// 职责：
    /// 1. 定义所有场景的通用行为框架
    /// 2. 管理UI元素的渲染和交互
    /// 3. 处理用户输入（键盘导航）
    /// 设计模式：模板方法模式 + 组合模式
    /// </summary>
    abstract class S_AbstractBaseScene : ISceneUpdate
    {
        #region 私有字段
        /// <summary>
        /// UI元素列表 - 存储场景中的所有UI元素（按显示顺序）
        /// 包括标题、按钮、文本、间距等所有类型的UI元素
        /// </summary>
        protected List<IUIElement> _uiElements = new List<IUIElement>();
        
        /// <summary>
        /// 按钮元素列表 - 单独存储可交互的按钮元素
        /// 用于键盘导航，只在按钮间切换选择状态
        /// </summary>
        protected List<ButtonElement> _buttons = new List<ButtonElement>();
        
        /// <summary>
        /// 当前选中的按钮索引 - 标识用户当前选中的按钮
        /// 对应_buttons列表中的索引位置
        /// </summary>
        protected int _selectedButtonIndex = 0;
        #endregion

        #region 模板方法模式实现
        /// <summary>
        /// 场景更新方法 - 模板方法模式的核心
        /// 定义了场景更新的标准流程：先渲染UI，再处理输入
        /// 子类无需重写此方法，只需实现InitializeUI即可
        /// </summary>
        public virtual void Update()
        {
            RenderUI();    // 第一步：渲染所有UI元素
            HandleInput(); // 第二步：处理用户输入
        }
        #endregion

        #region UI渲染系统
        /// <summary>
        /// 渲染UI系统 - 模板方法的具体步骤之一
        /// 负责将所有UI元素按顺序绘制到控制台上
        /// 核心逻辑：
        /// 1. 计算屏幕中心位置和起始Y坐标
        /// 2. 遍历所有UI元素进行渲染
        /// 3. 为按钮元素设置选中状态
        /// 4. 动态计算下一个元素的Y坐标
        /// </summary>
        private void RenderUI()
        {
            // 计算屏幕水平中心位置
            int centerX = Game.GlobalWidth / 2;
            
            // 设置UI起始位置为屏幕高度的30%处
            int currentY = (int)(Game.GlobalHeight * 0.3);

            // 遍历所有UI元素进行渲染
            for (int i = 0; i < _uiElements.Count; i++)
            {
                var element = _uiElements[i];
                bool isSelected = false;
                
                // 关键逻辑：判断当前元素是否为选中的按钮
                // 只有按钮类型的元素才可能被选中
                if (element is ButtonElement button)
                {
                    // 在按钮列表中查找当前按钮的索引
                    int buttonIndex = _buttons.IndexOf(button);
                    
                    // 比较索引是否匹配当前选中的按钮索引
                    isSelected = buttonIndex == _selectedButtonIndex;
                }

                // 调用元素的渲染方法，传入位置和选中状态
                element.Render(centerX, currentY, isSelected);
                
                // 更新下一个元素的Y坐标（累加当前元素的高度）
                currentY += element.GetHeight();
            }
        }
        #endregion

        #region 输入处理系统
        /// <summary>
        /// 处理用户输入 - 模板方法的具体步骤之一
        /// 实现键盘导航功能：
        /// - ↑键：向上选择按钮
        /// - ↓键：向下选择按钮  
        /// - Enter键：执行当前选中按钮的操作
        /// </summary>
        private void HandleInput()
        {
            // 读取用户按键（true表示不显示按键字符）
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.UpArrow:
                    // 向上导航：索引减1，但不能小于0
                    _selectedButtonIndex = Math.Max(0, _selectedButtonIndex - 1);
                    break;
                    
                case ConsoleKey.DownArrow:
                    // 向下导航：索引加1，但不能超过按钮总数-1
                    _selectedButtonIndex = Math.Min(_buttons.Count - 1, _selectedButtonIndex + 1);
                    break;
                    
                case ConsoleKey.Enter:
                    // 执行当前选中按钮的操作
                    // 先进行边界检查，确保索引有效
                    if (_selectedButtonIndex >= 0 && _selectedButtonIndex < _buttons.Count)
                    {
                        _buttons[_selectedButtonIndex].Execute();
                    }
                    break;
            }
        }
        #endregion

        #region UI元素管理
        /// <summary>
        /// 添加UI元素到场景中
        /// 双重管理策略：
        /// 1. 所有元素都添加到_uiElements用于渲染
        /// 2. 按钮元素额外添加到_buttons用于导航
        /// </summary>
        /// <param name="element">要添加的UI元素</param>
        protected void AddUIElement(IUIElement element)
        {
            // 添加到总的UI元素列表
            _uiElements.Add(element);
            
            // 如果是按钮元素，同时添加到按钮列表
            if (element is ButtonElement button)
            {
                _buttons.Add(button);
            }
        }

        /// <summary>
        /// 添加间距的便捷方法
        /// 内部创建SpacerElement实现间距控制
        /// </summary>
        /// <param name="height">间距高度（行数），默认为1行</param>
        protected void AddSpacer(int height = 1)
        {
            AddUIElement(new SpacerElement(height));
        }
        #endregion

        #region 抽象方法
        /// <summary>
        /// 初始化UI元素的抽象方法
        /// 子类必须实现此方法来定义具体的UI布局
        /// 通常在此方法中调用AddUIElement来构建界面
        /// </summary>
        protected abstract void InitializeUI();
        #endregion

        #region 构造函数
        /// <summary>
        /// 抽象基类构造函数
        /// 自动调用InitializeUI方法，确保子类在创建时完成UI初始化
        /// 体现了模板方法模式的"框架调用子类实现"的特点
        /// </summary>
        protected S_AbstractBaseScene()
        {
            InitializeUI();
        }
        #endregion
    }
}

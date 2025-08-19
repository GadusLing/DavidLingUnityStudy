using System;
using System.Threading;

namespace 多线程作业
{
    /// <summary>
    /// 多线程控制台游戏程序
    /// 设计目标：演示多线程编程的基本概念，包括线程同步、资源共享和并发控制
    /// 游戏功能：一个绿色方块在控制台窗口中移动，用户可以通过键盘控制方向
    /// </summary>
    internal class Program
    {
        #region 全局变量定义和说明
        
        /// <summary>
        /// 方块的当前坐标位置
        /// x: 水平位置（列），从左到右递增
        /// y: 垂直位置（行），从上到下递增
        /// 初始值设为(10,10)，确保方块不会出现在屏幕边缘
        /// </summary>
        private static int x = 10, y = 10;
        
        /// <summary>
        /// 方块的移动方向向量
        /// dx: 水平方向增量，-1表示左移，0表示不动，1表示右移
        /// dy: 垂直方向增量，-1表示上移，0表示不动，1表示下移
        /// 初始值(1,0)表示方块初始向右移动
        /// </summary>
        private static int dx = 1, dy = 0;
        
        /// <summary>
        /// 线程同步锁对象 - 多线程编程的核心概念
        /// 作用：确保同一时间只有一个线程能访问共享资源（x,y,dx,dy等变量）
        /// 原理：当一个线程获得锁时，其他线程必须等待，避免数据竞争和不一致状态
        /// readonly确保锁对象本身不会被重新赋值，保证线程安全
        /// </summary>
        private static readonly object lockObj = new object();
        
        /// <summary>
        /// 程序运行状态标志
        /// 作用：控制主循环和输入线程的生命周期
        /// 当用户按ESC键时，此值会被设为false，所有线程将优雅退出
        /// </summary>
        private static bool running = true;
        
        #endregion

        /// <summary>
        /// 程序入口点 - 多线程游戏的初始化和主循环
        /// 设计思路：
        /// 1. 初始化控制台环境
        /// 2. 创建并启动输入处理线程（后台线程）
        /// 3. 主线程负责游戏逻辑循环（移动和绘制）
        /// 4. 两个线程并行工作，通过锁机制保证数据一致性
        /// </summary>
        /// <param name="args">命令行参数（本程序中未使用）</param>
        static void Main(string[] args)
        {
            #region 控制台环境初始化
            
            // 清屏，为游戏提供干净的显示环境
            Console.Clear();
            
            // 隐藏光标，避免光标闪烁影响游戏体验
            // 在游戏中，我们手动控制所有显示内容，不需要系统光标
            Console.CursorVisible = false;
            
            // 显示游戏说明，告诉用户如何操作
            Console.WriteLine("多线程控制游戏 - 使用 WASD 控制方向，ESC 退出");
            
            #endregion

            #region 输入处理线程创建和启动
            
            // 创建新线程专门处理用户输入
            // 为什么需要单独线程：因为Console.ReadKey()是阻塞操作，
            // 如果在主线程中处理输入，会导致游戏画面卡顿
            Thread inputThread = new Thread(InputHandler)
            {
                // 设为后台线程：当主线程结束时，后台线程会自动终止
                // 这确保程序能够正常退出，不会因为输入线程而挂起
                IsBackground = true
            };
            
            // 启动输入线程，开始监听键盘输入
            inputThread.Start();
            
            #endregion
            
            #region 主游戏循环
            
            // 主线程的游戏循环：负责游戏逻辑的核心部分
            // 设计原理：将游戏分解为重复的"移动-绘制"循环
            while (running)
            {
                // 步骤1：根据当前方向移动方块
                MoveBlock();
                
                // 步骤2：在新位置绘制方块
                DrawBlock();
                
                // 步骤3：控制游戏速度
                // 200毫秒的延时使游戏以合适的速度运行（每秒5帧）
                // 太快会导致方块移动过快，太慢会感觉迟钝
                Thread.Sleep(200);
            }
            
            #endregion
            
            // 游戏结束时的提示信息
            Console.WriteLine("\n游戏结束！");
        }
        
        /// <summary>
        /// 移动方块的核心逻辑
        /// 功能说明：
        /// 1. 清除方块在当前位置的显示
        /// 2. 根据方向向量更新方块坐标
        /// 3. 检测边界碰撞并处理反弹
        /// 4. 确保方块始终在有效范围内
        /// 
        /// 设计原理：
        /// - 使用锁确保移动操作的原子性（不会被其他线程中断）
        /// - 先清除再移动的策略避免屏幕残影
        /// - 边界反弹让游戏更有趣，方块不会消失在屏幕外
        /// </summary>
        private static void MoveBlock()
        {
            // 获取线程锁，确保移动操作不被中断
            // 这防止了输入线程在移动过程中修改方向，避免显示错乱
            lock (lockObj)
            {
                #region 清除当前位置的方块显示
                
                // 将光标移动到方块当前位置
                Console.SetCursorPosition(x, y);
                
                // 用两个空格覆盖原来的方块字符
                // 使用两个空格是因为中文字符"■"占用两个字符宽度
                Console.Write("  ");
                
                #endregion
                
                #region 更新方块位置
                
                // 根据方向向量更新坐标
                // 这是向量运动的基本原理：新位置 = 当前位置 + 方向向量
                x += dx;  // 水平方向移动
                y += dy;  // 垂直方向移动
                
                #endregion
                
                #region 水平边界检测和反弹处理
                
                // 检测是否碰到左右边界
                // x <= 0: 碰到左边界
                // x >= Console.WindowWidth - 1: 碰到右边界（减1是因为坐标从0开始）
                if (x <= 0 || x >= Console.WindowWidth - 1)
                {
                    // 反转水平方向：左变右，右变左
                    dx = -dx;
                    
                    // 将方块位置拉回到有效范围内
                    // Math.Max(1, ...): 确保不小于1（避免贴边）
                    // Math.Min(..., Console.WindowWidth - 2): 确保不超出右边界
                    x = Math.Max(1, Math.Min(x, Console.WindowWidth - 2));
                }
                
                #endregion
                
                #region 垂直边界检测和反弹处理
                
                // 检测是否碰到上下边界
                // y <= 2: 碰到上边界（为游戏信息显示预留前两行）
                // y >= Console.WindowHeight - 1: 碰到下边界
                if (y <= 2 || y >= Console.WindowHeight - 1)
                {
                    // 反转垂直方向：上变下，下变上
                    dy = -dy;
                    
                    // 将方块位置拉回到有效范围内
                    // Math.Max(3, ...): 确保不覆盖游戏信息显示区域
                    // Math.Min(..., Console.WindowHeight - 2): 确保不超出下边界
                    y = Math.Max(3, Math.Min(y, Console.WindowHeight - 2));
                }
                
                #endregion
            }
        }
        
        /// <summary>
        /// 绘制方块和游戏信息的显示逻辑
        /// 功能说明：
        /// 1. 在新位置绘制绿色方块
        /// 2. 显示当前位置和方向信息
        /// 3. 重置控制台颜色避免影响其他输出
        /// 
        /// 设计原理：
        /// - 使用锁确保绘制的完整性
        /// - 分离方块绘制和信息显示，便于维护
        /// - 使用颜色增强视觉效果
        /// </summary>
        private static void DrawBlock()
        {
            // 获取线程锁，确保绘制操作的完整性
            // 防止在绘制过程中被移动线程或输入线程中断
            lock (lockObj)
            {
                #region 绘制方块
                
                // 将光标移动到方块的新位置
                Console.SetCursorPosition(x, y);
                
                // 设置方块颜色为绿色，增强视觉效果
                Console.ForegroundColor = ConsoleColor.Green;
                
                // 绘制方块字符
                // 使用"■"字符作为方块，这是一个实心方形，视觉效果好
                Console.Write("■");
                
                // 重置控制台颜色为默认值
                // 这是良好的编程习惯，避免影响后续的文本输出
                Console.ResetColor();
                
                #endregion
                
                #region 显示游戏状态信息
                
                // 将光标移动到屏幕第二行开始位置，显示游戏信息
                Console.SetCursorPosition(0, 1);
                
                // 格式化显示当前位置和方向
                // {x:D2}, {y:D2}: 使用两位数字格式，不足两位时前面补0，保持对齐
                // 末尾的空格用于清除之前可能残留的字符
                Console.Write($"位置: ({x:D2}, {y:D2}) 方向: {GetDirectionText()}    ");
                
                #endregion
            }
        }
        
        /// <summary>
        /// 输入处理线程的主函数
        /// 功能说明：
        /// 1. 持续监听键盘输入（非阻塞检测）
        /// 2. 根据用户输入改变方块移动方向
        /// 3. 处理退出命令
        /// 4. 防止反向移动（避免方块"自杀"）
        /// 
        /// 设计原理：
        /// - 独立线程避免阻塞主游戏循环
        /// - 使用KeyAvailable进行非阻塞检测
        /// - 锁机制确保方向改变的线程安全
        /// - 反向移动检测增强游戏体验
        /// </summary>
        private static void InputHandler()
        {
            // 输入处理循环：只要游戏在运行就持续监听
            while (running)
            {
                #region 非阻塞键盘输入检测
                
                // Console.KeyAvailable: 检查是否有键盘输入等待处理
                // 这是非阻塞的，不会让线程暂停等待用户输入
                // 如果没有输入，线程会继续执行，不影响游戏流畅性
                if (Console.KeyAvailable)
                {
                    // 读取用户按键，true参数表示不在控制台显示按键字符
                    // 这避免了按键字符干扰游戏画面
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    
                    #region 线程安全的方向更新
                    
                    // 获取锁，确保方向更新的原子性
                    // 防止在更新dx,dy时被主线程的移动操作中断
                    lock (lockObj)
                    {
                        // 使用switch语句处理不同的按键
                        // 支持WASD和方向键两套控制方案，提高用户体验
                        switch (keyInfo.Key)
                        {
                            #region 向上移动 (W键或上方向键)
                            
                            case ConsoleKey.W:
                            case ConsoleKey.UpArrow:
                                // 防止反向移动检测：如果当前正在向下移动(dy=1)，则禁止向上
                                // 原因：如果允许反向移动，方块会立即"撞死"自己，游戏体验差
                                if (dy != 1)
                                {
                                    dx = 0;   // 停止水平移动
                                    dy = -1;  // 设置向上移动（y坐标减小）
                                }
                                break;
                                
                            #endregion
                                
                            #region 向下移动 (S键或下方向键)
                            
                            case ConsoleKey.S:
                            case ConsoleKey.DownArrow:
                                // 防止反向移动：如果当前正在向上移动(dy=-1)，则禁止向下
                                if (dy != -1)
                                {
                                    dx = 0;  // 停止水平移动
                                    dy = 1;  // 设置向下移动（y坐标增加）
                                }
                                break;
                                
                            #endregion
                                
                            #region 向左移动 (A键或左方向键)
                            
                            case ConsoleKey.A:
                            case ConsoleKey.LeftArrow:
                                // 防止反向移动：如果当前正在向右移动(dx=1)，则禁止向左
                                if (dx != 1)
                                {
                                    dx = -1; // 设置向左移动（x坐标减小）
                                    dy = 0;  // 停止垂直移动
                                }
                                break;
                                
                            #endregion
                                
                            #region 向右移动 (D键或右方向键)
                            
                            case ConsoleKey.D:
                            case ConsoleKey.RightArrow:
                                // 防止反向移动：如果当前正在向左移动(dx=-1)，则禁止向右
                                if (dx != -1)
                                {
                                    dx = 1; // 设置向右移动（x坐标增加）
                                    dy = 0; // 停止垂直移动
                                }
                                break;
                                
                            #endregion
                                
                            #region 退出游戏 (ESC键)
                            
                            case ConsoleKey.Escape:
                                // 设置运行标志为false，这会导致：
                                // 1. 主线程的while循环退出
                                // 2. 输入线程的while循环退出
                                // 3. 程序优雅地结束
                                running = false;
                                break;
                                
                            #endregion
                        }
                    }
                    
                    #endregion
                }
                
                #endregion
                
                #region CPU占用优化
                
                // 短暂休眠减少CPU占用
                // 50毫秒的延时确保输入响应足够快速（每秒检测20次）
                // 同时避免空循环消耗过多CPU资源
                // 这是多线程程序中常用的优化技巧
                Thread.Sleep(50);
                
                #endregion
            }
        }
        
        /// <summary>
        /// 获取当前移动方向的文本描述
        /// 功能说明：将数字化的方向向量转换为人类可读的文本
        /// 设计原理：使用C# 8.0的模式匹配特性，代码简洁且易读
        /// 
        /// 方向向量到文本的映射关系：
        /// (0, -1) → "↑ 上"  : y坐标减小，视觉上向上移动
        /// (0, 1)  → "↓ 下"  : y坐标增加，视觉上向下移动  
        /// (-1, 0) → "← 左"  : x坐标减小，视觉上向左移动
        /// (1, 0)  → "→ 右"  : x坐标增加，视觉上向右移动
        /// 其他情况 → "停止"  : 理论上不应该出现，但提供默认值确保程序健壮性
        /// </summary>
        /// <returns>当前方向的中文描述，包含箭头符号和文字说明</returns>
        private static string GetDirectionText()
        {
            // 使用元组模式匹配 (C# 8.0特性)
            // 根据dx和dy的组合值返回对应的方向描述
            // 这种写法比传统的if-else更简洁和直观
            return (dx, dy) switch
            {
                (0, -1) => "↑ 上",    // 向上：只有y方向为负
                (0, 1) => "↓ 下",     // 向下：只有y方向为正  
                (-1, 0) => "← 左",    // 向左：只有x方向为负
                (1, 0) => "→ 右",     // 向右：只有x方向为正
                _ => "停止"           // 默认情况：理论上dx和dy不应该同时为0或其他值
            };
        }
    }
}

using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace 控制台小游戏_飞行棋
{
    /// <summary>
    /// 场景枚举类型
    /// </summary>
    enum E_SceneType
    {
        /// <summary>
        /// 开始场景
        /// </summary>
        Begin,

        /// <summary>
        /// 游戏场景
        /// </summary>
        Game,

        /// <summary>
        /// 制作者场景
        /// </summary>
        Fabri

    }

    class Program
    {
        // 主舞台
        const int sceneWidth = 80;
        const int sceneHeight = 30;

        // 主函数
        static void Main(string[] args)
        {
            //隐藏闪烁的光标
            Console.CursorVisible = false;

            Console.SetWindowSize(sceneWidth, sceneHeight);
            Console.SetBufferSize(sceneWidth, sceneHeight);

            E_SceneType nowScene = E_SceneType.Begin;// 用枚举来代替数字

            // 游戏主体
            while (true)
            {
                switch (nowScene)
                {
                    //开始界面
                    case E_SceneType.Begin:
                        nowScene = BeginScene(nowScene);
                        break;

                    //游戏界面
                    case E_SceneType.Game:
                        nowScene = GameScene(nowScene);
                        break;

                    //制作者名单
                    case E_SceneType.Fabri:
                        nowScene = FabriScene(nowScene);
                        break;
                }
            }
        }

        /// <summary>
        /// 开始界面
        /// </summary>
        static E_SceneType BeginScene(E_SceneType nowScene)
        {
            Console.Clear();

            Console.SetCursorPosition(sceneWidth / 2 - 5, sceneHeight / 2 - 6);
            Console.Write("好运飞行棋");

            Console.SetCursorPosition(sceneWidth / 2 - 10, sceneHeight - 2);
            Console.Write("控制方式：WASD和回车");

            int nowSelIndex = 1; //1-开始游戏,2-制作者名单,3-退出游戏，默认红色高亮在开始游戏上

            while (true)// 负责开始界面阻塞接口之间的切换
            {
                bool isQuitWhile = false;//设置标识,决定是否退出"开始游戏"界面转到其他界面

                Console.SetCursorPosition(sceneWidth / 2 - 4, sceneHeight / 2 - 1);//设置光标位置
                Console.ForegroundColor = nowSelIndex == 1 ? ConsoleColor.Red : ConsoleColor.White;
                Console.Write("开始游戏");//显示UI按钮

                Console.SetCursorPosition(sceneWidth / 2 - 4, sceneHeight / 2 + 1);
                Console.ForegroundColor = nowSelIndex == 2 ? ConsoleColor.Red : ConsoleColor.White;
                Console.Write("作者名单");

                Console.SetCursorPosition(sceneWidth / 2 - 4, sceneHeight / 2 + 3);
                Console.ForegroundColor = nowSelIndex == 3 ? ConsoleColor.Red : ConsoleColor.White;
                Console.Write("退出游戏");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                        nowSelIndex = nowSelIndex == 1 ? 3 : nowSelIndex - 1;
                        break;

                    case ConsoleKey.S:
                        nowSelIndex = nowSelIndex == 3 ? 1 : nowSelIndex + 1;
                        break;
                    case ConsoleKey.Enter:
                        // 定义行为映射
                        Action[] actions =
                        {
                            null,
                            () => { nowScene = E_SceneType.Game; isQuitWhile = true; },
                            () => { nowScene = E_SceneType.Fabri; isQuitWhile = true; },
                            () => { Console.ForegroundColor = ConsoleColor.White; Environment.Exit(0); },
                        };
                        actions[nowSelIndex]();
                        break;
                }
                if (isQuitWhile) return nowScene;
            }
        }

        /// <summary>
        /// 游戏界面
        /// </summary>
        static E_SceneType GameScene(E_SceneType nowScene)
        {
            DrawGameSceInfo();
            Map map = new Map(8, 3, 120);
            map.Draw();

            Player player = new Player(1, E_PlayerType.Player);
            Player computer = new Player(0, E_PlayerType.Computer);
            DrawPlayer(player, computer, map);

            while (true)
            {
                // 玩家回合 - 需要按键
                PrintGameMessage("玩家回合，按任意键掷骰子...");
                SafeWaitForKey();
                if (ThrowDice(ref player, ref computer, map, true))
                {
                    PrintGameMessage("玩家获胜！按任意键返回...");
                    SafeWaitForKey();
                    return E_SceneType.Fabri;
                }
                map.Draw();
                DrawPlayer(player, computer, map);

                // 电脑回合 - 自动进行
                PrintGameMessage("电脑回合，掷骰子...");
                Thread.Sleep(1000); // 给玩家1秒时间看清电脑的行动
                if (ThrowDice(ref computer, ref player, map, false))
                {
                    PrintGameMessage("电脑获胜！按任意键返回...");
                    SafeWaitForKey();
                    return E_SceneType.Fabri;
                }
                map.Draw();
                DrawPlayer(player, computer, map);
            }
        }

        /// <summary>
        /// 制作者界面
        /// </summary>
        static E_SceneType FabriScene(E_SceneType nowScene)
        {
            Console.Clear();

            Console.SetCursorPosition(sceneWidth / 2 - 4, sceneHeight / 2 - 12);//设置光标位置
            Console.Write("THE END:");//结束标题

            Console.SetCursorPosition(sceneWidth / 2 - 4, sceneHeight / 2 - 10);//设置光标位置
            Console.Write("感谢游玩");//对应文字

            Console.SetCursorPosition(sceneWidth / 2 - 5, sceneHeight / 2 - 7);//设置光标位置
            Console.Write("制作者名单");

            Console.SetCursorPosition(sceneWidth / 2 - 5, sceneHeight / 2 - 5);//设置光标位置
            Console.Write("程序 David");

            Console.SetCursorPosition(sceneWidth / 2 - 5, sceneHeight / 2 - 4);//设置光标位置
            Console.Write("美术 David");

            Console.SetCursorPosition(sceneWidth / 2 - 5, sceneHeight / 2 - 3);//设置光标位置
            Console.Write("策划 David");

            Console.SetCursorPosition(sceneWidth / 2 - 9, sceneHeight / 2 - 2);//设置光标位置
            Console.Write("音乐 无 但还是David");

            Console.SetCursorPosition(sceneWidth / 2 - 17, sceneHeight / 2 + 1);//设置光标位置
            Console.Write("走到这里花了不少时间,也费了不少功夫");
            Console.SetCursorPosition(sceneWidth / 2 - 17, sceneHeight / 2 + 2);//设置光标位置
            Console.Write("虽然没有那么困难,但也不比想象中简单");
            Console.SetCursorPosition(sceneWidth / 2 - 17, sceneHeight / 2 + 3);//设置光标位置
            Console.Write("那么就继续吧,不要停下来做你想做的事");

            Console.SetCursorPosition(sceneWidth / 2 - 6, sceneHeight / 2 + 8);//设置光标位置
            Console.Write("回到开始界面");

            Console.SetCursorPosition(sceneWidth / 2 - 4, sceneHeight / 2 + 10);//设置光标位置
            Console.Write("退出游戏");

            int nowSelEndIndex = 0;//开始界面选择按钮的编号,0代表回开始界面,1为退出游戏按钮

            while (true)
            {
                bool isQuitWhile = false;//设置标识,决定是否退出"开始游戏"界面转到其他界面

                Console.SetCursorPosition(sceneWidth / 2 - 6, sceneHeight / 2 + 8);//设置光标位置
                Console.ForegroundColor = nowSelEndIndex == 0 ? ConsoleColor.Red : ConsoleColor.White;
                Console.Write("回到开始界面");//显示UI按钮

                Console.SetCursorPosition(sceneWidth / 2 - 4, sceneHeight / 2 + 10);//设置光标位置
                Console.ForegroundColor = nowSelEndIndex == 1 ? ConsoleColor.Red : ConsoleColor.White;
                Console.Write("退出游戏");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W:
                        nowSelEndIndex = nowSelEndIndex == 0 ? 1 : 0;
                        break;

                    case ConsoleKey.S:
                        nowSelEndIndex = nowSelEndIndex == 1 ? 0 : 1;
                        break;

                    case ConsoleKey.Enter:
                        Action[] actions =
                        {
                            () => { nowScene = E_SceneType.Begin; isQuitWhile = true; },
                            () => { Console.ForegroundColor = ConsoleColor.White; Environment.Exit(0); },
                        };
                        actions[nowSelEndIndex]();
                        break;
                }
                if (isQuitWhile) return nowScene;
            }
        }

        /// <summary>
        /// 按键前清空缓冲区，防止连续按键持续反应
        /// </summary>
        static void SafeWaitForKey()
        {
            // 清空键盘缓冲区
            while (Console.KeyAvailable)
                Console.ReadKey(true);

            // 等待新输入
            Console.ReadKey(true);
        }

        /// <summary>
        /// 绘制游戏运行时的固有信息
        /// </summary>
        static void DrawGameSceInfo()
        {
            Console.Clear();
            //布景,外围的墙壁
            Console.ForegroundColor = ConsoleColor.White;//墙壁颜色
            //横墙
            for (int i = 0; i < sceneWidth; i += 2)
            {
                //上方墙壁
                Console.SetCursorPosition(i, 0);
                Console.Write("▓");
                //下方墙壁
                Console.SetCursorPosition(i, sceneHeight - 1);
                Console.Write("▓");
                //中部分割墙壁
                Console.SetCursorPosition(i, sceneHeight - 8);
                Console.Write("▓");
            }
            //竖墙
            for (int i = 0; i < sceneHeight; i++)
            {
                //左
                Console.SetCursorPosition(0, i);
                Console.Write("▓");
                //右
                Console.SetCursorPosition(sceneWidth - 2, i);
                Console.Write("▓");
            }

            // 道具们
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(sceneWidth - 76, sceneHeight - 7);
            Console.Write("●: 炸弹，后退5格");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(sceneWidth - 56, sceneHeight - 7);
            Console.Write("||: 停止，暂停1回合");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(sceneWidth - 34, sceneHeight - 7);
            Console.Write("⑦: 幸运7，再掷一遍");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(sceneWidth - 76, sceneHeight - 6);
            Console.Write("⊙: 飞机重叠");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(sceneWidth - 56, sceneHeight - 6);
            Console.Write("★: 玩家");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(sceneWidth - 34, sceneHeight - 6);
            Console.Write("▲: 电脑");

            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(sceneWidth - 76, sceneHeight - 3);
            Console.Write("按回车键掷出骰子!");
        }

        /// <summary>
        /// 绘制玩家重合时的图标
        /// </summary>
        static void DrawPlayer(Player player, Player computer, Map map)
        {
            // 若位置重合
            if (player.nowIndex == computer.nowIndex)
            {
                Grid grid = map.grids[player.nowIndex];
                Console.SetCursorPosition(grid.pos.x, grid.pos.y);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("⊙");
            }
            // 若不重合
            else
            {
                player.Draw(map);
                computer.Draw(map);
            }
        }


        /// <summary>
        /// 在固定位置输出信息（自动清空行）
        /// </summary>
        /// <param name="message">要显示的信息</param>
        static void PrintGameMessage(string message)
        {
            Console.SetCursorPosition(sceneWidth - 76, sceneHeight - 3);
            Console.Write(new string(' ', 40)); // 清空行（40个空格）
            Console.SetCursorPosition(sceneWidth - 76, sceneHeight - 3);
            Console.Write(message);
        }

        /// <summary>
        /// 扔骰子函数
        /// </summary>
        static bool ThrowDice(ref Player role, ref Player other, Map map, bool isPlayerTurn = true)
        {
            string name = role.type == E_PlayerType.Player ? "先先" : "大伟";
            if (role.skipTurn > 0)
            {
                role.skipTurn--;
                PrintGameMessage($"{name}暂停中，还剩{role.skipTurn}回合");
                Thread.Sleep(1000);
                return false;
            }

            Random r = new Random();
            int randomNum;
            if (isPlayerTurn) { randomNum = r.Next(4, 7); }
            else { randomNum = r.Next(1, 7); }
            

            // 只有玩家回合才显示掷骰子动画
            if (isPlayerTurn)
            {
                PrintGameMessage("。");
                Thread.Sleep(100);
                PrintGameMessage("。。");
                Thread.Sleep(100);
                PrintGameMessage("。。。");
                Thread.Sleep(100);
                PrintGameMessage("。。。。");
                Thread.Sleep(100);
                PrintGameMessage("。。。。。");
                Thread.Sleep(100);
                PrintGameMessage("。。。。。。");
            }

            PrintGameMessage($"{name}掷出了{randomNum}点");
            Thread.Sleep(isPlayerTurn ? 700 : 500); // 电脑回合时等待时间稍短

            // 移动玩家
            role.nowIndex += randomNum;

            // 检查是否到达终点
            if (role.nowIndex >= map.grids.Length - 1)
            {
                role.nowIndex = map.grids.Length - 1;
                map.Draw();
                DrawPlayer(role, other, map);
                PrintGameMessage($"{name}到达了终点！");
                Thread.Sleep(1000);
                return true;
            }

            // 获取当前格子
            Grid grid = map.grids[role.nowIndex];

            switch (grid.type)
            {
                case E_GridType.Normal:
                    break;

                case E_GridType.Boom:
                    map.Draw();
                    DrawPlayer(role, other, map);
                    role.nowIndex -= 5;
                    if (role.nowIndex < 0) role.nowIndex = 0;
                    PrintGameMessage($"{name}踩到炸弹，后退5格！");
                    Thread.Sleep(1000);
                    break;

                case E_GridType.Pause:
                    role.skipTurn += 1;
                    map.Draw();
                    DrawPlayer(role, other, map);
                    PrintGameMessage($"{name}被暂停1回合！");
                    Thread.Sleep(1000);
                    break;

                case E_GridType.No7:
                    PrintGameMessage($"{name}获得幸运7，{(isPlayerTurn ? "按任意键再掷一次" : "自动再掷一次")}！");
                    if (isPlayerTurn)
                    {
                        map.Draw();
                        DrawPlayer(role, other, map);
                        SafeWaitForKey(); // 只有玩家回合需要按键
                    }
                    else
                    {
                        Thread.Sleep(1000); // 电脑回合时稍作延迟
                    }
                    return ThrowDice(ref role, ref other, map, isPlayerTurn); // 递归调用
            }
            return false;
        }


        /// <summary>
        /// 地图格子枚举和格子结构体
        /// </summary>
        enum E_GridType
        {
            /// <summary>
            /// 普通格子
            /// </summary>
            Normal,

            /// <summary>
            /// 炸弹
            /// </summary>
            Boom,

            /// <summary>
            /// 停止
            /// </summary>
            Pause,

            /// <summary>
            /// 幸运7
            /// </summary>
            No7

        }
        // 位置信息结构，包含横纵轴（int整形）
        struct Vector2
        {
            public int x;
            public int y;

            public Vector2(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
        //格子结构
        struct Grid
        {
            //格子类型
            public E_GridType type;
            //格子位置
            public Vector2 pos;

            //初始化构造函数
            public Grid(int x, int y, E_GridType type)
            {
                pos.x = x;
                pos.y = y;
                this.type = type;
            }

            //格子生成函数
            public void Draw()
            {
                Console.SetCursorPosition(pos.x, pos.y);
                switch (type)
                {
                    case E_GridType.Normal:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("□");
                        break;

                    case E_GridType.Boom:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("●");
                        break;

                    case E_GridType.Pause:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("||");
                        break;

                    case E_GridType.No7:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("⑦");
                        break;

                }
            }
        }


        /// <summary>
        /// 地图结构体
        /// </summary>
        struct Map
        {
            public Grid[] grids;
            public Map(int x, int y, int num)
            {
                grids = new Grid[num];
                int randomNum, indexX = 0, indexY = 0, stepNum = 2;
                Random r = new Random();

                // 循环生成每个格子
                for (int i = 0; i < num; i++)
                {
                    // 0-100 100个概率值
                    randomNum = r.Next(0, 101);

                    // 85%概率是普通格子(包括起点和终点强制普通)
                    if (randomNum < 85 || i == 0 || i == num - 1)
                    {
                        grids[i].type = E_GridType.Normal;
                    }

                    // 2%概率是炸弹格子
                    else if (randomNum < 87)
                    {
                        grids[i].type |= E_GridType.Boom;
                    }

                    // 3%概率是暂停格子
                    else if (randomNum < 90)
                    {
                        grids[i].type |= E_GridType.Pause;
                    }

                    // 其余10%概率是幸运7格子
                    else
                    {
                        grids[i].type |= E_GridType.No7;
                    }

                    //当前格子生成位置，初始为传入的x y
                    grids[i].pos = new Vector2(x, y);

                    //改变下一个格子的生成位置
                    if (indexX == 31)// 当横着走完31个格子
                    {
                        y += 1;//下移
                        ++indexY;
                        if (indexY == 2)// 下移两格后
                        {
                            indexX = 0;//计数清零
                            indexY = 0;

                            stepNum = -stepNum;//倒着走
                        }
                    }
                    else
                    {
                        x += stepNum;// 正常每次横移2步
                        ++indexX;
                    }
                    //最终形成蛇形地图
                }
            }

            public void Draw()
            {
                for (int i = 0; i < grids.Length; ++i)
                {
                    grids[i].Draw();
                }
            }
        }


        /// <summary>
        /// 玩家类型枚举和玩家结构体
        /// </summary>
        enum E_PlayerType
        {
            /// <summary>
            /// 玩家
            /// </summary>
            Player,

            /// <summary>
            /// 电脑
            /// </summary>
            Computer
        }
        struct Player
        {
            // 玩家类型
            public E_PlayerType type;
            // 在棋盘所处的位置(棋盘索引)
            public int nowIndex;
            //暂停回合数
            public int skipTurn;

            public Player(int index, E_PlayerType type)
            {
                nowIndex = index;
                this.type = type;
                skipTurn = 0;
            }

            public void Draw(Map mapInfo)
            {
                //设置位置，先拿到棋盘信息，再拿到格子索引
                Grid grid = mapInfo.grids[nowIndex];
                Console.SetCursorPosition(grid.pos.x, grid.pos.y);
                //设置图案颜色
                switch (type)
                {
                    case E_PlayerType.Player:
                        Console.ForegroundColor = ConsoleColor.Yellow;//颜色
                        Console.Write("★");
                        break;

                    case E_PlayerType.Computer:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("▲");
                        break;
                }
            }
        }
    }
}
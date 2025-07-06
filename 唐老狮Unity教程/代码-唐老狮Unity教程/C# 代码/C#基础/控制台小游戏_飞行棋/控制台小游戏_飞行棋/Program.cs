using System;
using System.Diagnostics;

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

            Grid grid = new Grid(6, 4, E_GridType.Normal);
            grid.Draw();

            Grid grid1 = new Grid(8, 4, E_GridType.Pause);
            grid1.Draw();

            Grid grid2 = new Grid(10, 4, E_GridType.Boom);
            grid2.Draw();

            Grid grid3 = new Grid(12, 4, E_GridType.No7);
            grid3.Draw();

            ////飞行棋盘-横
            //for (int i = 6; i < sceneWidth - 6; i += 2)
            //{
            //    for (int j = 4; j < sceneHeight - 10; j += 2)
            //    {
            //        Console.SetCursorPosition(i, j);
            //        Console.Write("□");
            //    }
            //}

            ////飞行棋盘-竖左
            //for (int j = 7; j < sceneHeight - 11; j += 4)
            //{
            //    Console.SetCursorPosition(6, j);
            //    Console.Write("□");
            //}
            ////飞行棋盘-竖右
            //for (int j = 5; j < sceneHeight - 11; j += 4)
            //{
            //    Console.SetCursorPosition(72, j);
            //    Console.Write("□");
            //}

            //AI飞机属性相关
            int AIX = 6;//横坐标
            int AIY = 1;//纵坐标
            string AIIcon = "▲";//AI ICON
            ConsoleColor AIColor = ConsoleColor.Gray;//颜色

            //玩家飞机属性相关
            int playerX = 4;//横坐标
            int playerY = 1;//纵坐标
            string playerIcon = "★";//ICON
            ConsoleColor playerColor = ConsoleColor.Yellow;//颜色

            char playerInput;//检测玩家按键

            bool isOver = false;// 游戏结束标识，从while循环内部的switch改变标识来跳出while循环

            while (true)
            {
                //绘制玩家飞机图标
                Console.SetCursorPosition(playerX, playerY);
                Console.ForegroundColor = playerColor;
                Console.Write(playerIcon);

                //绘制AI飞机图标
                Console.SetCursorPosition(AIX, AIY);
                Console.ForegroundColor = AIColor;
                Console.Write(AIIcon);

                //得到玩家输入
                playerInput = Console.ReadKey(true).KeyChar;

                //擦除原本玩家图像
                Console.SetCursorPosition(playerX, playerY);
                Console.Write("  ");

                // 计算玩家新位置
                int newPlayerX = playerX, newPlayerY = playerY;
                //根据输入更新玩家位置
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
            Console.Write("7: 幸运7，再掷一遍");

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

        public Vector2 (int x, int y)
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
                    Console.Write("7");
                    break;

            }
        }
    }


    /// <summary>
    /// 地图结构体
    /// </summary>
    struct Map
    {

    }
}
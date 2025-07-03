

//隐藏闪烁的光标
Console.CursorVisible = false;


// 主舞台
int sceneWidth = 80;
int sceneHeight = 30;

Console.SetWindowSize(sceneWidth, sceneHeight);
Console.SetBufferSize(sceneWidth, sceneHeight);

int nowSceneID = 0;// 0代表开始界面

string gameOverInfo = "感谢游玩";// 游戏结束后制作者界面对应的结局提示

// 游戏主体
while (true)
{
    switch(nowSceneID)
    {
        //开始界面
        case 0:
            Console.Clear();

            Console.SetCursorPosition(sceneWidth /2 - 5, sceneHeight / 2 - 6);
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

                char input = Console.ReadKey(true).KeyChar;
                switch(input) 
                {
                    case 'W':
                    case 'w':
                        nowSelIndex = nowSelIndex == 1 ? 3 : nowSelIndex - 1;
                        break;

                    case 'S':
                    case 's':
                        nowSelIndex = nowSelIndex == 3 ? 1 : nowSelIndex + 1;
                        break;
                    case '\r'://这里注意,对于回车，Windows 系统通常会同时产生 \r 和 \n，而 Linux 系统只产生 \n,
                              //所以考虑适配性,正式写项目推荐使用ConsoleKey.Enter捕捉
                              // 定义行为映射
                        Action[] actions = 
                        {
                            null,
                            () => { nowSceneID = 1; isQuitWhile = true; }, 
                            () => { nowSceneID = 2; isQuitWhile = true; },
                            () => { Console.ForegroundColor = ConsoleColor.White; Environment.Exit(0); },
                        };
                        actions[nowSelIndex]();
                        break;
                }
                if (isQuitWhile) break;
            }
            break;

        //游戏界面
        case 1:
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

            //飞行棋盘-横
            for (int i = 6; i < sceneWidth - 6; i += 2)
            {
                for(int j = 4; j < sceneHeight - 10; j += 2)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write("□");
                }
            }

            //飞行棋盘-竖左
            for (int j = 7; j < sceneHeight - 11; j += 4)
            {
                Console.SetCursorPosition(6, j);
                Console.Write("□");
            }
            //飞行棋盘-竖右
            for (int j = 5; j < sceneHeight - 11; j += 4)
            {
                Console.SetCursorPosition(72, j);
                Console.Write("□");
            }




            //AI飞机属性相关
            int AIX = 6;//横坐标
            int AIY = 1;//纵坐标
            string AIIcon = "★";//AI ICON
            ConsoleColor AIColor = ConsoleColor.Red;//颜色

            //玩家飞机属性相关
            int playerX = 4;//横坐标
            int playerY = 1;//纵坐标
            string playerIcon = "☆";//ICON
            ConsoleColor playerColor = ConsoleColor.Yellow;//颜色

            //终点属性相关
            int desX = 80;//横坐标
            int desY = 80;//纵坐标
            string desIcon = "终";//ICON
            ConsoleColor desColor = ConsoleColor.DarkYellow;//颜色

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

            break;

        //制作者名单
        case 2:
            Console.Clear();

            Console.SetCursorPosition(sceneWidth / 2 - 4, sceneHeight / 2 - 12);//设置光标位置
            Console.Write("THE END:");//结束标题

            Console.SetCursorPosition(sceneWidth / 2 - 4, sceneHeight / 2 - 10);//设置光标位置
            Console.Write(gameOverInfo);//对应文字

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


                //检测玩家键盘输入
                char input = Console.ReadKey(true).KeyChar;
                switch (input)
                {
                    case 'W':
                    case 'w':
                        nowSelEndIndex = nowSelEndIndex == 0 ? 1 : 0;
                        break;

                    case 'S':
                    case 's':
                        nowSelEndIndex = nowSelEndIndex == 1 ? 0 : 1;
                        break;
                    case '\r':
                        Action[] actions = 
                        {
                            () => { nowSceneID = 0; isQuitWhile = true; }, // 开始游戏
                            () => { Console.ForegroundColor = ConsoleColor.White; Environment.Exit(0); },   // 退出游戏
                        };
                        // 执行当前选中的操作
                        actions[nowSelEndIndex]();
                        break;
                }
                if (isQuitWhile) break;
            }
            break;
    }




}
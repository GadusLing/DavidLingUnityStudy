// See https://aka.ms/new-console-template for more information


using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;


//首先需要将闪烁的光标隐藏起来，不要破坏游戏氛围
Console.CursorVisible = false;

//布景
int sceneWidth = 80;
int sceneHeight = 30;
Console.SetWindowSize(sceneWidth, sceneHeight);//设置控制台的窗口大小
Console.SetBufferSize(sceneWidth, sceneHeight);//设置缓冲区大小，缓冲区大小不能小于窗口大小，这里为了防止有滑动条，所以设置为和窗口一般大小

int nowSceneID = 1;//当前的场景编号
while (true)
{
    switch(nowSceneID)
    {
        //开始界面
        case 1:
            Console.Clear();
            //sceneWidth / 2 和 sceneHeight / 2代表光标居中,后面的+-是UI按钮位置的调整
            Console.SetCursorPosition(sceneWidth / 2 - 5, sceneHeight / 2 - 6);//设置光标位置
            Console.Write("勇者救公主");//游戏标题

            Console.SetCursorPosition(sceneWidth / 2 - 10, sceneHeight - 2);//设置光标位置
            Console.Write("控制方式：WASD和回车");//游戏标题

            int nowSelIndex = 0;//开始界面选择按钮的编号,0代表开始游戏按钮,1为退出游戏按钮,2为制作者名单

            while (true)
            {
                bool isQuitWhile = false;//设置标识,决定是否退出"开始游戏"界面转到其他界面

                Console.SetCursorPosition(sceneWidth / 2 - 4, sceneHeight / 2 - 1);//设置光标位置
                Console.ForegroundColor = nowSelIndex == 0 ? ConsoleColor.Red : ConsoleColor.White;
                Console.Write("开始游戏");//显示UI按钮

                Console.SetCursorPosition(sceneWidth / 2 - 4, sceneHeight / 2 + 1);
                Console.ForegroundColor = nowSelIndex == 1 ? ConsoleColor.Red : ConsoleColor.White;
                Console.Write("退出游戏");

                Console.SetCursorPosition(sceneWidth / 2 - 5, sceneHeight / 2 + 3);
                Console.ForegroundColor = nowSelIndex == 2 ? ConsoleColor.Red : ConsoleColor.White;
                Console.Write("制作者名单");

                //检测玩家键盘输入
                char input = Console.ReadKey(true).KeyChar;
                switch(input) 
                {
                    case 'W':
                    case 'w':
                        nowSelIndex = nowSelIndex == 0 ? 2 : nowSelIndex - 1;
                        break;

                    case 'S':
                    case 's':
                        nowSelIndex = nowSelIndex == 2 ? 0 : nowSelIndex + 1;
                        break;
                    case '\r'://这里注意,对于回车，Windows 系统通常会同时产生 \r 和 \n，而 Linux 系统只产生 \n,所以考虑适配性,正式写项目推荐使用ConsoleKey.Enter捕捉
                              // 定义行为映射
                        Action[] actions = {
                        () => { nowSceneID = 2; isQuitWhile = true; }, // 开始游戏
                        () => { Environment.Exit(0); },              // 退出游戏
                        () => { nowSceneID = 3; isQuitWhile = true; } // 制作者名单
                        };
                        // 执行当前选中的操作
                        actions[nowSelIndex]();
                        break;
                }
                if (isQuitWhile)
                {
                    break;
                }
            }
            break;

        //游戏界面
        case 2:
            Console.Clear();
            //布景,外围的墙壁
            Console.ForegroundColor = ConsoleColor.White;//墙壁颜色
            //横墙
            for (int i = 0; i < sceneWidth; i+=2)
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

            //BOSS属性相关
            int bossX = sceneWidth / 2 - 2;//横坐标
            int bossY = sceneHeight / 2;//纵坐标
            int bossAtkMin = 7;//最小攻击
            int bossAtkMax = 13;//最大攻击
            int bossHp = 100;//血量
            string bossIcon = "★";//boss ICON
            ConsoleColor bossColor = ConsoleColor.Red;//颜色

            //玩家属性相关
            int playerX = 6;//横坐标
            int playerY = 3;//纵坐标
            int playerAtkMin = 9;//最小攻击
            int playerAtkMax = 12;//最大攻击
            int playerHp = 100;//血量
            string playerIcon = "☆";//ICON
            ConsoleColor playerColor = ConsoleColor.Yellow;//颜色

            char playerInput;//检测玩家按键

            //游戏场景运行的循环
            while (true)
            {
                if (bossHp > 0)//当boss血量健康才绘制boss到屏幕上
                {
                    //绘制boss图标
                    Console.SetCursorPosition(bossX, bossY);
                    Console.ForegroundColor = bossColor;
                    Console.Write(bossIcon);
                }

                //绘制勇者图标
                Console.SetCursorPosition(playerX, playerY);
                Console.ForegroundColor = playerColor;
                Console.Write(playerIcon);

                //得到玩家输入
                playerInput = Console.ReadKey(true).KeyChar;

                //擦除原本玩家图像
                Console.SetCursorPosition(playerX, playerY);
                Console.Write("  ");

                // 计算玩家新位置
                int newPlayerX = playerX, newPlayerY = playerY;

                //根据输入更新玩家位置
                switch (playerInput)
                {
                    case 'W':
                    case 'w':
                        newPlayerY--;
                        break;
                    case 'A':
                    case 'a':
                        newPlayerX-=2;
                        break;
                    case 'S':
                    case 's':
                        newPlayerY++;
                        break;
                    case 'D':
                    case 'd':
                        newPlayerX+=2;
                        break;
                }
                // 检查是否可以移动到新位置
                bool isCollidingWithBoss = bossHp > 0 && newPlayerX == bossX && newPlayerY == bossY;
                bool isOutOfBounds = newPlayerX < 2 || newPlayerX > sceneWidth - 4 ||
                                     newPlayerY < 1 || newPlayerY > sceneHeight - 9;

                if (!isCollidingWithBoss && !isOutOfBounds)//判断位置移动是否合法
                {
                    // 更新玩家位置
                    playerX = newPlayerX;
                    playerY = newPlayerY;
                }
            }

            break;
        //制作者名单界面
        case 3:
            Console.ForegroundColor = ConsoleColor.White; // 显式设置为白色,因制作者界面是最后打印的,此时的颜色设置还没有恢复为白色,所以要手动恢复一下
            Console.Clear();
            Console.WriteLine("制作者名单界面");
            break;
    }

}



// See https://aka.ms/new-console-template for more information


using System.Data;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using static System.Net.Mime.MediaTypeNames;


//首先需要将闪烁的光标隐藏起来，不要破坏游戏氛围
Console.CursorVisible = false;

//布景
int sceneWidth = 80;
int sceneHeight = 30;
Console.SetWindowSize(sceneWidth, sceneHeight);//设置控制台的窗口大小
Console.SetBufferSize(sceneWidth, sceneHeight);//设置缓冲区大小，缓冲区大小不能小于窗口大小，这里为了防止有滑动条，所以设置为和窗口一般大小

int nowSceneID = 1;//当前的场景编号

string gameOverInfo = "感谢游玩";// 游戏结束后制作者界面对应的结局提示
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
            int bossAtkMin = 10;//最小攻击
            int bossAtkMax = 12;//最大攻击
            int bossHp = 100;//血量
            string bossIcon = "★";//boss ICON
            ConsoleColor bossColor = ConsoleColor.Red;//颜色

            //玩家属性相关
            int playerX = 6;//横坐标
            int playerY = 3;//纵坐标
            int playerAtkMin = 8;//最小攻击
            int playerAtkMax = 13;//最大攻击
            int playerHp = 100;//血量
            string playerIcon = "☆";//ICON
            ConsoleColor playerColor = ConsoleColor.Yellow;//颜色

            //公主属性相关
            int princessX = 60;//横坐标
            int princessY = 3;//纵坐标
            string princessIcon = "★";//ICON
            ConsoleColor princessColor = ConsoleColor.DarkYellow;//颜色

            char playerInput;//检测玩家按键
            
            bool startFight = false;// 战斗开始标识
            bool isOver = false;// 游戏结束标识，从while循环内部的switch改变标识来跳出while循环

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
                else //boss死亡就绘制公主
                {
                    Console.SetCursorPosition(princessX, princessY);
                    Console.ForegroundColor = princessColor;
                    Console.Write(princessIcon);
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
                    case '\r':
                        if((playerX == bossX && playerY == bossY - 1 || playerX == bossX && playerY == bossY + 1 ||
                           playerY == bossY && playerX == bossX - 2 || playerY == bossY && playerX == bossX + 2) && bossHp > 0)//在boss身边按回车是否开启战斗
                        {
                            startFight = true;
                            Console.SetCursorPosition(2, sceneHeight - 7);//设置光标到信息栏位置,-7是因为之前中间的分割墙是-8
                            Console.ForegroundColor = ConsoleColor.White;//设置播报字体颜色
                            Console.Write("开始战斗!, 按回车键继续");
                            Console.SetCursorPosition(2,sceneHeight - 6);
                            Console.Write("勇者血量{0}", playerHp);
                            Console.SetCursorPosition(2, sceneHeight - 5);
                            Console.Write("魔王血量{0}", bossHp);
                        }
                        else if((playerX == princessX && playerY == princessY - 1 || playerX == princessX && playerY == princessY + 1 ||
                                playerY == princessY && playerX == princessX - 2 || playerY == princessY && playerX == princessX + 2) && bossHp <= 0)
                                //在公主身边按回车开启结局
                        {
                            // 触发结局,改变场景ID
                            nowSceneID = 3;
                            gameOverInfo = "英雄救美";
                            // 跳出当前循环,返回选择界面循环
                            isOver = true;
                            break;
                        }
                        break;
                }
                // 检查是否可以移动到新位置
                bool isCollidingWithBoss = bossHp > 0 && newPlayerX == bossX && newPlayerY == bossY;
                bool isCollidingWithPrincess = bossHp <= 0 && newPlayerX == princessX && newPlayerY == princessY;
                bool isOutOfBounds = newPlayerX < 2 || newPlayerX > sceneWidth - 4 ||
                                     newPlayerY < 1 || newPlayerY > sceneHeight - 9;
                //判断是否在战斗,位置移动是否合法
                if (!startFight && (newPlayerX != playerX || newPlayerY != playerY) && !isCollidingWithBoss && !isOutOfBounds && !isCollidingWithPrincess)
                {
                    // 更新玩家位置
                    playerX = newPlayerX;
                    playerY = newPlayerY;
                }
                else // 进战斗了
                {
                    if(playerInput == '\r' && startFight)// 当玩家在战斗状态,继续按回车则持续打架
                    {
                        //在这里判断玩家和怪物是否死亡及之后的流程
                        if(playerHp <= 0)// 玩家被打败之后
                        {
                            //有戏结束,需要弹出当前界面
                            nowSceneID = 3;
                            gameOverInfo = "从头再来";
                            break;
                        }
                        else if(bossHp <= 0)// BOSS被打败之后
                        {
                            //擦除boss, 营救公主
                            Console.SetCursorPosition(bossX,bossY);
                            Console.Write("  ");
                            startFight = false;
                        }
                        else// 玩家和boss都还健康,继续战斗
                        {
                            Random r = new Random();//生成伪随机数
                            int atk = r.Next(playerAtkMin, playerAtkMax);// 随机玩家攻击力在Min到<max之间浮动
                            bossHp -= atk;// 扣除Boss血量
                            Console.ForegroundColor = ConsoleColor.Green;//设置播报字体颜色
                            Console.SetCursorPosition(2, sceneHeight - 6);
                            Console.Write("                                        ");// 通过空格擦除之前的信息
                            Console.SetCursorPosition(2, sceneHeight - 6);
                            Console.Write("勇者造成了{0}点伤害,魔王剩余血量{1}", atk, bossHp);

                            //怪物打玩家
                            if (bossHp > 0)// 若怪物存活
                            {
                                atk = r.Next(bossAtkMin, bossAtkMax);// 随机怪物攻击力在Min到<max之间浮动
                                playerHp -= atk;//玩家扣血
                                Console.ForegroundColor = ConsoleColor.Red;//设置播报字体颜色
                                Console.SetCursorPosition(2, sceneHeight - 5);
                                Console.Write("                                        ");// 通过空格擦除之前的信息
                                if (playerHp <= 0)// 如果怪物把玩家打死了
                                {
                                    Console.SetCursorPosition(2, sceneHeight - 5);
                                    Console.Write("哦，不！你死了。");// 不弹出怪物伤害而是直接弹失败字幕
                                }
                                else// 玩家血量健康
                                {
                                    Console.SetCursorPosition(2, sceneHeight - 5);
                                    Console.Write("魔王造成了{0}点伤害,勇者剩余血量{1}", atk, playerHp);
                                }
                            }
                            else// 打败怪物
                            {
                                Console.SetCursorPosition(2, sceneHeight - 7);//设置光标到信息栏位置,-7是因为之前中间的分割墙是-8
                                Console.ForegroundColor = ConsoleColor.Yellow;//设置播报字体颜色
                                Console.Write("                                        ");
                                Console.SetCursorPosition(2, sceneHeight - 6);
                                Console.Write("                                        ");
                                Console.SetCursorPosition(2, sceneHeight - 5);
                                Console.Write("                                        ");
                                Console.SetCursorPosition(2, sceneHeight - 7);
                                Console.Write("恭喜你,获得了最终的胜利");
                                Console.SetCursorPosition(2, sceneHeight - 6);
                                Console.Write("去公主身边,按回车继续");
                            }
                        }   
                    }
                }
                if(isOver)//判断是否结局,结局则跳出到切换场景的循环,后续才会进入制作者名单
                {
                    break;
                }
            }
            break;

        //游戏结束/制作者名单界面
        case 3:
            Console.ForegroundColor = ConsoleColor.White; // 显式设置为白色,因制作者界面是最后打印的,此时的颜色设置还没有恢复为白色,所以要手动恢复一下
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
                    case '\r'://这里注意,对于回车，Windows 系统通常会同时产生 \r 和 \n，而 Linux 系统只产生 \n,所以考虑适配性,正式写项目推荐使用ConsoleKey.Enter捕捉
                              // 定义行为映射
                        Action[] actions = {
                        () => { nowSceneID = 1; isQuitWhile = true; }, // 开始游戏
                        () => { Console.ForegroundColor = ConsoleColor.White; Environment.Exit(0); },   // 退出游戏
                        };
                        // 执行当前选中的操作
                        actions[nowSelEndIndex]();
                        break;
                }
                if (isQuitWhile)
                {
                    break;
                }
            }
            break;
    }

}



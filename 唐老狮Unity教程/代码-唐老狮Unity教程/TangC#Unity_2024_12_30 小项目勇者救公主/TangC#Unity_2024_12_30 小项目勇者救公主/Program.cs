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

            int nowSelIndex = 0;//开始界面选择按钮的编号,0代表开始游戏按钮,1为退出游戏按钮

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
                        if (nowSelIndex == 0)//0代表"开始游戏"
                        {
                            nowSceneID = 2;//2代表游戏界面
                            isQuitWhile = true;
                        }
                        else if(nowSelIndex == 2)//2代表"制作者名单"
                        {
                            nowSceneID = 3;//3代表制作者名单界面
                            isQuitWhile = true;
                        }
                        else
                        {
                            Environment.Exit(0);//退出控制台
                        }
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
            Console.WriteLine("游戏界面");
            break;
        //制作者名单界面
        case 3:
            Console.ForegroundColor = ConsoleColor.White; // 显式设置为白色,因制作者界面是最后打印的,此时的颜色设置还没有恢复为白色,所以要手动恢复一下
            Console.Clear();
            Console.WriteLine("制作者名单界面");
            break;
    }

}



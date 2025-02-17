// See https://aka.ms/new-console-template for more information
//Console.WriteLine("控制台相关");
//#region 知识点一 复习 输入输出
////输出
////Console.WriteLine("123123");//光标空行
////Console.Write("123123123123");//不空行
////输入
////string str = Console.ReadLine();
////如果在ReadKey(true)不会把输入的内容显示在控制台上
////char c = Console.ReadKey(true).KeyChar;
////Console.WriteLine(c);
//#endregion

//#region 知识点二 控制台其它方法
////1.清空
//Console.Clear();

////2.设置控制台大小
//// 窗口大小 缓冲区大小
//// 注意：
////1.先设置窗口大小，再设置缓冲区大小
////2.缓冲区的大小不能小于窗口的大小
////3.窗口的大小不能大于控制台的最大尺寸
////窗口大小
//Console.SetWindowSize(100, 50);
////缓冲区大小 （可打印内容区域的宽高 ）
//Console.SetBufferSize(1000, 1000);

////3.设置光标的位置
////控制台左上角为原点0 0 右侧是X轴正方向 下方是Y轴正方向 它是一个平面二维坐标系
////注意：
////1.边界问题
////2.横纵距离单位不同 1y = 2x 视觉上的
//Console.SetCursorPosition(10, 5);
//Console.WriteLine("123123");

////4.设置颜色相关
////文字颜色设置
//Console.ForegroundColor = ConsoleColor.Red;
//Console.WriteLine("123123123");
//Console.ForegroundColor = ConsoleColor.Green;
////背景颜色设置
////Console.BackgroundColor = ConsoleColor.White;
////重置背景颜色过后 需要Clear一次 才能把整个背景颜色改变
////Console.Clear();

////5.光标显隐
//Console.CursorVisible = false;

////6.关闭控制台
//Environment.Exit(0);
//#endregion

Console.WriteLine("控制台相关练习题");

//改背景颜色
Console.BackgroundColor = ConsoleColor.Red;
Console.Clear();
//改变字体颜色
Console.ForegroundColor = ConsoleColor.Yellow;
//隐藏光标
Console.CursorVisible = false;

//黄色方块感觉像人一样 这个人身上有位置信息
// x y
int x = 0;
int y = 0;

//不停的输入 wasd键 都可以控制它移动
//根据不停 就分析 用while循环是最简单的一种方式
while (true)
{
    //第一种清空之前信息的方式
    //Console.Clear();
    Console.SetCursorPosition(x, y);
    Console.Write("■");
    //得到玩家的输入信息
    char c = Console.ReadKey(true).KeyChar;
    //第二种方式把之前的方块擦除了
    Console.SetCursorPosition(x, y);
    Console.Write("  ");
    switch (c)
    {
        //贯穿
        case 'W':
        case 'w':
            y -= 1;
            //改变位置过后 判断新位置 是否越界
            if (y < 0)
            {
                y = 0;
            }
            break;
        case 'A':
        case 'a':
            //中文符号 在控制台上占2个位置
            x -= 2;
            if (x < 0)
            {
                x = 0;
            }
            break;
        case 'S':
        case 's':
            y += 1;
            if (y > Console.BufferHeight - 1)
            {
                y = Console.BufferHeight - 1;
            }
            break;
        case 'D':
        case 'd':
            x += 2;
            if (x > Console.BufferWidth - 2)
            {
                x = Console.BufferWidth - 2;
            }
            break;
    }
}

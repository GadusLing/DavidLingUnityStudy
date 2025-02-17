// See https://aka.ms/new-console-template for more information
//Console.WriteLine("条件分支语句");

//#region 知识点一 作用
////让顺序执行的代码 产生分支
////if语句是第一个 可以让我们的程序 产生逻辑变化的 语句
//#endregion

//#region 知识点二 if语句
////作用： 满足条件时 多执行一些代码
////语法：
//// if( bool类型值 )  // bool类型相关：bool变量 条件运算符表达式 逻辑运算符表达式
//// {
////     满足条件要执行的代码 写在if代码块中;
//// }
//// 注意：
//// 1.if语句的语法部分， 不需要写分号
//// 2.if语句可以嵌套使用

//if (false)
//{
//    Console.WriteLine("进入了if语句代码块，执行其中的代码逻辑");
//    Console.WriteLine("进入了if语句代码块，执行其中的代码逻辑");
//    Console.WriteLine("进入了if语句代码块，执行其中的代码逻辑");
//}
//Console.WriteLine("if语句外的代码");

//int a = 1;
//if (a > 0 && a < 5)
//{
//    Console.WriteLine("a在0到5之间");
//}

//string name = "唐老狮";
//string passWord = "666";
//if (name == "唐老狮" && passWord == "666")
//{
//    Console.WriteLine("登录成功");
//}

////嵌套使用
//if (name == "唐老狮")
//{
//    Console.WriteLine("用户名验证成功");
//    if (passWord == "666")
//    {
//        Console.WriteLine("密码验证成功");
//        //可以无限嵌套
//    }
//    //可以无限嵌套
//}

//#endregion

//#region 知识点三 if...else语句
//// 作用：产生两条分支 十字路 满足条件做什么 不满足条件做什么

////语法：
//// if( bool类型值 )
//// {
////      满足条件执行的代码;
//// }
//// else
//// {
////      不满足条件执行的代码：
//// }
//// 注意：
//// 1.if ...else 语句 语法部分 不需要写分号
//// 2.if ...else 语句 可以嵌套

//if (false)
//{
//    Console.WriteLine("满足if条件 做什么");
//    if (true)
//    {
//        if (true)
//        {

//        }
//        else
//        {

//        }
//    }
//    else
//    {
//        if (true)
//        {

//        }
//        else
//        {

//        }
//    }
//}
//else
//{
//    Console.WriteLine("不满足if条件 做什么");
//    if (true)
//    {

//    }
//    else
//    {

//    }
//}

////其它的使用和if的使用时一样
//// 嵌套使用 也是和if语句 一样的

//#endregion

//#region 知识点四 if...else if...else 语句
////作用：产生n条分支 多条道路选择 最先满足其中的一个条件 就做什么

//// 语法：
//// if( bool类型值 )
//// {
////      满足条件执行的代码;
//// }
//// else if( bool类型值 )
//// {
////      满足条件执行的代码;
//// }
//// ...中间可以有n个 else if语句代码块
//// else
//// {
////      不满足条件执行的代码：
//// }

//// 注意：
//// 1. 和前面两个是一样的 不需要写分号
//// 2. 是可以嵌套的
//// 3. else 是可以省略的
//// 4. 注意 条件判断 从上到下执行 满足了第一个后 之后的都不会执行了

//int a3 = 6;
//if (a3 >= 10)
//{
//    Console.WriteLine("a大于等于10");
//}
//else if (a3 > 5 && a3 < 10)
//{
//    Console.WriteLine("a在6和9之间");
//}
//else if (a3 >= 0 && a3 <= 5)
//{
//    Console.WriteLine("a在0和5之间");
//}
//else
//{
//    Console.WriteLine("a小于0");
//}

////if语句相关 if if..else  if...else if...else
//// else if 和 else 是组合套餐 根据实际情况选择使用




//#endregion


Console.WriteLine("if语句练习题");

#region 练习题一
//请用户输入今日看唐老狮视频花了多少分钟，如果大于60分钟，
//那么在控制台输出“今天看视频花了XX分钟，看来你离成功又进了一步!”
// 控制台输入  类型转换  异常捕获  条件运算符  if语句
//Console.WriteLine("请输入今日看唐老狮视频花了多少时间(分钟)");
//try
//{
//    string input = Console.ReadLine();
//    int min = int.Parse(input);
//    if (min > 60)
//    {
//        Console.WriteLine("今天看视频花了{0}分钟，看来你离成功又近了一步", min);
//    }
//    else
//    {
//        Console.WriteLine("你还需要努力啊！");
//    }
//}
//catch
//{
//    Console.WriteLine("请输入正确格式的时间");
//}

#endregion

#region 练习题二
//请输入你的 语文，数学，英语成绩，满足以下任意条件，则输出“非常棒，继续加油”
//语文成绩大于70 并且 数学成绩大于80 并且英语成绩大于90
//语文成绩等于100或者数学成绩等于100或者英语成绩等于100
//语文成绩大于90 并且 其它两门中有一门成绩大于70
// 控制台输入 类型转换 异常捕获  条件运算符 逻辑运算符  if语句
//try
//{
//    Console.WriteLine("请输入语文成绩");
//    int yuwen = int.Parse(Console.ReadLine());
//    Console.WriteLine("请输入数学成绩");
//    int shuxue = int.Parse(Console.ReadLine());
//    Console.WriteLine("请输入英语成绩");
//    int yingyu = int.Parse(Console.ReadLine());

//    bool c1 = yuwen > 70 && shuxue > 80 && yingyu > 90;
//    bool c2 = yuwen == 100 || shuxue == 100 || yingyu == 100;
//    bool c3 = yuwen > 90 && (shuxue > 70 || yingyu > 70);

//    if (c1 || c2 || c3)
//    {
//        Console.WriteLine("非常棒，继续加油");
//    }
//}
//catch
//{
//    Console.WriteLine("成绩请输入数字");
//}

#endregion

#region 练习题三
//定义一个变量，存储小赵的考试成绩，如果小赵的考试成绩大于（含）90分，
//那么爸爸奖励100元钱，否则一个月不能玩游戏
// if else语句 条件运算符
//int cj = 90;
//if( cj >= 90 )
//{
//    Console.WriteLine("奖励100元");
//}   
//else
//{
//    Console.WriteLine("一个月不能玩游戏");
//}

#endregion

#region 练习题四
//要求用户输入两个数a、b，如果两个数可以整除或者这两个数加起来大于100，
//则输出a的值，否则输出b的值
// 控制台输入 类型转换  异常捕获  算数运算符  条件运算符  逻辑运算符
// if else 语句

//try
//{
//    Console.WriteLine("请输入一个数");
//    int a = int.Parse(Console.ReadLine());
//    Console.WriteLine("请再输入一个数");
//    int b = int.Parse(Console.ReadLine());

//    bool c1 = a % b == 0 || b % a == 0;
//    bool c2 = a + b > 100;

//    if (c1 || c2)
//    {
//        Console.WriteLine(a);
//    }
//    else
//    {
//        Console.WriteLine(b);
//    }

//}
//catch
//{
//    Console.WriteLine("请输入数字");
//}

#endregion

#region 练习题五
//输入一个整数，如果这个数是偶数，则打印“Your input is even”，否则打印“Your input is odd”
// 控制台输入 类型转换 异常捕获  条件运算符  if语句  算术运算符
//try
//{
//    Console.WriteLine("请输入一个整数");
//    int num = int.Parse(Console.ReadLine());
//    //能被2整除的数 叫偶数
//    if( num % 2 == 0 )
//    {
//        Console.WriteLine("Your input is even");
//    }
//    else
//    {
//        Console.WriteLine("Your input is odd");
//    }
//}
//catch
//{
//    Console.WriteLine("请输入数字");
//}

#endregion

#region 练习题六
//有3个整形变量，分别存储不同的值，编写代码输出其中最大的整数
// 条件运算符 逻辑运算符 if else if else 语句
//int a = 98;
//int b = 5;
//int c = 11;
//if( a > b && a > c )
//{
//    Console.WriteLine(a);
//}
//else if( b > a && b > c )
//{
//    Console.WriteLine(b);
//}
//else
//{
//    Console.WriteLine(c);
//}
#endregion

#region 练习题七
////写一个程序接受用户输入的字符，如果输入的字符是0~9数字中的一个，
////则显示“您输入了一个数字”，否则显示这不是一个数字
//// 控制台输入 类型转换 异常捕获 条件运算符 逻辑运算符 if语句

//Console.WriteLine("请输入一个字符");
//// char类型可以隐式转换为 数值类型 
//int askii = Console.ReadKey().KeyChar;
////int zeroAsk = '0';
////Console.WriteLine(zeroAsk);
////int nineAsk = '9';
////Console.WriteLine(nineAsk);
//if (askii >= '0' && askii <= '9')
//{
//    Console.WriteLine("您输入了一个数字");
//}
//else
//{
//    Console.WriteLine("这不是一个数字");
//}

//try
//{
//    Console.WriteLine("请输入一个字符");
//    // 通过 ReadKey().KeyChar得到的输入的字符
//    char c = Console.ReadKey().KeyChar;
//    int num = int.Parse(c.ToString());
//    //通过Convert把char转成整形 转过去的是对应的ASKII码的数值
//    //int num = Convert.ToInt32(c);
//    Console.WriteLine(num);
//    if( num >= 0 && num <= 9 )
//    {
//        Console.WriteLine("您输入了一个数字");
//    }
//    //else
//    //{
//    //    Console.WriteLine("这不是一个数字");
//    //}
//}
//catch
//{
//    Console.WriteLine("这不是一个数字");
//}

#endregion

#region 练习题八
////提示用户输入用户名，然后再提示输入密码，如果用户名是“admin”，
////并且密码是"8888"，则提示正确，否则，
////如果用户名不是admin还提示用户用户名不存在，如果用户名是admin则提示密码错误
//// if嵌套使用 
//Console.WriteLine("请输入用户名");
//string name = Console.ReadLine();
//Console.WriteLine("请输入密码");
//string passWord = Console.ReadLine();
//if (name == "admin" && passWord == "8888")
//{
//    Console.WriteLine("登录成功");
//}
//else
//{
//    if (name != "admin")
//    {
//        Console.WriteLine("用户名不存在");
//    }
//    else
//    {
//        Console.WriteLine("密码错误");
//    }
//}

#endregion

#region 练习题九
//提示用户输入年龄，如果大于等于18，则告知用户可以查看，
//如果小于13岁，则告知不允许查看，如果大于等于13并且小于18，
//则提示用户是否继续查看（yes、no），
//如果输入的是yes则提示用户请查看，否则提示“退出”。
// if else if else  嵌套使用 
try
{
    //输入年龄 
    Console.WriteLine("请输入你的年龄");
    int age = int.Parse(Console.ReadLine());
    //大于等于18 做什么
    if (age >= 18)
    {
        Console.WriteLine("你可以查看");
    }
    //13 18之间 
    // 判断玩家输入 根据输入内容 决定显示什么
    else if (age < 18 && age >= 13)
    {
        Console.WriteLine("是否继续查看(yes/no)");
        string str = Console.ReadLine();
        if (str == "yes")
        {
            Console.WriteLine("请查看");
        }
        else if (str == "no")
        {
            Console.WriteLine("退出");
        }
        else
        {
            Console.WriteLine("输入内容不正确，退出");
        }
    }
    //小于13 做什么
    else
    {
        Console.WriteLine("不允许查看");
    }
}
catch
{
    Console.WriteLine("请输入正确内容");
}


#endregion

#region 练习题十
//请说明以下代码的打印结果（不要打一遍代码，请直接通过阅读说出结果）

// 语句块 会影响 变量的 生命周期

//函数语句块 目前我们学习知识时  是层级最高的语句块

int a = 1;
int b = 2;
{
    b = 3;
    Console.WriteLine(a);
    Console.WriteLine(b);
}
Console.WriteLine(b);

a = 5;
if (a > 3)
{
    b = 0;
    ++b;
    b += a;
}
Console.WriteLine(b);
#endregion
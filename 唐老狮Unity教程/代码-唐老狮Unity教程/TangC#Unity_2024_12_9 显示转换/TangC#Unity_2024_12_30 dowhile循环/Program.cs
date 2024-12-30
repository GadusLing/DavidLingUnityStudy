// See https://aka.ms/new-console-template for more information
Console.WriteLine("do...while循环");

#region 知识点一 基本语法
// while循环 是先判断条件再执行
// do while循环 是先斩后奏 先至少执行一次循环语句块中的逻辑 再判断是否继续
//do
//{
//    //do while 循环语句块;
//} while (bool类型的值);
// 注意 do while 语句 存在一个重要的分号
#endregion

#region 知识点二 实际使用

// do while 使用较多

//do
//{
//    Console.WriteLine("do while 循环语句块");
//} while (true);

//int a = 0;
//do
//{
//    Console.WriteLine(a);
//    ++a;
//} while (a < 2);
#endregion

#region 知识点三 嵌套使用
// if switch while do while
do
{
    //if(true)
    //{


    //}
    //while(true)
    //{

    //}
    //int i = 1;
    //switch (i)
    //{
    //    default:
    //        break;
    //}

    //break;
    //Console.WriteLine("111");
    //continue;
    //Console.WriteLine("111");
} while (false);
#endregion



Console.WriteLine("do while语句练习题");

#region 练习题一
//要求用户输入用户名和密码，
//只要不是admin和8888就一直提示用户名或密码错误，请重新输入
// 控制台输入 条件运算符 逻辑运算符

//变量申明 一定要注意申明在哪个语句块中
string userName;
string passWord;
bool isShow = false;
do
{
    //这句代码 第一次 肯定不能执行
    if (isShow)
    {
        Console.WriteLine("用户名或密码错误，请重新输入");
    }
    //循环输入
    Console.WriteLine("请输入用户名");
    userName = Console.ReadLine();
    Console.WriteLine("请输入密码");
    passWord = Console.ReadLine();
    isShow = true;
} while (userName != "admin" || passWord != "8888");

#endregion

#region 练习题二
//不断提示请输入你的姓名，直到输入q结束
string input;
do
{
    Console.WriteLine("请输入你的姓名");
    input = Console.ReadLine();
} while (input != "LDW");
#endregion
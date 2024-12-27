// See https://aka.ms/new-console-template for more information
//Console.WriteLine("三目运算符");

//#region 知识点一 基本语法
////套路： 3个空位 2个符号！！！
////固定语法：空位     ? 空位                : 空位;
////关键信息：bool类型 ? bool类型为真返回内容 : bool类型为假返回内容;
////三目运算符 会有返回值，这个返回值类型必须一致，并且必须使用！
//#endregion

//#region 知识点二 具体使用
//string str = false ? "条件为真" : "条件为假";
//Console.WriteLine(str);

//int a = 5;
//str = a < 1 ? "a大于1" : "a不满条件";
//Console.WriteLine(str);

//int i = a > 1 ? 123 : 234;

////第一个空位 始终是结果为bool类型的表达式 bool变量 条件表达式 逻辑运算符表达式
////第二三个空位 什么表达式都可以 只要保证他们的结果类型是一致的 

//bool b = a > 1 ? a > 6 : !false;
//#endregion


Console.WriteLine("三目运算符练习题");
#region 练习题一
////比较两个数的大小，求出最大的。
//int a = 10;
//int b = 15;
//string str = a >= b ? "较大数是a" + a : "较大的数是b" + b;
//Console.WriteLine(str);
try
{
    Console.WriteLine("请分别输入两个数比较大小");
    string str = Console.ReadLine();
    int a = int.Parse(str);
    str = Console.ReadLine();
    int b = int.Parse(str);

    str = a >= b ? "较大的数是" + a : "较大的数是" + b;
    Console.WriteLine(str);
}
catch
{
    Console.WriteLine("请输入数字");
}
#endregion

#region 练习题二
////提示用户输入一个姓名，然后再控制台输出姓名，只要输入的不是帅哥，就显示美女。
Console.WriteLine("请输入一个姓名");
string name = Console.ReadLine();
string str2 = name == "帅哥" ? name : "美女";
Console.WriteLine(str2);
#endregion

#region 练习题三
//依次输入学生的姓名，C#语言的成绩，Unity的成绩，
//两门成绩都大于等于90分，才能毕业，请输出最后的结果。
try
{
    Console.WriteLine("请输入姓名");
    string yourName = Console.ReadLine();
    Console.WriteLine("请输入C#成绩");
    int csharp = int.Parse(Console.ReadLine());
    Console.WriteLine("请输入Unity成绩");
    int unity = int.Parse(Console.ReadLine());

    //string result = csharp + unity >= 90 ? "顺利毕业" : "请再学一次！";
    Console.WriteLine(csharp + unity >= 90 ? "顺利毕业" : "请再学一次！");
}
catch
{
    Console.WriteLine("成绩只能输入整数");
}

#endregion

#region 练习题四
//要求用户输入一个年份，然后判断是不是闰年？
//闰年判断条件：
//年份能被400整除（2000）
//或者
//年份能被4整除，但是不能被100整除（2008）

try
{
    Console.WriteLine("请输入一个年份，用来判断是不是闰年");
    int year = int.Parse(Console.ReadLine());
    //string result = year % 400 == 0 || year % 4 == 0 && year % 100 != 0 ? "闰年" : "不是闰年";
    Console.WriteLine(year % 400 == 0 || year % 4 == 0 && year % 100 != 0 ? "闰年" : "不是闰年");
}
catch
{
    Console.WriteLine("年份要是整数");
}

#endregion
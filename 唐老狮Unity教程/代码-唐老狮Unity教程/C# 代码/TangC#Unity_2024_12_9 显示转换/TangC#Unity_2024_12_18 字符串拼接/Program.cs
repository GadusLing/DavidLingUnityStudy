// See https://aka.ms/new-console-template for more information
//Console.WriteLine("字符串拼接");

//#region 知识点一 字符串拼接方式1
////之前的算数运算符 只是用来数值类型变量进行数学运算的
////而 string 不存在算数运算符不能计算 但是可以通过+号来进行字符串拼接
//string str = "123";
////用+号进行字符串拼接
//str = str + "456";
//Console.WriteLine(str);
//str = str + 1;
//Console.WriteLine(str);

//// 复合运算符 += 
//str = "123";
//str += "1" + 4 + true;
//Console.WriteLine(str);

//str += 1 + 2 + 3 + 4;
//Console.WriteLine(str);

//str += "" + 1 + 2 + 3 + 4;
//Console.WriteLine(str);

//str = "";
//str += 1 + 2 + "" + (3 + 4);
//Console.WriteLine(str);

//str = "123";

//str = str + (1 + 2 + 3);
//Console.WriteLine(str);

////注意 ： 用+号拼接 是用符号唯一方法 不能用-*/%....
//#endregion

//#region 知识点二 字符串拼接方式2
////固定语法
////string.Format("待拼接的内容", 内容1, 内容2,......);
////拼接内容中的固定规则
////想要被拼接的内容用占位符替代 {数字} 数字:0~n 依次往后 
//string str2 = string.Format("我是{0}, 我今年{1}岁, 我想要{2}", "凌大伟", 27, "天天学习，好好向上");
//Console.WriteLine(str2);

//str2 = string.Format("asdf{0},{1},sdfasdf{2}", 1, true, false);
//Console.WriteLine(str2);

//#endregion

//#region 控制台打印拼接
////后面的 内容 比占位符多 不会报错 
////后面的 内容 比占位符少 会报错
///*Console.WriteLine("A{0}B{1}C{2}", 1, true, false, 1, 2);
//Console.Write("A{0}B{1}C{2}", 1, true);*/
//#endregion



Console.WriteLine("字符串拼接 练习题");

#region 练习题一
//定义一个变量存储客户的姓名，然后再屏幕上显示：“你好，XXX”
//XXX代表客户的姓名

string name = "凌大伟";
Console.WriteLine("你好," + name);
Console.WriteLine("你好,{0}", name);
string str = string.Format("你好,{0}", name);
Console.WriteLine(str);
#endregion

#region 练习题二
//定义两个变量，一个存储客户的姓名，另一个存储年龄，
//然后再屏幕上显示：“xxx + yyy岁了”。xxx代表客户的姓名，yyy代表年龄
//举例（凌大伟18岁了）
string name2 = "凌大伟";
int age = 99;

Console.WriteLine(name2 + age + "岁了");

str = string.Format("{0}{1}岁了", name2, age);
Console.WriteLine(str);

Console.WriteLine("{0}{1}岁了", name2, age);

#endregion

#region 练习题三
//当我们去面试时，前台会要求我们填一张表格，
//有姓名，年龄，邮箱，家庭住址，期望工资，
//请把这些信息在控制台输出。

string name3 = "凌大伟";
int age2 = 190;
string email = "hcxiaoling@gmail.com";
string address = "地球深处";
long money = 999999999999999;

Console.WriteLine("姓名:{0}\n年龄：{1}\n邮箱：{2}\n家庭住址：{3}\n期望薪资：{4}\n", name3,
    age2, email, address, money);

#endregion

#region 练习题四
//请用户输入用户名、年龄、班级，最后一起用占位符形式打印出来
Console.WriteLine("请输入你的用户名");
string adminName = Console.ReadLine();
Console.WriteLine("请输入你的年龄");
string ageStr = Console.ReadLine();
Console.WriteLine("请输入你的班级");
string classStr = Console.ReadLine();

Console.WriteLine("{0},{1},{2}", adminName, ageStr, classStr);
#endregion
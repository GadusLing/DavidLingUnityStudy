// See https://aka.ms/new-console-template for more information
Console.WriteLine("异常捕获");


#region 作用

//将玩家输入的内容 存储 string类型的变量(容器)中
//string str = Console.ReadLine();

//Parse转字符串为 数值类型时 必须 要合法合规
//int i = int.Parse(str);


#endregion


#region 基本语法

//必备部分
try
{
    //希望进行异常捕获的代码块
    //放到try中
    //如果try中的代码 报错了 不会让程序卡死



}
catch//(Exception e)
{
    //如果出错了 会执行 catch中的代码 来捕获异常
    //catch(Exception e)具体报错跟踪 通过e得到 具体的错误信息


}

//可选部分
finally
{
    //最后执行的代码 不管有没有出错 都会执行其中的代码
    //目前 大家可以不用写

}
//注意:异常捕获代码基本结构中 不需要加;在里面去写代码逻辑时 每一句代码才加;


#endregion



#region 实践

//try
//{
//    string str = Console.ReadLine();
//    int i = int.Parse(str);
//    Console.WriteLine(i);
//}
//catch
//{
//    Console.WriteLine("请输入合法数字");
//}
//finally 
//{ 
//    Console.WriteLine("执行完毕");
//}

#endregion


//请用户输入一个数字  如果输入有误，则提示用户输入错误
try
{
    Console.WriteLine("请输输入一个数字");
    int i1 = int.Parse(Console.ReadLine());
    Console.WriteLine("你输入的数字为:" + i1);

}
catch
{
    Console.WriteLine("输入错误");
}




//提示用户输入姓名、语文、数学、英语成绩  如果输入的成绩有误，则提示用户输入错误  否则将输入的字符串转为整形变量存储

/*try
{
    Console.WriteLine("请输入您的姓名、语文、数学、英语成绩");
    int Chinese = int.Parse(Console.ReadLine());
    int Mathematics = int.Parse(Console.ReadLine());
    int English = int.Parse(Console.ReadLine());
    Console.WriteLine("语文:" + Chinese + "\n数学:" + Mathematics + "\n英语:" + English);
}
catch
{
    Console.WriteLine("输入错误");
}*/

try
{
    Console.WriteLine("请输入用户名");
    string yourName = Console.ReadLine();
    Console.WriteLine("请输入语文成绩");
    string yuWenStr = Console.ReadLine();
    int yuWen = int.Parse(yuWenStr);
}
catch
{
    Console.WriteLine("语文成绩输入格式不正确");
}

try
{
    Console.WriteLine("请输入数学成绩");
    int shuXue = int.Parse(Console.ReadLine());
}
catch
{
    Console.WriteLine("数学成绩输入格式不正确");
}


try
{
    Console.WriteLine("请输入英语成绩");
    int yingYu = int.Parse(Console.ReadLine());
}
catch
{
    Console.WriteLine("英语成绩输入格式不正确");
}


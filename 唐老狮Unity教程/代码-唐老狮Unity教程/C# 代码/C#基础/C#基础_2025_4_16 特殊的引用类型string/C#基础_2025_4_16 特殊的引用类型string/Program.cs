// See https://aka.ms/new-console-template for more information

Console.WriteLine("特殊的引用类型string");
#region 知识点一 复习值和引用类型
//值类型——它变我不变——存储在栈内存中
// 无符号整形  有符号整形 浮点数 char bool 结构体（未学习）

//引用类型——它变我也变——存储在堆内存中
// 数组（一维、二维、交错）  string  类（未学习）

#endregion

#region 知识点二 string的它变我不变
//string str1 = "123";
//string str2 = str1;
////因为string是引用类型 按理说 应该是它变我也变
//// string非常的特殊 它具备 值类型的特征 它变我不变
//str2 = "321";

//Console.WriteLine(str1);
//Console.WriteLine(str2);

//string 虽然方便 但是有一个小缺点 就是频繁的 改变string 重新赋值
//会产生 内存垃圾
// 优化替代方案 我们会在 C#核心当中进行讲解 

#endregion

//通过断点调试 在监视窗口中查看 内存信息
string str1 = "123";
string str2 = str1;
//因为string是引用类型 按理说 应该是它变我也变
// string非常的特殊 它具备 值类型的特征 它变我不变
str2 = "321";

Console.WriteLine(str1);
Console.WriteLine(str2);
//string虽然方便但是有一个小缺点就是频繁的改变string重新赋值
//会产生内存垃圾
//优化替代方案我们会在C#核心当中进行讲解

//通过断点调试在监视窗口中查看内存信息


Console.WriteLine("引用类型练习题");

int[] a = new int[] { 10 };
int[] b = a;
b = new int[5];
Console.WriteLine(a[0]);// 10

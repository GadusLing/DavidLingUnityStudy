// See https://aka.ms/new-console-template for more information



#region 知识点一 转义字符的使用
//什么是转义字符?
//它是字符串的一部分 用来表示一些特殊含义的字符
//比如:在字符串中表现 单引号 引号 空行等等
//string str ="asld"fk"

#region 固定写法
//固定写法 \字符
//不同的\和字符的组合 表示不同的含义

//常用转义字符

// 单引号\'
string str = "\'哈哈哈\'";
Console.WriteLine(str);

// 双引号
str = "\"哈哈哈\"";
Console.WriteLine(str);

// 换行
str = "1231231\n23123123123";
Console.WriteLine(str);

// 斜杠
str ="哈\\哈哈";
Console.WriteLine(str);

//不常用转义字符(了解)

// 制表符(空一个tab键)
str = "哈\t哈哈";
Console.WriteLine(str);

// 光标退格
str = "123\b123";
Console.WriteLine(str);

// 空字符
str = "1234\0123";
Console.WriteLine(str);

// 警报音
str = "\a";
Console.WriteLine(str);


Console.WriteLine("123123123\n123123213\a\t123123");
#endregion


#region 知识点二 取消转义字符
string str2 = @"哈哈\哈哈";
Console.WriteLine(str2);

Console.WriteLine(@"\n\\");
#endregion

#endregion

//作业
//很简单

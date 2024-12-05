// See https://aka.ms/new-console-template for more information
Console.WriteLine("变量命名规则");


#region 知识点一:必须遵守的规则
//1.不能重名
//2.不能以数字开头
//3.不能使用程序关键字命名
//4.不能有特殊符号(下划线除外)

//建议的命名规则:变量名要有含义->用英文(拼音)表示变量的作用
//非常不建议的命名规则:用汉字命名
int i = 1;
string a = "123";


#endregion


#region 常用命名规则

//驼峰命名法一首字母小写，之后单词首字母大写(变量)
string myName = "唐老狮";
string yourName = "你的名字";
string yourMotherName = "";

//帕斯卡命名法-所有单词首字母都大写(函数、类)
string MyName = "dskafj";
//潜在知识点-c#中对大小写是敏感的 是区分的

#endregion


//作业

//下面的变量名哪些是错误的?

//U3d  √
//$money   ×
//class   ×
//Main   √ 函数名不是关键字
//No.1  ×
//discount_1  √
//int  ×
//示  ×
//3day  ×
//Shang Hai  ×
//_a_b_c_   √
//print  √   C#里面print不是关键字


//按照驼峰命名法命名以下变量(使用英文)
//我的年龄、我的性别、我的攻击力、我的防御力、你的身高、你的体重

int myAge;
int mySex;
int myAttack;
int myDefense;
int yourHeight;
int yourWeight;





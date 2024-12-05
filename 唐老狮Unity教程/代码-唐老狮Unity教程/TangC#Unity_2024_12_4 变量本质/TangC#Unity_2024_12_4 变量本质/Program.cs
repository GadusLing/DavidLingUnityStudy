// See https://aka.ms/new-console-template for more information
Console.WriteLine("变量本质");


#region 变量类型回顾
// 整数类型
// 有符号整数类型
//sbyte s1;      // 范围: -128 到 127, 占用字节数: 1 字节
//short s2;      // 范围: -32,768 到 32,767, 占用字节数: 2 字节
//int s3;        // 范围: -2,147,483,648 到 2,147,483,647, 占用字节数: 4 字节
//long s4;       // 范围: -9,223,372,036,854,775,808 到 9,223,372,036,854,775,807, 占用字节数: 8 字节

//// 无符号整数类型
//byte u1;       // 范围: 0 到 255, 占用字节数: 1 字节
//ushort u2;     // 范围: 0 到 65,535, 占用字节数: 2 字节
//uint u3;       // 范围: 0 到 4,294,967,295, 占用字节数: 4 字节
//ulong u4;      // 范围: 0 到 18,446,744,073,709,551,615, 占用字节数: 8 字节

//// 浮点数类型
//float f1;      // 范围: ±1.5 × 10^−45 到 ±3.4 × 10^38, 精度: 7 位有效数字, 占用字节数: 4 字节
//double f2;     // 范围: ±5.0 × 10^−324 到 ±1.7 × 10^308, 精度: 15-17 位有效数字, 占用字节数: 8 字节
//decimal f3;    // 范围: ±1.0 × 10^−28 到 ±7.9 × 10^28, 精度: 28-29 位有效数字, 占用字节数: 16 字节

//// 特殊类型
//bool b1;       // 值: true 或 false, 占用字节数: 1 字节
//char c1;       // 1 个字符 (Unicode 字符), 范围: '\u0000' 到 '\uFFFF', 占用字节数: 2 字节
//string str1;   // 字符串类型, 占用字节数: 根据字符串长度和编码方式变化

// 有符号 
//sbyte   (-128~127) 
//int     (-21亿多~21亿多) 
//short   (-3万多~3万多)
//long    (-9百万兆多~9百万兆多)

// 无符号 
//byte    (0~255) 
//uint    (0~42亿多) 
//ushort  (0~6万多)
//ulong   (0~18百万兆多)

// 浮点数 
//float   (7~8位有效数字) 
//double  (15~17位有效数字)
//decimal (27~28位有效数字)

// 特殊 
//bool    (true和false) 
//char    (一个字符)
//string  (一串字符)

#endregion


#region 知识点一 变量的存储空间(内存中)
// 1byte = 8bit
// 1KB = 1024byte
// 1MB = 1024KB
// 1GB = 1024MB
// 1TB = 1024GB

// 通过sizeof方法 可以获取变量类型所占的内存空间(单位:字节)
//有符号
int sbyteSize = sizeof(sbyte);
Console.WriteLine("sbyte 所占的字节数为:" + sbyteSize);

int intSize = sizeof(int);
Console.WriteLine("int 所占的字节数为:" + intSize);

int shortSize = sizeof(short);
Console.WriteLine("short 所占的字节数为:" + shortSize);

int longSize = sizeof(long);
Console.WriteLine("long 所占的字节数为:" + longSize);

Console.WriteLine("\n");

//无符号
int byteSize = sizeof(byte);
Console.WriteLine("byte 所占的字节数为:" + byteSize);

int uintSize = sizeof(uint);
Console.WriteLine("uint 所占的字节数为:" + uintSize);

int ushortSize = sizeof(ushort);
Console.WriteLine("ushort 所占的字节数为:" + ushortSize);

int ulongSize = sizeof(ulong);
Console.WriteLine("ulong 所占的字节数为:" + ulongSize);

Console.WriteLine("\n");

//浮点数
int floatsize = sizeof(float);
Console.WriteLine("float 所占的字节数为:" + floatsize);

int doublesize = sizeof(double);
Console.WriteLine("double 所占的字节数为:" + doublesize);

int decimalsize = sizeof(decimal);
Console.WriteLine("decimal 所占的字节数为:" + decimalsize);


//特殊类型
int boolsize = sizeof(bool);
Console.WriteLine("bool 所占的字节数为:" + boolsize);

int charSize = sizeof(char);
Console.WriteLine("char 所占的字节数为:" + charSize);

//sizeof是不能够得到string类型所占的内存大小的
//因为字符串长度是可变的 不定
//int stringsize=sizeof(string);


#endregion


#region 知识点二 变量的本质
//变量的本质是2进制->计算机中所有数据的本质都是二进制 是一堆θ和1
//为什么是2进制?
//数据传递只能通过电信号，只有开和关两种状态。所以就用0和1来表示这两种状态
//计算机中的存储单位最小为bit(位)，他只能表示0和1两个数字
// 1bit 就是1个数 要不是0要不是1
// 为了方便数据表示
// 出现一个叫byte(字节)的单位，它是由8个bit组成的存储单位。
//所以我们一般说一个字节为8位
//1byte = 0000 0000

//2进制和10进制的对比
//2进制和10进制之间的相互转换
#endregion


//作业
//请默写出常用的14个变量类型，以及他们所占用的内存空间。

/*sbyte 1 -128-127
short 2 -32768-32767
int 4 -2147483648-2147483647 负21亿4千7百万到21亿4千7百万
long 8 -9,223,372,036,854,775,808 到 9,223,372,036,854,775,807  -9百万兆多~9百万兆多

byte 1 255
ushort 2 65535
uint 4 4,294,967,295 0~42亿多
ulong 8 18,446,744,073,709,551,615 0~一千八百万兆多

float 4 (7~8位有效数字) 
double 8 (15~17位有效数字)
decimal 16 (27~28位有效数字)

bool 1 
char 1 
string .. ..*/





//请将2进制11000111、001101、01010101转为10进制，写出计算过程
/*11000111 = 2^0 + 2^1 + 2^2 + 0 + 0 + 0 + 2^6 + 2^7  = 199

001101 = 2^0 + 0 + 2^2 + 2^3 + 0 + 0 = 13

01010101 = 2^0 + 0 + 2^2 + 0 + 2^4 + 0 + 2^6 + 0 = 85*/



//请将10进制99、1024、78937转为2进制，写出计算过程

/*99 = 110 0011
1024 = 100 0000 0000
78937 = 1 0011 0100 0101 1001*/







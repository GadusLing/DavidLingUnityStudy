// See https://aka.ms/new-console-template for more information
Console.WriteLine("算数运算符");

//算数运算符 是用于 数值类型变量计算的运算符
//它的返回结果是数值

//#region 知识点一：赋值符号
//// =
//// 关键知识点 : 
//// 先看右侧 再看左侧 把右侧的值赋值给左侧的变量
//string myName = "唐老狮";
//int myAge = 18;
//float myHeight = 177.5f;

//#endregion

//#region 知识点二： 算数运算符

//#region 加 +
//// 用自己计算 先算右侧结果 再赋值给左侧变量
//int i = 1;
//// 3
//i = i + 2;
//Console.WriteLine(i);
//// 连续运算 先算右侧结果 再赋值给左侧变量
//// 99
//i = 1 + 3 + 89 + i + i;
//Console.WriteLine(i);
////4
//i = 1 + 2 + 1;
//Console.WriteLine(i);

//// 初始化时就运算  先算右侧结果 再赋值给左侧变量
//int i2 = 1 + 2 + 4 + i;
//Console.WriteLine(i2);
//#endregion

//#region 减 -
//// 用自己计算 先算右侧结果 再赋值给左侧变量
//int j = 1;
//j = j - 1;
//Console.WriteLine(j);
//// 连续运算 先算右侧结果 再赋值给左侧变量
//j = 1 - 2 - 3;
//Console.WriteLine(j);
//j = 1 - j;
//Console.WriteLine(j);//5
//                     // 初始化时就运算  先算右侧结果 再赋值给左侧变量
//int j2 = 1 - j - 0;
//Console.WriteLine(j2);
//#endregion

//#region 乘 *
//// 用自己计算 先算右侧结果 再赋值给左侧变量
//int c = 1;
//c = c * 10;
//Console.WriteLine(c);
//// 连续运算 先算右侧结果 再赋值给左侧变量
//c = 1 * 2 * 3;
//Console.WriteLine(c);
//c = 2 * c * 2;
//Console.WriteLine(c);
//// 初始化时就运算  先算右侧结果 再赋值给左侧变量
//int c2 = c * 2;
//Console.WriteLine(c2);
//#endregion

//#region 除 / 
//// 用自己计算 先算右侧结果 再赋值给左侧变量
//int chu = 1;
//chu = 10 / chu;
//Console.WriteLine(chu);
//// 连续运算 先算右侧结果 再赋值给左侧变量

//// 初始化时就运算  先算右侧结果 再赋值给左侧变量

//chu = 1;
//chu = 1 / 2;
//Console.WriteLine(chu);
////默认的整数 是int 如果用来做除法运算 要注意 会丢失小数点后的小数
////如果你想用浮点数来存储 一定是 在运算时要有浮点数的特征
//float f = 1 / 2f;
//Console.WriteLine(f);

//#endregion

//#region 取余 %
//// 用自己计算 先算右侧结果 再赋值给左侧变量
//int y = 4;
//// 4 / 3 得到余数 1
//y = y % 3;
//Console.WriteLine(y);
//// 连续运算 先算右侧结果 再赋值给左侧变量
//y = 4 % 3 % 2;
//Console.WriteLine(y);
//// 初始化时就运算  先算右侧结果 再赋值给左侧变量

//#endregion

//#endregion

//#region 知识点三：算数运算符的 优先级
////优先级 是指 在混合运算时的运算顺序

////乘除取余 优先级高于 加减 先算乘除取余 后算加减
//// 1 + 3 + 1 + 6
//int a = 1 + 2 * 3 / 2 + 1 + 2 * 3;
//Console.WriteLine(a);

//a = 1 + 4 % 2 * 3 / 2 + 1;
//Console.WriteLine(a);

////括号可以改变优先级 优先计算括号内内容
//a = 1 + 4 % (2 * 3 / 2) + 1;
//Console.WriteLine(a);

////多组括号 先算最里层括号 依次往外算
//a = 1 + (4 % (2 * (3 / 2))) + 1;
//Console.WriteLine(a);
//#endregion

//#region 知识点四：算数运算符的 复合运算符
//// 固定写法 运算符=
//// += -= *= /= %=
////复合运算符 是用于 自己=自己进行运算
//int i3 = 1;
//i3 = i3 + 2;
//Console.WriteLine(i3);

//i3 = 1;
//i3 += 2;//i3 = i3 + 2;
//Console.WriteLine(i3);

//i3 = 2;
//i3 += 2;//4
//i3 -= 2;//2
//i3 /= 2;//1
//i3 *= 2;//2
//i3 %= 2;//0
//Console.WriteLine(i3);

//int i4 = 10;
//// i4 += 4
//i4 += 20 * 2 / 10;
//Console.WriteLine(i4);

////注意：复合运算符 只能进行一种运算 不能混合运算
////i4 */-= 2;

//#endregion

//#region 知识点五：算术运算符的 自增减
//int a2 = 1;
//a2 = a2 + 1;

//a2 = 1;
//a2 += 1;

////自增运算符  让自己+1 
//a2 = 1;
//a2++;//先用再加
//Console.WriteLine(a2);
//++a2;//先加再用
//Console.WriteLine(a2);

//a2 = 1;
//Console.WriteLine(a2++);//1
//                        //2
//Console.WriteLine(++a2);//3

////自减运算符 让自己-1
//a2 = 1;
//a2--;//先用再减
//--a2;//先减再用

//a2 = 1;
//Console.WriteLine(a2--);//1
//                        //0
//Console.WriteLine(--a2);//-1

//#endregion

Console.WriteLine("算数运算符练习题");
#region 练习题一
//定义一个变量存储你的年龄，十年后你的年龄是多少？请输出到控制台
int myAge = 27;
myAge = myAge + 10;
//myAge += 10;
Console.WriteLine("十年后我的年龄是" + myAge);

#endregion

Console.WriteLine("**************");
#region 练习题二
//计算一个半径为5的圆的面积和周长
//面积公式：πr²
//周长公式：2πr
float r = 5;
float PI = 3.1415926f;
float mianJi = PI * r * r;
Console.WriteLine("圆的面积是" + mianJi);
float zhouChang = 2 * PI * r;
Console.WriteLine("圆的周长是" + zhouChang);

Console.WriteLine("**************");

Console.WriteLine("圆的面积是" + 3.14f * 5 * 5);
Console.WriteLine("圆的周长是" + 2 * 3.14f * 5);


#endregion

#region 练习题三
//计算任意三门成绩的总分、平均分打印到控制台。
//三门成绩均为整数类型（C#  Unity  Math）
int cSharp = 81;
int unity = 70;
int math = 90;
//总分
int sum = cSharp + unity + math;
//平均分
float avg = sum / 3f;
Console.WriteLine("总分为" + sum);
Console.WriteLine("平均分" + avg);

#endregion

Console.WriteLine("**************");
#region 练习题四
//商店T恤的价格为285元 / 件，裤子的价格为720元 / 条，
//小李在该店卖了2件T恤和3条裤子，请计算小李该付多少钱？打3.8折后呢？
int trousersPrice = 720;
int tShirtPrice = 285;

int sum2 = tShirtPrice * 2 + trousersPrice * 3;
Console.WriteLine("总价：" + sum2);
Console.WriteLine("总价：" + (285 * 2 + 720 * 3));
float sum3 = sum2 * 0.38f;
Console.WriteLine("打3.8折后：" + sum3);
Console.WriteLine("总价：" + (285 * 2 + 720 * 3) * 0.38f);
#endregion

Console.WriteLine("**************");
#region 练习题五
int a = 10, b = 20;
// 11 + 20
int number1 = ++a + b;
Console.WriteLine(number1);//31
a = 10;
b = 20;
//10 + 20
int number2 = a + b++;
Console.WriteLine(number2);//30
a = 10;
b = 20;
//10 + 21 + 11
int number3 = a++ + ++b + a++;
Console.WriteLine(number3);//42
Console.WriteLine(a);//12
                     //请说明 number1和number2和number3的结果


#endregion

#region 练习题六
//有两个数a = 99，b = 87，请写出两种方法交换他们的值，让a = 87，b = 99
int a2 = 99;
int b2 = 87;
//中间商 不赚差价
//a2 = b2;//这样不行
//b2 = a2;
int temp = a2; //temp = 99;
a2 = b2; // a2 = 87;
b2 = temp;//b2 = 99;

//算术运算符 加减法
a2 = 99;
b2 = 87;
a2 = a2 + b2;// a2 = 99 + 87;
b2 = a2 - b2;// b2 = 99 + 87 - 87;  b2 = 99;
a2 = a2 - b2;// a2 = 99 + 87 - 99;  a2 = 87;

#endregion

#region 练习题七
//请把987652秒通过代码转为n天n小时n分钟n秒显示在控制台中
//一天对应秒数 = 60 * 60 * 24
int oneDayS = 60 * 60 * 24;
//一小时对应秒数 = 60 * 60
int oneHourS = 60 * 60;
//一分钟对应的秒数 = 60
int oneMinS = 60;

// 算一天 用总的秒数 / 一天有多少秒 
int day = 987652 / oneDayS;
// 算小时 用不足一天的秒 / 一小时对应的秒数
// 987652 % oneDays 余数结果 就是剩下的不足一天的秒数
int hour = 987652 % oneDayS / oneHourS;
// 算分钟 用不足一小时的秒 / 一分钟对应的秒数
// 987652 % oneHourS 余数结果 就是剩下的不足一小时的秒数
int min = 987652 % oneHourS / oneMinS;
// 得到 秒数
int s = 987652 % oneMinS;

Console.WriteLine("987652秒对应的时间是：" + day + "天" + hour + "小时" + min + "分钟" + s + "秒");

#endregion


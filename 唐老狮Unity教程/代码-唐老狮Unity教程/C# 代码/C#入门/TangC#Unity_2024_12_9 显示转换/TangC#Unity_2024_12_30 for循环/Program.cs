// See https://aka.ms/new-console-template for more information
Console.WriteLine("for循环");

#region 知识点一 基本语法
//for( /*初始表达式*/; /*条件表达式*/; /*增量表达式*/ )
//{
//    //循环代码逻辑;
//}
// 第一个空（初始表达式）： 一般声明一个临时变量，用来计数用
// 第二个空（条件表达式）： 表明进入循环的条件 一个bool类型的结果（bool变量 条件运算符 逻辑运算符 算术运算符）
// 第三个空（增量表达式）： 用第一个空中的变量 进行 自增减运算

// 第一次进入循环时 才会调用 第一个空中的代码
// 每次进入循环之前 都会判断第二个空中的条件 满足才会进入循环逻辑
for (int i = 0; i < 10; i++)
{
    Console.WriteLine(i);
    //执行完循环语句块中的逻辑后
    //最后执行第三个空中的代码
}

for (int i = 10; i >= 0; i--)
{
    Console.WriteLine(i);
}

//每个空位 可以按照规则进行书写
//第一个空位 就是申明变量  所以可以连续申明
//第二个空位 就是进入条件 只要是bool结果的表达式 都可以
//第三个空位 就是执行一次循环逻辑过后要做的事情 做啥都行
//for( int i = 0, j = 0; i < 10 && j < 0; ++i, j = j + 1)
//{

//}

#endregion

#region 知识点二 支持嵌套
//for( int i = 0; i < 10; i++ )
//{
//    for( int j = 0; j < 10; j++)
//    {
//        Console.WriteLine(i + "_" + j);
//    }
//    while(true)
//    {

//    }
//    if(true)
//    {

//    }
//    do
//    {

//    } while (true);
//}

#endregion

#region 知识点三 特殊写法
//for循环 这三个空位 可以都空着 可以根据需求去填写

// for循环可以写死循环
//for( ; ; )
//{
//    Console.WriteLine("for循环的死循环");
//}

//int k = 0;
//for(; k < 10; )
//{


//    ++k;//k++, k += 1;
//}

//for( k = 0; ; ++k )
//{
//    if( k >= 10 )
//    {
//        break;
//    }
//}

#endregion

#region 知识点四 对比while循环
//for循环 一般用来可以准确得到 一个范围中的所有数
for (int i = 0; i < 10; ++i)
{

}

int j = 0;
while (j < 10)
{
    //......................
    ++j;
}

#endregion


Console.WriteLine("for循环练习题");
#region 练习题一
//输出1到100之间的整数（包含本身）
//for(int i = 1; i <= 100; ++i)
//{
//    Console.WriteLine(i);
//}
#endregion

#region 练习题二
//求1~100之间所有偶数的和
//int sum = 0;
//for (int i = 1; i <= 100; i++)
//{
//    //判断是否是偶数 是否能整除2
//    if( i % 2 == 0 )
//    {
//        sum += i;
//    }
//}
//for (int i = 2; i <= 100; i += 2)
//{
//    sum += i;
//}
//Console.WriteLine(sum);
#endregion

#region 练习题三
//找出100~999之间的水仙花数
//例如：153 = 1 * 1 * 1 + 5 * 5 * 5 + 3 * 3 * 3 这个数就是水仙花数
//int bai, shi, ge;
//for (int i = 100; i <= 999; i++)
//{
//    //判断 每一位 的立方加起来 是不是等于自己
//    //得到每一位  百位  十位 个位
//    bai = i / 100;
//    shi = i % 100 / 10;
//    ge = i % 10;
//    //是否满足水仙花数条件
//    if( bai * bai * bai + shi * shi * shi + ge * ge * ge == i  )
//    {
//        Console.WriteLine(i);
//    }
//}
#endregion

#region 练习题四
//在控制台上输出九九乘法表
//for (int i = 1; i <= 9; i++)
//{
//    //1 1 X 1 = 1 空行
//    //2 1 X 2 = 2 2 X 2 = 4 空行
//    //3 1 X 3 = 3 2 X 3 = 6 3 X 3 = 9 空行
//    for (int j = 1; j <= i; j++)
//    {
//        Console.Write("{0}X{1}={2}   ", j, i, i * j);
//    }
//    Console.WriteLine();
//}
#endregion

#region 练习题五
//在控制台上输出如下10 * 10的空心星型方阵
//**********
//*        *
//*        *
//*        *
//*        *
//*        *
//*        *
//*        *
//*        *
//**********
//行
//for (int j = 0; j < 10; j++)
//{
//    //列
//    for (int i = 0; i < 10; i++)
//    {
//        //列 如果是 第1行和最后1行 那么 内层列循环 都打印星号
//        // 按照 **********的规则打印
//        if( j == 0 || j == 9 )
//        {
//            Console.Write("*");
//        }
//        //否则 就是 按照*         *的规则打印
//        else
//        {
//            if (i == 0 || i == 9)
//            {
//                Console.Write("*");
//            }
//            else
//            {
//                Console.Write(" ");
//            }
//        }
//    }
//    Console.WriteLine();
//}



#endregion

#region 练习题六
//在控制台上输出如下10 * 10的三角形方阵
//*       1   1
//**      2   2
//***     3   3
//****    4   4
//*****
//******
//*******
//********
//*********
//**********
//行
//for (int i = 1; i <= 10; i++)
//{
//    //列
//    //**********
//    for (int j = 1; j <= i; j++)
//    {
//        Console.Write("*");
//    }
//    Console.WriteLine();
//}
#endregion

#region 练习题七
//在控制台上输出如下10行的三角形方阵
//         *            1    1   -> 2i - 1    9    10 - i
//        ***           2    3   -> 2i - 1    8    10 - i
//       *****          3    5                7    10 - i
//      *******         4    7                6    10 - i
//     *********        5    9                5
//    ***********       6    11               4
//   *************      7    13               3
//  ***************     8    15               2
// *****************    9    17               1
//*******************   10   19               0    10 - i
//行
for (int i = 1; i <= 10; i++)
{
    //打印空格的列
    for (int k = 1; k <= 10 - i; k++)
    {
        Console.Write(" ");
    }

    //打印星号的列
    for (int j = 1; j <= 2 * i - 1; j++)
    {
        Console.Write("*");
    }
    Console.WriteLine();
}

#endregion
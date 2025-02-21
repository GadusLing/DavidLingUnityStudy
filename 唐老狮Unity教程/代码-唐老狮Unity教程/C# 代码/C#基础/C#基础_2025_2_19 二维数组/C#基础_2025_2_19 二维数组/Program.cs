// See https://aka.ms/new-console-template for more information




//Console.WriteLine("二维数组");

//#region 知识点一 基本概念
////二维数组 是使用两个下标(索引)来确定元素的数组
////两个下标可以理解成 行标  和 列标
////比如矩阵
//// 1 2 3
//// 4 5 6
//// 可以用二维数组 int[2,3]表示 
//// 好比 两行 三列的数据集合
//#endregion

//#region 知识点二 二维数组的申明

////变量类型[,] 二维数组变量名;
//int[,] arr; //申明过后 会在后面进行初始化

////变量类型[,] 二维数组变量名 = new 变量类型[行,列];
//int[,] arr2 = new int[3, 3];

////变量类型[,] 二维数组变量名 = new 变量类型[行,列]{ {0行内容1, 0行内容2, 0行内容3.......}, {1行内容1, 1行内容2, 1行内容3.......}.... };
//int[,] arr3 = new int[3, 3] { { 1, 2, 3 },
//                              { 4, 5, 6 },
//                              { 7, 8, 9 } };

////变量类型[,] 二维数组变量名 = new 变量类型[,]{ {0行内容1, 0行内容2, 0行内容3.......}, {1行内容1, 1行内容2, 1行内容3.......}.... };
//int[,] arr4 = new int[,] { { 1, 2, 3 },
//                           { 4, 5, 6 },
//                           { 7, 8, 9 } };

////变量类型[,] 二维数组变量名 = { {0行内容1, 0行内容2, 0行内容3.......}, {1行内容1, 1行内容2, 1行内容3.......}.... };
//int[,] arr5 = { { 1, 2, 3 },
//                { 4, 5, 6 },
//                { 7, 8, 9 } };
//#endregion

//#region 知识点三 二维数组的使用
//int[,] array = new int[,] { { 1, 2, 3 },
//                            { 4, 5, 6 } };
////1.二维数组的长度
////我们要获取 行和列分别是多长
////得到多少行 0
//Console.WriteLine(array.GetLength(0));
////得到多少列 1
//Console.WriteLine(array.GetLength(1));

////2.获取二维数组中的元素
//// 注意：第一个元素的索引是0 最后一个元素的索引 肯定是长度-1
//Console.WriteLine(array[0, 1]);
//Console.WriteLine(array[1, 2]);

////3.修改二维数组中的元素
//array[0, 0] = 99;
//Console.WriteLine(array[0, 0]);
//Console.WriteLine("**********");
////4.遍历二维数组
//for (int i = 0; i < array.GetLength(0); i++)
//{
//    for (int j = 0; j < array.GetLength(1); j++)
//    {
//        //i 行 0 1
//        //j 列 0 1 2
//        Console.WriteLine(array[i, j]);
//        //0,0  0,1  0,2
//        //1,0  1,1  1,2
//    }
//}

////5.增加数组的元素
//// 数组 声明初始化过后 就不能再原有的基础上进行 添加 或者删除了
//int[,] array2 = new int[3, 3];
//for (int i = 0; i < array.GetLength(0); i++)
//{
//    for (int j = 0; j < array.GetLength(1); j++)
//    {
//        array2[i, j] = array[i, j];
//    }
//}
//array = array2;
//array[2, 0] = 7;
//array[2, 1] = 8;
//array[2, 2] = 9;
//Console.WriteLine("**********");
//for (int i = 0; i < array.GetLength(0); i++)
//{
//    for (int j = 0; j < array.GetLength(1); j++)
//    {
//        //i 行 0 1
//        //j 列 0 1 2
//        Console.WriteLine(array[i, j]);
//        //0,0  0,1  0,2
//        //1,0  1,1  1,2
//    }
//}

////6.删除数组的元素
////留给大家思考 自己去做一次

////7.查找数组中的元素
//// 如果要在数组中查找一个元素是否等于某个值
//// 通过遍历的形式去查找

//#endregion

////总结：
////1.概念：同一变量类型的 行列数据集合
////2.一定要掌握的内容：申明，遍历，增删查改
////3.所有的变量类型都可以申明为 二维数组
////4.游戏中一般用来存储 矩阵，再控制台小游戏中可以用二维数组 来表示地图格子


using System.Diagnostics;
using System.Reflection.Metadata;

Console.WriteLine("二维数组练习题");

#region 练习题一
////将1到10000赋值给一个二维数组（100行100列）
//int[,] arr = new int[100, 100];
//int x = 1;
//for (int i = 0; i < arr.GetLength(0); i++)
//{
//    for (int j = 0; j < arr.GetLength(1); j++)
//    {
//        arr[i, j] += x;
//        ++x;
//        Console.Write(arr[i, j] + " ");
//    }
//}
#endregion

#region 练习题二
////将二维数组（4行4列）的右上半部分置零（元素随机1~100）
//int[,] arr = new int[4, 4];
//Random random = new Random();
//for (int i = 0; i < arr.GetLength(0); i++)
//{
//    for (int j = 0; j < arr.GetLength(1); j++)
//    {
//        arr[i, j] = random.Next(1, 101);
//        if ((i < arr.GetLength(0) / 2) && (j >= arr.GetLength(1) / 2)) arr[i, j] = 0;// 通用  arr.GetLength(0)是行数,arr.GetLength(1)是列数,记住了
//        Console.Write("{0,-4}", arr[i, j]);// {0} 表示第一个参数（即 arr[i, j]）, -4 表示占 4 个字符的宽度，左对齐,如果要右对齐就是正数 4
//    }
//    Console.WriteLine();// 空行
//}

#endregion

#region 练习题三
////求二维数组（3行3列）的对角线元素的和（元素随机1~10）
//int size = 3; // 可更改为任意 n×n
//int[,] arr = new int[size, size];
//Random random = new Random();
//int sum = 0;
//for (int i = 0; i < arr.GetLength(0); i++)
//{
//    for (int j = 0; j < arr.GetLength(1); j++)
//    {
//        arr[i, j] = random.Next(1, 11);
//        Console.Write("{0,-4}", arr[i, j]);// {0} 表示第一个参数（即 arr[i, j]）, -4 表示占 4 个字符的宽度，左对齐,如果要右对齐就是正数 4
//        if (i == j) sum += arr[i, j];
//        if (i + j == size - 1) sum += arr[i, j];
//    }
//    Console.WriteLine();// 空行
//}
//int centerValue = (size % 2 == 1) ? arr[size / 2, size / 2] : 0;// size % 2 == 1：检查矩阵的大小是否为奇数。例如，3×3、5×5 这样的矩阵有一个明确的中心元素，而 4×4、6×6 这样的偶数矩阵没有唯一的中心点。
//// arr[size / 2, size / 2]：计算中心点的位置  例如，对于 size = 3：size / 2 = 3 / 2 = 1（整数除法）。中心元素位于 matrix[1,1]。对于 size = 5：size / 2 = 5 / 2 = 2。中心元素位于 matrix[2,2]。
//// 如果是奇数矩阵中心点会被计算两次,所以需要减去一次,偶数矩阵没有这样的问题,赋0不影响计算结果
//sum -= centerValue;
//Console.WriteLine("对角线元素总和: " + sum);

#endregion

#region 练习题四
//求二维数组（5行5列）中最大元素值及其行列号（元素随机1~500)
//int size = 5; // 可更改为任意 n×n
//int[,] arr = new int[size, size];
//int max = int.MinValue;
//for (int i = 0; i < arr.GetLength(0); i++)
//{
//	for (int j = 0; j < arr.GetLength(1); j++)
//	{
//		arr[i,j] = new Random().Next(1, 501);// 使用Random匿名对象,不用额外写一行 Random random = new Random();
//		Console.Write("{0,-4}", arr[i, j]);
//        if (arr[i, j] > max) max = arr[i, j];
//    }
//	Console.WriteLine();
//}
//Console.WriteLine("最大元素值为: " + max);
//for (int i = 0; i < arr.GetLength(0); i++)
//{
//    for (int j = 0; j < arr.GetLength(1); j++)
//    {
//        if (arr[i, j] == max) Console.WriteLine("最大元素的行号为: " + i + " 列号为: " + j);
//    }
//}

#endregion

#region 练习题五
//给一个M*N的二维数组，数组元素的值为0或者1，
//要求转换数组，将含有1的行和列全部置1
int[,] arr = new int[5, 5] { { 0,0,0,0,0},
                             { 0,0,0,0,0},
                             { 0,0,1,1,0},
                             { 0,0,0,0,0},
                             { 0,0,0,0,0} };// 手动做一个符合题意的数组

int rows = arr.GetLength(0);// 求行数
int columns = arr.GetLength(1);// 求列数

bool[] rowHas_1 = new bool[rows];// 定义一个布尔数组用来表明为1的元素,行最多有rows个,所以用rows初始化
bool[] columnHas_1 = new bool[columns];// 列用columns初始化
for (int i = 0; i < rows; i++)
{
    for (int j = 0; j < columns; j++)
    {
        if (arr[i,j] == 1)
        {
            rowHas_1[i] = true;
            columnHas_1[j] = true;
        }
    }
}
for (int i = 0; i < rows; i++)
{
    for (int j = 0; j < columns; j++)
    {
        if (rowHas_1[i] || columnHas_1[j])
        {
            arr[i, j] = 1;
        }
    }
}

for (int i = 0; i < rows; i++)
{
    for (int j = 0; j < columns; j++)
    {
        Console.Write(arr[i, j] + " ");
    }
    Console.WriteLine();
}



#endregion


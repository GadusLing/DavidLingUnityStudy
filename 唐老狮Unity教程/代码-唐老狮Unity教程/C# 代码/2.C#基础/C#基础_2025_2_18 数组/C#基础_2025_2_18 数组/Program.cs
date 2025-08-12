// See https://aka.ms/new-console-template for more information



/*Console.WriteLine("一维数组");

#region 知识点一 基本概念
//数组是存储一组相同类型数据的集合
//数组分为 一维、多维、交错数组
//一般情况 一维数组 就简称为 数组
#endregion

#region 知识点二 数组的申明

// 变量类型[] 数组名;//只是申明了一个数组 但是并没有开房
// 变量类型 可以是我们学过的 或者 没学过的所有变量类型
int[] arr1;

// 变量类型[] 数组名 = new 变量类型[数组的长度];
int[] arr2 = new int[5]; //这种方式 相当于开了5个房间 但是房间里面的int值 默认为0

// 变量类型[] 数组名 = new 变量类型[数组的长度]{内容1,内容2,内容3,.......};
int[] arr3 = new int[5] { 1, 2, 3, 4, 5 };

// 变量类型[] 数组名 = new 变量类型[]{内容1,内容2,内容3,.......};
int[] arr4 = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; //后面的内容就决定了 数组的长度 “房间数”

// 变量类型[] 数组名 = {内容1,内容2,内容3,.......};
int[] arr5 = { 1, 3, 4, 5, 6 };//后面的内容就决定了 数组的长度 “房间数”


bool[] arr6 = { true, false };
#endregion

#region 知识点三 数组的使用

int[] array = { 1, 2, 3, 4, 5 };
//1.数组的长度
// 数组变量名.Length
Console.WriteLine(array.Length);

//2.获取数组中的元素
//数组中的下标和索引 他们是从0开始的
//通过 索引下标去 获得数组中某一个元素的值时
//一定注意！！！！！！！！
//不能越界  数组的房间号 范围 是 0 ~ Length-1
Console.WriteLine(array[0]);
Console.WriteLine(array[2]);
Console.WriteLine(array[4]);

//3.修改数组中的元素
array[0] = 99;
Console.WriteLine(array[0]);

//4.遍历数组 通过循环 快速获取数组中的每一个元素
Console.WriteLine("**********************");
for (int i = 0; i < array.Length; i++)
{
    Console.WriteLine(array[i]);
}
Console.WriteLine("**********************");
//5.增加数组的元素
// 数组初始化以后 是不能够 直接添加新的元素的
int[] array2 = new int[6];
//搬家
for (int i = 0; i < array.Length; i++)
{
    array2[i] = array[i];//相当于异地扩容然后复制了
}
array = array2;
for (int i = 0; i < array.Length; i++)
{
    Console.WriteLine(array[i]);
}
array[5] = 999;
//C# 使用垃圾回收器自动管理内存，开发者无需手动释放内存，而 C++ 依靠开发者手动管理内存，比如delete[]
// 故而这里不用手动delete[] array的空间

Console.WriteLine("**********************");
//6.删除数组的元素
// 数组初始化以后 是不能够 直接删除元素的
// 搬家的原理
int[] array3 = new int[5];
//搬家
for (int i = 0; i < array3.Length; i++)
{
    array3[i] = array[i];
}
array = array3;
Console.WriteLine(array.Length);

//7.查找数组中的元素
// 99 2 3 4 5 
// 要查找 3这个元素在哪个位置
// 只有通过遍历才能确定 数组中 是否存储了一个目标元素
int a = 3;

for (int i = 0; i < array.Length; i++)
{
    if (a == array[i])
    {
        Console.WriteLine("和a相等的元素在{0}索引位置", i);
        break;
    }
}

#endregion

//总结
//1.概念：同一变量类型的数据集合
//2.一定要掌握的知识：申明，遍历，增删查改
//3.所有的变量类型都可以申明为 数组
//4.她是用来批量存储游戏中的同一类型对象的 容器  比如 所有的怪物 所有玩家*/



//作业
Console.WriteLine("数组练习题");

#region 练习题一
//请创建一个一维数组并赋值，让其值与下标一样，长度为100
//int[] arr = new int[100];
//for (int i = 0; i < arr.Length; i++)
//{
//    arr[i] = i;
//    Console.WriteLine(arr[i]);
//}
#endregion

#region 练习题二
//创建另一个数组B，让数组A中的每个元素的值乘以2存入到数组B中
//int[] A = { 1, 2, 3, 4, 5 };
//int[] B = new int[5];
//for (int i = 0; i < B.Length; i++)
//{
//    B[i] = A[i] * 2;
//    Console.WriteLine(B[i]);
//}
#endregion

#region 练习题三
//随机（0~100）生成1个长度为10的整数数组

//int[] arr= new int[10];

//Random random = new Random();
//for (int i = 0; i < arr.Length; i++)
//{
//    arr[i] = random.Next(0, 101);
//    Console.WriteLine(arr[i]);
//}

#endregion

#region 练习题四
// 从一个整数数组中找出最大值、最小值、总合、平均值
//（可以使用随机数1~100）

//int[] arr = new int[10];

//Random random = new Random();
//for (int i = 0; i < arr.Length; i++)
//{
//    arr[i] = random.Next(0, 101);
//    Console.WriteLine(arr[i]);
//}
//int Max = int.MinValue;
//int Min = int.MaxValue;
//int Sum = 0;
//int average = 0;
//for (int i = 0;i < arr.Length;i++)
//{
//    if (arr[i] > Max) Max = arr[i];
//    if (arr[i] < Min) Min = arr[i];
//    Sum += arr[i];
//}
//average = Sum / arr.Length;
//Console.WriteLine("最大值{0}、最小值{1}、总合{2}、平均值{3}", Max, Min, Sum, average);

#endregion

#region 练习题五
//交换数组中的第一个和最后一个、第二个和倒数第二个，依次类推，把数组进行反转并打印
//int[] arr = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
////Array.Reverse(arr);
////Console.WriteLine(string.Join(", ", arr));// 直接用reverse函数逆置

////双指针
//int left = 0, right = arr.Length - 1;
//while (left < right)
//{
//    // 交换左右元素
//    int temp = arr[left];
//    arr[left] = arr[right];
//    arr[right] = temp;

//    left++;
//    right--;
//}
//Console.WriteLine(string.Join(", ", arr));// Join:将数组 arr 中的元素连接成一个字符串，每个元素之间用逗号和空格分隔


#endregion

#region 练习题六
//将一个整数数组的每一个元素进行如下的处理：
//如果元素是正数则将这个位置的元素值加1；
//如果元素是负数则将这个位置的元素值减1；
//如果元素是0，则不变

//int[] arr = { 1, -2, 3, -4, 5, -6, 7, -8, 9 };
//for (int i = 0; i < arr.Length; i++)
//{
//    arr[i] = arr[i] > 0 ? arr[i] + 1 : arr[i] < 0 ? arr[i] - 1 : arr[i];// 嵌套三目 condition1 ? value_if_true1 : condition2 ? value_if_true2 : value_if_false2;
//    // 如果 condition1 为 true，则返回 value_if_true1；
//    // 如果 condition1 为 false，则继续判断 condition2：
//    // 如果 condition2 为 true，返回 value_if_true2；
//    // 如果 condition2 为 false，返回 value_if_false2。
//}
//Console.WriteLine(string.Join(", ", arr)); //Join: 将数组 arr 中的元素连接成一个字符串，每个元素之间用逗号和空格分隔


#endregion

#region 练习题七
//定义一个有10个元素的数组，使用for循环，输入10名同学的数学成绩，
//将成绩依次存入数组，然后分别求出最高分和最低分，
//并且求出10名同学的数学平均成绩
//int[] array = new int[10];

//try
//{
//    int min = 0;
//    int max = 0;
//    int sum = 0;
//    int avg = 0;
//    for (int i = 0; i < array.Length; i++)
//    {
//        Console.WriteLine("请输入第{0}位同学的成绩", i + 1);
//        array[i] = int.Parse(Console.ReadLine());
//        //第一次进来 min max 没有任何意义 所以第一次可以就认为该成绩即使 最高分也是最低分
//        if( i == 0 )
//        {
//            min = array[i];
//            max = array[i];
//        }
//        else
//        {
//            //只有 除了第一次以外  才来进行 大小值的判断 
//            if (min > array[i])
//            {
//                min = array[i];
//            }
//            if (max < array[i])
//            {
//                max = array[i];
//            }
//        }
//        sum += array[i];
//    }
//    avg = sum / array.Length;

//    Console.WriteLine("最高分{0}最低分{1}平均分{2}", max, min, avg);
//}
//catch
//{
//    Console.WriteLine("请输入数字");
//}
#endregion

#region 练习题八
//请声明一个string类型的数组(长度为25)（该数组中存储着符号），
//通过遍历数组的方式取出其中存储的符号打印出以下效果
//string[] strArr = new string[25];
//for (int i = 0; i < strArr.Length; i++)
//{
//    strArr[i] = i % 2 == 0 ? "■" : "□";// 初始化25个黑白交替的方块
//    if (i != 0 && i % 5 == 0) Console.Write("\n");
//    Console.Write(strArr[i]);
//}
#endregion




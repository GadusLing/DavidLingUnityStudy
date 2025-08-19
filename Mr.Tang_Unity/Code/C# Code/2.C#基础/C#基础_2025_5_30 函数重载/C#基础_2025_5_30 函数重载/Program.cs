//using System;

//namespace Lesson10_函数重载
//{
//    class Program
//    {
//        #region 知识点一 基本概念
//        //重载概念
//        //在同一语句块(class或者struct)中
//        //函数（方法）名相同
//        //参数的数量不同 
//        //或者
//        //参数的数量相同，但参数的类型或顺序不同

//        //作用：
//        //1.命名一组功能相似的函数，减少函数名的数量，避免命名空间的污染
//        //2.提升程序可读性
//        #endregion

//        #region 知识点二 实例
//        //注意：
//        //1.重载和返回值类型无关，只和参数类型，个数，顺序有关
//        //2.调用时 程序会自己根据传入的参数类型判断使用哪一个重载
//        static int CalcSum(int a, int b)
//        {
//            return a + b;
//        }

//        //参数数量不同
//        static int CalcSum(int a, int b, int c)
//        {
//            return a + b + c;
//        }

//        //数量相同 类型不同
//        static float CalcSum(float a, float b)
//        {
//            return a + b;
//        }

//        //数量相同 类型不同
//        static float CalcSum(int a, float f)
//        {
//            return a + f;
//        }

//        //数量相同 顺序不同
//        static float CalcSum(float f, int a)
//        {
//            return f + a;
//        }

//        //ref 和 out

//        // ref和out 可以理解成 他们也是一种变量类型 所以可以用在重载中 但是 ref和out不能同时修饰
//        static float CalcSum(ref float f, int a)
//        {
//            return f + a;
//        }

//        static float CalcSum(int a, int b, params int[] arr)
//        {
//            return 1;
//        }


//        #endregion


//        //总结
//        //概念：同一个语句块中，函数名相同，参数数量、类型、顺序不同的函数 就称为我们的重载函数
//        //注意：和返回值无关
//        //作用：一般用来处理不同参数的同一类型的逻辑处理

//        static void Main(string[] args)
//        {
//            Console.WriteLine("函数重载");

//            CalcSum(1, 2);
//            CalcSum(1.1f, 2);
//            CalcSum(1, 2, 3);
//            CalcSum(1, 1.2f);
//        }
//    }
//}


namespace Lesson10_练习题
{
    class Program
    {
        #region 练习题一
        //请重载一个函数
        //让其可以比较两个int或两个float或两个double的大小
        //并返回较大的那个值

        static int GetMax(int a, int b)
        {
            return a > b ? a : b;
        }

        static float GetMax(float a, float b)
        {
            return a > b ? a : b;
        }

        static double GetMax(double a, double b)
        {
            return a > b ? a : b;
        }

        #endregion

        #region 练习题二
        //请重载一个函数
        //让其可以比较n个int或n个float或n个double的大小
        //并返回最大的那个值。（用params可变参数来完成）

        static int GetMax(params int[] arr)
        {
            if (arr.Length == 0)
            {
                Console.WriteLine("没有传入任何参数");
                return -1;
            }
            //因为默认认为第一个数就是最大值了 所以没有必要和第一个数比较
            int max = arr[0];
            //所以直接从i=1开始遍历
            for (int i = 1; i < arr.Length; i++)
            {
                if (max < arr[i])
                {
                    max = arr[i];
                }
            }
            return max;
        }

        static float GetMax(params float[] arr)
        {
            if (arr.Length == 0)
            {
                Console.WriteLine("没有传入任何参数");
                return -1;
            }
            //因为默认认为第一个数就是最大值了 所以没有必要和第一个数比较
            float max = arr[0];
            //所以直接从i=1开始遍历
            for (int i = 1; i < arr.Length; i++)
            {
                if (max < arr[i])
                {
                    max = arr[i];
                }
            }
            return max;
        }

        static double GetMax(params double[] arr)
        {
            if (arr.Length == 0)
            {
                Console.WriteLine("没有传入任何参数");
                return -1;
            }
            //因为默认认为第一个数就是最大值了 所以没有必要和第一个数比较
            double max = arr[0];
            //所以直接从i=1开始遍历
            for (int i = 1; i < arr.Length; i++)
            {
                if (max < arr[i])
                {
                    max = arr[i];
                }
            }
            return max;
        }

        #endregion

        static void Main(string[] args)
        {
            Console.WriteLine("函数重载练习题");

            Console.WriteLine(GetMax(1, 2));
            Console.WriteLine(GetMax(1.1f, 2.2f));
            Console.WriteLine(GetMax(10.2, 20.3));

            Console.WriteLine(GetMax(1, 2, 3, 4, 5, 6));
            Console.WriteLine(GetMax(1.1f, 2.2f, 3, 4, 5, 6));
            Console.WriteLine(GetMax(1.1, 2, 3, 4, 5, 6));
        }
    }
}


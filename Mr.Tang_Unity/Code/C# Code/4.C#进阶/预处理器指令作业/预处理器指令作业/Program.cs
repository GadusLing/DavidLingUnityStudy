#define UNITY_5
#define UNITY_2017
#define UNITY_2020

#undef UNITY_5
#undef UNITY_2017
//#undef UNITY_2020

using System;


namespace 预处理器指令作业
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("请说出至少4种预处理器指令:");
            Console.WriteLine("#define, #undef, #if, #elif, #else, #endif, " +
                "#region, #endregion, #warning, #error, #pragma");
            Console.WriteLine();

            Console.WriteLine("请使用预处理器指令实现:");
            Console.WriteLine("写一个函数计算两个数");
            
            int a = 10;
            int b = 5;
            
            Console.WriteLine($"数字1: {a}");
            Console.WriteLine($"数字2: {b}");
            Console.WriteLine();

#if UNITY_5
            int result = Add(a, b);
            Console.WriteLine($"当是Unity5版本时算加法: {a} + {b} = {result}");
#elif UNITY_2017
            int result = Multiply(a, b);
            Console.WriteLine($"当是Unity2017版本时算乘法: {a} × {b} = {result}");
#elif UNITY_2020
            int result = Subtract(a, b);
            Console.WriteLine($"当时Unity2020版本时算减法: {a} - {b} = {result}");
#else
            Console.WriteLine("都不是返回0");
            int result = 0;
#endif

            Console.WriteLine($"最终结果: {result}");
        }

        /// <summary>
        /// 加法运算
        /// </summary>
        static int Add(int x, int y)
        {
            return x + y;
        }

        /// <summary>
        /// 乘法运算
        /// </summary>
        static int Multiply(int x, int y)
        {
            return x * y;
        }

        /// <summary>
        /// 减法运算
        /// </summary>
        static int Subtract(int x, int y)
        {
            return x - y;
        }
    }
}

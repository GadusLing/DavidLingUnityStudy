using System;

namespace 匿名函数作业
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 题目：匿名函数练习
            // 
            // 要求：
            // 1. 写一个函数传入一个整数，返回一个函数
            // 2. 之后执行这个匿名函数时传入一个整数和之前那个函数传入的数相乘
            // 3. 返回结果
            //
            // 示例流程：
            // - 第一步：调用函数传入整数 A
            // - 第二步：返回一个匿名函数
            // - 第三步：调用返回的匿名函数，传入整数 B
            // - 第四步：计算 A * B 并返回结果

            static Func<int, int> myfunc(int a)
            {
                // 返回一个匿名函数
                return delegate (int b)
                {
                    // 匿名函数内部计算 A * B
                    return a * b;
                };
            }

            // 测试代码
            int A = 5; // 第一步：传入整数 A
            Func<int, int> multiplyFunction = myfunc(A); // 第二步：获取匿名函数
            int B = 10; // 第三步：传入整数 B
            int result = multiplyFunction(B); // 第四步：计算 A * B
            Console.WriteLine($"结果是: {result}"); // 输出结果



        }
    }
}

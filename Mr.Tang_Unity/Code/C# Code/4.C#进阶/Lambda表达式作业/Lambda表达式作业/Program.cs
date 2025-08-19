using static System.Net.Mime.MediaTypeNames;

namespace Lambda表达式作业
{
    internal class Program
    {
        // 自定义委托类型
        public delegate void MyDelegate();

        static void Main(string[] args)
        {
            Console.WriteLine("Lambda表达式作业");
            Console.WriteLine("题目：有一个函数，会返回一个委托函数，这个委托函数只有一句打印代码。");
            Console.WriteLine("之后执行返回的委托函数时，可以打出1～10");
            Console.WriteLine();
            Console.WriteLine("请在下方编写你的代码实现：");
            Console.WriteLine();
            
            Console.WriteLine("=== 错误的闭包写法（会打印10个11）===");
            testWrong()();
            
            Console.WriteLine();
            Console.WriteLine("=== 正确的闭包写法（打印1到10）===");
            testCorrect()();
        }

        /// <summary>
        /// 错误的闭包写法 - 经典的变量捕获陷阱
        /// </summary>
        static MyDelegate testWrong()
        {
            MyDelegate action = null;
            
            // 【问题所在】：闭包的变量捕获陷阱
            for (int i = 1; i <= 10; i++)
            {
                // ❌ 错误：所有的Lambda表达式都捕获了同一个变量i的【引用】
                // 不是捕获i的值，而是捕获i这个变量本身
                // 当循环结束时，i的最终值是11（因为i++后变成11才退出循环）
                // 所以所有Lambda执行时都会打印i的当前值：11
                action += () => Console.WriteLine($"错误版本: {i}");
                
                // 编译器实际上会把这个循环转换成类似这样的代码：
                // var closure = new ClosureClass();
                // closure.i = 1;  // 所有Lambda共享同一个i变量
                // action += () => Console.WriteLine(closure.i);
                // closure.i = 2;
                // action += () => Console.WriteLine(closure.i); // 仍然是同一个closure.i
                // ...
                // closure.i = 11; // 循环结束后i变成11
            }
            
            return action;
        }

        /// <summary>
        /// 正确的闭包写法 - 使用局部变量副本
        /// </summary>
        static MyDelegate testCorrect()
        {
            MyDelegate action = null;
            
            // 【解决方案】：为每个Lambda创建独立的变量
            for (int i = 1; i <= 10; i++)
            {
                // ✅ 正确：创建局部变量副本
                // 每次循环都会创建一个新的temp变量
                // 每个Lambda捕获的是不同的temp变量
                int temp = i; // 关键：每次循环都是新的变量实例
                
                action += () => Console.WriteLine($"正确版本: {temp}");
                
                // 编译器实际上会把这个循环转换成类似这样的代码：
                // var closure1 = new ClosureClass(); closure1.temp = 1;
                // action += () => Console.WriteLine(closure1.temp);
                // var closure2 = new ClosureClass(); closure2.temp = 2;  
                // action += () => Console.WriteLine(closure2.temp);
                // ...
                // 每个Lambda都有自己独立的closure实例
            }
            
            return action;
        }

        /// <summary>
        /// 原始的test方法 - 展示错误情况
        /// </summary>
        static MyDelegate test()
        {
            MyDelegate action = null;
            for (int i = 1; i <= 10; i++)
            {
                // 这就是经典的闭包陷阱！
                action += () => Console.WriteLine(i);
            }
            return action;
        }
    }
}

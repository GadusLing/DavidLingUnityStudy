using System;
using System.Collections.Generic;

namespace Stack作业
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Stack作业");
            
            // 题目1：请简述栈的存储规则
            Console.WriteLine("\n题目1：栈的存储规则");
            DemonstrateStackRules();
            
            // 题目2：写一个方法计算任意一个数的二进制数，使用栈结构方式存储，之后打印出来
            Console.WriteLine("\n题目2：使用栈计算并打印二进制数");
            int number = 42; // 示例数字
            string binaryResult = ConvertToBinaryUsingStack(number);
            Console.WriteLine($"数字 {number} 的二进制表示为: {binaryResult}");
            
            // 测试更多数字
            int[] testNumbers = { 8, 15, 255, 1024 };
            foreach (int num in testNumbers)
            {
                Console.WriteLine($"数字 {num} 的二进制表示为: {ConvertToBinaryUsingStack(num)}");
            }
        }

        /// <summary>
        /// 题目1：演示栈的存储规则
        /// 栈是一种后进先出(LIFO - Last In First Out)的数据结构
        /// </summary>
        static void DemonstrateStackRules()
        {
            Console.WriteLine("栈的存储规则：后进先出(LIFO - Last In First Out)");
            
            // 创建一个栈来演示
            Stack<string> stack = new Stack<string>();
            
            // 压栈操作（Push）- 添加元素到栈顶
            Console.WriteLine("\n压栈操作：");
            string[] items = { "第一个元素", "第二个元素", "第三个元素" };
            
            foreach (string item in items)
            {
                stack.Push(item);
                Console.WriteLine($"压入: {item}，当前栈顶: {stack.Peek()}");
            }
            
            // 弹栈操作（Pop）- 从栈顶移除元素
            Console.WriteLine("\n弹栈操作：");
            while (stack.Count > 0)
            {
                string poppedItem = stack.Pop();
                Console.WriteLine($"弹出: {poppedItem}");
            }
            
            Console.WriteLine("可以看到，最后压入的元素最先被弹出，这就是栈的LIFO特性");
        }

        /// <summary>
        /// 题目2：使用栈结构计算任意数字的二进制表示
        /// 原理：通过不断除以2取余数，余数从下到上组成二进制数
        /// 使用栈可以很好地实现这个"从下到上"的顺序
        /// </summary>
        /// <param name="number">要转换的十进制数字</param>
        /// <returns>二进制字符串表示</returns>
        static string ConvertToBinaryUsingStack(int number)
        {
            // 处理特殊情况：0的二进制就是0
            if (number == 0)
            {
                return "0";
            }
            
            // 创建栈来存储二进制位
            Stack<int> binaryStack = new Stack<int>();
            
            // 将输入数字保存，用于输出
            int originalNumber = number;
            
            Console.WriteLine($"\n计算 {originalNumber} 的二进制过程：");
            
            // 不断除以2，将余数压入栈中
            while (number > 0)
            {
                int remainder = number % 2;  // 取余数（二进制位）
                binaryStack.Push(remainder); // 将余数压入栈
                
                Console.WriteLine($"{number} ÷ 2 = {number / 2} ... 余数 {remainder} (压入栈)");
                
                number = number / 2;         // 整数除法，继续下一位计算
            }
            
            // 从栈中弹出所有元素，组成二进制字符串
            Console.WriteLine("\n从栈中弹出元素组成二进制数：");
            string binaryResult = "";
            
            while (binaryStack.Count > 0)
            {
                int bit = binaryStack.Pop();
                binaryResult += bit.ToString();
                Console.WriteLine($"弹出: {bit}，当前二进制字符串: {binaryResult}");
            }
            
            return binaryResult;
        }
    }
}

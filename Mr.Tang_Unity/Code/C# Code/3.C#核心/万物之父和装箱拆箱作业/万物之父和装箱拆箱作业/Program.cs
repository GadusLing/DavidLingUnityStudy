namespace 万物之父和装箱拆箱作业
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 万物之父 Object 类演示 ===");
            DemonstrateObjectClass();
            
            Console.WriteLine("\n=== 装箱拆箱演示 ===");
            DemonstrateBoxingUnboxing();
            
            Console.WriteLine("\n=== 避免装箱拆箱的方法 ===");
            DemonstrateAvoidBoxing();
        }

        /// <summary>
        /// 演示 Object 类作为万物之父的特性
        /// </summary>
        static void DemonstrateObjectClass()
        {
            // Object 是所有类型的基类
            object obj1 = "字符串";
            object obj2 = 123;
            object obj3 = true;
            object obj4 = new DateTime(2023, 1, 1);

            Console.WriteLine($"字符串: {obj1}, 类型: {obj1.GetType()}");
            Console.WriteLine($"整数: {obj2}, 类型: {obj2.GetType()}");
            Console.WriteLine($"布尔值: {obj3}, 类型: {obj3.GetType()}");
            Console.WriteLine($"日期: {obj4}, 类型: {obj4.GetType()}");

            // 演示 Object 的基本方法
            Console.WriteLine($"obj1.ToString(): {obj1.ToString()}");
            Console.WriteLine($"obj2.GetHashCode(): {obj2.GetHashCode()}");
            Console.WriteLine($"obj1.Equals(obj2): {obj1.Equals(obj2)}");
        }

        /// <summary>
        /// 演示装箱和拆箱过程
        /// </summary>
        static void DemonstrateBoxingUnboxing()
        {
            // 装箱 (Boxing): 值类型 -> 引用类型
            int valueType = 42;
            object boxedValue = valueType;  // 装箱发生
            Console.WriteLine($"装箱: int {valueType} -> object {boxedValue}");

            // 拆箱 (Unboxing): 引用类型 -> 值类型
            int unboxedValue = (int)boxedValue;  // 拆箱发生
            Console.WriteLine($"拆箱: object {boxedValue} -> int {unboxedValue}");

            // 演示更多类型的装箱拆箱
            double doubleValue = 3.14;
            object boxedDouble = doubleValue;
            double unboxedDouble = (double)boxedDouble;
            Console.WriteLine($"double 装箱拆箱: {doubleValue} -> {boxedDouble} -> {unboxedDouble}");

            // 演示错误的拆箱
            try
            {
                string wrongUnbox = (string)boxedValue;  // 这会抛出异常
                // 拆箱必须拆箱为原始的确切类型。由于 boxedValue 包装的是 int，只能拆箱为 int，不能直接拆箱为 string。
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine($"错误的拆箱操作: {ex.Message}");
            }

            // 使用 is 和 as 安全转换
            if (boxedValue is int safeUnbox)
            {
                Console.WriteLine($"安全拆箱使用 is: {safeUnbox}");
            }
        }

        /// <summary>
        /// 演示如何避免不必要的装箱拆箱
        /// </summary>
        static void DemonstrateAvoidBoxing()
        {
            // 使用泛型避免装箱
            Console.WriteLine("使用泛型避免装箱:");
            
            // 不好的做法 - 会发生装箱
            object[] badArray = { 1, 2, 3, 4, 5 };
            Console.WriteLine($"装箱数组长度: {badArray.Length}");

            // 好的做法 - 使用泛型，避免装箱
            int[] goodArray = { 1, 2, 3, 4, 5 };
            Console.WriteLine($"泛型数组长度: {goodArray.Length}");

            // 使用泛型集合
            var genericList = new System.Collections.Generic.List<int> { 1, 2, 3 };
            Console.WriteLine($"泛型列表计数: {genericList.Count}");

            // 使用 ToString() 而不是字符串连接来避免装箱
            int number = 100;
            string result1 = number.ToString();  // 好的做法
            string result2 = "" + number;        // 会发生装箱
            Console.WriteLine($"ToString(): {result1}, 字符串连接: {result2}");
        }
    }
}

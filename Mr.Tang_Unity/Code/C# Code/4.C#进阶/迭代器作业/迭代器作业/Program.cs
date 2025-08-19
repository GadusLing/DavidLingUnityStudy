using System.Collections;

namespace 迭代器作业
{
    // 方法一：实现 IEnumerable 接口
    public class NumberCollection : IEnumerable
    {
        private int[] numbers;

        public NumberCollection(params int[] nums)
        {
            numbers = nums;
        }

        public IEnumerator GetEnumerator()
        {
            foreach (int number in numbers)
            {
                yield return number;
            }
        }
    }

    // 方法二：鸭子类型 - 只需要有 GetEnumerator() 方法
    public class StringCollection
    {
        private string[] strings;

        public StringCollection(params string[] strs)
        {
            strings = strs;
        }

        // 不实现接口，但有 GetEnumerator 方法
        public IEnumerator GetEnumerator()
        {
            foreach (string str in strings)
            {
                yield return str;
            }
        }
    }

    // 自定义枚举器类
    public class CustomEnumerator : IEnumerator
    {
        private int[] data;
        private int position = -1;

        public CustomEnumerator(int[] array)
        {
            data = array;
        }

        public object Current
        {
            get
            {
                if (position == -1 || position >= data.Length)
                    throw new InvalidOperationException();
                return data[position];
            }
        }

        public bool MoveNext()
        {
            position++;
            return position < data.Length;
        }

        public void Reset()
        {
            position = -1;
        }
    }

    // 使用自定义枚举器的类
    public class ManualCollection : IEnumerable
    {
        private int[] items;

        public ManualCollection(params int[] values)
        {
            items = values;
        }

        public IEnumerator GetEnumerator()
        {
            return new CustomEnumerator(items);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 方法一：实现 IEnumerable 接口 ===");
            var numbers = new NumberCollection(1, 2, 3, 4, 5);
            foreach (int num in numbers)
            {
                Console.WriteLine($"数字: {num}");
            }

            Console.WriteLine("\n=== 方法二：鸭子类型（只实现 GetEnumerator 方法）===");
            var strings = new StringCollection("Apple", "Banana", "Cherry");
            foreach (string str in strings)
            {
                Console.WriteLine($"字符串: {str}");
            }

            Console.WriteLine("\n=== 使用自定义枚举器 ===");
            var manual = new ManualCollection(10, 20, 30, 40);
            foreach (int value in manual)
            {
                Console.WriteLine($"值: {value}");
            }

            Console.WriteLine("\n=== 使用 yield return 的优势演示 ===");
            var fibonacci = new FibonacciSequence(10);
            foreach (int fib in fibonacci)
            {
                Console.WriteLine($"斐波那契数: {fib}");
            }
        }
    }

    // 额外示例：使用 yield return 生成斐波那契数列
    public class FibonacciSequence : IEnumerable
    {
        private int count;

        public FibonacciSequence(int maxCount)
        {
            count = maxCount;
        }

        public IEnumerator GetEnumerator()
        {
            int a = 0, b = 1;
            for (int i = 0; i < count; i++)
            {
                yield return a;
                int temp = a + b;
                a = b;
                b = temp;
            }
        }
    }
}

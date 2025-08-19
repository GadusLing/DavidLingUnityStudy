// 题目1：用泛型实现一个单例模式基类
// 题目2：利用泛型知识点，仿造ArrayList实现一个不确定数组类型的类，实现增删查改方法
using System.Reflection.Metadata;

namespace 泛型约束作业
{
    class Singleton<T> where T : new() 
    {
        private static T instance = new T();

        public static T Instance
        {
            get
            {
                return instance;
            } 
        }
    }

    class myArrayList<T> where T : new()
    {
        private int _capacity;
        private T[] _array;
        private int _count;

        public myArrayList()
        {
            _capacity = 16;
            _array = new T[_capacity];
            _count = 0;
        }

        public void Add(T item)
        {
            if(_count >= _capacity)
            {
                T[] newArray = new T[_capacity * 2];
                for(int i = 0; i < _count; i++)
                {
                    newArray[i] = _array[i];
                }
                _array = newArray;
            }
            _array[_count++] = item;
        }

        public void Remove(T item)
        {
            for(int i = 0; i <_count; ++i)
            {
                if(_array[i].Equals(item))
                {
                    RemoveAt(i);
                    break;
                }
            }
        }

        public void RemoveAt(int index)
        {
            if(index < 0 || index >= _count)
            {
                Console.WriteLine("索引不合法");
                return;
            }
            for (int j = index; j < _count - 1; ++j)
            {
                _array[j] = _array[j + 1];
            }
            _array[_count - 1] = default(T);
            --_count;
        }


        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                {
                    Console.WriteLine("索引不合法");
                    return default(T);
                }
                return _array[index];
            }
            set
            {
                if (index < 0 || index >= _count)
                {
                    Console.WriteLine("索引不合法");
                    return;
                }
                _array[index] = value;
            }
        }

        public int Capacity
        {
            get { return _capacity; }
        }

        public int Count
        {
            get { return _count; }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            // 请在此处完成上述两个题目的代码实现

            // 测试
            var singletonInstance = Singleton<Program>.Instance;
            Console.WriteLine($"Singleton Instance: {singletonInstance.GetType().Name}");
            var myList = new myArrayList<int>();
            myList.Add(1);
            myList.Add(2);
            myList.Add(3);
            Console.WriteLine($"Count after adding elements: {myList.Count}");
            myList.Remove(2);
            Console.WriteLine($"Count after removing element: {myList.Count}");
            myList.Add(4);
            Console.WriteLine($"Count after adding another element: {myList.Count}");
            Console.WriteLine($"Element at index 1: {myList[1]}");
            myList[1] = 5;
            Console.WriteLine($"Element at index 1 after modification: {myList[1]}");
            Console.WriteLine($"Capacity of myArrayList: {myList.Capacity}");



        }
    }
}

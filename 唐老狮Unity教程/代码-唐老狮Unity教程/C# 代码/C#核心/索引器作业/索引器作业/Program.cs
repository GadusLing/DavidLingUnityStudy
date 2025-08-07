using Microsoft.VisualBasic.FileIO;

namespace 索引器作业
{
    internal class Program
    {
        //自定义一个整形数组类，其中有一个整形数组属性，并提供封装增删查改的方法
        class IntArray
        {
            private int[] _array; // 存储整形数组
            private int _capacity = 5;// 数组容量，缺省默认为5
            private int _size;// 当前元素个数

            public IntArray()
            {
                _array = new int[_capacity];
                _size = 0;
            }

            public void Add(int value)
            {
                if (_size >= _capacity) 
                {
                    // 2倍扩展数组容量
                    ResizeArray(_capacity * 2);
                }
                _array[_size] = value;
                _size++;
            }
             
            public void Remove(int value)
            {
                int index = Array.IndexOf(_array, value, 0, _size);
                if (index < 0)
                    throw new ArgumentException("Value not found in the array.", nameof(value));
                RemoveAt(index); // 修正方法名拼写
            }

            public void RemoveAt(int index) // 修正方法名拼写
            {
                if (index < 0 || index >= _size)
                    throw new IndexOutOfRangeException("Index out of range.");
                for (int i = index; i < _size - 1; i++)
                {
                    _array[i] = _array[i + 1];
                }
                _array[_size - 1] = default(int); // 清除最后一个元素
                _size--;
            }

            // 查找方法
            public int IndexOf(int value)
            {
                return Array.IndexOf(_array, value, 0, _size);
            }

            public bool Contains(int value)
            {
                return IndexOf(value) >= 0;
            }

            private void CopyArray(ref int[] newArray)
            {
                if (newArray == null)
                    throw new ArgumentNullException(nameof(newArray), "New array cannot be null.");
                if (newArray.Length < _size)
                    throw new ArgumentException("New array is too small to hold the elements.", nameof(newArray));
                for (int i = 0; i < _size; i++)
                {
                    newArray[i] = _array[i];
                }
            }

            private void ResizeArray(int capacity)
            {
                if (capacity <= 0)
                    throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");
                if (capacity < _size)
                    throw new ArgumentException("New capacity cannot be smaller than current size.", nameof(capacity));
                
                int[] newArray = new int[capacity];
                CopyArray(ref newArray);
                _array = newArray;
                _capacity = capacity;
            }

            public int this[int index]
            {
                get
                {
                    if (index < 0 || index >= _size)
                        throw new IndexOutOfRangeException("Index out of range.");
                    return _array[index];
                }
                set
                {
                    if (index < 0 || index >= _size)
                        throw new IndexOutOfRangeException("Index out of range.");
                    _array[index] = value;
                }
            }

            // 添加显示方法，便于调试
            public void Display()
            {
                Console.Write($"IntArray[{_size}/{_capacity}]: [");
                for (int i = 0; i < _size; i++)
                {
                    Console.Write(_array[i]);
                    if (i < _size - 1) Console.Write(", ");
                }
                Console.WriteLine("]");
            }
        }

        static void Main(string[] args)
        {
            // 测试代码
            IntArray arr = new IntArray();
            
            // 测试添加
            arr.Add(10);
            arr.Add(20);
            arr.Add(30);
            arr.Display();
            
            // 测试索引器
            Console.WriteLine($"arr[1] = {arr[1]}");
            arr[1] = 25;
            arr.Display();
            
            // 测试删除
            arr.Remove(25);
            arr.Display();


        }
    }
}

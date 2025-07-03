//using System;

//namespace Lesson14_练习题
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            Console.WriteLine("选择排序练习题");
//            #region 练习题一
//            //定义一个数组，长度为20，每个元素值随机0~100的数
//            //使用选择排序进行升序排序并打印
//            //使用选择排序进行降序排序并打印

//            Random r = new Random();

//            int[] arr = new int[20];

//            for (int i = 0; i < arr.Length; i++)
//            {
//                arr[i] = r.Next(0, 101);
//                Console.Write(arr[i] + " ");
//            }
//            Console.Write("\n");



//            for (int i = 0; i < arr.Length; i++)
//            {
//                int tmp = 0;
//                for (int j = 1; j < arr.Length - i; j++)
//                {
//                    if (arr[tmp] > arr[j])
//                    {
//                        tmp = j;
//                    }
//                }

//                if (arr[tmp] != arr[arr.Length - 1 - i])
//                {
//                    int swap = arr[tmp];
//                    arr[tmp] = arr[arr.Length - 1 - i];
//                    arr[arr.Length - 1 - i] = swap;
//                }
//            }

//            for (int i = 0; i < arr.Length; i++)
//            {
//                Console.Write(arr[i] + " ");
//            }
//            Console.Write("\n");

//            #endregion
//        }
//    }
//}

using System;

namespace Lesson14_练习题
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("选择排序练习题");

            // 1. 生成随机数组
            Random r = new Random();
            int[] arr = new int[20];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = r.Next(0, 101);

            Console.WriteLine("原始数组: " + string.Join(" ", arr));

            // 2. 升序排序并打印
            SelectionSort(arr, ascending: true);
            Console.WriteLine("升序排序: " + string.Join(" ", arr));

            // 3. 降序排序并打印
            SelectionSort(arr, ascending: false);
            Console.WriteLine("降序排序: " + string.Join(" ", arr));
        }

        // 通用的选择排序方法（ascending=true升序，ascending=false降序）
        static void SelectionSort(int[] array, bool ascending)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                int targetIndex = i;
                for (int j = i + 1; j < array.Length; j++)
                {
                    bool shouldSwap = ascending ? array[j] < array[targetIndex]
                                               : array[j] > array[targetIndex];
                    if (shouldSwap)
                        targetIndex = j;
                }

                if (targetIndex != i)
                    (array[i], array[targetIndex]) = (array[targetIndex], array[i]); // 使用元组交换
            }
        }
    }
}

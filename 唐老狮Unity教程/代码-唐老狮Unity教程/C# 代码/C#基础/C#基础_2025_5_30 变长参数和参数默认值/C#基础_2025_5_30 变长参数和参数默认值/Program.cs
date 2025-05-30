//using System;

//namespace Lesson9_变长参数和参数默认值
//{
//    class Program
//    {
//        #region 知识点一 函数语法复习
//        //    1    2     3                  4
//        //static void 函数名(参数类型 参数1, 参数类型 参数2......)
//        //{
//        //       5
//        //    return 返回值;
//        //}
//        //1.静态关键词 可选，目前对于我们来说必须写
//        //2.返回值，没有返回值填void 可以填写任意类型的变量
//        //3.函数名，帕斯卡命名法
//        //4.参数可以是0~n个  前面可以加   ref和 out 用来传递想要在函数内部改变内容的变量
//        //5.如果返回值不是void 那么必须有return对应类型的内容  return可以打断函数语句块中的逻辑 直接停止后面的逻辑
//        #endregion

//        #region 知识点二 变长参数关键词
//        //举例  函数要计算 n个整数的和
//        //static int Sum(int a, int b,..........)

//        //变长参数关键字 params
//        static int Sum(params int[] arr)
//        {
//            int sum = 0;
//            for (int i = 0; i < arr.Length; i++)
//            {
//                sum += arr[i];
//            }
//            return sum;
//        }

//        //params int[] 意味着可以传入n个int参数 n可以等于0  传入的参数会存在arr数组中
//        // 注意：
//        //1.params关键字后面必为数组
//        //2.数组的类型可以是任意的类型


//        //3.函数参数可以有 别的参数和 params关键字修饰的参数
//        //4.函数参数中只能最多出现一个params关键字 并且一定是在最后一组参数 前面可以有n个其它参数
//        static void Eat(string name, int a, int b, params string[] things)
//        {

//        }

//        #endregion

//        #region 知识点三 参数默认值
//        //有参数默认值的参数 一般称为可选参数
//        //作用是 当调用函数时可以不传入参数，不传就会使用默认值作为参数的值
//        static void Speak(string str = "我没什么话可说")
//        {
//            Console.WriteLine(str);
//        }


//        //注意：
//        //1.支持多参数默认值 每个参数都可以有默认值
//        //2.如果要混用 可选参数 必须写在 普通参数后面

//        static void Speak2(string a, string test, string name = "David", string str = "我没什么话可说")
//        {
//            Console.WriteLine(a + " " + test + " " + name + " " + str);
//        }
//        #endregion


//        //总结
//        // 1 变长参数关键字 params
//        // 作用： 可以传入n个同类型参数   n可以是0
//        // 注意：
//        // 1. params后面必须是数组 意味着只能是同一类型的可变参数
//        // 2. 变长参数只能有一个
//        // 3. 必须在所有参数最后写变长参数 

//        // 2 参数默认值（可选参数）
//        // 作用：可以给参数默认值 使用时可以不传参 不传用默认的 传了用传的
//        // 注意：
//        // 1. 可选参数可以有多个
//        // 2. 正常参数比写在可选参数前面，可选参数只能写在所有参数的后面

//        static void Main(string[] args)
//        {
//            Console.WriteLine("变长参数和参数默认值");

//            Sum();
//            Sum(1, 2, 3, 4, 5, 6, 7, 8, 1, 22, 456, 123, 1);

//            Speak();
//            Speak("123123");

//            Speak2("I", "am");

//        }
//    }
//}


namespace Lesson9_练习题
{
    class Program
    {
        #region 练习题一
        //使用param参数，求多个数字的和以及平均数
        static void CalcSumAvg(params int[] arr)
        {
            if (arr.Length == 0)
            {
                Console.WriteLine("没有参数");
                return;
            }

            int sum = 0;
            int avg = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                //求总合
                sum += arr[i];
            }
            avg = sum / arr.Length;

            Console.WriteLine("总合{0}平均数{1}", sum, avg);
        }
        #endregion

        #region 练习题二
        //使用param参数，求多个数字的偶数和奇数和
        static void Calc(params int[] arr)
        {
            if (arr.Length == 0)
            {
                Console.WriteLine("没有传参数");
                return;
            }
            //奇数和
            int sum1 = 0;
            //偶合和
            int sum2 = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] % 2 == 0)
                {
                    sum2 += arr[i];
                }
                else
                {
                    sum1 += arr[i];
                }
            }
            Console.WriteLine("奇数和{0}偶数和{1}", sum1, sum2);
        }
        #endregion

        static void Main(string[] args)
        {
            Console.WriteLine("变长参数练习题");

            CalcSumAvg();
            CalcSumAvg(1, 2, 3, 4, 5, 6, 7);

            Calc();
            Calc(1, 2, 3, 4, 5, 6, 7);
        }
    }
}


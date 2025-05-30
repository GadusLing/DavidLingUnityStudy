﻿//using System;

//namespace Lesson11_递归函数
//{
//    class Program
//    {
//        #region 知识点一 基本概念
//        //递归函数 就是 让函数自己调用自己
//        //static void Fun()
//        //{
//        //    if( false )
//        //    {
//        //        return;
//        //    }
//        //    Fun();
//        //}
//        //一个正确的递归函数
//        // 1.必须有结束调用的条件
//        // 2.用于条件判断的 这个条件 必须改变 能够达到停止的目的
//        #endregion

//        #region 知识点二 实例
//        //用递归函数打印出 0到10
//        //递归函数 就是自己调用自己
//        static void Fun(int a)
//        {
//            //第四步：结束条件
//            if (a > 10)
//            {
//                return;
//            }
//            //第二步：完成要求 打印
//            Console.WriteLine(a);
//            //第三步：完成一个 递归的变化 作为我们条件的判断
//            ++a;
//            //第一步：构造了一个递归
//            Fun(a);
//        }

//        #endregion

//        static void Main(string[] args)
//        {
//            Console.WriteLine("递归函数");
//            Fun(0);
//        }
//    }
//}


using System;

namespace Lesson11_练习题
{
    class Program
    {
        #region 练习题一
        //使用递归的方式打印0~10
        static void Fun(int a)
        {
            if (a > 10)
            {
                return;
            }
            Console.WriteLine(a);
            ++a;
            Fun(a);
        }

        #endregion

        #region 练习题二
        //传入一个值，递归求该值的阶乘 并返回
        //5! = 1*2*3*4*5

        //5! = 5*4*3*2*1;
        static int Fun2(int num)
        {
            if (num == 1)
            {
                return 1;
            }
            //return 5*4*3*2*1;
            return num * Fun2(num - 1);
            // 5 * Fun2(4) = 5 * 4 * 3 * 2 * 1
            // 4 * Fun2(3) = 4 * 3 * 2 * 1
            // 3 * Fun2(2) = 3 * 2 * 1
            // 2 * Fun2(1) = 2 * 1
            // 1
        }
        #endregion

        #region 练习题三
        //使用递归求 1! + 2! + 3! + 4! + ..... + 10!
        // 阶乘(1) + 阶乘(2) + 阶乘(3).......+阶乘(10)
        // 阶乘(10) + 阶乘(9) + 阶乘(8) + ........ 阶乘(1)
        static int Fun3(int num)
        {
            if (num == 1)
            {
                return Fun2(1);
                //return 1;
            }
            // 阶乘(10) + 阶乘(9) + 阶乘(8) + ........ 阶乘(1)
            return Fun2(num) + Fun3(num - 1);
            // 阶乘(10) + Fun3(9) 阶乘(10) + 阶乘(9) .....+  阶乘(2) + 阶乘(1)
            // 阶乘(9) + Fun3(8)  阶乘(9) + 阶乘(8) .....+  阶乘(2) + 阶乘(1)
            // 阶乘(8) + Fun3(7)  阶乘(8) + 阶乘(7) .....+  阶乘(2) + 阶乘(1)
            //.....
            // 阶乘(2) + Fun3(1)  阶乘(2) + 阶乘(1)
            // 阶乘(1)
        }
        #endregion

        #region 练习题四
        //一根竹竿长100m，每天砍掉一半，求第十天它的长度是多少（从第0天开始）
        // 一是长度 二是天数
        static void Fun4(float length, int day = 0)
        {
            length /= 2;
            //第十天砍了过后 的长度 
            if (day == 10)
            {
                Console.WriteLine("第十天砍后竹子长{0}米", length);
                return;
            }
            ++day;
            Fun4(length, day);
        }
        #endregion

        #region 练习题五
        //不允许使用循环语句、条件语句，在控制台中打印出1-200这200个数（提示：递归+短路）
        static bool Fun5(int num)
        {
            Console.WriteLine(num);
            return num == 200 || Fun5(num + 1);
        }
        #endregion

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Fun(0);

            Console.WriteLine(Fun2(6));
            //5! = 1*2*3*4*5;

            Console.WriteLine(Fun3(10));

            Fun4(100);

            Fun5(1);
        }
    }
}


using System;

namespace 运算符重载作业
{
    /*
     * 题目要求：
     * 1. 定义一个位置结构体或类，为其重载判断是否相等的运算符
     *    (x1,y1) == (x2,y2) => 两个值同时相等才为true
     * 
     * 2. 定义一个Vector3类 (x,y,z) 通过重载运算符实现以下运算
     *    (x1,y1,z1) + (x2,y2,z2) = (x1+x2,y1+y2,z1+z2)
     *    (x1,y1,z1) - (x2,y2,z2) = (x1-x2,y1-y2,z1-z2)
     *    (x1,y1,z1) * num = (x1*num,y1*num,z1*num)
     */

    // 定义位置结构体
    public struct Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        // 重载相等运算符
        public static bool operator ==(Position pos1, Position pos2)
        {
            return pos1.X == pos2.X && pos1.Y == pos2.Y;
        }

        // 重载不等运算符
        public static bool operator !=(Position pos1, Position pos2)
        {
            return !(pos1 == pos2);
        }

        //C#规则：当你重载 == 运算符时，必须同时重写 Equals() 和 GetHashCode()
        // 重写Equals方法
        public override bool Equals(object obj)
        {
            if (obj is Position position)
            {
                return this == position;
            }
            return false;
        }

        // 重写GetHashCode方法
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }

    // 定义Vector3类
    public class Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        // 重载加法运算符
        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        // 重载减法运算符
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        // 重载乘法运算符（向量与标量相乘）
        public static Vector3 operator *(Vector3 v, float num)
        {
            return new Vector3(v.X * num, v.Y * num, v.Z * num);
        }

        // 重载乘法运算符（标量与向量相乘）
        public static Vector3 operator *(float num, Vector3 v)
        {
            return v * num;
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 运算符重载作业测试 ===");
            
            // 测试位置结构体相等判断
            Console.WriteLine("1. 位置结构体测试：");
            Position pos1 = new Position(1, 1);
            Position pos2 = new Position(2, 2);
            Position pos3 = new Position(1, 1);

            Console.WriteLine($"pos1: {pos1}");
            Console.WriteLine($"pos2: {pos2}");
            Console.WriteLine($"pos3: {pos3}");
            Console.WriteLine($"pos1 == pos2: {pos1 == pos2}");
            Console.WriteLine($"pos1 == pos3: {pos1 == pos3}");

            // 测试Vector3类运算
            Console.WriteLine("\n2. Vector3类测试：");
            Vector3 v1 = new Vector3(1, 1, 1);
            Vector3 v2 = new Vector3(2, 2, 2);
            float num = 3;

            Console.WriteLine($"v1: {v1}");
            Console.WriteLine($"v2: {v2}");
            Console.WriteLine($"num: {num}");
            
            Vector3 sum = v1 + v2;
            Console.WriteLine($"v1 + v2 = {sum}");
            
            Vector3 diff = v1 - v2;
            Console.WriteLine($"v1 - v2 = {diff}");
            
            Vector3 scaled = v1 * num;
            Console.WriteLine($"v1 * num = {scaled}");
        }
    }
}

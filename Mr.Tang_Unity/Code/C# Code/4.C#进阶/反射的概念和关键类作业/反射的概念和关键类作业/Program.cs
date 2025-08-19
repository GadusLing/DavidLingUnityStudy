using System.Reflection;

namespace 反射的概念和关键类作业
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             * 题目要求：
             * 1. 新建一个类库工程
             * 2. 有一个Player类，有姓名，血量，攻击力，防御力，位置等信息
             * 3. 有一个无参构造函数
             * 4. 再新建一个控制台工程
             * 5. 通过反射的形式使用类库工程生成的dll程序集
             * 6. 实例化一个Player对象
             */
            string dllPath = @"E:\GithubDownLoad\DavidLingUnityStudy\唐老狮Unity教程\代码-唐老狮Unity教程\C# 代码\4.C#进阶\反射作业\反射作业\bin\Debug\反射作业.dll";

            // 1. 加载程序集
            Assembly asm = Assembly.LoadFrom(dllPath);

            Type[] types = asm.GetTypes();// 获取程序集中的所有类型
            for (int i = 0; i < types.Length; i++)
            {
                Console.WriteLine(types[i]);
            }

            // 2. 获取 Player 类型
            Type playerType = asm.GetType("反射作业.Player");

            // 3. 使用无参构造实例化 Player
            object playerObj = Activator.CreateInstance(playerType);
            Console.WriteLine(playerObj);

            // 给 Name、Health 等赋值
            playerType.GetProperty("Name").SetValue(playerObj, "大卫");
            playerType.GetProperty("Health").SetValue(playerObj, 100);
            playerType.GetProperty("Attack").SetValue(playerObj, 20);
            playerType.GetProperty("Defense").SetValue(playerObj, 10);

            // 给 Position 赋值
            Type positionType = asm.GetType("反射作业.position");
            object posObj = Activator.CreateInstance(positionType);
            positionType.GetField("x").SetValue(posObj, 5);
            positionType.GetField("y").SetValue(posObj, 10);
            playerType.GetProperty("Position").SetValue(playerObj, posObj);

            Console.WriteLine(playerObj);


        }
    }
}


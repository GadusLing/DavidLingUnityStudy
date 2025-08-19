using System.Reflection;

namespace 特性作业
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //为反射练习题中的Player对象
            //随便为其中一个成员变量加一个自定义特性
            //同样实现反射练习题中的要求

            //但是当在设置加了自定义特性的成员变量时，在控制台中打印一句
            //非法操作，随意修改XXX成员

            Assembly assembly = Assembly.LoadFrom(@"E:\GithubDownLoad\DavidLingUnityStudy\唐老狮Unity教程\代码-唐老狮Unity教程\C# 代码\4.C#进阶\反射作业\反射作业\bin\Debug\反射作业.dll");
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                Console.WriteLine(type.FullName);
            }
            Type playerType = assembly.GetType("反射作业.Player");
            object playerObj = Activator.CreateInstance(playerType);
            Console.WriteLine(playerObj);

            PropertyInfo[] Properties = playerType.GetProperties();
            for(int i = 0; i < Properties.Length; i++)
            {
                Console.WriteLine(Properties[i]);
            }

            Type attribute = assembly.GetType("反射作业.MYCustomAttribute");

            PropertyInfo propertyStr = playerType.GetProperty("Name");
            if(propertyStr.GetCustomAttribute(attribute) != null)
            {
                Console.WriteLine("非法操作，随意修改Name成员"); 
            }
            else
            {
                Console.WriteLine("合法操作，修改Name成员");
                propertyStr.SetValue(playerObj, "David");
            }
        }
    }
}

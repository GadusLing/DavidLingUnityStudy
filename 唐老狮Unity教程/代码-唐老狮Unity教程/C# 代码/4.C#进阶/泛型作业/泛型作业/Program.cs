namespace 泛型作业
{
    internal class Program
    {
        /*
         * 题目：定义一个泛型方法，方法内判断该类型为何类型，并返回类型的名称与
         * 占有的字节数，如果是int，则返回"整形，4字节"
         * 只考虑以下类型
         * int：整形
         * char：字符
         * float：单精度浮点数
         * string：字符串
         * 如果是其它类型，则返回"其它类型"
         * （可以通过typeof(类型) == typeof(类型)的方式进行类型判断）
         */
        
        public string GetTypeInfo<T>(T input)
        {
            switch (input)
            {

                case int:
                    int bytes = sizeof(int);
                    return $"整形，{bytes}字节";
                case char:
                    int charBytes = sizeof(char);
                    return $"字符，{charBytes}字节";
                case float:
                    int floatBytes = sizeof(float);
                    return $"单精度浮点数，{floatBytes}字节";
                case string:
                    return "字符串，引用类型";
                default:
                    return "其它类型";
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            Program program = new Program();
            Console.WriteLine(program.GetTypeInfo(10));
            Console.WriteLine(program.GetTypeInfo('A'));
            Console.WriteLine(program.GetTypeInfo(3.14f));
            Console.WriteLine(program.GetTypeInfo("Hello"));
            Console.WriteLine(program.GetTypeInfo(true));
        }
    }
}

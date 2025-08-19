namespace LinkedList作业
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 题目要求：
            // 使用LinkedList，向其中加入10个随机整形变量
            // 正向遍历一次打印出信息
            // 反向遍历一次打印出信息
            
            LinkedList<int> LKL = new LinkedList<int>();
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                // 向LinkedList中添加随机整数
                LKL.AddLast(random.Next(1, 100));// 1-99
            }

            // 正向遍历打印信息
            Console.WriteLine("正向遍历：");
            foreach (var item in LKL)
            {
                Console.WriteLine(item);
            }

            // 反向遍历打印信息
            Console.WriteLine("反向遍历：");
            for (var node = LKL.Last; node != null; node = node.Previous)
            {
                Console.WriteLine(node.Value);
            }
        }
    }
}

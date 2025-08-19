using System.Collections;

namespace Queue作业
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Queue作业");
            
            // 第一题：请简述队列的存储规则
            Question1();
            
            // 第二题：使用队列存储消息，一次性存10条消息，每隔一段时间打印一条消息，控制台打印消息的要有明显停顿感
            Console.WriteLine("\n第二题：队列消息处理（请在下方完成代码）");
            Question2();
        }
        
        /// <summary>
        /// 第一题：请简述队列的存储规则
        /// </summary>
        static void Question1()
        {
            Console.WriteLine("第一题：请简述队列的存储规则");
            Console.WriteLine("答案：");
            Console.WriteLine("队列（Queue）是一种先进先出（FIFO - First In First Out）的数据结构。");
            Console.WriteLine("存储规则如下：");
            Console.WriteLine("1. 入队（Enqueue）：新元素总是从队列的尾部（rear）加入");
            Console.WriteLine("2. 出队（Dequeue）：元素总是从队列的头部（front）移除");
            Console.WriteLine("3. 先进入队列的元素会先被处理，后进入的元素排在后面等待");
            Console.WriteLine("4. 队列只允许在两端进行操作：一端用于插入（入队），另一端用于删除（出队）");
            Console.WriteLine("5. 遵循FIFO原则，类似现实中的排队场景");
            
            // 示例演示
            Console.WriteLine("\n演示示例：");
            Queue queue = new Queue();
            
            // 入队演示
            Console.WriteLine("入队操作：");
            queue.Enqueue("第1个元素");
            queue.Enqueue("第2个元素");
            queue.Enqueue("第3个元素");
            Console.WriteLine($"当前队列元素个数：{queue.Count}");
            Console.WriteLine(queue.Peek()); // 查看队首元素但不移除

            // 出队演示
            Console.WriteLine("\n出队操作：");
            while (queue.Count > 0)
            {
                Console.WriteLine($"出队：{queue.Dequeue()}");
            }
        }
        
        /// <summary>
        /// 第二题：使用队列存储消息，一次性存10条消息，每隔一段时间打印一条消息，控制台打印消息的要有明显停顿感
        /// </summary>
        static void Question2()
        {
            // 创建一个队列
            Queue messageQueue = new Queue();

            // 一次性向队列中添加10条消息
            for (int i = 1; i <= 10; i++)
            {
                messageQueue.Enqueue($"{i}枚金币");
            }

            // 使用循环从队列中取出消息并打印
            Console.WriteLine("消息处理开始：");
            while (messageQueue.Count > 0)
            {
                // 每次打印消息后停顿一段时间
                Console.WriteLine($"获得{messageQueue.Dequeue()}");
                
                // 使用Thread.Sleep模拟停顿感
                System.Threading.Thread.Sleep(1000); // 停顿1秒
            }
            Console.WriteLine("消息处理完成！");
        }
    }
}

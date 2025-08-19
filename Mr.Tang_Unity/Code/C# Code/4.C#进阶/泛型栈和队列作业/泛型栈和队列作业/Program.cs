namespace 泛型栈和队列作业
{
    /*
     * 题目：自己总结一下数组、List、Dictionary、Stack、Queue、LinkedList
     * 这些存储容器，对于我们来说应该如何选择它们来便用
     * 
     * 作业要求：
     * 1. 实现泛型栈(Stack<T>)和队列(Queue<T>)的基本操作
     * 2. 比较不同存储容器的特点和适用场景
     * 3. 提供实际使用示例
     */
    
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 泛型栈和队列作业 ===\n");
            
            // 演示栈的使用
            DemoStack();
            
            Console.WriteLine();
            
            // 演示队列的使用
            DemoQueue();
            
            Console.WriteLine();
            
            // 存储容器选择指南
            ShowContainerGuide();
        }
        
        /// <summary>
        /// 演示栈(Stack)的使用 - LIFO (后进先出)
        /// </summary>
        static void DemoStack()
        {
            Console.WriteLine("=== 栈(Stack)演示 - LIFO ===");
            Stack<string> stack = new Stack<string>();
            
            // 入栈
            stack.Push("第一个元素");
            stack.Push("第二个元素");
            stack.Push("第三个元素");
            
            Console.WriteLine($"栈中元素个数: {stack.Count}");
            Console.WriteLine($"栈顶元素: {stack.Peek()}");
            
            // 出栈
            Console.WriteLine("出栈顺序:");
            while (stack.Count > 0)
            {
                Console.WriteLine($"出栈: {stack.Pop()}");
            }
        }
        
        /// <summary>
        /// 演示队列(Queue)的使用 - FIFO (先进先出)
        /// </summary>
        static void DemoQueue()
        {
            Console.WriteLine("=== 队列(Queue)演示 - FIFO ===");
            Queue<int> queue = new Queue<int>();
            
            // 入队
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            
            Console.WriteLine($"队列中元素个数: {queue.Count}");
            Console.WriteLine($"队首元素: {queue.Peek()}");
            
            // 出队
            Console.WriteLine("出队顺序:");
            while (queue.Count > 0)
            {
                Console.WriteLine($"出队: {queue.Dequeue()}");
            }
        }
        
        /// <summary>
        /// 存储容器选择指南
        /// </summary>
        static void ShowContainerGuide()
        {
            Console.WriteLine("=== 存储容器选择指南 ===");
            Console.WriteLine();
            
            Console.WriteLine("1. 数组 (Array):");
            Console.WriteLine("   - 固定大小，类型安全");
            Console.WriteLine("   - 适用场景: 已知数据量大小，需要快速随机访问");
            Console.WriteLine("   - 时间复杂度: 访问O(1)，插入/删除O(n)");
            Console.WriteLine();
            
            Console.WriteLine("2. List<T>:");
            Console.WriteLine("   - 动态数组，可变大小");
            Console.WriteLine("   - 适用场景: 需要频繁添加/删除元素，支持索引访问");
            Console.WriteLine("   - 时间复杂度: 访问O(1)，末尾添加O(1)，中间插入O(n)");
            Console.WriteLine();
            
            Console.WriteLine("3. Dictionary<TKey, TValue>:");
            Console.WriteLine("   - 键值对存储，基于哈希表");
            Console.WriteLine("   - 适用场景: 需要根据键快速查找值");
            Console.WriteLine("   - 时间复杂度: 查找/插入/删除平均O(1)");
            Console.WriteLine();
            
            Console.WriteLine("4. Stack<T>:");
            Console.WriteLine("   - 后进先出(LIFO)");
            Console.WriteLine("   - 适用场景: 函数调用栈、撤销操作、表达式求值");
            Console.WriteLine("   - 时间复杂度: Push/Pop O(1)");
            Console.WriteLine();
            
            Console.WriteLine("5. Queue<T>:");
            Console.WriteLine("   - 先进先出(FIFO)");
            Console.WriteLine("   - 适用场景: 任务调度、广度优先搜索、缓冲区");
            Console.WriteLine("   - 时间复杂度: Enqueue/Dequeue O(1)");
            Console.WriteLine();
            
            Console.WriteLine("6. LinkedList<T>:");
            Console.WriteLine("   - 双向链表");
            Console.WriteLine("   - 适用场景: 频繁在中间插入/删除，不需要索引访问");
            Console.WriteLine("   - 时间复杂度: 插入/删除O(1)，查找O(n)");
            Console.WriteLine();
            
            Console.WriteLine("=== 选择建议 ===");
            Console.WriteLine("• 需要索引访问 → Array 或 List<T>");
            Console.WriteLine("• 键值对查找 → Dictionary<TKey, TValue>");
            Console.WriteLine("• 后进先出操作 → Stack<T>");
            Console.WriteLine("• 先进先出操作 → Queue<T>");
            Console.WriteLine("• 频繁中间插入删除 → LinkedList<T>");
        }
    }
}

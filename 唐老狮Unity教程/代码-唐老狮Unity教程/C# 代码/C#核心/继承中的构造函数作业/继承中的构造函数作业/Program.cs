namespace 继承中的构造函数作业
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // TODO: 实例化3个对象，分别是程序员、策划、美术
            Worker programmer = new Programmer();
            Worker planner = new Planner();
            Worker artist = new Artist();
            programmer.Work();
            planner.Work();
            artist.Work();
        }
    }
    
    // TODO: 创建打工人基类
    // 要求：
    // 1. 有工种属性
    // 2. 有工作内容属性（两个特征）
    // 3. 有一个工作方法
    // 4. 需要包含构造函数
    class Worker
    {   
        public string JobType { get; set; }
        public string WorkContent { get; set; }

        //public Worker()
        //{
        //    // 默认构造函数
        //}
        public Worker(string jobType, string workContent)
        {
            JobType = jobType;
            WorkContent = workContent;
        }
        public void Work()
        {
            Console.WriteLine($"正在从事{JobType}工作，内容是：{WorkContent}");
        }
    }

    // TODO: 创建程序员子类
    // 要求：
    // 1. 继承自打工人基类
    // 2. 维护打工人的特征
    // 3. 使用继承中的构造函数知识点
    class Programmer : Worker
    {
        public Programmer() : base("程序员", "编写代码，调试程序")
        {
            // 默认构造函数
        }
    }
    // TODO: 创建策划子类
    // 要求：
    // 1. 继承自打工人基类
    // 2. 维护打工人的特征
    // 3. 使用继承中的构造函数知识点
    class Planner : Worker
    {
        public Planner() : base("策划", "制定计划，撰写文档")
        {
            // 默认构造函数
        }
    }

    // TODO: 创建美术子类
    // 要求：
    // 1. 继承自打工人基类
    // 2. 维护打工人的特征
    // 3. 使用继承中的构造函数知识点
    class Artist : Worker
    {
        public Artist() : base("美术", "绘制原画，制作模型")
        {
            // 默认构造函数
        }
    }
}

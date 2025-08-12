namespace 命名空间作业
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 题目1：请说明关键词using有什么作用
            // using关键字用于引入命名空间，使得在代码中可以直接使用该命名空间中的类、方法等，
            // 而不需要写出完整的命名空间路径。

            // 题目2：有两个命名空间，UI(用户界面)和Graph(图表)
            // 两个命名空间中都有一个Image类
            // 请在主函数中实例化两个不同命名空间中的Image对象
            UI.Image uiImage = new UI.Image();
            Graph.Image graphImage = new Graph.Image();
        }
    }
}

namespace UI
{ 
    public class Image
    {
        // UI命名空间中的Image类
        public Image()
        {
            Console.WriteLine("UI命名空间中的Image类实例化成功");
        }
    }
}

namespace Graph
{
    public class Image
    {
        // Graph命名空间中的Image类
        public Image()
        {
            Console.WriteLine("Graph命名空间中的Image类实例化成功");
        }
    }
}

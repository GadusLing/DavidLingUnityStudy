namespace 事件作业
{
    // 题目：热水器事件编程练习
    // 场景描述：
    // 有一个热水器，包含一个加热器、一个报警器、一个显示器
    // 我们给热水器通上电，当水温超过95度时：
    // 1. 报警器会开始发出声音，告诉你温的温度
    // 2. 显示器也会改变水温提示，提示水已经烧开了

    // 需要实现的组件：
    // 1. 热水器类 (Heater) - 主要事件发布者
    //    - 包含温度属性
    //    - 包含加热方法
    //    - 当温度超过95度时触发事件

    // 2. 报警器类 (Alarm) - 事件订阅者
    //    - 订阅热水器的高温事件
    //    - 发出声音警告并显示温度

    // 3. 显示器类 (Display) - 事件订阅者  
    //    - 订阅热水器的高温事件
    //    - 显示水已烧开的提示信息

    // 核心技术点：
    // - 事件的声明与触发
    // - 委托的使用
    // - 多播委托（多个订阅者）
    // - 事件订阅与取消订阅

    internal class Program
    {
        class Heater
        {
            private float _dangerousTemperature = 95.0f;
            public float DangerousTemperature
            {
                get { return _dangerousTemperature; }
                set { _dangerousTemperature = value; }
            }
            public float Temperature { get; set; }

            public void Heating(bool powerSupply)
            {
                if (powerSupply)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        Temperature += 1.0f; // 每次加热1度
                        Console.WriteLine("当前温度: " + Temperature + "°C");
                        if (Temperature >= DangerousTemperature)
                        {
                            HighTemperatureReached?.Invoke(Temperature);
                            HighTemperatureReached = null; // 触发一次后取消订阅，避免重复触发
                        }
                        //间隔零点2秒
                        System.Threading.Thread.Sleep(50);
                    }
                }
                else
                {
                    Console.WriteLine("温度显示错误，加热器未通电");
                }
            }
            public event Action<float> HighTemperatureReached = null;
        }

        class Alarm
        {
            public Alarm(Heater heater)
            {
                heater.HighTemperatureReached += OnHighTemperatureReached;
            }
            private void OnHighTemperatureReached(float temperature)
            {
                Console.WriteLine("报警器：水温过高，当前温度为 " + temperature + "°C");
            }
        }

        class Display
        {
            public Display(Heater heater)
            {
                heater.HighTemperatureReached += OnHighTemperatureReached;
            }
            private void OnHighTemperatureReached(float temperature)
            {
                Console.WriteLine("显示器：水已烧开，当前温度为 " + temperature + "°C");
            }
        }

        static void Main(string[] args)
        {
            // 主程序逻辑：
            // 1. 创建热水器、报警器、显示器实例
            // 2. 将报警器和显示器订阅到热水器事件
            // 3. 开始加热过程
            // 4. 观察当温度达到95度时的事件触发效果
            Heater heater = new Heater();
            Alarm alarm = new Alarm(heater);
            Display display = new Display(heater);
            Console.WriteLine("热水器开始加热...");
            heater.Heating(true);
            Console.WriteLine("加热完成，按任意键退出...");
        }
    }
}

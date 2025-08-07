namespace 密封类作业_继承收尾_
{
    /*
     * 密封类作业 - 继承收尾
     * 
     * 题目要求：
     * 定义一个载具类，包含以下内容：
     * 
     * 基本属性：
     * - 速度
     * - 最大速度
     * - 可乘人数
     * - 司机和乘客等
     * 
     * 基本方法：
     * - 上车、下车
     * - 行驶
     * - 车辆等方法
     * 
     * 具体要求：
     * 1. 用载具类声明一个对象
     * 2. 并将若干人装载上车
     * 
     * 技术要点：
     * - 使用继承机制设计类层次结构
     * - 应用密封类(sealed class)概念
     * - 体现多态性
     * - 实现封装特性
     * 
     * 预期实现：
     * - 创建载具基类
     * - 派生具体的载具子类（如汽车、自行车等）
     * - 使用sealed关键字密封某些类
     * - 演示对象的创建和使用
     */

    class Vehicle
    {
        public Vehicle(int speed, int maxSpeed, int capacity)
        {
            Speed = speed;
            MaxSpeed = maxSpeed;
            Capacity = capacity;
            Size = 0; // 初始乘客为0
        }
        public void Board(Passenger passenger)
        {
            Console.WriteLine($"{passenger.Name}上车啦");
            Size++;
        }
        public void Drive(Driver driver)
        {
            Console.WriteLine($"{driver.Name}正在开车");
            Accident();
        }
        public void Exit(Passenger passenger)
        {
            Console.WriteLine($"{passenger.Name}下车啦");
            Size--;
        }
        public void Accident()
        {
            Console.WriteLine($"车辆行驶中...");
            System.Threading.Thread.Sleep(1000);
            if (Size > Capacity)
            {
                Console.WriteLine("超载了！乘客在车上打架，发生了事故");
            }
            else if (Speed > MaxSpeed)
            {
                Console.WriteLine("车辆超速行驶，发生了事故");
            }
            else
            {
                Console.WriteLine("车辆安全行驶，没有事故。");
            }
        }
        public int Speed { get; set; }
        public int MaxSpeed { get; set; }
        public int Capacity { get; set; }
        public int Size { get; set; }
    }

    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }

    class Driver : Person
    {
        public Driver(string name, int age) : base(name, age) { }
    }

    class Passenger : Person
    {
        public Passenger(string name, int age) : base(name, age) { }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // 创建载具对象
            Vehicle vehicle = new Vehicle(110, 120, 4);
            // 创建司机和乘客
            Driver driver = new Driver("张三", 30);
            Passenger passenger1 = new Passenger("李四", 25);
            Passenger passenger2 = new Passenger("王五", 28);
            Passenger passenger3 = new Passenger("赵六", 22);
            Passenger passenger4 = new Passenger("钱七", 26);
            // 下车
            vehicle.Exit(passenger1);
            vehicle.Exit(passenger2);
            // 上车
            vehicle.Board(passenger3);
            vehicle.Board(passenger4);
            // 驾驶
            vehicle.Drive(driver);

            // 事故
        }
    }
}

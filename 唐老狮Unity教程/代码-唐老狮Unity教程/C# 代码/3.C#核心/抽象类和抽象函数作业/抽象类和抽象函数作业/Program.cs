namespace 抽象类和抽象函数作业
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // TODO: 题目1测试代码
            // 创建人类、狗类、猫类实例并调用相关方法
            Animal human = new Human();
            human.MakeSound(); // 输出：人类说话
            Animal dog = new Dog();
            dog.MakeSound(); // 输出：狗叫
            Animal cat = new Cat();
            cat.MakeSound(); // 输出：猫叫

            // TODO: 题目2测试代码  
            // 创建矩形、正方形、圆形实例并测试面积和周长计算
            Shape rectangle = new Rectangle(5, 10);
            Console.WriteLine($"矩形面积: {rectangle.CalculateArea()}"); // 输出：矩形面积: 50
            Console.WriteLine($"矩形周长: {rectangle.CalculatePerimeter()}"); // 输出：矩形周长: 30
            Shape square = new Square(4);
            Console.WriteLine($"正方形面积: {square.CalculateArea()}"); // 输出：正方形面积: 16
            Console.WriteLine($"正方形周长: {square.CalculatePerimeter()}"); // 输出：正方形周长: 16
            Shape circle = new Circle(3);
            Console.WriteLine($"圆形面积: {circle.CalculateArea()}"); // 输出：圆形面积: 28.274333882308138
            Console.WriteLine($"圆形周长: {circle.CalculatePerimeter()}"); // 输出：圆形周长: 18.84955592153876

        }
    }
    
    #region 题目1：动物抽象类与子类实现
    /*
     * 题目要求：
     * 1. 写一个动物抽象类
     * 2. 写三个子类：人类、狗类、猫类
     * 
     * 设计要点：
     * - 动物抽象类应包含通用属性和抽象方法
     * - 每个子类需要实现抽象方法，体现各自特性
     * - 考虑动物的共同行为（如移动、发声等）
     */
    
    // TODO: 实现动物抽象类 Animal
    // - 抽象方法：Move()、MakeSound()
    // - 可选属性：Name、Age等
    abstract class Animal
    {
        public abstract void MakeSound();
    }

    class Human : Animal
    {
        public override void MakeSound()
        {
            Console.WriteLine("人类说话");
        }
    }

    class Dog : Animal
    {
        public override void MakeSound()
        {
            Console.WriteLine("狗叫");
        }
    }

    class Cat : Animal
    {
        public override void MakeSound()
        {
            Console.WriteLine("猫叫");
        }
    }
    #endregion
    
    #region 题目2：图形抽象类与面积周长计算
    /*
     * 题目要求：
     * 1. 创建一个图形类，有求面积和周长两个方法
     * 2. 创建矩形类、正方形类、圆形类继承图形类
     * 3. 实例化矩形、正方形、圆形对象求面积和周长
     * 
     * 设计要点：
     * - 图形抽象类定义通用接口
     * - 各子类根据自身几何特性实现面积和周长计算
     * - 考虑构造函数参数（长宽、边长、半径等）
     */
    abstract class Shape
    {
        public abstract double CalculateArea();
        public abstract double CalculatePerimeter();
    }

    class Rectangle : Shape
    {
        public double Length { get; set; }
        public double Width { get; set; }

        public Rectangle(double length, double width)
        {
            Length = length;
            Width = width;
        }

        public override double CalculateArea()
        {
            return Length * Width;
        }

        public override double CalculatePerimeter()
        {
            return 2 * (Length + Width);
        }
    }

    class Square : Rectangle
    { 
        public Square(double side) : base(side, side)
        {
        }
        // 正方形可以复用矩形的面积和周长计算
        // 也可以重写方法，但这里不需要
    }
    class Circle : Shape
    {
        public double Radius { get; set; }
        public Circle(double radius)
        {
            Radius = radius;
        }
        public override double CalculateArea()
        {
            return Math.PI * Radius * Radius;
        }
        public override double CalculatePerimeter()
        {
            return 2 * Math.PI * Radius;
        }
    }
    // - 构造函数接收边长
    // - 实现面积和周长计算

    // TODO: 实现圆形类 Circle : Shape
    // - 属性：Radius
    // - 构造函数接收半径
    // - 实现面积和周长计算（使用Math.PI）
    #endregion
}

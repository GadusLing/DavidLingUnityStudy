namespace 多态作业
{
    internal class Program
    {
        class Duck
        {
            public virtual string Quack()
            {
                return "鸭子叫声";
            }
        }
        class RealDuck : Duck
        {
            public override string Quack()
            {
                return "嘎嘎叫";
            }
        }
        class WoodenDuck : Duck
        {
            public override string Quack()
            {
                return "吱吱叫";
            }
        }
        class RubberDuck : Duck
        {
            public override string Quack()
            {
                return "唧唧叫";
            }
        }

        class Employee
        {
            public virtual string ClockIn()
            {
                return "员工9点打卡";
            }
        }
        class Manager : Employee
        {
            public override string ClockIn()
            {
                return "经理11点打卡";
            }
        }
        class Programmer : Employee
        {
            public override string ClockIn()
            {
                return "程序员不打卡";
            }
        }

        class Shape
        {
            public virtual double Area()
            {
                return 0;
            }
            public virtual double Perimeter()
            {
                return 0;
            }
        }
        class Rectangle : Shape
        {
            private double width;
            private double height;
            public Rectangle(double width, double height)
            {
                this.width = width;
                this.height = height;
            }
            public override double Area()
            {
                return width * height;
            }
            public override double Perimeter()
            {
                return 2 * (width + height);
            }
        }
        class Square : Rectangle
        {
            public Square(double side) : base(side, side) { }
        }
        class Circle : Shape
        {
            private double radius;
            public Circle(double radius)
            {
                this.radius = radius;
            }
            public override double Area()
            {
                return Math.PI * radius * radius;
            }
            public override double Perimeter()
            {
                return 2 * Math.PI * radius;
            }
        }


        static void Main(string[] args)
        {
            // 题目1：动物叫声多态示例
            // 真的鸭子嘎嘎叫，木头鸭子吱吱叫，橡皮鸭子唧唧叫
            // 要求：创建动物基类和不同类型的鸭子子类，实现多态的叫声方法
            Duck Duck = new RealDuck();
            Console.WriteLine(Duck.Quack());
            Duck = new WoodenDuck();
            Console.WriteLine(Duck.Quack());
            Duck = new RubberDuck();
            Console.WriteLine(Duck.Quack());

            // 题目2：员工打卡系统多态示例  
            // 所有员工9点打卡
            // 但经理十一点打卡，程序员不打卡
            // 要求：创建员工基类和不同职位的子类，实现多态的打卡行为
            Employee employee = new Employee();
            Console.WriteLine(employee.ClockIn());
            employee = new Manager();
            Console.WriteLine(employee.ClockIn());
            employee = new Programmer();
            Console.WriteLine(employee.ClockIn());

            // 题目3：图形计算多态示例
            // 创建一个图形类，有求面积和周长两个方法
            // 创建矩形类，正方形类，圆形类继承图形类
            // 实例化矩形、正方形、圆形对象求面积和周长
            // 要求：使用多态实现不同图形的面积和周长计算
            Shape shape = new Rectangle(5, 10);
            Console.WriteLine($"矩形面积: {shape.Area()}");
            Console.WriteLine($"矩形周长: {shape.Perimeter()}");
            shape = new Square(5);
            Console.WriteLine($"正方形面积: {shape.Area()}");
            Console.WriteLine($"正方形周长: {shape.Perimeter()}");
            shape = new Circle(5);
            Console.WriteLine($"圆形面积: {shape.Area()}");
            Console.WriteLine($"圆形周长: {shape.Perimeter()}");
        }
    }
}

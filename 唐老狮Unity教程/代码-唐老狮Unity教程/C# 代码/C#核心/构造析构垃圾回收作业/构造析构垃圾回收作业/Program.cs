

// 题目1: 基于成员方法练习题  
// 对人类的构造函数进行重载，用人类创建若干个对象  
// - 定义一个类 `Person`，包含多个构造函数，支持不同参数初始化。  
// - 使用这些构造函数创建多个 `Person` 对象。  
class Person
{
    // 默认构造函数
    public Person()
    {
        Name = "David";
        Age = 20;
    }
    // 带一个参数的构造函数
    public Person(string name)
    {
        Name = name;
        Age = 20;
    }
    // 带两个参数的构造函数
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }
    // 显示信息的方法
    public void DisplayInfo()
    {
        Console.WriteLine($"Name: {Name}, Age: {Age}");
    }

    public string Name { get; set; }
    public int Age { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        // 创建不同的 Person 对象
        Person person1 = new Person();
        Person person2 = new Person("Alice");
        Person person3 = new Person("Bob", 30);
        // 显示信息
        person1.DisplayInfo(); // 输出: Name: David, Age: 20
        person2.DisplayInfo(); // 输出: Name: Alice, Age: 20
        person3.DisplayInfo(); // 输出: Name: Bob, Age: 30

        // 创建 Classroom 对象
        Classroom class1 = new Classroom();
        Classroom class2 = new Classroom("Math Class");
        Classroom class3 = new Classroom("Science Class", 25);
        // 显示班级信息
        class1.DisplayInfo(); // 输出: Class Name: Default Class, Student Count: 30
        class2.DisplayInfo(); // 输出: Class Name: Math Class, Student Count: 30
        class3.DisplayInfo(); // 输出: Class Name: Science Class, Student Count: 25

        // 创建 Ticket 对象
        Ticket ticket1 = new Ticket(100);
        Ticket ticket2 = new Ticket(150);
        Ticket ticket3 = new Ticket(250);
        Ticket ticket4 = new Ticket(350);
        // 显示票的信息
        ticket1.Display(); // 输出: 100公里 100块钱
        ticket2.Display(); // 输出: 150公里 142.5块钱
        ticket3.Display(); // 输出: 250公里 225块钱
        ticket4.Display(); // 输出: 350公里 280块钱

    }
}


// 题目2: 基于成员变量练习题  
// 对班级类的构造函数进行重载，用班级类创建若干个对象  
// - 定义一个类 `Classroom`，包含多个构造函数，支持不同参数初始化。  
// - 使用这些构造函数创建多个 `Classroom` 对象。  

class Classroom
{
    // 默认构造函数
    public Classroom()
    {
        ClassName = "Default Class";
        StudentCount = 30;
    }
    // 带一个参数的构造函数
    public Classroom(string className)
    {
        ClassName = className;
        StudentCount = 30;
    }
    // 带两个参数的构造函数
    public Classroom(string className, int studentCount)
    {
        ClassName = className;
        StudentCount = studentCount;
    }
    // 显示信息的方法
    public void DisplayInfo()
    {
        Console.WriteLine($"Class Name: {ClassName}, Student Count: {StudentCount}");
    }
    public string ClassName { get; set; }
    public int StudentCount { get; set; }
}

// 题目3: Ticket类  
// - 定义一个类 `Ticket`，包含一个 `distance` 成员变量（构造时赋值，不能为负数）。  
// - 定义一个 `GetPrice` 方法，根据 `distance` 计算价格：  
//   - 0~100公里：1元/公里  
//   - 101~200公里：9.5折  
//   - 201~300公里：9折  
//   - 300公里以上：8折  
// - 定义一个 `Display` 方法，显示票的信息，例如：`100公里100块钱`。

class Ticket
{
    // 构造函数，确保距离不能为负数
    public Ticket(double distance)
    {
        if (distance < 0)
        {
            throw new ArgumentException("Distance cannot be negative.");
        }
        this.distance = distance;
    }
    // 计算价格的方法
    public double GetPrice()
    {
        if (distance <= 100)
        {
            return distance * 1.0;
        }
        else if (distance <= 200)
        {
            return distance * 1.0 * 0.95; // 9.5折
        }
        else if (distance <= 300)
        {
            return distance * 1.0 * 0.90; // 9折
        }
        else
        {
            return distance * 1.0 * 0.80; // 8折
        }
    }
    // 显示票的信息
    public void Display()
    {
        Console.WriteLine($"{distance}公里 {GetPrice()}块钱");
    }

    private double distance;
}

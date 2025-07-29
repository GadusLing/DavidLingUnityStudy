// 1. 3P是什么？
//private public protected

// 2. 定义一个人类，有姓名，身高，年龄，家庭住址等特征
//    用人创建若干个对象
class Person
{
    public string Name;
    public double Height;
    public int Age;
    public string Address;
}
class Program
{
    static void Main(string[] args)
    {
        Person person1 = new Person();
        person1.Name = "Alice";
        person1.Height = 1.65;
        person1.Age = 25;
        person1.Address = "123 Main St";

        Person person2 = new Person();
        person2.Name = "Bob";
        person2.Height = 1.80;
        person2.Age = 30;
        person2.Address = "456 Elm St";

        // 输出对象信息
        Console.WriteLine($"{person1.Name}, {person1.Height}, {person1.Age}, {person1.Address}");
        Console.WriteLine($"{person2.Name}, {person2.Height}, {person2.Age}, {person2.Address}");

        Student student = new Student();
        student.Name = "Charlie";
        student.Height = 1.75;
        student.Age = 20;
        student.Address = "789 Oak St";
        student.StudentID = "S12345";
        student.Deskmate = (Student)person1; // 设置同桌为person1

        ClassRoom classRoom = new ClassRoom("Computer Science", 10);
        classRoom.AddStudent(student);
        classRoom.AddStudent((Student)person1); // 将person1作为学生添加到班级中

    }
}

// 3. 定义一个学生类，有姓名，学号，年龄，同桌等特征，有学习方法。
//    用学生类创建若干个学生
class Student : Person
{
    public string StudentID;
    public Student Deskmate;
}

// 4. 定义一个班级类，有专业名称，教师容量，学生等
//    创建一个班级对象

class ClassRoom
{
    public string MajorName;
    public int Capacity;
    public List<Student> Students;

    public ClassRoom(string majorName, int capacity)
    {
        MajorName = majorName;
        Capacity = capacity;
        Students = new List<Student>();
    }

    public void AddStudent(Student student)
    {
        if (Students.Count < 10)
        {
            Students.Add(student);
        }
        else
        {
            Console.WriteLine("班级已满，无法添加更多学生。");
        }
    }
}

// 5. Person p = new Person();
//    p.age = 10;
//    Person p2 = new Person();
//    p2.age = 20;
//    请问p.age为多少?
// 10


// 6. Person p = new Person();
//    p.age = 10;
//    Person p2 = p;
//    p2.age = 20;
//    请问p.age为多少?
// 20

// 7. Student s = new Student();
//    s.age = 10;
//    int age = s.age;
//    age = 20;
//    请问s.age为多少?
// 10

// 8. Student s = new Student();
//    s.deskmate = new Student();
//    s.deskmate.age = 10;
//    Student s2 = s.deskmate;
//    s2.age = 20;
//    请问s.deskmate.age为多少?
// 20
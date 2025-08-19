using System.Reflection;

namespace 成员属性作业
{

    internal class Program
    {
        // 1. 定义一个学生类，包含五个属性：姓名、性别、年龄、CSharp成绩、Unity成绩。
        // 2. 有两个方法：
        //    - 一个打招呼方法：介绍自己叫什么名字，今年几岁了，是男同学还是女同学。
        //    - 一个计算自己总分数和平均分并显示的方法。
        // 3. 使用属性实现：
        //    - 年龄必须是0~150岁之间。
        //    - 成绩必须是0~100之间。
        //    - 性别只能是男或女。
        // 4. 实例化两个对象并测试。
        class Student
        {
            public void Hello()
            {
                Console.WriteLine($"大家好，我叫{Name}，今年{Age}岁了，是{Sex}同学。");
            }

            public void ShowScores()
            {
                double totalScore = CSharpScore + UnityScore;
                double averageScore = totalScore / 2.0;
                Console.WriteLine($"我的总分数是{totalScore}，平均分是{averageScore}。");
            }

            public string Name { get; set; }

            private string sex;
            public string Sex
            {
                get => sex;
                set
                {
                    if (value != "男" && value != "女")
                    {
                        throw new ArgumentException("性别只能是男或女。");
                    }
                    sex = value;
                }
            }

            private int age;
            public int Age
            {
                get => age;
                set
                {
                    if (value < 0 || value > 150)
                    {
                        throw new ArgumentOutOfRangeException("年龄必须在0到150岁之间。");
                    }
                    age = value;
                }
            }

            private double csharpScore;
            public double CSharpScore
            {
                get => csharpScore;
                set
                {
                    if (value >= 0 && value <= 100)
                        csharpScore = value;
                    else
                        throw new ArgumentException("CSharp成绩必须是0~100之间");
                }
            }

            private double unityScore;
            public double UnityScore
            {
                get => unityScore;
                set
                {
                    if (value >= 0 && value <= 100)
                        unityScore = value;
                    else
                        throw new ArgumentException("Unity成绩必须是0~100之间");
                }
            }
        }

        static void Main(string[] args)
        {
            // 实例化两个学生对象
            Student student1 = new Student
            {
                Name = "张三",
                Sex = "男",
                Age = 20,
                CSharpScore = 85,
                UnityScore = 90
            };

            Student student2 = new Student
            {
                Name = "李四",
                Sex = "女",
                Age = 20,
                CSharpScore = 98,
                UnityScore = 88
            };

            student1.Hello();
            student1.ShowScores();

            student2.Hello();
            student2.ShowScores();
        }
    }
}

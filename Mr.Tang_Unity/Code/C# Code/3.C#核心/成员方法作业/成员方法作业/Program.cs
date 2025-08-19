namespace 成员方法作业
{
    // 练习题1：
    // 基于成员变量练习题
    // 为人类（Person）类定义说话（Speak）、走路（Walk）、吃饭（Eat）等方法
    class Person
    {
        public void Speak()
        {
            Console.WriteLine($"你好，我是 {this._Name}，今年 {this._age} 岁，身高 {this._height} 米，住在 {this._address}。");
        }

        public void Walk()
        {
            Console.WriteLine($"{this._Name} 正在前往目的地。");
        }

        public void Eat()
        {
            Console.WriteLine($"{this._Name} 正在进食。");
        }

        public string _Name;
        public float _height;
        public int _age;
        public string _address;
    }

    // 练习题2：
    // 基于成员变量练习题
    // 为学生（Student）类定义学习（Study）、吃饭（Eat）等方法

    class Student : Person
    {
        public void Study()
        {
            Console.WriteLine($"{this._Name} 正在学习。");
        }

        // 重写 Eat 方法
        public new void Eat()
        {
            Console.WriteLine($"{this._Name} 正在吃学生餐。");
        }
    }

    // 练习题3：
    // 定义一个食物（Food）类，有名称（Name）、热量（Calories）等特征
    // 思考如何和人类（Person）以及学生（Student）类联系起来 

    class Food
    {
        public void Describe()
        {
            Console.WriteLine($"食物名称：{_Name}，热量：{_Calories} 卡路里。");
        }

        public void Consume(Person person)
        {
            Console.WriteLine($"{person._Name} 正在吃 {_Name}。");
        }

        public string _Name { get; set; }
        public int _Calories { get; set; }

    }

    internal class Program
    {

        static void Main(string[] args)
        {
            Person person = new Person
            {
                _Name = "小伟",
                _age = 27,
                _height = 1.75f,
                _address = "武汉"
            };
            person.Speak();
            person.Walk();
            person.Eat();

            Student student = new Student
            {
                _Name = "小明",
                _age = 15,
                _height = 1.80f,
                _address = "北京"
            };
            student.Study();
            student.Eat();// 这里是发生了方法隐藏，调用的是 Student 类中的 Eat 方法

            Food food = new Food
            {
                _Name = "苹果",
                _Calories = 52
            };
            food.Describe();
            food.Consume(person); // Person 吃苹果
        }
    }
}

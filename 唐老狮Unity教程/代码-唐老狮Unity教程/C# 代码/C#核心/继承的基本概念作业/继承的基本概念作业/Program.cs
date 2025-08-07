namespace 继承的基本概念作业
{
    internal class Program
    {
        /*
        * 作业题目：继承的基本概念
        * 要求：
        * 1. 写一个人类，人类中有姓名、年龄属性，有说话行为
        * 2. 写一个战士类继承人类，有攻击行为
        */

        class Person
        {
            public string _Name { get; set; }
            public int _Age { get; set; }

            public void Speak()
            {
                Console.WriteLine($"{_Name}正在说话");
            }
        }

        class Warrior : Person
        {
            public void Attack()
            {
                Console.WriteLine($"{_Name}正在攻击");
            }
        }

        static void Main(string[] args)
        {
            // 创建一个人类实例
            Person person = new Person();
            person._Name = "张三";
            person._Age = 25;
            person.Speak(); // 输出：张三正在说话
            // 创建一个战士类实例
            Warrior warrior = new Warrior();
            warrior._Name = "李四";
            warrior._Age = 30;
            warrior.Speak(); // 输出：李四正在说话
            warrior.Attack(); // 输出：李四正在攻击

        }
    }
}

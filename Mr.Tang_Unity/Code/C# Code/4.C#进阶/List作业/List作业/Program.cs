using System.Reflection.Metadata.Ecma335;

namespace List作业
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 题目1：请描述List<T>和ArrayList的区别
            /*
             * List<T> 和 ArrayList 的主要区别：
             * 
             * 1. 类型安全性：
             *    - List<T>：泛型集合，编译时类型安全，只能存储指定类型T的元素
             *    - ArrayList：非泛型集合，存储object类型，运行时可能出现类型错误
             * 
             * 2. 性能：
             *    - List<T>：无需装箱/拆箱操作，性能更好
             *    - ArrayList：值类型需要装箱/拆箱，性能较差
             * 
             * 3. 内存使用：
             *    - List<T>：内存使用更高效，无额外的装箱开销
             *    - ArrayList：值类型装箱会产生额外的内存开销
             * 
             * 4. IntelliSense支持：
             *    - List<T>：IDE能提供准确的智能提示和编译时检查
             *    - ArrayList：返回object类型，需要手动类型转换
             * 
             * 5. .NET版本：
             *    - List<T>：.NET 2.0引入的泛型集合，推荐使用
             *    - ArrayList：.NET 1.0的遗留类型，不推荐在新项目中使用
             * 
             * 示例对比：
             * List<int> numbers = new List<int>(); // 类型安全
             * numbers.Add(1); // 直接添加int
             * int value = numbers[0]; // 无需转换
             * 
             * ArrayList list = new ArrayList(); // 非类型安全
             * list.Add(1); // 装箱为object
             * int value = (int)list[0]; // 需要强制转换和拆箱
             */

            // 题目2：建立一个整形List，为它添加10~1
            // 删除List中第五个元素
            // 遍历剩余元素并打印
            List<int> numbers = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                numbers.Add(10 - i);
            }

            numbers.RemoveAt(4);

            foreach (int number in numbers)
            {
                Console.WriteLine(number);
            }

            // 题目3：一个Monster基类，Boss和Gablin类继承它。
            // 在怪物类的构造函数中，将其存储到一个怪物List中
            // 遍历列表可以让Boss和Gablin对象产生不同攻击
            // 测试
            Monster boss = new Boss("大魔王");
            Monster gablin = new Gablin("哥布林");
            foreach (var monster in Monster.Monsters)
            {
                Console.WriteLine($"怪物ID: {monster.ID}, 名称: {monster.Name}");
                monster.Attack();
            }

        }

        abstract class Monster
        {
            private static int _nextId = 10000;
            public int ID { get; }
            public string Name { get; set; }
            public static List<Monster> Monsters { get; } = new List<Monster>();

            public Monster(string name)
            {
                ID = _nextId++;
                Name = name;
            }
            public virtual void Attack()
            {
                Console.WriteLine($"{Name} attacks!");
            }
        }

        class Boss : Monster
        {
            public Boss(string name) : base(name)
            {
                Monsters.Add(this);
            }
            public override void Attack()
            {
                Console.WriteLine($"{Name}发动“毁天灭地”");
            }
        }

        class Gablin : Monster
        {
            public Gablin(string name) : base(name)
            {
                Monsters.Add(this);
            }
            public override void Attack()
            {
                Console.WriteLine($"{Name}发动“偷袭”");
            }
        }
    }
}

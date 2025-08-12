namespace 万物之父object中的方法
{
    class Player
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int AttackPower { get; set; }
        public int Defense { get; set; }
        public double DodgeRate { get; set; }
        public override string ToString()
        {
            return $"玩家{Name}，血量{Health}，攻击力{AttackPower}，防御力{Defense}, 闪避率{DodgeRate}";
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // 题目1：
            // 有一个玩家类，有姓名，血量，攻击力，防御力，闪避率等特征
            // 请在控制台打印出 "玩家XXX，血量XX，攻击力XXX，防御力XX" XX为具体内容
            Player player = new Player
            {
                Name = "张三",
                Health = 100,
                AttackPower = 20,
                Defense = 10,
                DodgeRate = 0.1
            };
            Console.WriteLine(player.ToString());

            // 题目2：
            // 一个Monster类的引用对象A，Monster类有 攻击力、防御力、血量、
            // 技能ID等属性。我想复制一个和A对象一模一样的B对象，并且改变了B
            // 的属性，A不会受到影响。请问如何实现？
            Monster monsterA = new Monster
            {
                AttackPower = 50,
                Defense = 30,
                Health = 200,
                SkillID = 1
            };
            Monster monsterB = monsterA.Clone(); // 使用Clone方法复制A对象,
                                                 // 因为不涉及引用类型，所以可以直接用MemberwiseClone()
                                                 // 复制A对象的值类型属性，是浅拷贝

            // 改变B的属性
            monsterB.AttackPower = 60;
            monsterB.Defense = 40;
            monsterB.Health = 250;
            monsterB.SkillID = 2;
            // 打印A和B的属性，再打印A的属性，验证A没有受到影响
            Console.WriteLine("A对象的属性：" + monsterA.ToString());
            Console.WriteLine("B对象的属性：" + monsterB.ToString());
            Console.WriteLine("A对象的属性：" + monsterA.ToString());
        }
    }

    class Monster
    {
        public int AttackPower { get; set; }
        public int Defense { get; set; }
        public int Health { get; set; }
        public int SkillID { get; set; }
        public override string ToString()
        {
            return $"怪物攻击力{AttackPower}，防御力{Defense}，血量{Health}，技能ID{SkillID}";
        }
        public Monster Clone()
        {
            return MemberwiseClone() as Monster;
        }

    }

}

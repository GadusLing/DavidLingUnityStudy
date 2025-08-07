namespace 里氏替换原则作业
{
    internal class Program
    {
        class Monster
        { 
        }

        class Boss : Monster
        {
            public void UseSkill()
            {
                Console.WriteLine("Boss使用技能!");
            }
        }
        class Goblin : Monster
        {
            public void Attack()
            {
                Console.WriteLine("Goblin普通攻击!");
            }
        }

        static void Main(string[] args)
        {
            // TODO: 实现以下三道里氏替换原则练习题

            // 题目1: is和as的区别是什么
            // 要求：解释is和as两个关键字的区别和使用场景
            // is关键字用于检查一个对象是否是某个特定类型的实例，返回布尔值。
            // as关键字用于将一个对象转换为指定类型，成功返回对应类型，转换失败则返回null。

            // 题目2: 怪物攻击系统
            // 要求：写一个Monster类，它派生出Boss和Goblin两个类，Boss有技能；
            // 小怪有攻击；随机生成10个怪，装载到数组中，遍历这个数组，调用它们的
            // 攻击方法，如果是boss就释放技能
            Monster[] monsters = new Monster[10];
            Random random = new Random();
            for (int i = 0; i < monsters.Length; i++)
            {
                if (random.Next(2) == 0)
                {
                    monsters[i] = new Boss();
                }
                else
                {
                    monsters[i] = new Goblin();
                }
            }
            foreach (var monster in monsters)
            {
                if (monster is Boss)
                {
                    (monster as Boss).UseSkill();
                }
                else if (monster is Goblin)
                {
                    (monster as Goblin).Attack();
                }
            }

            // 题目3: FPS游戏模拟
            // 要求：写一个玩家类，玩家可以拥有各种武器
            // 现在有四种武器，冲锋枪，散弹枪，手枪，匕首
            // 玩家默认拥有匕首
            // 请在玩家类中写一个方法，可以拾取不同的武器替换自己拥有的枪械
            Player player = new Player();
            player.ShowCurrentWeapon(); // 显示当前武器
            player.UseWeapon(); // 使用当前武器
            player.PickUpWeapon(new SubmachineGun()); // 捡起冲锋枪
            player.ShowCurrentWeapon(); // 显示当前武器
            player.UseWeapon(); // 使用冲锋枪
            player.PickUpWeapon(new ShotGun()); // 捡起散弹枪
            player.ShowCurrentWeapon(); // 显示当前武器
            player.UseWeapon(); // 使用散弹枪
            player.PickUpWeapon(new Pistol()); // 捡起手枪
            player.ShowCurrentWeapon(); // 显示当前武器
            player.UseWeapon(); // 使用手枪
            player.PickUpWeapon(new Dagger()); // 捡起匕首
            player.ShowCurrentWeapon(); // 显示当前武器
            player.UseWeapon(); // 使用匕首


        }

        abstract class Weapon
        {
            public abstract void Use();
        }

        class SubmachineGun : Weapon
        {
            public override void Use()
            {
                Console.WriteLine("使用冲锋枪扫射!");
            }
        }

        class ShotGun : Weapon
        {
            public override void Use()
            {
                Console.WriteLine("使用散弹枪轰击!");
            }
        }

        class Pistol : Weapon
        {
            public override void Use()
            {
                Console.WriteLine("使用手枪射击!");
            }
        }

        class Dagger : Weapon
        {
            public override void Use()
            {
                Console.WriteLine("使用匕首刺击!");
            }
        }

        class Player
        {
            private Weapon _currentWeapon = new Dagger(); // 默认武器是匕首

            public void PickUpWeapon(Weapon weapon)
            {
                _currentWeapon = weapon; // 里氏替换原则：子类可以替换父类
                Console.WriteLine($"玩家拾取了新武器{weapon.GetType().Name}");
            }

            public void UseWeapon()
            {
                _currentWeapon.Use(); // 多态调用
            }

            public void ShowCurrentWeapon()
            {
                Console.WriteLine($"玩家当前武器: {_currentWeapon.GetType().Name}");
            }
        }

    }
}

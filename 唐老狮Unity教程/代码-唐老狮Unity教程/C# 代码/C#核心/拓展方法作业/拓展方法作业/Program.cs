using System;

namespace 拓展方法作业
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            
            // TODO: 在这里测试扩展方法
            // 1. 测试整形扩展方法 - 求平方
            Console.WriteLine(5.GetSquare());
            // 2. 测试玩家类扩展方法 - 受伤方法
            Player player = new Player { Name = "Hero", Health = 100, Defense = 10 };
            player.TakeDamage(30);
            // 3. 测试玩家类扩展方法 - 自杀方法
            player.Suicide();
            // 4. 测试玩家类扩展方法 - 移动方法
            player.Move("东北方");
        }
    }
    
    // TODO: 为整形类型添加扩展方法
    public static class IntExtensions
    {
        /// <summary>
        /// 为整形添加求平方的扩展方法
        /// </summary>
        /// <param name="number">要求平方的整数</param>
        /// <returns>返回平方结果</returns>
        public static int GetSquare(this int number)
        {
            // 实现求平方逻辑
            return number * number;
        }
    }
    
    // TODO: 定义玩家类
    public class Player
    {
        /// <summary>
        /// 玩家姓名
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 玩家血量
        /// </summary>
        public int Health { get; set; }
        
        /// <summary>
        /// 玩家攻击力
        /// </summary>
        public int Attack { get; set; }
        
        /// <summary>
        /// 玩家防御力
        /// </summary>
        public int Defense { get; set; }
        
        // TODO: 添加构造函数和其他必要的属性
    }
    
    // TODO: 为玩家类添加扩展方法
    public static class PlayerExtensions
    {
        /// <summary>
        /// 玩家受伤扩展方法
        /// </summary>
        /// <param name="player">受伤的玩家</param>
        /// <param name="damage">受到的伤害值</param>
        public static void TakeDamage(this Player player, int damage)
        {
            // TODO: 实现受伤逻辑
            // 考虑防御力对伤害的减免
            // 更新玩家血量
            // 处理玩家死亡情况
            if (player == null) throw new ArgumentNullException(nameof(player));
            if (damage < 0) throw new ArgumentOutOfRangeException(nameof(damage), "Damage cannot be negative.");
            int effectiveDamage = Math.Max(0, damage - player.Defense);
            player.Health -= effectiveDamage;
            if (player.Health <= 0)
            {
                player.Health = 0;
                Console.WriteLine($"{player.Name} has died.");
            }
            else
            {
                Console.WriteLine($"{player.Name} took {effectiveDamage} damage, remaining health: {player.Health}");
            }
        }
        
        /// <summary>
        /// 玩家移动扩展方法
        /// </summary>
        /// <param name="player">要移动的玩家</param>
        /// <param name="direction">移动方向</param>
        public static void Move(this Player player, string direction)
        {
            // TODO: 实现移动逻辑
            Console.WriteLine($"{player.Name} moved {direction}.");
        }
        
        /// <summary>
        /// 玩家自杀扩展方法
        /// </summary>
        /// <param name="player">要自杀的玩家</param>
        public static void Suicide(this Player player)
        {
            // TODO: 实现自杀逻辑
            // 直接将血量设为0或负数
            Console.WriteLine($"{player.Name} has committed suicide.");
            player.Health = 0;
        }
    }
}

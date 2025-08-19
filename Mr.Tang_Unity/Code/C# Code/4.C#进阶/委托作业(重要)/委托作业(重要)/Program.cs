using System;
using System.Diagnostics;

namespace 委托作业_重要_
{
    internal class Program
    {
        static Func<bool, bool> cooking = Cook;
        static Func<bool, bool> dinner = ServeDinner;
        static Action<bool> eating = Eat;

        static bool Cook(bool almosthome)
        {
            Console.WriteLine("妈妈开始做饭...");
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("妈妈做饭中...");
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("妈妈做饭中...");
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("饭做好了!!！");
            return true;
        }
        static bool ServeDinner(bool isServed)
        {
            Console.WriteLine("妈妈去叫人");
            System.Threading.Thread.Sleep(3000);
            Console.WriteLine("饭做好了，快过来吃饭！");
            return true;
        }
        static void Eat(bool isEating)
        {
            if (isEating)
            {
                Console.WriteLine("爸爸和小孩听到，去餐桌");
                System.Threading.Thread.Sleep(2000);
                Console.WriteLine("爸爸、妈妈和孩子都在吃饭...");
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("吃饭中...");
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("吃饭中...");
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("都吃饱啦！！！");
            }
        }
        static void Main(string[] args)
        {
            // 题目1：一家三口，妈妈做饭，爸爸妈妈和孩子都要吃饭
            // 用委托模拟做饭——>开饭——>吃饭的过程
            // 
            // 实现要求：
            // - 定义做饭委托
            // - 定义开饭委托  
            // - 定义吃饭委托
            // - 妈妈负责做饭
            // - 爸爸、妈妈、孩子都要吃饭
            // - 按照做饭->开饭->吃饭的流程执行

            //Console.WriteLine("=== 题目1：一家三口吃饭场景 ===");
            //// 在这里实现题目1的代码...
            //Console.WriteLine("爸爸下班啦，给妈妈打电话");
            //bool goOffWork = true;
            //Console.WriteLine("爸爸：妈妈，我快到家了，你做好饭了吗？");
            //eating(dinner(cooking(goOffWork)));



            Console.WriteLine("\n=== 题目2：怪物死亡奖励系统 ===");

            // 题目2：怪物死亡后，玩家要加10块钱，界面要更新数据，成就要累加怪物击杀数
            // 请用委托来模拟实现这些功能，只用与核心逻辑相关表现这个过程，不用写的太复杂
            //
            // 实现要求：
            // - 定义怪物死亡事件委托
            // - 玩家金钱增加功能 (+10块钱)
            // - 界面数据更新功能
            // - 成就系统击杀数累加功能
            // - 当怪物死亡时，通过委托触发所有相关功能
            // - 保持代码简洁，专注于委托的使用

            // 在这里实现题目2的代码...
            Console.WriteLine("\n=== 题目2：怪物死亡奖励系统 ===");

            DefeatMonster += Increasemoney;
            DefeatMonster += UpdateUI;
            DefeatMonster += IncrementKillCount;
            Console.WriteLine("激烈的战斗");
            Console.WriteLine("玩家击杀了怪物");
            bool index = true;
            DefeatMonster(index);

            DefeatMonster = null;
            Console.WriteLine("\n委托已断开，尝试再次调用...");

            // 方法1：使用 null 条件运算符（推荐）
            DefeatMonster?.Invoke(index);

            // 方法2：手动检查null
            if (DefeatMonster != null)
            {
                DefeatMonster(index);
            }
            else
            {
                Console.WriteLine("委托为null，无法触发奖励事件");
            }
        }

        static Action<bool> DefeatMonster;
        static void Increasemoney(bool index)
        {
            if(index)
                Console.WriteLine("玩家获得10块钱奖励");
        }
        private static void UpdateUI(bool index)
        {
            if (index)
                Console.WriteLine("界面数据已更新");
        }
        static void IncrementKillCount(bool index)
        {
            if (index)
                Console.WriteLine("成就系统：击杀数增加1");
        }
    }
}

using System.Collections;
using System.Diagnostics;

namespace Hashtable作业
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hashtable作业");

            // 题目1：请描述Hashtable的存储规则
            /*
             * Hashtable存储规则：
             * 
             * 1. 哈希函数计算：
             *    - 对键（Key）调用GetHashCode()方法计算哈希值
             *    - 将哈希值通过模运算映射到内部数组的索引位置
             *    - 公式：index = Math.Abs(key.GetHashCode()) % buckets.Length
             * 
             * 2. 存储结构：
             *    - 内部使用数组（bucket）存储键值对
             *    - 每个数组元素可以存储一个或多个键值对（处理冲突）
             *    - 初始容量通常为质数（如11），有利于均匀分布
             * 
             * 3. 冲突处理（链地址法）：
             *    - 当多个键映射到同一索引时发生哈希冲突
             *    - 使用链表结构在同一位置存储多个键值对
             *    - 查找时需要遍历链表比较键的相等性
             * 
             * 4. 动态扩容：
             *    - 当负载因子（元素数量/数组长度）超过阈值（通常0.75）时自动扩容
             *    - 扩容时创建新的更大数组（通常是原来的2倍）
             *    - 重新计算所有元素的哈希值并重新分布（rehashing）
             * 
             * 5. 查找过程：
             *    - 计算键的哈希值得到索引
             *    - 在对应位置的链表中查找匹配的键
             *    - 使用Equals()方法比较键的相等性
             * 
             * 6. 性能特点：
             *    - 平均时间复杂度：O(1)
             *    - 最坏时间复杂度：O(n)（所有元素都冲突到同一位置）
             *    - 空间复杂度：O(n)
             */
            
            // 题目2：制作一个怪物管理器，提供创建怪物，移除怪物的方法。每个怪物都有自己的唯一ID
            // 测试怪物管理器
            MonsterManager manager = MonsterManager.Instance;
            
            // 创建怪物
            Monster monster1 = manager.AddMonster("哥布林", "小型绿色怪物");
            Monster monster2 = manager.AddMonster("兽人", "强壮的战士");
            Monster monster3 = manager.AddMonster("巨龙", "传说中的强大生物");
            
            Console.WriteLine($"创建了 {manager.GetMonsterCount()} 个怪物");
            
            // 显示所有怪物
            manager.DisplayAllMonsters();
            
            // 根据ID查找怪物
            Monster foundMonster = manager.GetMonsterById(monster2.Id);
            if (foundMonster != null)
            {
                Console.WriteLine($"找到怪物: ID={foundMonster.Id}, 名称={foundMonster.Name}");
            }
            
            // 移除怪物
            bool removed = manager.RemoveMonster(monster2.Id);
            Console.WriteLine($"移除怪物 {monster2.Id}: {(removed ? "成功" : "失败")}");
            
            Console.WriteLine($"移除后剩余 {manager.GetMonsterCount()} 个怪物");
            manager.DisplayAllMonsters();
        }

        class Monster
        {
            public int Id { get; }
            public string Name { get; }
            public string Description { get; }
            
            public Monster(int id, string name = "未命名怪物", string description = "无描述")
            {
                Id = id;
                Name = name;
                Description = description;
            }
            
            public override string ToString()
            {
                return $"Monster[ID={Id}, Name={Name}, Description={Description}]";
            }
        }

        // 管理器一般都是唯一的，所以采用单例模式
        class MonsterManager
        {
            // 饿汉单例模式————一开始就把实例化做好
            private static MonsterManager instance = new MonsterManager();
            private Hashtable _monstersTable = new Hashtable();
            private int monsterID = 10000;

            private MonsterManager()
            {
            }

            public static MonsterManager Instance
            {
                get { return instance; }
            }

            /// <summary>
            /// 添加怪物
            /// </summary>
            /// <param name="name">怪物名称</param>
            /// <param name="description">怪物描述</param>
            /// <returns>创建的怪物对象</returns>
            public Monster AddMonster(string name = "未命名怪物", string description = "无描述")
            {
                Monster monster = new Monster(monsterID, name, description);
                _monstersTable.Add(monsterID, monster);
                monsterID++; // 确保下一个ID是唯一的
                Console.WriteLine($"添加怪物成功: {monster}");
                return monster;
            }
            
            /// <summary>
            /// 根据ID移除怪物
            /// </summary>
            /// <param name="id">怪物ID</param>
            /// <returns>是否移除成功</returns>
            public bool RemoveMonster(int id)
            {
                if (_monstersTable.ContainsKey(id))
                {
                    Monster monster = (Monster)_monstersTable[id];
                    _monstersTable.Remove(id);
                    Console.WriteLine($"移除怪物成功: {monster}");
                    return true;
                }
                Console.WriteLine($"未找到ID为 {id} 的怪物");
                return false;
            }
            
            /// <summary>
            /// 根据ID获取怪物
            /// </summary>
            /// <param name="id">怪物ID</param>
            /// <returns>怪物对象，如果不存在则返回null</returns>
            public Monster GetMonsterById(int id)
            {
                if (_monstersTable.ContainsKey(id))
                {
                    return (Monster)_monstersTable[id];
                }
                return null;
            }
            
            /// <summary>
            /// 获取怪物总数
            /// </summary>
            /// <returns>怪物数量</returns>
            public int GetMonsterCount()
            {
                return _monstersTable.Count;
            }
            
            /// <summary>
            /// 显示所有怪物信息
            /// </summary>
            public void DisplayAllMonsters()
            {
                Console.WriteLine("=== 所有怪物列表 ===");
                if (_monstersTable.Count == 0)
                {
                    Console.WriteLine("暂无怪物");
                    return;
                }
                
                foreach (DictionaryEntry entry in _monstersTable)
                {
                    Console.WriteLine($"  {entry.Value}");
                }
                Console.WriteLine("==================");
            }
            
            /// <summary>
            /// 检查指定ID的怪物是否存在
            /// </summary>
            /// <param name="id">怪物ID</param>
            /// <returns>是否存在</returns>
            public bool ContainsMonster(int id)
            {
                return _monstersTable.ContainsKey(id);
            }
        }
    }
}

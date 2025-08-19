using System.Diagnostics;

namespace list排序作业
{
    internal class Program
    {
        class Monster
        {
            private Monster(int id, string name, int attack, int defense, int health)
            {
                Id = id;
                Name = name;
                Attack = attack;
                Defense = defense;
                Health = health;
            }
            public static void AddMonster(int id, string name, int attack, int defense, int health)
            {
                GlobalMonsterList.Add(new Monster(id, name, attack, defense, health));
            }
            public override string ToString()
            {
                return $"Id: {Id}, 名称: {Name}, 攻击力: {Attack}, 防御力: {Defense}, 血量: {Health}";
            }
            public int CompareTo(Monster other, string sortBy)
            {
                //switch (sortBy)
                //{
                //    //传统写法
                //    case "Attack":
                //        return Attack.CompareTo(other.Attack);
                //    case "Defense":
                //        return Defense.CompareTo(other.Defense);
                //    case "Health":
                //        return Health.CompareTo(other.Health);
                //    default:
                //        return 0;
                //}
                return sortBy switch
                {
                    "攻击力" => Attack.CompareTo(other.Attack),
                    "防御力" => Defense.CompareTo(other.Defense),
                    "血量" => Health.CompareTo(other.Health),
                    _ => 0,
                };
            }
            private static List<Monster> GlobalMonsterList = new List<Monster>();
            public static List<Monster> GetAllMonsters()
            {
                return GlobalMonsterList;
            }
            public int Id { get; set; }
            public string Name { get; set; }
            public int Attack { get; set; }
            public int Defense { get; set; }
            public int Health { get; set; }
        }

        class Item
        {
            public Item(string type, string name, string quality)
            {
                Type = type;
                Name = name;
                Quality = quality;
            }
            public string Type { get; set; }
            public string Name { get; set; }
            public string Quality { get; set; }
            public override string ToString()
            {
                return $"类型: {Type}, 品质: {Quality}, 名称: {Name}";
            }
        }

        static void Main(string[] args)
        {
            //// 练习题1：怪物类排序
            //// 写一个怪物类，创建10个怪物将其添加到List中
            //// 对List列表进行行排序，根据用户输入数字进行排序
            //// 1、攻击排序
            //// 2、防御排序
            //// 3、血量排序
            //// 4、反转

            //Random r = new Random();
            //List<Monster> monsters = Monster.GetAllMonsters();

            //// 创建怪物
            //for (int i = 1; i <= 10; i++)
            //{
            //    Monster.AddMonster(i, $"怪物{i}号", r.Next(1, 100), r.Next(1, 100), r.Next(1, 100));
            //}

            //Console.WriteLine("原始怪物列表：");
            //foreach (var monster in monsters)
            //{
            //    Console.WriteLine(monster);
            //}

            //while (true)
            //{
            //    Console.WriteLine("\n请选择排序方式：");
            //    Console.WriteLine("1. 攻击力升序");
            //    Console.WriteLine("2. 攻击力降序");
            //    Console.WriteLine("3. 防御力升序");
            //    Console.WriteLine("4. 防御力降序");
            //    Console.WriteLine("5. 血量升序");
            //    Console.WriteLine("6. 血量降序");
            //    Console.WriteLine("7. 反转");
            //    Console.WriteLine("0. 退出");

            //    string input = Console.ReadLine();

            //    switch (input)
            //    {
            //        case "1":
            //            monsters.Sort((x, y) => x.CompareTo(y, "攻击力"));
            //            break;
            //        case "2":
            //            monsters.Sort((x, y) => y.CompareTo(x, "攻击力"));
            //            break;
            //        case "3":
            //            monsters.Sort((x, y) => x.CompareTo(y, "防御力"));
            //            break;
            //        case "4":
            //            monsters.Sort((x, y) => y.CompareTo(x, "防御力"));
            //            break;
            //        case "5":
            //            monsters.Sort((x, y) => x.CompareTo(y, "血量"));
            //            break;
            //        case "6":
            //            monsters.Sort((x, y) => y.CompareTo(x, "血量"));
            //            break;
            //        case "7":
            //            monsters.Reverse();
            //            break;
            //        case "0":
            //            return;
            //        default:
            //            Console.WriteLine("无效选择！");
            //            continue;
            //    }

            //    Console.WriteLine("\n排序结果：");
            //    foreach (var monster in monsters)
                 //        Console.WriteLine(monster);
            //    }

            //}


            // 练习题2：物品类排序
            // 写一个物品类（类型，名字，品质），创建10个物品
            // 添加到List中
            // 同时使用类型，品质、名字长度进行比较
            // 排序的权重是：类型 > 品质 > 名字长度
            List<Item> items = new List<Item>
            {
                new Item("武器", "剑", "优质"),
                new Item("护甲", "盾牌", "普通"),
                new Item("药水", "生命药水", "优质"),
                new Item("武器", "斧头", "劣质"),
                new Item("护甲", "头盔", "优质"),
                new Item("武器", "长枪", "普通"),
                new Item("药水", "圣水", "优质"),
                new Item("护甲", "靴子", "普通"),
                new Item("武器", "弓箭", "优质"),
                new Item("护甲", "手套", "劣质")
            };
            Console.WriteLine("原始物品列表：");
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }

            items.Sort((x, y) =>
            {
                // 类型比较
                int typeComparison = GetTypeLevel(x.Type).CompareTo(GetTypeLevel(y.Type));
                if (typeComparison != 0) return typeComparison;  // 类型不同，直接按类型排序

                // 只有当类型相同时，才会比较品质
                int qualityComparison = GetQualityLevel(y.Quality).CompareTo(GetQualityLevel(x.Quality));
                if (qualityComparison != 0) return qualityComparison;  // 品质不同，直接按品质排序

                // 只有类型和品质都相同时，才会比较名字长度
                return y.Name.Length.CompareTo(x.Name.Length);
            });

            Console.WriteLine("\n排序后的物品列表：");
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }

            static int GetTypeLevel(string type)
            {
                return type switch
                {
                    "武器" => 1,
                    "护甲" => 2,
                    "药水" => 3,
                    "饰品" => 4,
                    "材料" => 5,
                    _ => 99  // 未知类型排在最后
                };
            }

            static int GetQualityLevel(string quality)
            {
                return quality switch
                {
                    "劣质" => 1,
                    "普通" => 2,
                    "优质" => 3,
                    "史诗" => 4,
                    "传奇" => 5,
                    _ => 0
                };
            }

            // 练习题3：Dictionary排序
            // 请尝试利用List排序方式对Dictionary中的内容排序
            // 提示：得到Dictionary的所有键值对信息存入List中

            // 创建一个示例 Dictionary
            Dictionary<string, int> playerScores = new Dictionary<string, int>
            {
                {"张三", 85},
                {"李四", 92},
                {"王五", 78},
                {"赵六", 96},
                {"孙七", 81},
                {"周八", 89},
                {"吴九", 74},
                {"郑十", 93}
            };

            Console.WriteLine("\n练习题3：Dictionary排序");
            Console.WriteLine("原始字典内容：");
            foreach (var kvp in playerScores)
            {
                Console.WriteLine($"姓名: {kvp.Key}, 分数: {kvp.Value}");
            }

            // 方法1：转换为 KeyValuePair 列表，按值(分数)降序排序
            var sortedByValue = playerScores.ToList();
            sortedByValue.Sort((x, y) => y.Value.CompareTo(x.Value));

            Console.WriteLine("\n按分数降序排序：");
            foreach (var kvp in sortedByValue)
            {
                Console.WriteLine($"姓名: {kvp.Key}, 分数: {kvp.Value}");
            }

            // 方法2：按键(姓名)升序排序
            var sortedByKey = playerScores.ToList();
            sortedByKey.Sort((x, y) => x.Key.CompareTo(y.Key));

            Console.WriteLine("\n按姓名升序排序：");
            foreach (var kvp in sortedByKey)
            {
                Console.WriteLine($"姓名: {kvp.Key}, 分数: {kvp.Value}");
            }

            // 方法3：使用 LINQ 方式（更简洁）
            Console.WriteLine("\n使用 LINQ 排序（按分数升序）：");
            var linqSorted = playerScores.OrderBy(x => x.Value).ToList();
            foreach (var kvp in linqSorted)
            {
                Console.WriteLine($"姓名: {kvp.Key}, 分数: {kvp.Value}");
            }

            // 方法4：复杂排序 - 先按分数降序，分数相同则按姓名升序
            var complexSorted = playerScores.ToList();
            complexSorted.Sort((x, y) =>
            {
                // 首先按分数降序
                int scoreComparison = y.Value.CompareTo(x.Value);
                if (scoreComparison != 0) return scoreComparison;
                
                // 分数相同时按姓名升序
                return x.Key.CompareTo(y.Key);
            });

            Console.WriteLine("\n复杂排序（分数降序，姓名升序）：");
            foreach (var kvp in complexSorted)
            {
                Console.WriteLine($"姓名: {kvp.Key}, 分数: {kvp.Value}");
            }

            // 方法5：如果需要保持 Dictionary 格式，可以创建新的有序字典
            var orderedDict = new Dictionary<string, int>();
            foreach (var kvp in sortedByValue)
            {
                orderedDict.Add(kvp.Key, kvp.Value);
            }

            Console.WriteLine("\n转换回新的 Dictionary（按分数排序）：");
            foreach (var kvp in orderedDict)
            {
                Console.WriteLine($"姓名: {kvp.Key}, 分数: {kvp.Value}");
            }
        }
    }
}

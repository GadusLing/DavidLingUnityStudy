using System.Collections;

namespace ArrayList作业
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ArrayList作业");

            // ============================================
            // 题目1：请简述ArrayList和数组的区别
            // ============================================
            // 1. 大小：
            //    - 数组：固定大小，创建时必须指定长度
            //    - ArrayList：动态大小，可以自动扩容
            //
            // 2. 类型安全：
            //    - 数组：强类型，只能存储指定类型的元素
            //    - ArrayList：弱类型，可以存储任意类型的对象
            //
            // 3. 性能：
            //    - 数组：访问和操作速度更快
            //    - ArrayList：由于装箱拆箱操作，性能相对较低
            //
            // 4. 内存使用：
            //    - 数组：内存使用效率更高
            //    - ArrayList：存储object类型，占用更多内存
            //
            // 5. 功能： 
            //    - 数组：基本的存储和访问功能
            //    - ArrayList：提供丰富的方法（Add、Remove、Insert、Count等）


            // ============================================
            // 题目2：创建一个背包管理类，使用ArrayList存储物品，
            //       实现购买物品，卖出物品，显示物品的功能。
            //       购买与卖出物品会导致金钱变化     
            // ============================================

            // 简单测试
            BagManager bag = new BagManager();
            Item sword = new Item("剑", 100, "锋利的剑") { _Id = 1 };
            Item shield = new Item("盾", 80, "坚固的盾") { _Id = 2 };
            Item potion = new Item("药水", 50, "恢复生命") { _Id = 3 };

            bag.ShowMoney();
            bag.BuyItem(sword);
            bag.BuyItem(shield);
            bag.BuyItem(potion);
            bag.ShowItems();
            bag.ShowMoney();

            bag.SellItem(1); // 按索引卖出
            bag.ShowItems();
            bag.ShowMoney();
        }

        class Item
        {
            public Item(string name, int price, string description)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("物品名称不能为空", nameof(name));
                if (price < 0)
                    throw new ArgumentException("物品价格不能为负数", nameof(price));
                
                _Name = name;
                _Price = price;
                _Description = description ?? "";
            }

            public int _Id { get; set; }
            public int _Numbers { get; set; } = 1;
            public string _Name { get; }
            public int _Price { get; }
            public string _Description { get; }

            public override string ToString()
            {
                return $"[{_Name}] 价格: {_Price}元 - {_Description}";
            }

            // 🔧 修复：添加nullable注解
            public override bool Equals(object? obj)
            {
                return obj is Item item && _Id == item._Id;
            }

            // 🔧 修复：GetHashCode与Equals保持一致，都使用ID
            public override int GetHashCode()
            {
                return HashCode.Combine(_Id);
            }
        }

        class BagManager
        {
            private readonly ArrayList _items;
            private int _money;
            private const int MAX_CAPACITY = 50; // 🔧 修复：改为const

            public BagManager(int initialMoney = 1000)
            {
                _items = new ArrayList();
                _money = Math.Max(0, initialMoney);
            }

            public int Money => _money;
            public int ItemCount => _items.Count;
            public bool IsFull => _items.Count >= MAX_CAPACITY;

            public bool BuyItem(Item item)
            {
                if (item == null)
                {
                    Console.WriteLine("错误：物品不能为空。");
                    return false;
                }

                if (_money < item._Price)
                {
                    Console.WriteLine($"金钱不足！需要 {item._Price}元，当前只有 {_money}元。");
                    return false;
                }

                if (IsFull)
                {
                    Console.WriteLine($"背包已满！无法购买 {item._Name}。");
                    return false;
                }

                // 检查是否已存在同ID物品，有则堆叠
                foreach (Item existingItem in _items)
                {
                    if (existingItem.Equals(item))
                    {
                        existingItem._Numbers += item._Numbers;
                        _money -= item._Price; // 🔧 修复：堆叠时也要扣钱
                        Console.WriteLine($"已存在物品：{existingItem._Name}，数量增加到 {existingItem._Numbers}");
                        return true;
                    }
                }

                _items.Add(item);
                _money -= item._Price;
                Console.WriteLine($"购买成功：{item._Name} | 花费：{item._Price}元 | 当前金钱：{_money}元");
                return true;
            }

            public bool SellItem(Item item)
            {
                if (item == null)
                {
                    Console.WriteLine("错误：物品不能为空。");
                    return false;
                }

                if (_items.Contains(item))
                {
                    _items.Remove(item);
                    _money += item._Price;
                    Console.WriteLine($"卖出成功：{item._Name} | 获得：{item._Price}元 | 当前金钱：{_money}元");
                    return true;
                }
                else
                {
                    Console.WriteLine($"背包中没有找到物品：{item._Name}");
                    return false;
                }
            }

            public bool SellItem(int index)
            {
                if (index < 0 || index >= _items.Count)
                {
                    Console.WriteLine($"无效的物品索引：{index}。有效范围：0-{_items.Count - 1}");
                    return false;
                }

                Item item = (Item)_items[index];
                return SellItem(item);
            }

            public Item FindItem(string name)
            {
                foreach (Item item in _items)
                {
                    if (item._Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                        return item;
                }
                return null;
            }

            public void ShowItems()
            {
                Console.WriteLine("\n" + new string('=', 50));
                Console.WriteLine($"背包状态 ({_items.Count}/{MAX_CAPACITY})");
                Console.WriteLine(new string('=', 50));
                
                if (_items.Count == 0)
                {
                    Console.WriteLine("背包为空。");
                }
                else
                {
                    for (int i = 0; i < _items.Count; i++)
                    {
                        Console.WriteLine($"{i + 1,2}. {_items[i]}");
                    }
                }
                Console.WriteLine(new string('=', 50));
            }

            public void ShowMoney()
            {
                Console.WriteLine($"当前金钱：{_money}元");
            }

            public void ShowStatus()
            {
                Console.WriteLine($"\n背包状态：{_items.Count}/{MAX_CAPACITY} | 金钱：{_money}元\n");

            }
        }
    }
}

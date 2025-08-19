using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 反射作业
{
    public struct position
    {
        public int x;
        public int y;
    }
    public class Player
    {
        [MYCustom()]
        public string Name { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public position Position { get; set; }

        public Player()
        {
        }

        public override string ToString()
        {
            return $"名称:{Name}，血量:{Health}，攻击力:{Attack}，防御力:{Defense}，位置:({Position.x}, {Position.y})";
        }

    }

    // 这个特性可以贴在 属性 或 字段 上
    public sealed class MYCustomAttribute : Attribute
    {
    }
}

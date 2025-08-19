using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 贪吃蛇.Main.Object
{
    internal class Wall : GameObject
    {
        public Wall(Position position) : base(position)
        {
        }

        public override void Draw()
        {
            // 在这里实现墙壁的绘制逻辑
            Console.SetCursorPosition(Position.X, Position.Y);
            Console.ForegroundColor = ConsoleColor.White; // 设置墙壁颜色
            Console.Write("▓"); // 假设用"▓"表示墙壁
        }
    }
}

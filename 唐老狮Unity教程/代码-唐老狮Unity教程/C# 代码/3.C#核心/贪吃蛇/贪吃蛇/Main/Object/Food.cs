using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 贪吃蛇.Main.Object
{
    internal class Food : GameObject
    {
        public Food(Snake snake) // 传递一个初始位置
        {
            GenRandomPosition(snake);
        }
        public override void Draw()
        {
            // 在这里实现食物的绘制逻辑
            Console.SetCursorPosition(Position.X, Position.Y);
            Console.ForegroundColor = ConsoleColor.Green; // 设置食物颜色
            Console.Write("★"); // 假设用"★"表示食物
        }

        // 随机生成食物位置，不能和蛇身或墙壁重叠
        public void GenRandomPosition(Snake snake)
        {
            Random random = new Random();
            int x = random.Next(2, Game.GlobalWidth / 2 - 1) * 2; // 避免边界
            int y = random.Next(1, Game.GlobalHeight - 3); // 避免上下边界
            Position = new Position(x, y);
            // 确保新位置不与蛇身重叠
            if (snake.CheckSamePosition(Position))
            {
                GenRandomPosition(snake); // 递归调用直到找到合法位置
            }
        }
    }
}

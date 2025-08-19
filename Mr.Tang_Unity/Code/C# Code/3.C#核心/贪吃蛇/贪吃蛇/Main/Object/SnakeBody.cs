using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 贪吃蛇.Main.Object
{
    enum E_SnakeBodyType
    {
        Head, // 蛇头
        Body, // 蛇身
    }
    internal class SnakeBody : GameObject
    {
        public SnakeBody(E_SnakeBodyType snakeBodyType, Position position) : base(position)
        {
            _snakeBodyType = snakeBodyType;
        }

        public override void Draw()
        {
            // 在这里实现蛇身的绘制逻辑
            Console.SetCursorPosition(Position.X, Position.Y);
            switch (_snakeBodyType)
            {
                case E_SnakeBodyType.Head:
                    Console.ForegroundColor = ConsoleColor.Yellow; // 设置蛇头颜色为黄色
                    Console.Write("●"); // 假设用"●"表示蛇头
                    break;
                case E_SnakeBodyType.Body:
                    Console.ForegroundColor = ConsoleColor.Yellow; // 设置蛇身颜色为绿色
                    Console.Write("◎"); // 假设用"◎"表示蛇身
                    break;
            }
        }

        private E_SnakeBodyType _snakeBodyType;
    }
}

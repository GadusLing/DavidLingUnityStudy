using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 贪吃蛇.Main.Object
{
    enum E_MoveDirection// 蛇的移动方向
    {
        Up, // 向上移动
        Down, // 向下移动
        Left, // 向左移动
        Right // 向右移动
    }

    internal class Snake : IDraw
    {
        SnakeBody[] bodies;
        int nowLength; // 当前蛇的长度
        E_MoveDirection dir; // 蛇的移动方向

        public Snake(Position position)
        {
            bodies = new SnakeBody[200];
            bodies[0] = new SnakeBody(E_SnakeBodyType.Head, position); // 蛇头
            nowLength = 1; // 初始长度为3

            dir = E_MoveDirection.Right; // 初始移动方向为向右
        }

        public void Draw()
        {
            // 绘制蛇的每个部分
            for (int i = 0; i < nowLength; i++)
            {
                bodies[i].Draw();
            }
        }

        public void Move()
        {
            // 擦除上一次移动的最后一个位置
            if (nowLength > 0)
            {
                Console.SetCursorPosition(bodies[nowLength - 1].Position.X, bodies[nowLength - 1].Position.Y);
                Console.Write("  "); // 擦除蛇身最后一个位置
            }

            // 在蛇头移动之前，从蛇尾开始，让最后一个部分移动到前一个部分的位置
            for (int i = nowLength - 1; i > 0; i--)
            {
                bodies[i].Position = bodies[i - 1].Position; // 将蛇身的每个部分移动到前一个部分的位置
            }

            // 获取蛇头当前位置
            Position newPosition = bodies[0].Position;
            switch (dir)
            {
                case E_MoveDirection.Up:
                    newPosition.Y--;
                    break;
                case E_MoveDirection.Down:
                    newPosition.Y++;
                    break;
                case E_MoveDirection.Left:
                    newPosition.X -= 2;
                    break;
                case E_MoveDirection.Right:
                    newPosition.X += 2;
                    break;
            }
            bodies[0].Position = newPosition;
        }

        public void ChangeDirection(E_MoveDirection newDir)
        {
            // 防止蛇头向相反方向移动
            if (newDir == dir || nowLength > 1 &&
               ((dir == E_MoveDirection.Up && newDir == E_MoveDirection.Down) ||
                (dir == E_MoveDirection.Down && newDir == E_MoveDirection.Up) ||
                (dir == E_MoveDirection.Left && newDir == E_MoveDirection.Right) ||
                (dir == E_MoveDirection.Right && newDir == E_MoveDirection.Left)))
            {
                return; // 如果新方向是相反的，直接返回
            }
            dir = newDir; // 更新移动方向
        }

        public bool CheckEnd(Map map)
        {
            // 检查蛇头是否碰到边界
            for (int i = 0; i < map._Walls.Length; i++)
            {
                if (bodies[0].Position == map._Walls[i].Position)
                {
                    return true; // 碰到边界，游戏结束
                }
            }
            // 检查蛇头是否与蛇身相撞
            for (int i = 1; i < nowLength; i++)
            {
                if (bodies[0].Position == bodies[i].Position)
                {
                    return true; // 碰到自己，游戏结束
                }
            }
            return false; // 游戏继续
        }

        public bool CheckSamePosition(Position position)
        {
            for (int i = 0; i < nowLength; i++)
            {
                if (bodies[i].Position == position)
                {
                    return true; // 碰到蛇身，返回true
                }
            }
            return false; // 没有碰到蛇身，返回false
        }

        public void CheckEatFood(Food food)
        {
            // 检查蛇头是否吃到食物
            if (bodies[0].Position == food.Position)
            {
                // 增加蛇的长度
                //bodies[nowLength] = new SnakeBody(E_SnakeBodyType.Body, bodies[nowLength - 1].Position);
                //nowLength++;
                // 重新生成食物位置
                food.GenRandomPosition(this);
                // 长身体
                Grow();
            }
        }

        private void Grow()
        {
            // 增加蛇的长度
            if (nowLength < bodies.Length)
            {
                bodies[nowLength] = new SnakeBody(E_SnakeBodyType.Body, bodies[nowLength - 1].Position);
                nowLength++;
            }
        }
    }
}

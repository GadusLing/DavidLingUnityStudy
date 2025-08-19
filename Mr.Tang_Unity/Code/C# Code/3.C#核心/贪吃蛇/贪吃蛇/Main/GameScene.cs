using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 贪吃蛇.Main.Object;
using 贪吃蛇.Main.UI;

namespace 贪吃蛇.Main
{
    internal class GameScene : ISceneUpdate
    {
        Map map; // 地图对象
        Snake snake; // 蛇对象
        Food food; // 食物对象


        public GameScene()
        {
            map = new Map();
            snake = new Snake(new Position(20, 5)); // 初始化蛇对象
            food = new Food(snake); // 初始化食物对象
        }

        public void Update()
        {

            if (Console.KeyAvailable)// 检查是否有键盘输入（非阻塞）
            {
                // 检测玩家的输入改变方向
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        snake.ChangeDirection(E_MoveDirection.Up);
                        break;
                    case ConsoleKey.DownArrow:
                        snake.ChangeDirection(E_MoveDirection.Down);
                        break;
                    case ConsoleKey.LeftArrow:
                        snake.ChangeDirection(E_MoveDirection.Left);
                        break;
                    case ConsoleKey.RightArrow:
                        snake.ChangeDirection(E_MoveDirection.Right);
                        break;
                    case ConsoleKey.Escape: // 按下ESC键返回主菜单
                        Game.ChangeScene(E_SceneType.Start); // 切换到开始场景
                        break;
                }

                // 清空输入缓冲区，防止按键积累
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }
            }
            map.Draw(); // 绘制地图
            food.Draw(); // 绘制食物
            snake.Move(); // 移动蛇
            snake.Draw(); // 绘制蛇
            if (snake.CheckEnd(map))
            {
                Game.ChangeScene(E_SceneType.End); // 切换到结束场景
            }
            snake.CheckEatFood(food); // 检查是否吃到食物

            //降速适配游玩节奏
            System.Threading.Thread.Sleep(100); // 控制游戏速度

        }

    }
}

using 俄罗斯方块小游戏.Input;
using 俄罗斯方块小游戏.Map;
using 俄罗斯方块小游戏.Objects;

namespace 俄罗斯方块.Main
{
    internal class GameScene : ISceneUpdate
    {
        GameMap map;
        BlockWorker blockWorker;
        //Thread inputThread;
        //bool isRunning = true; // 控制线程的运行状态

        public GameScene()
        {
            map = new GameMap(this);
            blockWorker = new BlockWorker();

            InputThread.Instance.inputEvent += InputHandler; // 订阅输入事件

            //inputThread = new Thread(InputHandler);
            //inputThread.IsBackground = true; // 设置为后台线程,生命周期与主线程一致
            //inputThread.Start(); // 启动输入处理线程
        }

        private void InputHandler()
        {
            //while (isRunning)
            //{
                // 处理用户输入
                if (Console.KeyAvailable)
                {
                    //为了避免影响主线程，在输入后加锁
                    lock (blockWorker)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.A:
                                if (blockWorker.CanChange(E_Change_Type.Left, map))
                                    blockWorker.ChangeBlockShape(E_Change_Type.Left);
                                break;
                            case ConsoleKey.D:
                                if (blockWorker.CanChange(E_Change_Type.Right, map))
                                    blockWorker.ChangeBlockShape(E_Change_Type.Right);
                                break;
                            case ConsoleKey.UpArrow:
                                if (blockWorker.CanChange(E_Change_Type.Right, map))
                                    blockWorker.ChangeBlockShape(E_Change_Type.Right);
                            break;

                        // 左右方向键控制移动
                        case ConsoleKey.LeftArrow:
                                if (blockWorker.CanMove(E_Change_Type.Left, map))
                                    blockWorker.MoveRL(E_Change_Type.Left, map);
                                break;
                            case ConsoleKey.RightArrow:
                                if (blockWorker.CanMove(E_Change_Type.Right, map))
                                    blockWorker.MoveRL(E_Change_Type.Right, map);
                                break;
                            case ConsoleKey.DownArrow:
                                // 向下移动
                                if (blockWorker.CanMoveDown(map))
                                    blockWorker.MoveDown(map);
                                break;
                        }
                    }
                }
            //}
        }

        public void StopInputThread()
        {
            //isRunning = false; // 停止线程运行
            //if (inputThread.IsAlive)
            //{
            //    inputThread.Join(); // 等待线程结束
            //}
            //inputThread = null; // 清理线程引用
            InputThread.Instance.inputEvent -= InputHandler; // 取消订阅输入事件
        }

        public void Update()
        {
            // 锁内部不要包含休眠 
            lock (blockWorker)
            {
                map.Draw();
                blockWorker.Draw();

                // 检查当前方块是否可以下落,能就正常下落，不能就添加到地图中，然后生成新的方块
                if (blockWorker.CanMoveDown(map))
                    blockWorker.MoveDown(map);
            }
            // 用线程休眠 控制下落速度
            Thread.Sleep(200);
        }
    }
}

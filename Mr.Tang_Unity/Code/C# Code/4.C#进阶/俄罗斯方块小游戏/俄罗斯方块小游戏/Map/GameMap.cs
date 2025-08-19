using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 俄罗斯方块.Main;
using 俄罗斯方块小游戏.Objects;

namespace 俄罗斯方块小游戏.Map
{
    internal class GameMap : IDraw
    {
        //固定墙壁，三堵，没有天花板
        private List<DrawObject> walls = new List<DrawObject>();
        //动态墙壁
        public List<DrawObject> dynamicWalls = new List<DrawObject>();
        private GameScene nowGameScene; // 当前游戏场景

        //初始化固定墙壁

        // 由于外部快速获取地图宽高
        public int w;// 动态墙壁的宽容量，一行有多少个方块
        public int h;
        private int[] recordInfo; // 用于记录每一行的砖块数量

        public GameMap(GameScene scene)
        {
            this.nowGameScene = scene;
            h = Game.GlobalHeight - 6; // 留出6行给信息打印
            recordInfo = new int[h];
            w = 0;
            for (int i = 0; i < Game.GlobalWidth; i+=2)
            {
                walls.Add(new DrawObject(E_DrawType.Wall, new Position(i, Game.GlobalHeight - 6)));
                // 留了一个-6的高度给之后打印信息留个题目
                ++w;
            }
            w -= 2;
            for (int i = 0; i < h; i++)
            {
                walls.Add(new DrawObject(E_DrawType.Wall, new Position(0, i)));
                walls.Add(new DrawObject(E_DrawType.Wall, new Position(Game.GlobalWidth - 2, i)));
            }
        }

        public void Draw()
        {
            // 绘制固定墙壁
            for (int i = 0; i < walls.Count; i++)
            {
                walls[i].Draw();
            }
            // 绘制动态墙壁
            for (int i = 0; i < dynamicWalls.Count; i++)
            {
                dynamicWalls[i].Draw();
            }
        }

        // 清除所有动态墙壁
        public void ClearDynamicWalls()
        {
            for (int i = 0; i < dynamicWalls.Count; i++)
            {
                dynamicWalls[i].ClearDraw();
            }
        }

        /// <summary>
        /// 提供给外部添加动态墙壁的方法
        /// </summary>
        /// <param name="walls"></param>
        public void AddWalls(List<DrawObject> walls)
        {
            for(int i = 0; i < walls.Count; i++)
            {
                // 传递方块进来时，把类型变成墙壁类型
                walls[i].ChangeType(E_DrawType.Wall);
                dynamicWalls.Add(walls[i]);

                // 如果墙壁的Y坐标小于0，则不添加，因为它已经超出屏幕范围了，游戏结束了
                if (walls[i].pos.Y <= 0)
                {
                    // 关闭输入线程
                    nowGameScene.StopInputThread();

                    // 场景切换到结束界面
                    Game.ChangeScene(E_SceneType.End);
                    return;
                }

                // 添加动态墙壁的计数
                recordInfo[h - 1 - walls[i].pos.Y] += 1;
                
            }

            ClearDynamicWalls();
            CheckLineFull(); // 检查是否有行满了
            Draw();
        }

        // 当某一个砖块变成了动态墙，检测该砖块对应的每一行是否已横向变满，满了则清除这一行砖块
        public void CheckLineFull()
        {
            List<DrawObject> toRemove = new List<DrawObject>();// 用于存储需要移除的动态墙壁
            // 我不能一边遍历动态墙壁一边移除，因为可能会导致索引错误，所以先用一个表记录下来，等所有动态墙壁都遍历完后再移除
            for (int i = 0; i < recordInfo.Length; i++)
            {
                // 这个w在之前已经减去了两边多余的两个墙壁方块 
                if(recordInfo[i] == w)
                {
                    for(int j = 0; j < dynamicWalls.Count; j++)
                    {
                        // 清除这一行的所有动态墙壁
                        if (i == h - 1 - dynamicWalls[j].pos.Y)
                        {
                            toRemove.Add(dynamicWalls[j]);
                        }
                             
                        // 如果当前位置是需要移除的行之上，那就下移一行
                        else if (i < h - 1 - dynamicWalls[j].pos.Y)
                        {
                            dynamicWalls[j].pos.Y++;
                        }
                    }
                    // 移除待删除的动态墙壁
                    for (int j = 0; j < toRemove.Count; j++)
                    {
                        dynamicWalls.Remove(toRemove[j]);
                    }

                    // 迁移砖块信息，最底层的被清除后，其他砖块的信息要在dynamicWalls中往小的那边移动
                    for (int j = i; j < recordInfo.Length - 1; j++)
                    {
                        recordInfo[j] = recordInfo[j + 1];
                    }
                    // 最后一行的砖块信息清零
                    recordInfo[recordInfo.Length - 1] = 0;

                    CheckLineFull(); // 递归检查下一行是否也满了
                    break;
                }
            }
        }
    }
}

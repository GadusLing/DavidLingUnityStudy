using System.Collections.Generic;
using 俄罗斯方块.Main;
using 俄罗斯方块小游戏.Map;
namespace 俄罗斯方块小游戏.Objects
{
    enum E_Change_Type
    {
        Left,   // 逆时针变 左
        Right,  // 顺时针变 右
    }

    internal class BlockWorker : IDraw
    {
        private List<DrawObject> blocks = new List<DrawObject>();// 当前砖块的集合，包含4个小方块
        private Dictionary<E_DrawType, BlockInfo> blockInfodic;// 砖块信息字典，存储不同类型砖块的形态信息
        private BlockInfo nowBlockInfo;// 当前砖块信息
        private int nowInfoIndex = 0; // 当前形态索引
        private bool needsRedraw = false; // 添加重绘标记

        public BlockWorker()
        {
            blockInfodic = new Dictionary<E_DrawType, BlockInfo>()
            {
                { E_DrawType.Square, new BlockInfo(E_DrawType.Square) },
                { E_DrawType.Line, new BlockInfo(E_DrawType.Line) },
                { E_DrawType.Tank, new BlockInfo(E_DrawType.Tank) },
                { E_DrawType.LeftLShape, new BlockInfo(E_DrawType.LeftLShape) },
                { E_DrawType.RightLShape, new BlockInfo(E_DrawType.RightLShape) },
                { E_DrawType.LeftZShape, new BlockInfo(E_DrawType.LeftZShape) },
                { E_DrawType.RightZShape, new BlockInfo(E_DrawType.RightZShape) }
            };

            RandomAddBlock();
        }

        // 随机生成一个砖块
        public void RandomAddBlock()
        {
            Random r = new Random();
            E_DrawType type = (E_DrawType)r.Next(1, 8);// 左包含右不包含
            //E_DrawType type = E_DrawType.Line;// 测试
            // 每次创建 一个砖块，其实就是创建4个小方块
            blocks = new List<DrawObject>()
            {
                new DrawObject(type),
                new DrawObject(type),
                new DrawObject(type),
                new DrawObject(type)
            };
            blocks[0].pos = new Position(Game.GlobalWidth / 2, -4); // 中心方块的坐标
            nowBlockInfo = blockInfodic[type];
            // 随机砖块不同的形态
            nowInfoIndex = r.Next(0, nowBlockInfo.Count);
            //取出其中一种形态的坐标信息
            Position[] pos = nowBlockInfo[nowInfoIndex];
            for (int i = 0; i < pos.Length; i++)
            {
                // 设置每个小方块的坐标
                blocks[i + 1].pos = blocks[0].pos + pos[i];
            }
        }

        public void Draw()
        {
            // 绘制当前砖块
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].Draw();
            }
        }

        // 清除当前砖块的绘制
        public void ClearDraw()
        {
            // 清除当前砖块
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].ClearDraw();
            }
        }

        //改变砖块的形态，通过ad键来改变
        public void ChangeBlockShape(E_Change_Type changeType)
        {
            // 不要提前清除，先计算新位置
            int newInfoIndex = nowInfoIndex;
            switch (changeType)
            {
                case E_Change_Type.Left:
                    newInfoIndex = newInfoIndex == 0 ? nowBlockInfo.Count - 1 : newInfoIndex - 1;
                    break;
                case E_Change_Type.Right:
                    newInfoIndex = newInfoIndex == nowBlockInfo.Count - 1 ? 0 : newInfoIndex + 1;
                    break;
            }

            // 获取新的形态坐标
            Position[] pos = nowBlockInfo[newInfoIndex];

            // 批量清除
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].ClearDraw();
            }

            // 更新形态索引和位置
            nowInfoIndex = newInfoIndex;
            for (int i = 0; i < pos.Length; i++)
            {
                blocks[i + 1].pos = blocks[0].pos + pos[i];
            }

            // 批量重绘
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].Draw();
            }
        }

        /// <summary>
        /// 判断能否变形
        /// </summary>
        /// <param name="changeType">变形方向</param>
        /// <param name="map">地图类型</param>
        /// <returns></returns>
        public bool CanChange(E_Change_Type changeType, GameMap map)
        {
            // 判断当前砖块是否可以变形
            int nowIndex = nowInfoIndex;
            switch (changeType)
            {
                case E_Change_Type.Left:
                    nowIndex--;
                    if (nowIndex < 0)
                    {
                        nowIndex = nowBlockInfo.Count - 1;
                    }
                    break;
                case E_Change_Type.Right:
                    nowIndex++;
                    if (nowIndex >= nowBlockInfo.Count)
                    {
                        nowIndex = 0;
                    }
                    break;
            }

            // 通过临时索引获取新的形态坐标用于重合判断
            Position[] nowPos = nowBlockInfo[nowIndex];
            Position tempPos;
            // 判断是否和地图重合
            for (int i = 0; i < nowPos.Length; i++)
            {
                tempPos = blocks[0].pos + nowPos[i];
                // 判断是否超出地图边界
                if (tempPos.X < 2 || tempPos.X >= Game.GlobalWidth - 2 || tempPos.Y >= map.h)
                {
                    return false;
                }
            }
            // 判断是否和地图中的动态墙壁重合
            for (int i = 0; i < nowPos.Length; i++)
            {
                tempPos = blocks[0].pos + nowPos[i];
                // 如果重合了动态墙壁，则不能变形
                for (int j = 0; j < map.dynamicWalls.Count; j++)
                {
                    if (tempPos == map.dynamicWalls[j].pos)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 判断砖块是否可以移动（左右移动）复用了之前的E_Change_Type类型，
        /// 以后如果想要用↑改变形状也可以增加个枚举类型去复用，反正都是代表方向的
        /// </summary>
        /// <param name="type">移动方向</param>
        /// <param name="map">地图对象</param>
        /// <returns>如果可以移动返回true，否则返回false</returns>
        public bool CanMove(E_Change_Type type, GameMap map)
        {
            // 计算移动后的位置偏移量
            Position offsetPos = new Position(0, 0);
            switch (type)
            {
                case E_Change_Type.Left:
                    offsetPos = new Position(-2, 0); // 向左移动2个单位
                    break;
                case E_Change_Type.Right:
                    offsetPos = new Position(2, 0);  // 向右移动2个单位
                    break;
            }

            // 检查每个小方块移动后是否越界或与墙壁重合
            Position newPos;
            for (int i = 0; i < blocks.Count; i++)
            {
                // 计算移动后的位置
                newPos = blocks[i].pos + offsetPos;

                // 判断是否超出地图边界
                if (newPos.X < 2 || newPos.X >= Game.GlobalWidth - 2 || newPos.Y >= map.h)
                {
                    return false;
                } 
            }
            // 判断是否与动态墙壁重合, 这里为什么要用两个循环判断越界和动态？代码都差不多
            // 因为动态到后期数据会越来越多，可能会影响性能，如果之前就判断越界了，就没必要再去算这个大的数据块了
            for (int i = 0; i < blocks.Count; i++)
            {
                newPos = blocks[i].pos + offsetPos;
                for (int j = 0; j < map.dynamicWalls.Count; j++)
                {
                    if (newPos.X == map.dynamicWalls[j].pos.X && newPos.Y == map.dynamicWalls[j].pos.Y)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 控制砖块的左右移动
        /// </summary>
        /// <param name="type">移动方向</param>
        /// <param name="map">地图对象</param>
        public void MoveRL(E_Change_Type type, GameMap map)
        {
            if (CanMove(type, map))
            {
                ClearDraw(); // 先清除当前位置
                
                // 更新所有位置
                for (int i = 0; i < blocks.Count; i++)
                {
                    switch (type)
                    {
                        case E_Change_Type.Left:
                            blocks[i].pos.X -= 2;
                            break;
                        case E_Change_Type.Right:
                            blocks[i].pos.X += 2;
                            break;
                    }
                }
                Draw();
            }
        }

        // 控制砖块的下落
        public void MoveDown(GameMap map)
        {
            ClearDraw();
            // 批量更新位置
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].pos.Y++;
            }
            
            // 批量重绘
            Draw();  
        }

        // 检测砖块能否下落，砖块碰到地图底部时，改变砖块的类型，变成地图的一部分
        public bool CanMoveDown(GameMap map)
        {
            // 检查每个小方块是否可以下落
            for (int i = 0; i < blocks.Count; i++)
            {
                Position newPos = new Position(blocks[i].pos.X, blocks[i].pos.Y + 1);
                // 判断是否超出地图边界
                if (newPos.Y >= map.h)
                {
                    // 如果有一个小方块不能下落，则改变砖块类型
                    map.AddWalls(blocks);
                    RandomAddBlock();
                    return false;
                }
                // 判断是否与动态墙壁重合，重合则不能下落，改变砖块类型
                for (int j = 0; j < map.dynamicWalls.Count; j++)
                {
                    if (newPos.X == map.dynamicWalls[j].pos.X && newPos.Y == map.dynamicWalls[j].pos.Y)
                    {
                        map.AddWalls(blocks);
                        RandomAddBlock();
                        return false;
                    }
                }
            }
            return true;
        }
    }
}

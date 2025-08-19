using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 俄罗斯方块小游戏.Objects
{
    internal class BlockInfo
    {
        // 方块信息坐标的容器
        private List<Position[]> list;
        public int Count => list.Count;// 提供给外部，获取形态有几种

        public BlockInfo(E_DrawType type)
        {
            list = new List<Position[]>();
            switch (type)
            {
                case E_DrawType.Square:
                    list.Add(new Position[3] {new Position(2, 0), new Position(0, 1), new Position(2, 1) });
                    break;
                case E_DrawType.Line:
                    list.Add(new Position[3] { new Position(0, -1), new Position(0, 1), new Position(0, 2) });
                    list.Add(new Position[3] { new Position(-4, 0), new Position(-2, 0), new Position(2, 0) });
                    list.Add(new Position[3] { new Position(0, -2), new Position(0, -1), new Position(0, 1) });
                    list.Add(new Position[3] { new Position(-2, 0), new Position(2, 0), new Position(4, 0) });
                    break;
                case E_DrawType.Tank:
                    list.Add(new Position[3] { new Position(-2, 0), new Position(2, 0), new Position(0, 1) });
                    list.Add(new Position[3] { new Position(-2, 0), new Position(0, -1), new Position(0, 1) });
                    list.Add(new Position[3] { new Position(-2, 0), new Position(2, 0), new Position(0, -1) });
                    list.Add(new Position[3] { new Position(0, -1), new Position(0, 1), new Position(2, 0) });
                    break;
                case E_DrawType.LeftLShape:
                    list.Add(new Position[3] { new Position(2, -1), new Position(0, -1), new Position(0, 1) });
                    list.Add(new Position[3] { new Position(-2, 0), new Position(2, 1), new Position(2, 0) });
                    list.Add(new Position[3] { new Position(0, -1), new Position(0, 1), new Position(-2, 1) });
                    list.Add(new Position[3] { new Position(-2, 0), new Position(2, 0), new Position(-2, -1) });
                    break;
                case E_DrawType.RightLShape:
                    list.Add(new Position[3] { new Position(-2, -1), new Position(0, -1), new Position(0, 1) });
                    list.Add(new Position[3] { new Position(-2, 0), new Position(2, -1), new Position(2, 0) });
                    list.Add(new Position[3] { new Position(0, -1), new Position(0, 1), new Position(2, 1) });
                    list.Add(new Position[3] { new Position(-2, 0), new Position(2, 0), new Position(-2, 1) });
                    break;
                case E_DrawType.LeftZShape:
                    list.Add(new Position[3] { new Position(0, -1), new Position(2, 0), new Position(2, 1) });
                    list.Add(new Position[3] { new Position(-2, 1), new Position(0, 1), new Position(2, 0) });
                    list.Add(new Position[3] { new Position(0, 1), new Position(-2, 0), new Position(-2, -1) });
                    list.Add(new Position[3] { new Position(-2, 0), new Position(0, -1), new Position(2, -1) });
                    break;
                case E_DrawType.RightZShape:
                    list.Add(new Position[3] { new Position(0, -1), new Position(-2, 0), new Position(-2, 1) });
                    list.Add(new Position[3] { new Position(-2, -1), new Position(0, -1), new Position(2, 0) });
                    list.Add(new Position[3] { new Position(0, 1), new Position(2, 0), new Position(2, -1) });
                    list.Add(new Position[3] { new Position(-2, 0), new Position(0, 1), new Position(2, 1) });
                    break;
                default:
                    throw new ArgumentException("Unknown block type");
            }
        }

        /// <summary>
        /// 提供给外部根据索引获取位置偏移信息
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Position[] this[int index]
        {
            get
            {
                if(index < 0) return list[0];
                else if (index >= list.Count) return list[list.Count - 1];
                else return list[index]; 
            }
        }
    }
}

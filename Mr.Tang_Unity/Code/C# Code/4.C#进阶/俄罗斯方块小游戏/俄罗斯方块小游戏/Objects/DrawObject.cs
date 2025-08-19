using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 俄罗斯方块小游戏.Objects
{
    /// <summary>
    /// 绘制方块的类型，有不同的颜色和形状，包括墙壁、方形、长条形、坦克形、左右L形、左右z形等。
    /// </summary>
    enum E_DrawType
    { 
        Wall,     // 墙壁
        Square,    // 方形
        Line,      // 长条形
        Tank,      // 坦克形
        LeftLShape,    // 左L形
        RightLShape,// 右L形
        LeftZShape,// 左Z形
        RightZShape// 右Z形
    }

    internal class DrawObject : IDraw
    {
        public Position pos;
        public E_DrawType type;

        public DrawObject(E_DrawType drawType)
        {
            type = drawType;
        }

        public DrawObject(E_DrawType drawType, Position position) : this(drawType)
        {
            pos = position;
        }

        public void Draw()
        {
            if(pos.Y < 0)
            {
                return; // 如果位置超出屏幕范围，则不绘制
            }

            Console.SetCursorPosition(pos.X, pos.Y);
            switch (type)
            {
                case E_DrawType.Wall:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case E_DrawType.Square:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case E_DrawType.Line:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case E_DrawType.Tank:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case E_DrawType.LeftLShape:
                case E_DrawType.RightLShape:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case E_DrawType.LeftZShape:
                case E_DrawType.RightZShape:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            Console.Write("■");
        }

        public void ClearDraw()
        {
            if (pos.Y < 0)
            {
                return; // 如果位置超出屏幕范围，则不绘制
            }
            Console.SetCursorPosition(pos.X, pos.Y);
            Console.Write("  "); // 清除方块的绘制
        }

        /// <summary>
        /// 切换方块类型，主要用于方块下落到地图最底端时，改变方块的类型，变成地图的一部分。
        /// </summary>
        /// <param name="newType"></param>
        public void ChangeType(E_DrawType newType)
        {
            type = newType;
        }

    }
}

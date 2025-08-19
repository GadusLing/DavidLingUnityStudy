using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 贪吃蛇.Main.Object
{
    internal class Map : IDraw
    {
        public Map()
        {
            _Walls = new Wall[Game.GlobalWidth + (Game.GlobalHeight - 3) * 2];
            int index = 0;
            // 上下边界
            for(int i = 0; i < Game.GlobalWidth; i+=2)
            {
                _Walls[index++] = new Wall(new Position(i, 0)); // 上边界
                _Walls[index++] = new Wall(new Position(i, Game.GlobalHeight - 2)); // 下边界
            }
            // 左右边界（排除角落，避免重复）
            for (int i = 1; i < Game.GlobalHeight - 2; i++)
            {
                _Walls[index++] = new Wall(new Position(0, i)); // 左边界
                _Walls[index++] = new Wall(new Position(Game.GlobalWidth - 2, i)); // 右边界
            }
        }

        public void Draw()
        {
            // 绘制地图边界
            foreach (var wall in _Walls)
            {
                wall.Draw();
            }
        }

        public Wall[] _Walls;
    }
}

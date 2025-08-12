using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 贪吃蛇.Main.Object
{
    abstract class GameObject : IDraw
    {
        public Position Position { get; set; }
        public GameObject()
        {
        }
        public GameObject(Position position)
        {
            Position = position;
        }
        public abstract void Draw();
    }
}

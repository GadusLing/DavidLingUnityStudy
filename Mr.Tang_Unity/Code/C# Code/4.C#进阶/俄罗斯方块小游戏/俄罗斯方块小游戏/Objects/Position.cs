using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 俄罗斯方块小游戏.Objects
{
    internal class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Position() : this(0, 0) { }
        public override string ToString()
        {
            return $"({X}, {Y})";
        }
        public static Position operator +(Position a, Position b)
        {
            return new Position(a.X + b.X, a.Y + b.Y);
        }
        public static Position operator -(Position a, Position b)
        {
            return new Position(a.X - b.X, a.Y - b.Y);
        }
        public static Position operator *(Position a, int scalar)
        {
            return new Position(a.X * scalar, a.Y * scalar);
        }
        public static Position operator /(Position a, int scalar)
        {
            if (scalar == 0) throw new DivideByZeroException("Cannot divide by zero.");
            return new Position(a.X / scalar, a.Y / scalar);
        }
        public static bool operator ==(Position a, Position b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.X == b.X && a.Y == b.Y;
        }
        public static bool operator !=(Position a, Position b)
        {
            return !(a == b);
        }
    }
}

using System;
using MessagePack;

namespace Altseed2
{
    [MessagePackObject]
    public struct Vector2F
    {
        [Key(0)]
        public readonly float X;

        [Key(1)]
        public readonly float Y;

        public Vector2F(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2F operator+ (Vector2F a, Vector2F b)
        {
            return new Vector2F(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2F operator* (Vector2F a, Vector2F b)
        {
            return new Vector2F(a.X * b.X, a.Y * b.Y);
        }
        public static Vector2F operator* (Vector2F a, float b)
        {
            return new Vector2F(a.X * b, a.Y * b);
        }
        public static Vector2F operator* (float a, Vector2F b)
        {
            return new Vector2F(a * b.X, a * b.Y);
        }
    }

    [MessagePackObject]
    public struct Vector2I
    {
        [Key(0)]
        public readonly int X;

        [Key(1)]
        public readonly int Y;

        public Vector2I(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector2I operator +(Vector2I a, Vector2I b)
        {
            return new Vector2I(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2I operator *(Vector2I a, Vector2I b)
        {
            return new Vector2I(a.X * b.X, a.Y * b.Y);
        }
        public static Vector2I operator *(Vector2I a, int b)
        {
            return new Vector2I(a.X * b, a.Y * b);
        }
        public static Vector2I operator *(int a, Vector2I b)
        {
            return new Vector2I(a * b.X, a * b.Y);
        }
    }
}

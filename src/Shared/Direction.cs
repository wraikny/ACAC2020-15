using Altseed2;

namespace ACAC2020_15.Shared
{
    public enum Direction : byte
    {
        Right,
        Left,
        Up,
        Down,
    }

    static class DirectionExtension
    {
        public static Vector2I To2I(this Direction dir)
        {
            return dir switch
            {
                Direction.Right => new Vector2I(1, 0),
                Direction.Left => new Vector2I(-1, 0),
                Direction.Up => new Vector2I(0, -1),
                Direction.Down => new Vector2I(0, 1),
                _ => new Vector2I(0, 0),
            };
        }

        public static float ToDegree(this Direction dir)
        {
            return dir switch
            {
                Direction.Right => 0f,
                Direction.Left => 180f,
                Direction.Up => 270f,
                Direction.Down => 90f,
                _ => 0f,
            };
        }
    }
}

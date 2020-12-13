using System.Collections.Generic;
using Altseed2;

namespace ACAC2020_15.Shared
{
    static class GameUtils
    {
        // 25 * (32, 24) = (800, 600)
        public static Vector2F CellSize = new Vector2F(25f, 25f);
        public static Vector2I AreaSize = new Vector2I(32, 24);

        public static Vector2I InitPosition = AreaSize / 2;
        public static Direction InitDirection = Direction.Up;

        public static bool ValidatePosition(Vector2I position)
        {
            return 0 <= position.X && position.X < AreaSize.X && 0 <= position.Y && position.Y < AreaSize.Y;
        }

        public static readonly IReadOnlyList<Vector2I> AllCells;

        static GameUtils()
        {
            var cells = new List<Vector2I>(AreaSize.X * AreaSize.Y);

            for (int y = 0; y < AreaSize.Y; y++)
            {
                for (int x = 0; x < AreaSize.X; x++)
                {
                    cells.Add(new Vector2I(x, y));
                }
            }

            AllCells = cells;

        }
    }
}

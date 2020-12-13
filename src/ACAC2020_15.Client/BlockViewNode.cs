using System;
using System.Collections.Generic;
using System.Text;
using Altseed2;
using ACAC2020_15.Shared;

namespace ACAC2020_15.Client
{
    class BlockViewNode : RectangleNode
    {
        public readonly Vector2I CellPosition;

        public BlockViewNode(Vector2I position)
        {
            CellPosition = position;

            var size = GameUtils.CellSize * 0.8f;

            RectangleSize = size;
            CenterPosition = size * 0.5f;

            Position = position.To2F() * GameUtils.CellSize + GameUtils.CellSize * 0.5f;

            Color = new Color(200, 80, 80);
        }
    }
}

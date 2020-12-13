using System;
using System.Collections.Generic;
using System.Text;
using Altseed2;
using ACAC2020_15.Shared;

namespace ACAC2020_15.Client
{
    class BackgroundNode : Node
    {
        public BackgroundNode()
        {
            var count = GameUtils.AreaSize;

            var size = GameUtils.CellSize * 0.9f;

            foreach (var v in GameUtils.AllCells)
            {
                var node = new RectangleNode
                {
                    Position = new Vector2F(v.X + 0.5f, v.Y + 0.5f) * GameUtils.CellSize,
                    CenterPosition = size * 0.5f,
                    RectangleSize = size,
                    ZOrder = ZOrders.Background,
                    Color = new Color(20, 20, 20),
                };

                AddChildNode(node);
            }
        }
    }
}

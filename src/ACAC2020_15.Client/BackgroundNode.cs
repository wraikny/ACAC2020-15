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

            for (int y = 0; y < count.Y; y++)
            {
                for (int x = 0; x < count.X; x++)
                {
                    var node = new RectangleNode
                    {
                        Position = new Vector2F(x + 0.5f, y + 0.5f) * GameUtils.CellSize,
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
}

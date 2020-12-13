using System;
using System.Collections.Generic;
using System.Text;
using Altseed2;
using ACAC2020_15.Shared;

namespace ACAC2020_15.Client
{
    class PlayerViewNode : TriangleNode
    {
        public PlayerViewNode()
        {
            var size = GameUtils.CellSize * 0.8f;
            Point1 = new Vector2F(0.5f, 0.0f) * size;
            Point2 = new Vector2F(-0.5f, 0.5f) * size;
            Point3 = new Vector2F(-0.5f, -0.5f) * size;

            ZOrder = ZOrders.Player;
        }

        public void Update(Direction direction, Vector2I position)
        {
            Angle = direction.ToDegree();
            Position = position.To2F() * GameUtils.CellSize + GameUtils.CellSize * 0.5f;
        }

        public void Update(GamePlayer player)
        {
            Update(player.Direction, player.Position);
        }
    }
}

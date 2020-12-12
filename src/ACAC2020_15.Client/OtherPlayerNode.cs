using System;
using System.Collections.Generic;
using System.Text;
using Altseed2;

using ACAC2020_15.Shared;

namespace ACAC2020_15.Client
{
    class OtherPlayerNode : TransformNode
    {
        private readonly PlayerViewNode viewNode;

        public OtherPlayerNode()
        {
            viewNode = new PlayerViewNode();
            AddChildNode(viewNode);
        }

        public void Update(GamePlayer player)
        {
            viewNode.Update(player.Direction, player.Position);
        }
    }
}

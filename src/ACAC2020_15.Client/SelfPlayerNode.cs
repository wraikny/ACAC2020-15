using System;
using System.Collections.Generic;
using System.Text;
using Altseed2;

using ACAC2020_15.Shared;

namespace ACAC2020_15.Client
{
    sealed class SelfPlayerNode : Node
    {
        private readonly PlayerInputNode inputNode;
        private readonly PlayerViewNode viewNode;

        public event Action<IPlayerAction> OnPlayerInput
        {
            add => inputNode.OnPlayerInput += value;
            remove => inputNode.OnPlayerInput -= value;
        }

        public SelfPlayerNode()
        {
            inputNode = new PlayerInputNode();
            AddChildNode(inputNode);

            viewNode = new PlayerViewNode();
            AddChildNode(viewNode);
        }

        public void Update(GamePlayer player)
        {
            viewNode.Update(player);
        }
    }
}

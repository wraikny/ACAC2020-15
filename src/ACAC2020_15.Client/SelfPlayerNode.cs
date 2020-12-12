using System;
using System.Collections.Generic;
using System.Text;
using Altseed2;
using ACAC2020_15.Shared;

namespace ACAC2020_15.Client
{
    class SelfPlayerNode : TransformNode
    {
        private readonly GamePlayer player;

        private readonly PlayerViewNode viewNode;

        public event Action<Direction> OnMove;

        public SelfPlayerNode()
        {
            viewNode = new PlayerViewNode();
            AddChildNode(viewNode);

            player = GamePlayer.CreateDefault();

            viewNode.Update(player.Direction, player.Position);
        }

        static bool IsPush(Key key)
        {
            return Engine.Keyboard.GetKeyState(key) == ButtonState.Push;
        }

        protected override void OnUpdate()
        {
            if (IsPush(Key.W))
            {
                Move(Direction.Up);
            }
            else if (IsPush(Key.S))
            {
                Move(Direction.Down);
            }
            else if (IsPush(Key.D))
            {
                Move(Direction.Right);
            }
            else if (IsPush(Key.A))
            {
                Move(Direction.Left);
            }
        }

        public void Move(Direction direction)
        {
            player.Move(direction);
            OnMove?.Invoke(direction);
            viewNode.Update(player.Direction, player.Position);
        }
    }
}

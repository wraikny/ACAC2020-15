using System;
using System.Collections.Generic;
using System.Text;
using Altseed2;
using ACAC2020_15.Shared;

namespace ACAC2020_15.Client
{
    class PlayerInputNode : Node
    {
        public event Action<IPlayerAction> OnPlayerInput;

        public PlayerInputNode()
        {
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
            else if (IsPush(Key.Z))
            {
                OnPlayerInput?.Invoke(IPlayerAction.CreateBlock.Instance);
            }
            else if (IsPush(Key.X))
            {
                OnPlayerInput?.Invoke(IPlayerAction.BreakBlock.Instance);
            }
        }

        public void Move(Direction direction)
        {
            OnPlayerInput?.Invoke(new IPlayerAction.Move(direction));
        }
    }
}

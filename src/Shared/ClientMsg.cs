using System.Collections.Generic;
using MessagePack;

namespace ACAC2020_15.Shared
{
    [MessagePack.Union(0, typeof(PlayerAction))]
    public interface IClientMsg
    {
        [MessagePackObject]
        public sealed class PlayerAction : IClientMsg
        {
            [Key(0)]
            public IPlayerAction Value { get; private set; }

            [SerializationConstructor]
            public PlayerAction(IPlayerAction value)
            {
                Value = value;
            }
        }
    }
}

using System.Collections.Generic;
using MessagePack;

namespace ACAC2020_15.Shared
{
    [MessagePack.Union(0, typeof(Move))]
    public interface IClientMsg
    {
        [MessagePackObject]
        public sealed class Move : IClientMsg
        {
            [Key(0)]
            public Direction Direction { get; set; }

            public Move(Direction direction)
            {
                Direction = direction;
            }
        }
    }
}

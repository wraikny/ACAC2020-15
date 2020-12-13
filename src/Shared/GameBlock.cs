using MessagePack;
using Altseed2;

namespace ACAC2020_15.Shared
{
    [MessagePackObject]
    public sealed class GameBlock
    {
        [Key(0)]
        public Vector2I Position { get; private set; }

        [SerializationConstructor]
        public GameBlock(Vector2I position)
        {
            Position = position;
        }
    }
}

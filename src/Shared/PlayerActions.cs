using MessagePack;

namespace ACAC2020_15.Shared
{
    [MessagePack.Union(0, typeof(Move))]
    [MessagePack.Union(1, typeof(CreateBlock))]
    [MessagePack.Union(2, typeof(BreakBlock))]
    public interface IPlayerAction
    {
        [MessagePackObject]
        public sealed class Move : IPlayerAction
        {
            [Key(0)]
            public Direction Direction { get; set; }

            [SerializationConstructor]
            public Move(Direction direction)
            {
                Direction = direction;
            }
        }

        [MessagePackObject]
        public sealed class CreateBlock : IPlayerAction
        {
            public static CreateBlock Instance;

            static CreateBlock()
            {
                Instance = new CreateBlock();
            }

            public CreateBlock()
            {

            }
        }

        [MessagePackObject]
        public sealed class BreakBlock : IPlayerAction
        {
            public static BreakBlock Instance;

            static BreakBlock()
            {
                Instance = new BreakBlock();
            }

            public BreakBlock() { }
        }
    }
}
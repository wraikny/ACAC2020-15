using MessagePack;
using Altseed2;

namespace ACAC2020_15.Shared
{
    [MessagePackObject]
    public sealed class GamePlayer
    {
        [Key(0)]
        public Vector2I Position { get; private set; }

        [Key(1)]
        public Direction Direction { get; set; }

        public static GamePlayer CreateDefault()
        {
            return new GamePlayer(GameUtils.InitPosition, GameUtils.InitDirection);
        }

        [SerializationConstructor]
        public GamePlayer(Vector2I position, Direction direction)
        {
            Position = position;
            Direction = direction;
        }

        public void StepForward()
        {
            var expectedPos = Position + Direction.To2I();

            if (GameUtils.ValidatePosition(expectedPos))
            {
                Position = expectedPos;
            }
        }

        public Vector2I? TryGetForwardCell()
        {
            var p = Position + Direction.To2I();

            if (GameUtils.ValidatePosition(p))
            {
                return p;
            }

            return null;
        }
    }
}

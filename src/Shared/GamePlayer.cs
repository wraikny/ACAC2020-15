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
        public Direction Direction { get; private set; }

        public static GamePlayer CreateDefault()
        {
            return new GamePlayer(GameUtils.InitPosition, GameUtils.InitDirection);
        }

        public GamePlayer(Vector2I position, Direction direction)
        {
            Position = position;
            Direction = direction;
        }

        public bool Move(Direction direction)
        {
            var lastPosition = Position;
            var lastDirection = Direction;

            Direction = direction;

            var expectedPos = Position + direction.To2I();

            if (GameUtils.ValidatePosition(expectedPos))
            {
                Position = expectedPos;
            }

            return !(lastPosition == Position && lastDirection == Direction);
        }
    }
}

using Altseed2;
using MessagePack;

namespace ACAC2020_15.Shared
{
    [MessagePack.Union(0, typeof(PlayerEntered))]
    [MessagePack.Union(1, typeof(PlayerExited))]
    [MessagePack.Union(2, typeof(PlayerUpdated))]
    public interface IGameEvent
    {
        [MessagePackObject]
        public sealed class PlayerEntered : IGameEvent
        {
            [Key(0)]
            public ulong Id { get; private set; }

            public PlayerEntered(ulong id)
            {
                Id = id;
            }
        }

        [MessagePackObject]
        public sealed class PlayerExited : IGameEvent
        {
            [Key(0)]
            public ulong Id { get; private set; }

            public PlayerExited(ulong id)
            {
                Id = id;
            }
        }

        [MessagePackObject]
        public sealed class PlayerUpdated : IGameEvent
        {
            [Key(0)]
            public ulong Id { get; private set; }

            [Key(1)]
            public GamePlayer Player { get; private set; }

            public PlayerUpdated(ulong id, GamePlayer player)
            {
                Id = id;
                Player = player;
            }
        }
    }
}

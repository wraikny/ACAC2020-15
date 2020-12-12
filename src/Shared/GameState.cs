using System.Collections.Generic;
using MessagePack;

namespace ACAC2020_15.Shared
{
    [MessagePackObject]
    public sealed class GameState
    {
        [IgnoreMember]
        public bool IsUpdated { get; private set; } = false;

        [Key(0)]
        public Dictionary<ulong, GamePlayer> Players { get; private set; }

        public GameState()
        {
            Players = new Dictionary<ulong, GamePlayer>();
        }

        public GameState(Dictionary<ulong, GamePlayer> players)
        {
            Players = players;
        }

        public void PlayerEnter(ulong id)
        {
            IsUpdated = true;
            var player = new GamePlayer(GameUtils.InitPosition, GameUtils.InitDirection);
            Players.Add(id, player);
        }

        public void PlayerExit(ulong id)
        {
            IsUpdated = true;
            Players.Remove(id);
        }

        public void PlayerMove(ulong id, Direction direction)
        {
            var player = Players[id];
            player.Move(direction);

            IsUpdated = true;
        }

        public void Serialized()
        {
            IsUpdated = false;
        }
    }
}

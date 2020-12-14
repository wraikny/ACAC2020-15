using System.Collections.Generic;
using MessagePack;
using Altseed2;

namespace ACAC2020_15.Shared
{
    [MessagePackObject]
    public sealed class GameState
    {
        [IgnoreMember]
        private ulong nextBlockId;

        [IgnoreMember]
        private Dictionary<Vector2I, ulong> blockIdsByPositon;

        [IgnoreMember]
        public bool IsUpdated { get; private set; } = false;

        [Key(0)]
        public Dictionary<ulong, GamePlayer> Players { get; private set; }

        [Key(1)]
        public Dictionary<ulong, GameBlock> Blocks { get; private set; }

        public GameState()
        {
            Players = new Dictionary<ulong, GamePlayer>();
            Blocks = new Dictionary<ulong, GameBlock>();
            blockIdsByPositon = new Dictionary<Vector2I, ulong>();
        }

        [SerializationConstructor]
        public GameState(Dictionary<ulong, GamePlayer> players, Dictionary<ulong, GameBlock> blocks)
        {
            Players = players;
            Blocks = blocks;
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

        public void PlayerACtion(ulong id, IPlayerAction action)
        {
            var player = Players[id];

            switch(action)
            {
                case IPlayerAction.Move a:
                    Move(player, a);
                    break;
                case IPlayerAction.CreateBlock _:
                    {
                        if (player.TryGetForwardCell() is Vector2I p)
                        {
                            if (!blockIdsByPositon.ContainsKey(p))
                            {
                                blockIdsByPositon.Add(p, nextBlockId);
                                Blocks.Add(nextBlockId, new GameBlock(p));
                                nextBlockId += 1;
                                IsUpdated = true;
                            }
                        }
                    }
                    break;
                case IPlayerAction.BreakBlock _:
                    {

                        if (player.TryGetForwardCell() is Vector2I p)
                        {
                            if (blockIdsByPositon.Remove(p, out ulong blockId))
                            {
                                Blocks.Remove(blockId);
                                IsUpdated = true;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void Move(GamePlayer player, IPlayerAction.Move move)
        {
            if (player.Direction != move.Direction)
            {
                player.Direction = move.Direction;
                IsUpdated = true;
            }

            if (player.TryGetForwardCell() is Vector2I p)
            {
                if (!blockIdsByPositon.ContainsKey(p))
                {
                    player.StepForward();
                    IsUpdated = true;
                }
            }
        }

        public void Update(float deltaSecond)
        {

        }

        public void Serialized()
        {
            IsUpdated = false;
        }
    }
}

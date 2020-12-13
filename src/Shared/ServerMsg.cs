using System;
using System.Collections.Generic;
using MessagePack;

namespace ACAC2020_15.Shared
{
    [MessagePack.Union(0, typeof(ClientId))]
    [MessagePack.Union(1, typeof(SyncGameState))]
    public interface IServerMsg
    {
        [MessagePackObject]
        public sealed class ClientId : IServerMsg
        {
            [Key(0)]
            public ulong Id { get; private set; }

            [SerializationConstructor]
            public ClientId(ulong id)
            {
                Id = id;
            }
        }

        [MessagePackObject]
        public sealed class SyncGameState : IServerMsg
        {
            [Key(0)]
            public GameState GameState { get; private set; }

            [SerializationConstructor]
            public SyncGameState(GameState state)
            {
                GameState = state;
            }
        }
    }
}

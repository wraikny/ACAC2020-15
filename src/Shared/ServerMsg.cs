using System;
using MessagePack;

namespace ACAC2020_15.Shared
{
    [MessagePack.Union(0, typeof(SendClientId))]
    public interface IServerMsg
    {
        [MessagePackObject]
        public class SendClientId : IServerMsg
        {
            [Key(0)]
            public ulong Id { get; private set; }
            public SendClientId(ulong id)
            {
                Id = id;
            }
        }
    }
}
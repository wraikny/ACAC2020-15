using System;
using System.Collections.Generic;
using System.Linq;
using LiteNetLib;
using MessagePack;

using ACAC2020_15.Shared;

namespace ACAC2020_15.Server
{
    sealed class Client
    {
        private readonly AveragedLatency latency;

        public UInt64 Id { get; private set; }
        public NetPeer Peer { get; private set; }

        public float LatencySecond => latency.LatencySecond;


        public Client(ulong id, NetPeer peer)
        {
            Id = id;
            Peer = peer;
            
            latency = new AveragedLatency();
        }

        public void UpdateLatency(int latencyMs)
        {
            latency.UpdateLatency(latencyMs);
        }
    }
}

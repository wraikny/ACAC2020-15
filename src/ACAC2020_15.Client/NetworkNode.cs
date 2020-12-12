using System;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using LiteNetLib;
using MessagePack;
using Altseed2;

using ACAC2020_15.Shared;

namespace ACAC2020_15.Client
{
    sealed class NetworkNode : Node, INetEventListener
    {
        private ulong? id;
        private NetManager manager;
        private readonly AveragedLatency latency;

        public ulong? Id => id;
        public bool IsRunning => manager?.IsRunning ?? false;

        public NetPeer Peer { get; private set; }

        public event Action<GameState> OnReceiveGameState;

        public NetworkNode()
        {
            latency = new AveragedLatency();
        }

        public async Task Start()
        {
            if (manager?.IsRunning ?? false) return;

            var config = await Config.Load(@"netconfig/clientconfig.json");

            manager = new NetManager(this);
            Setting.SettingNetManagerClient(manager);

            if (!manager.Start())
            {
                throw new Exception("Failed to start the client");
            }

            // サーバーへ接続する
            manager.Connect(config.Address, config.Port, Setting.ConnectionKey);
        }

        protected override void OnUpdate()
        {
            if (manager != null)
            {
                manager?.PollEvents();
            }
        }

        void INetEventListener.OnConnectionRequest(ConnectionRequest request) { }

        void INetEventListener.OnPeerConnected(NetPeer peer)
        {
            Peer = peer;
            Console.WriteLine("Connected to the server");
        }

        void INetEventListener.OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Console.WriteLine($"Disconnected: {disconnectInfo.Reason}");
        }

        void INetEventListener.OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            Console.WriteLine("Message received");

            // 処理
            var message = MessagePackSerializer.Deserialize<IServerMsg>(reader.GetRemainingBytesSegment());

            switch (message)
            {
                case IServerMsg.ClientId m:
                    if (id is null)
                    {
                        id = m.Id;
                    }
                    break;
                case IServerMsg.SyncGameState m:
                    OnReceiveGameState?.Invoke(m.GameState);
                    break;
                default:
                    Console.WriteLine("Unexpected message");
                    break;
            }

            reader.Recycle();
        }

        void INetEventListener.OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            Console.WriteLine($"Error: {socketError}");
        }

        void INetEventListener.OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {

        }

        void INetEventListener.OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
            this.latency.UpdateLatency(latency);
        }
    }
}
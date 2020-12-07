using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using MessagePack;
using Altseed2;

namespace ACAC2020_15.Client
{
    sealed class NetworkNode : Node, INetEventListener
    {
        NetManager manager;
        NetPeer peer;
        readonly Shared.AveragedLatency latency;

        public bool IsRunning => manager?.IsRunning ?? false;

        public NetworkNode()
        {
            latency = new Shared.AveragedLatency();
        }

        public async ValueTask Start()
        {
            if (manager?.IsRunning ?? true) return;

            var config = await Config.Load(@"netconfig/clientconfig.json");

            manager = new NetManager(this);
            Shared.Setting.SettingNetManagerClient(manager);

            if (!manager.Start())
            {
                throw new Exception("Failed to start the client");
            }

            // サーバーへ接続する
            manager.Connect(config.Address, config.Port, Shared.Setting.ConnectionKey);
        }

        protected override void OnUpdate()
        {
            if (manager != null)
            {
                manager?.PollEvents();

                // 送信
            }
        }

        void INetEventListener.OnConnectionRequest(ConnectionRequest request) { }

        void INetEventListener.OnPeerConnected(NetPeer peer)
        {
            this.peer = peer;
        }

        void INetEventListener.OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Console.WriteLine($"Disconnected: {disconnectInfo.Reason}");
        }

        void INetEventListener.OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            // 処理
            try
            {
                var message = MessagePackSerializer.Deserialize<Shared.IServerMsg>(reader.GetRemainingBytesSegment());

                // 処理
            }
            catch(MessagePackSerializationException e)
            {

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
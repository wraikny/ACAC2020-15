using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net;
using LiteNetLib;
using MessagePack;

namespace ACAC2020_15.Server
{
    sealed class Server : INetEventListener
    {
        private ulong netxtClientId = 0;

        private readonly Config config;

        // Key: NetPeer's id, Value: Client
        private readonly Dictionary<int, Client> clients;

        private readonly Stopwatch stopwatch;

        private NetManager manager;

        public Server(Config config)
        {
            this.config = config;
            clients = new Dictionary<int, Client>();
            stopwatch = new Stopwatch();
        }

        private ulong NewClientId()
        {
            var res = netxtClientId;
            netxtClientId += 1;
            return res;
        }

        public void Start()
        {
            manager = new NetManager(this);
            Shared.Setting.SettingNetManagerServer(manager);

            if (!manager.Start(config.Port))
            {
                throw new Exception("Failed to start the server");
            }

            stopwatch.Start();
        }

        public void Update()
        {
            manager?.PollEvents();

            if (stopwatch.IsRunning)
            {
                stopwatch.Stop();

                var deltaSeond = Shared.Utils.MsToSec((int)stopwatch.ElapsedMilliseconds);

                // 更新
            }

            stopwatch.Start();
        }

        void INetEventListener.OnConnectionRequest(ConnectionRequest request)
        {
            if (manager.ConnectedPeersCount < config.MaxClientCount)
            {
                request.AcceptIfKey(Shared.Setting.ConnectionKey);
            }
            else
            {
                request.Reject();
            }
        }

        void INetEventListener.OnNetworkError(IPEndPoint endPoint, System.Net.Sockets.SocketError socketError)
        {
            Console.WriteLine($"Error: {socketError}");
        }

        void INetEventListener.OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
            var client = clients[peer.Id];
            client.UpdateLatency(latency);
        }

        void INetEventListener.OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            var client = clients[peer.Id];

            // 処理
            try
            {
                var message = MessagePackSerializer.Deserialize<Shared.IClientMsg>(reader.GetRemainingBytesSegment());
            }
            catch (MessagePackSerializationException e)
            {
                Console.WriteLine($"{e.Message}");
                peer.Disconnect();
            }

            reader.Recycle();
        }

        void INetEventListener.OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {

        }

        void INetEventListener.OnPeerConnected(NetPeer peer)
        {
            var clientId = NewClientId();
            // peer.Send(new InitData(clientId), DeliveryMethod.ReliableOrdered);

            var client = new Client(clientId, peer);
            clients.Add(peer.Id, client);

            Console.WriteLine($"New client{clientId} is connected.");
        }

        void INetEventListener.OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            var client = clients[peer.Id];
            var clientId = client.Id;
            // Room0.OnPlayerExited(client);
            clients.Remove(peer.Id);

            Console.WriteLine($"Client({clientId}) is disconnected.");
        }
    }
}
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net;
using LiteNetLib;
using MessagePack;

using ACAC2020_15.Shared;

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

        private GameState gameState;


        public Server(Config config)
        {
            this.config = config;
            clients = new Dictionary<int, Client>();
            stopwatch = new Stopwatch();

            gameState = new GameState();
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
            Setting.SettingNetManagerServer(manager);

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

                var _ = Utils.MsToSec((int)stopwatch.ElapsedMilliseconds);

                // 更新

                // 処理があるならする


                if (gameState.IsUpdated)
                {
                    // 送信
                    var msg = new IServerMsg.SyncGameState(gameState);
                    var data = MessagePackSerializer.Serialize<IServerMsg>(msg);
                    foreach(var client in clients.Values)
                    {
                        client.Peer.Send(data, DeliveryMethod.ReliableOrdered);
                    }

                    gameState.Serialized();
                }
            }

            stopwatch.Start();
        }

        void INetEventListener.OnConnectionRequest(ConnectionRequest request)
        {
            if (manager.ConnectedPeersCount < config.MaxClientCount)
            {
                request.AcceptIfKey(Setting.ConnectionKey);
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

            Console.WriteLine($"Message received from client({client.Id})");

            // 処理
            try
            {
                var message = MessagePackSerializer.Deserialize<Shared.IClientMsg>(reader.GetRemainingBytesSegment());

                switch (message)
                {
                    case IClientMsg.Move m:
                        gameState.PlayerMove(client.Id, m.Direction);
                        break;
                    default:
                        break;
                }
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
            peer.Send(MessagePackSerializer.Serialize<IServerMsg>(new IServerMsg.ClientId(clientId)), DeliveryMethod.ReliableOrdered);

            var client = new Client(clientId, peer);
            clients.Add(peer.Id, client);

            Console.WriteLine($"New client({clientId}) is connected.");

            gameState.PlayerEnter(clientId);
        }

        void INetEventListener.OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            var client = clients[peer.Id];
            var clientId = client.Id;
            // Room0.OnPlayerExited(client);
            clients.Remove(peer.Id);

            Console.WriteLine($"Client({clientId}) is disconnected.");

            gameState.PlayerExit(clientId);
        }
    }
}
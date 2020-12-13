using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagePack;
using MessagePack.Resolvers;
using MessagePack.Altseed2;
using Altseed2;
using LiteNetLib;

using ACAC2020_15.Shared;

namespace ACAC2020_15.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            MessagePackSerializer.DefaultOptions = Utils.MessagePackOption;

            Engine.Initialize("ACAC2020", 800, 600);

            Engine.AddNode(new BackgroundNode());

            var networkNode = new NetworkNode();
            Engine.AddNode(networkNode);

            var selfPlayer = new PlayerInputNode();

            selfPlayer.OnPlayerInput += (action) => {
                if (networkNode.Peer is NetPeer peer)
                {
                    var msg = new IClientMsg.PlayerAction(action);
                    var data = MessagePackSerializer.Serialize<IClientMsg>(msg);
                    peer.Send(data, DeliveryMethod.ReliableOrdered);
                    Console.WriteLine("Send: Move");
                }
            };

            var selfPlayerViewNode = new PlayerViewNode();
            Engine.AddNode(selfPlayerViewNode);

            var otherPlayerNodes = new Dictionary<ulong, OtherPlayerNode>();

            var blockNodes = new Dictionary<ulong, BlockViewNode>();

            networkNode.OnReceiveGameState += (state) => {
                var selfId = networkNode.Id.Value;

                /* Players */

                foreach (var x in state.Players)
                {
                    if (x.Key == selfId)
                    {
                        selfPlayerViewNode.Update(x.Value);
                        continue;
                    }

                    if (!otherPlayerNodes.TryGetValue(x.Key, out OtherPlayerNode player))
                    {
                        Console.WriteLine($"New player({x.Key}) entered");
                        player = new OtherPlayerNode();
                        otherPlayerNodes.Add(x.Key, player);
                        Engine.AddNode(player);
                    }

                    player.Update(x.Value);
                }

                foreach (var id in otherPlayerNodes.Keys.Where(id => !state.Players.ContainsKey(id)).ToArray())
                {
                    otherPlayerNodes.Remove(id, out OtherPlayerNode player);
                    Engine.RemoveNode(player);
                }

                /* Blocks */
                foreach (var x in state.Blocks)
                {
                    var blockId = x.Key;
                    if (!blockNodes.TryGetValue(blockId, out BlockViewNode blockNode))
                    {
                        Console.WriteLine($"New block({blockId}) is created");
                        blockNode = new BlockViewNode(x.Value.Position);
                        blockNodes.Add(blockId, blockNode);
                        Engine.AddNode(blockNode);
                    }
                }

                foreach (var blockId in blockNodes.Keys.Where(id => !state.Blocks.ContainsKey(id)).ToArray())
                {
                    blockNodes.Remove(blockId, out BlockViewNode block);
                    Engine.RemoveNode(block);
                }
            };

            _ = networkNode.Start().ContinueWith(_ => {
                Engine.AddNode(selfPlayer);
            });

            while (Engine.DoEvents())
            {
                Engine.Update();
            }

            Engine.Terminate();
        }
    }
}

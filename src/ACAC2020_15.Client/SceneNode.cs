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
    sealed class SceneNode : Node
    {
        private readonly NetworkNode networkNode;
        private readonly SelfPlayerNode selfPlayerNode;

        private readonly Dictionary<ulong, OtherPlayerNode> otherPlayers;
        private readonly Dictionary<ulong, BlockViewNode> blocks;

        public SceneNode()
        {
            Engine.AddNode(new BackgroundNode());

            networkNode = new NetworkNode();
            networkNode.OnReceiveGameState += OnReceiveGameState;
            AddChildNode(networkNode);

            selfPlayerNode = new SelfPlayerNode();
            selfPlayerNode.OnPlayerInput += OnPlayerInput;

            otherPlayers = new Dictionary<ulong, OtherPlayerNode>();
            blocks = new Dictionary<ulong, BlockViewNode>();
        }

        protected override void OnAdded()
        {
            _ = networkNode.Start().ContinueWith(_ => {
                Engine.AddNode(selfPlayerNode);
            });
        }

        private void OnPlayerInput(IPlayerAction action)
        {
            if (networkNode.Peer is NetPeer peer)
            {
                var msg = new IClientMsg.PlayerAction(action);
                var data = MessagePackSerializer.Serialize<IClientMsg>(msg);
                peer.Send(data, DeliveryMethod.ReliableOrdered);
                Console.WriteLine("Send: Move");
            }
        }

        private void OnReceiveGameState(GameState state)
        {
            var selfId = networkNode.Id.Value;

            /* Players */

            foreach (var x in state.Players)
            {
                if (x.Key == selfId)
                {
                    selfPlayerNode.Update(x.Value);
                    continue;
                }

                if (!otherPlayers.TryGetValue(x.Key, out OtherPlayerNode player))
                {
                    Console.WriteLine($"New player({x.Key}) entered");
                    player = new OtherPlayerNode();
                    otherPlayers.Add(x.Key, player);
                    Engine.AddNode(player);
                }

                player.Update(x.Value);
            }

            foreach (var id in otherPlayers.Keys.Where(id => !state.Players.ContainsKey(id)).ToArray())
            {
                otherPlayers.Remove(id, out OtherPlayerNode player);
                Engine.RemoveNode(player);
            }

            /* Blocks */
            foreach (var x in state.Blocks)
            {
                var blockId = x.Key;
                if (!blocks.TryGetValue(blockId, out BlockViewNode blockNode))
                {
                    Console.WriteLine($"New block({blockId}) is created");
                    blockNode = new BlockViewNode(x.Value.Position);
                    blocks.Add(blockId, blockNode);
                    Engine.AddNode(blockNode);
                }
            }

            foreach (var blockId in blocks.Keys.Where(id => !state.Blocks.ContainsKey(id)).ToArray())
            {
                blocks.Remove(blockId, out BlockViewNode block);
                Engine.RemoveNode(block);
            }
        }
    }
}
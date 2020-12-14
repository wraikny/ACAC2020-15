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

            Engine.AddNode(new SceneNode());

            while (Engine.DoEvents())
            {
                Engine.Update();
            }

            Engine.Terminate();
        }
    }
}

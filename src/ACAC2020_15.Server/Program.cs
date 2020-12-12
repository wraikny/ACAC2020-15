using System;
using System.Threading;
using MessagePack;
using LiteNetLib;

using ACAC2020_15.Shared;

namespace ACAC2020_15.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            MessagePackSerializer.DefaultOptions = Utils.MessagePackOption;

            var config = Config.Load(@"netconfig/serverconfig.json");
            var server = new Server(config);
            server.Start();
            Console.WriteLine($"The server started as port {config.Port}");

            while (true)
            {
                Thread.Sleep(Setting.UpdateTime);
                server.Update();
            }
        }
    }
}

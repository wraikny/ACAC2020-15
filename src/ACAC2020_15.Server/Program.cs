using System;
using System.Threading;
using MessagePack;
using LiteNetLib;

namespace ACAC2020_15.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            MessagePackSerializer.DefaultOptions = Shared.Utils.MessagePackOption;

            var config = Config.Load(@"netconfig/serverconfig.json");
            var server = new Server(config);
            server.Start();
            Console.WriteLine($"The server started as port {config.Port}");

            while (true)
            {
                Thread.Sleep(Shared.Setting.UpdateTime);
                server.Update();
            }
        }
    }
}

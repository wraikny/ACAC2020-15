using System;
using MessagePack;
using Altseed2;

namespace ACAC2020_15.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            MessagePackSerializer.DefaultOptions = Shared.Utils.MessagePackOption;

            Engine.Initialize("ACAC2020", 800, 600);

            while (Engine.DoEvents())
            {
                Engine.Update();
            }

            Engine.Terminate();
        }
    }
}

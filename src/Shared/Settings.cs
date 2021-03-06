using LiteNetLib;
using Altseed2;

namespace ACAC2020_15.Shared
{
    static class Setting
    {
        public const string ConnectionKey = "SomeKey";
        public const int UpdateTime = 20;

        static void SettingNetManager(NetManager manager)
        {
            manager.SimulateLatency = true;
            manager.SimulationMinLatency = 17;
            manager.SimulationMaxLatency = 19;

            // 秒間50回
            manager.UpdateTime = UpdateTime;
        }

        public static void SettingNetManagerServer(NetManager manager)
        {
            SettingNetManager(manager);

            // メッセージのチャンネルを分ける場合に設定する
            // manager.ChannelsCount = 2;
        }
        
        public static void SettingNetManagerClient(NetManager manager)
        {
            SettingNetManager(manager);

            // メッセージのチャンネルを分ける場合に設定する
            // manager.ChannelsCount = 2;
        }
    }
}
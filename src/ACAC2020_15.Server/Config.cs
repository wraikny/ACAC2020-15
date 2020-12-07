using System.IO;
using System.Runtime.Serialization;

namespace ACAC2020_15.Server
{
    [DataContract]
    sealed class Config
    {
        [DataMember]
        public int Port { get; private set; }

        [DataMember]
        public int MaxClientCount { get; private set; }

        internal static Config Load(string path)
        {
            var json = File.ReadAllText(path);
            return Shared.Utils.DeserializeJson<Config>(json);
        }
    }
}
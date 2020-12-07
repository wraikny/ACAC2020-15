using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ACAC2020_15.Client
{
    [DataContract]
    internal class Config
    {
        [DataMember]
        public string Address { get; private set; }

        [DataMember]
        public int Port { get; private set; }

        internal static async ValueTask<Config> Load(string path)
        {
            using (var reader = new StreamReader(path))
            {
                var json = await reader.ReadToEndAsync();
                return Shared.Utils.DeserializeJson<Config>(json);
            }
        }
    }
}

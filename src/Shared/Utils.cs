using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using MessagePack;
using MessagePack.Resolvers;
using MessagePack.Altseed2;

namespace ACAC2020_15.Shared
{
    static class Utils
    {
        public static T DeserializeJson<T>(string json)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(ms);
            }
        }

        public static float MsToSec(int ms)
        {
            return (float)ms / 1000.0f;
        }

        public static MessagePackSerializerOptions MessagePackOption
        {
            get
            {
                var resolver = CompositeResolver.Create(Altseed2Resolver.Instance, StandardResolver.Instance);
                return MessagePackSerializerOptions.Standard
                    .WithResolver(resolver)
                    .WithSecurity(MessagePackSecurity.UntrustedData)
                    .WithCompression(MessagePackCompression.Lz4BlockArray);
            }
        }
    }
}

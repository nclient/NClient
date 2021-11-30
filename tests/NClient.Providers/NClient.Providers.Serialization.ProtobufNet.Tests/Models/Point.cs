using ProtoBuf;

namespace NClient.Providers.Serialization.ProtobufNet.Tests.Models
{
    [ProtoContract]
    public struct Point
    {
        [ProtoMember(1)]
        public int X { get; set; }
        [ProtoMember(2)]
        public int Y { get; set; }
    }
}

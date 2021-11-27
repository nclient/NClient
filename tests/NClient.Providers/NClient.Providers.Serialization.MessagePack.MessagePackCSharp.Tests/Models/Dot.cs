using MessagePack;

namespace NClient.Providers.Serialization.MessagePack.MessagePackCSharp.Tests.Models
{
    [MessagePackObject]
    public struct Dot
    {
        [Key(0)]
        public readonly int X;
        [Key(1)]
        public readonly int Y;
        
        [SerializationConstructor]
        public Dot(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}

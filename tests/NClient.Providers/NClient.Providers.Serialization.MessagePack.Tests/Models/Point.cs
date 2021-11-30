using MessagePack;

namespace NClient.Providers.Serialization.MessagePack.Tests.Models
{
    [MessagePackObject]
    public readonly struct Point
    {
        [Key(0)]
        public readonly int X;
        
        [Key(1)]
        public readonly int Y;

        [SerializationConstructor]
        public Point(int x)
        {
            X = x;
            Y = -1;
        }
        
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}

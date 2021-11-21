namespace NClient.Providers.Serialization.Protobuf.Tests.Models
{
    public readonly struct NotProto
    {
        public readonly int X;
        public readonly int Y;

        public NotProto(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}

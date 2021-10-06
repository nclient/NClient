using NClient.Abstractions.Serialization;

namespace NClient.Standalone.Serialization
{
    internal class StubSerializerProvider : ISerializerProvider
    {
        public ISerializer Create()
        {
            return new StubSerializer();
        }
    }
}

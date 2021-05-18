using NClient.Abstractions.Serialization;

namespace NClient.Core.Serialization
{
    public class StubSerializerProvider : ISerializerProvider
    {
        public ISerializer Create()
        {
            return new StubSerializer();
        }
    }
}
using Microsoft.Extensions.Logging;
using NClient.Providers.Serialization;

namespace NClient.Standalone.ClientProxy.Validation.Serialization
{
    internal class StubSerializerProvider : ISerializerProvider
    {
        public ISerializer Create(ILogger? logger)
        {
            return new StubSerializer();
        }
    }
}

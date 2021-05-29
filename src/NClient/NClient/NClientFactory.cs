using NClient.Providers.HttpClient.System;
using NClient.Providers.Serialization.System;

namespace NClient
{
    public class NClientFactory : NClientStandaloneFactory
    {
        public NClientFactory() : base(new SystemHttpClientProvider(), new SystemSerializerProvider())
        {
        }
    }
}
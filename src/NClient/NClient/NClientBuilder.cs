using NClient.Providers.HttpClient.System;
using NClient.Providers.Serialization.System;

namespace NClient
{
    public class NClientBuilder : NClientStandaloneBuilder
    {
        public NClientBuilder() : base(new SystemHttpClientProvider(), new SystemSerializerProvider())
        {
        }
    }
}
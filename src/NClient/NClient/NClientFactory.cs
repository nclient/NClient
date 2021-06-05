using NClient.Providers.HttpClient.System;
using NClient.Providers.Serialization.System;

namespace NClient
{
    /// <summary>
    /// The factory used to create the client.
    /// </summary>
    public class NClientFactory : NClientStandaloneFactory
    {
        public NClientFactory() : base(new SystemHttpClientProvider(), new SystemSerializerProvider())
        {
        }
    }
}
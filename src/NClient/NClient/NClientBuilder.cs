using NClient.Providers.HttpClient.System;
using NClient.Providers.Serialization.System;

namespace NClient
{
    /// <summary>
    /// The builder used to create the client.
    /// </summary>
    public class NClientBuilder : NClientStandaloneBuilder
    {
        public NClientBuilder() : base(new SystemHttpClientProvider(), new SystemSerializerProvider())
        {
        }
    }
}

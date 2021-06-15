using NClient.Providers.HttpClient.System;
using NClient.Providers.Serialization.System;

namespace NClient
{
    /// <summary>
    /// The builder used to create the client factory.
    /// </summary>
    public class NClientFactoryBuilder : NClientStandaloneFactoryBuilder
    {
        public NClientFactoryBuilder() : base(new SystemHttpClientProvider(), new SystemSerializerProvider())
        {
        }
    }
}
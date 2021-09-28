using System.Net.Http;
using NClient.Core.Resilience;
using NClient.Providers.HttpClient.System;
using NClient.Providers.Serialization.System;
using NClient.Resilience;

namespace NClient
{
    /// <summary>
    /// The builder used to create the client factory.
    /// </summary>
    public class NClientFactoryBuilder : NClientStandaloneFactoryBuilder<HttpRequestMessage, HttpResponseMessage>
    {
        public NClientFactoryBuilder() : base(
            new SystemHttpClientProvider(), 
            new SystemHttpMessageBuilderProvider(),
            new DefaultMethodResiliencePolicyProvider<HttpResponseMessage>(
                new DefaultResiliencePolicyProvider()),
            new SystemSerializerProvider())
        {
        }
    }
}

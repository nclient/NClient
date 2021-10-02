using System.Net.Http;
using NClient.Abstractions;
using NClient.Abstractions.Customization;
using NClient.Customization.Context;
using NClient.Providers.Resilience.Polly;
using NClient.Resilience;

namespace NClient
{
    /// <summary>
    /// The builder used to create the client factory.
    /// </summary>
    public class NClientFactoryBuilder : INClientFactoryBuilder<HttpRequestMessage, HttpResponseMessage>
    {
        public INClientFactoryCustomizer<HttpRequestMessage, HttpResponseMessage> For()
        {
            return new NClientStandaloneFactoryBuilder<HttpRequestMessage, HttpResponseMessage>(
                    customizerContext: new CustomizerContext<HttpRequestMessage, HttpResponseMessage>(),
                    defaultResiliencePolicyProvider: new ConfiguredPollyResiliencePolicyProvider<HttpRequestMessage, HttpResponseMessage>(new NoResiliencePolicySettings()))
                .For()
                .UsingHttpClient()
                .UsingSerializer()
                .WithoutHandling()
                .WithoutResilience()
                .WithoutLogging();
        }
    }
}

using System.Net.Http;
using NClient.Abstractions;
using NClient.Abstractions.Customization;
using NClient.Customization.Context;
using NClient.Providers.Resilience.Polly;
using NClient.Resilience;

namespace NClient
{
    /// <summary>
    /// The builder used to create the client.
    /// </summary>
    public class NClientBuilder : INClientBuilder<HttpRequestMessage, HttpResponseMessage>
    {
        public INClientBuilderCustomizer<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(string host) where TClient : class
        {
            return new NClientStandaloneBuilder<HttpRequestMessage, HttpResponseMessage>(
                    customizerContext: new CustomizerContext<HttpRequestMessage, HttpResponseMessage>(),
                    defaultResiliencePolicyProvider: new ConfiguredPollyResiliencePolicyProvider<HttpRequestMessage, HttpResponseMessage>(new NoResiliencePolicySettings()))
                .For<TClient>(host)
                .UsingHttpClient()
                .UsingSerializer()
                .WithoutHandling()
                .WithoutResilience()
                .WithoutLogging();
        }
    }
}

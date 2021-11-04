using System.Net.Http;
using NClient.Providers.Api.Rest.Extensions;

namespace NClient
{
    public interface INClientBasicFactoryBuilder
    {
        INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> For(string factoryName);
    }
    
    /// <summary>
    /// The builder used to create the client factory.
    /// </summary>
    public class NClientBasicFactoryBuilder : INClientBasicFactoryBuilder
    {
        public INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> For(string factoryName)
        {
            return new NClientFactoryAdvancedBuilder()
                .For(factoryName)
                .UsingRestApi()
                .UsingHttpTransport()
                .UsingJsonSerializer()
                .WithResponseValidation(x => x
                    .ForTransport().UseSystemResponseValidation())
                .WithoutHandling()
                .WithoutResilience()
                .WithResults(x => x
                    .ForTransport().UseHttpResults()
                    .ForClient().UseResults())
                .WithoutLogging()
                .AsBasic();
        }
    }
}

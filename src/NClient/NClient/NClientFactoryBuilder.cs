using System.Net.Http;
using NClient.Abstractions.Builders;

namespace NClient
{
    /// <summary>
    /// The builder used to create the client factory.
    /// </summary>
    public class NClientFactoryBuilder
    {
        public INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> For(string factoryName)
        {
            return new CustomNClientFactoryBuilder()
                .For(factoryName)
                .UsingHttpClient()
                .UsingJsonSerializer()
                .WithoutHandling()
                .WithoutResilience()
                .WithoutLogging();
        }
    }
}

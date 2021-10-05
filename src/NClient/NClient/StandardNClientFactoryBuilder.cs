using System.Net.Http;
using NClient.Abstractions.Builders;

namespace NClient
{
    public interface IStandardNClientFactoryBuilder
    {
        INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> For(string factoryName);
    }
    
    /// <summary>
    /// The builder used to create the client factory.
    /// </summary>
    public class StandardNClientFactoryBuilder : IStandardNClientFactoryBuilder
    {
        public INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> For(string factoryName)
        {
            return new CustomNClientFactoryBuilder()
                .For(factoryName)
                .UsingHttpClient()
                .UsingJsonSerializer()
                .EnsuringSuccess()
                .WithoutHandling()
                .WithoutResilience()
                .WithoutLogging();
        }
    }
}

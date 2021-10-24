using System.Net.Http;

namespace NClient
{
    public interface IBasicNClientFactoryBuilder
    {
        INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> For(string factoryName);
    }
    
    /// <summary>
    /// The builder used to create the client factory.
    /// </summary>
    public class BasicNClientFactoryBuilder : IBasicNClientFactoryBuilder
    {
        public INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> For(string factoryName)
        {
            return new CustomNClientFactoryBuilder()
                .For(factoryName)
                .UsingHttpTransport()
                .UsingJsonSerializer()
                .WithResponseValidation()
                .WithoutHandling()
                .WithoutResilience()
                .WithResults()
                .WithoutLogging();
        }
    }
}

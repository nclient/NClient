using System.Net.Http;

namespace NClient
{
    /// <summary>The client builder factory for a REST-like web API with JSON-formatted data.</summary>
    public interface INClientFactoryRestBuilder
    {
        /// <summary>Sets factory name. The factory name does not affect the functionality, it may be needed to identify the factory.</summary>
        /// <param name="factoryName">The factory name.</param>
        INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> For(string factoryName);
    }
    
    /// <summary>The client builder factory for a REST-like web API with JSON-formatted data.</summary>
    public class NClientFactoryRestBuilder : INClientFactoryRestBuilder
    {
        /// <summary>Sets factory name. The factory name does not affect the functionality, it may be needed to identify the factory.</summary>
        /// <param name="factoryName">The factory name.</param>
        public INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> For(string factoryName)
        {
            return new NClientFactoryBuilder()
                .For(factoryName)
                .UsingRestApi()
                .UsingHttpTransport()
                .UsingJsonSerializer()
                .WithResponseValidation()
                .WithoutHandling()
                .WithoutResilience()
                .WithResponseToHttpResponseMapping()
                .WithResponseToResultMapping()
                .WithoutLogging();
        }
    }
}

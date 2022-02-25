using System.Net.Http;

namespace NClient
{
    /// <summary>The client builder for a REST-like web API with JSON-formatted data.</summary>
    public interface INClientRestBuilder
    {
        /// <summary>Sets client interface type and web service host.</summary>
        /// <param name="host">The address of the web service host.</param>
        /// <typeparam name="TClient">The type of interface of controller used to create the client.</typeparam>
        INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(string host) 
            where TClient : class;
    }
    
    /// <summary>The client builder for a REST-like web API with JSON-formatted data.</summary>
    public class NClientRestBuilder : INClientRestBuilder
    {
        /// <summary>Sets client interface type and web service host.</summary>
        /// <param name="host">The address of the web service host.</param>
        /// <typeparam name="TClient">The type of interface of controller used to create the client.</typeparam>
        public INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(string host) 
            where TClient : class
        {
            return new NClientBuilder()
                .For<TClient>(host)
                .UsingRestApi()
                .UsingHttpTransport()
                .UsingJsonSerializer()
                .WithResponseValidation()
                .WithoutHandling()
                .WithoutResilience()
                .WithResponseToHttpResponseMapping()
                .WithResponseToStreamMapping()
                .WithResponseToResultMapping()
                .WithoutLogging();
        }
    }
}

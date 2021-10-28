using System.Net.Http;
using NClient.Providers.Api.Rest.Extensions;

namespace NClient
{
    public interface IBasicNClientBuilder
    {
        INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(string host) 
            where TClient : class;
    }
    
    /// <summary>
    /// The builder used to create the client.
    /// </summary>
    public class BasicNClientBuilder : IBasicNClientBuilder
    {
        public INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(string host) 
            where TClient : class
        {
            return new CustomNClientBuilder()
                .For<TClient>(host)
                .UsingRestApi()
                .UsingHttpTransport()
                .UsingJsonSerializer()
                .WithResponseValidation()
                .WithoutHandling()
                .WithoutResilience()
                .WithResults()
                .WithHttpResults()
                .WithoutLogging();
        }
    }
}

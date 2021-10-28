using System.Net.Http;
using NClient.Providers.Api.Rest.Extensions;

namespace NClient
{
    public interface IStandardNClientBuilder
    {
        INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(string host) 
            where TClient : class;
    }
    
    /// <summary>
    /// The builder used to create the client.
    /// </summary>
    public class StandardNClientBuilder : IStandardNClientBuilder
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
                .WithIdempotentResilience()
                .WithResults()
                .WithHttpResults()
                .WithoutLogging();
        }
    }
}

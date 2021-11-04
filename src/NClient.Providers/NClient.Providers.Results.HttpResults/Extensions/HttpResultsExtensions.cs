using System.Net.Http;
using NClient.Providers.Results.HttpResults;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class HttpResultsExtensions
    {
        public static INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> WithHttpResults<TClient>(
            this INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> clientOptionalBuilder) 
            where TClient : class
        {
            return clientOptionalBuilder.AsAdvanced()
                .WithResults(x => x
                    .ForTransport().Use(new HttpResponseBuilderProvider()))
                .AsBasic();
        }
        
        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> WithHttpResults(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> clientOptionalBuilder)
        {
            return clientOptionalBuilder.AsAdvanced()
                .WithResults(x => x
                    .ForTransport().Use(new HttpResponseBuilderProvider()))
                .AsBasic();
        }
    }
}

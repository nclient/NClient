using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    public static class AsHttpClientBuilderExtensions
    {
        public static IHttpClientBuilder AsHttpClientBuilder(
            this IDiNClientFactoryBuilder<HttpRequestMessage, HttpResponseMessage> builder)
        {
            return (IHttpClientBuilder) builder;
        }
        
        public static IHttpClientBuilder AsHttpClientBuilder<TClient>(
            this IDiNClientBuilder<TClient, HttpRequestMessage, HttpResponseMessage> builder) 
            where TClient : class
        {
            return (IHttpClientBuilder) builder;
        }
    }
}

using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    public static class AsHttpClientBuilderExtensions
    {
        public static IHttpClientBuilder AsHttpClientBuilder(
            this IDiNClientFactoryBuilder<HttpRequestMessage, HttpResponseMessage> builder)
        {
            Ensure.IsNotNull(builder, nameof(builder));
            
            return (IHttpClientBuilder) builder;
        }
        
        public static IHttpClientBuilder AsHttpClientBuilder<TClient>(
            this IDiNClientBuilder<TClient, HttpRequestMessage, HttpResponseMessage> builder) 
            where TClient : class
        {
            Ensure.IsNotNull(builder, nameof(builder));
            
            return (IHttpClientBuilder) builder;
        }
    }
}

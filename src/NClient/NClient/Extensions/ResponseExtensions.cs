using System.Net.Http;
using NClient.Abstractions.Builders;
using NClient.Providers.Results.HttpMessages.Mappers;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ResponseExtensions
    {
        public static INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> WithHttpResponse<TClient>(
            this INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> clientOptionalBuilder) 
            where TClient : class
        {
            return clientOptionalBuilder.WithCustomResponse(
                new HttpResponseWithDataMapper(),
                new HttpResponseWithErrorMapper(),
                new HttpResponseWithDataAndErrorMapper());
        }
        
        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> WithHttpResponse(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> factoryOptionalBuilder)
        {
            return factoryOptionalBuilder.WithCustomResponse(
                new HttpResponseWithDataMapper(),
                new HttpResponseWithErrorMapper(),
                new HttpResponseWithDataAndErrorMapper());
        }
    }
}

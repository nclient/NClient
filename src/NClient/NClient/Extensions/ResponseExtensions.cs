using System.Net.Http;
using NClient.Abstractions.Builders;
using NClient.Providers.HttpClient.System.Mapping;
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
                new HttpResponseMapper(),
                new HttpResponseWithDataMapper(),
                new HttpResponseWithErrorMapper(),
                new HttpResponseWithDataAndErrorMapper(),
                new CommonResponseMapper());
        }
        
        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> WithHttpResponse(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> factoryOptionalBuilder)
        {
            return factoryOptionalBuilder.WithCustomResponse(
                new HttpResponseMapper(),
                new HttpResponseWithDataMapper(),
                new HttpResponseWithErrorMapper(),
                new HttpResponseWithDataAndErrorMapper(),
                new CommonResponseMapper());
        }
    }
}

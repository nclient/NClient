using System.Net.Http;
using NClient.Providers.Mapping.HttpResponses;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class HttpResponsesExtensions
    {
        public static INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> WithResponseToHttpResponseMapping<TClient>(
            this INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> optionalBuilder) 
            where TClient : class
        {
            return optionalBuilder.WithAdvancedResponseMapping(x => x
                .ForTransport().Use(new ResponseToHttpResponseMapperProvider()));
        }
        
        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> WithResponseToHttpResponseMapping(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> optionalBuilder)
        {
            return optionalBuilder.WithAdvancedResponseMapping(x => x
                .ForTransport().Use(new ResponseToHttpResponseMapperProvider()));
        }
    }
}

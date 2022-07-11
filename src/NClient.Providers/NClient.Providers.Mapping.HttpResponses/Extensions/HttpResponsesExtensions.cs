using System.Net.Http;
using NClient.Providers.Mapping.HttpResponses;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class HttpResponsesExtensions
    {
        /// <summary>Sets the mapper that can convert System.Net.Http transport messages into HTTP NClient responses with deserialized data.</summary>
        /// <typeparam name="TClient">The type of client interface.</typeparam>
        public static INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> WithResponseToHttpResponseMapping<TClient>(
            this INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> optionalBuilder) 
            where TClient : class
        {
            return optionalBuilder.WithAdvancedResponseMapping(x => x
                .ForTransport().Use(new ResponseToHttpResponseMapperProvider()));
        }
        
        /// <summary>Sets the mapper that can convert System.Net.Http transport messages into HTTP NClient responses with deserialized data.</summary>
        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> WithResponseToHttpResponseMapping(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> optionalBuilder)
        {
            return optionalBuilder.WithAdvancedResponseMapping(x => x
                .ForTransport().Use(new ResponseToHttpResponseMapperProvider()));
        }
    }
}

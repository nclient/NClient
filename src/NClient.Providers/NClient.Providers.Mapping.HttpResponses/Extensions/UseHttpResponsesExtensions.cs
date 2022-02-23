using System.Net.Http;
using NClient.Providers.Mapping.HttpResponses;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UseHttpResponsesExtensions
    {
        /// <summary>Sets the mapper that can convert System.Net.Http transport messages into HTTP NClient responses with deserialized data.</summary>
        public static INClientResponseMappingSelector<HttpRequestMessage, HttpResponseMessage> UseHttpResponses(
            this INClientTransportResponseMappingSetter<HttpRequestMessage, HttpResponseMessage> optionalBuilder)
        {
            return optionalBuilder.Use(new ResponseToHttpResponseMapperProvider());
        }
    }
}

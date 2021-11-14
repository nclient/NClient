using System.Net.Http;
using NClient.Providers.Mapping.HttpResponses;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UseHttpResponsesExtensions
    {
        public static INClientResponseMappingSelector<HttpRequestMessage, HttpResponseMessage> UseHttpResponses(
            this INClientTransportResponseMappingSetter<HttpRequestMessage, HttpResponseMessage> optionalBuilder)
        {
            return optionalBuilder.Use(new ResponseToHttpResponseMapperProvider());
        }
    }
}

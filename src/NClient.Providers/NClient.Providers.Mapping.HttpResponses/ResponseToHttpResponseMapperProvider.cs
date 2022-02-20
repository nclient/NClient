using System.Net.Http;

namespace NClient.Providers.Mapping.HttpResponses
{
    /// <summary>The provider that converts System.Net.Http transport messages into HTTP NClient responses with deserialized data.</summary>
    public class ResponseToHttpResponseMapperProvider : IResponseMapperProvider<HttpRequestMessage, HttpResponseMessage>
    {
        public IResponseMapper<HttpRequestMessage, HttpResponseMessage> Create(IToolset toolset)
        {
            return new ResponseToHttpResponseMapper();
        }
    }
}

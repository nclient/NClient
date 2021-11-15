using System.Net.Http;

namespace NClient.Providers.Mapping.HttpResponses
{
    public class ResponseToHttpResponseMapperProvider : IResponseMapperProvider<HttpRequestMessage, HttpResponseMessage>
    {
        public IResponseMapper<HttpRequestMessage, HttpResponseMessage> Create(IToolset toolset)
        {
            return new ResponseToHttpResponseMapper();
        }
    }
}

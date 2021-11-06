using System.Net.Http;

namespace NClient.Providers.Results.HttpResponses
{
    public class HttpResponseBuilderProvider : IResultBuilderProvider<HttpRequestMessage, HttpResponseMessage>
    {
        public IResultBuilder<HttpRequestMessage, HttpResponseMessage> Create()
        {
            return new HttpResponseBuilder();
        }
    }
}

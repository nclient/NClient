using System.Net.Http;

namespace NClient.Providers.Results.HttpResults
{
    public class HttpResponseBuilderProvider : IResultBuilderProvider<HttpRequestMessage, HttpResponseMessage>
    {
        public IResultBuilder<HttpRequestMessage, HttpResponseMessage> Create()
        {
            return new HttpResponseBuilder();
        }
    }
}

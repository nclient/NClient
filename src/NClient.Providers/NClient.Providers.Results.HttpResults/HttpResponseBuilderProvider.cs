using System.Net.Http;

namespace NClient.Providers.Results.HttpResults
{
    public class HttpResponseBuilderProvider : IResultBuilderProvider<HttpResponseMessage>
    {
        public IResultBuilder<HttpResponseMessage> Create()
        {
            return new HttpResponseBuilder();
        }
    }
}

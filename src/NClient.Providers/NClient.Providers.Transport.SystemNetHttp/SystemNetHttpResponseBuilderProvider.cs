using System.Net.Http;

namespace NClient.Providers.Transport.SystemNetHttp
{
    public class SystemNetHttpResponseBuilderProvider : IResponseBuilderProvider<HttpRequestMessage, HttpResponseMessage>
    {
        public IResponseBuilder<HttpRequestMessage, HttpResponseMessage> Create(IToolset toolset)
        {
            return new SystemNetHttpResponseBuilder();
        }
    }
}

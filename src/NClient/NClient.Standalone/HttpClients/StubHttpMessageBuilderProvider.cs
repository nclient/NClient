using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;

namespace NClient.Standalone.HttpClients
{
    public class StubHttpMessageBuilderProvider : IHttpMessageBuilderProvider<HttpRequest, HttpResponse>
    {
        public IHttpMessageBuilder<HttpRequest, HttpResponse> Create(ISerializer serializer)
        {
            return new StubHttpMessageBuilder();
        }
    }
}

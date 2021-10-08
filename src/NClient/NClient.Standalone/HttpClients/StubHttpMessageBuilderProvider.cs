using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;

namespace NClient.Standalone.HttpClients
{
    public class StubHttpMessageBuilderProvider : IHttpMessageBuilderProvider<IHttpRequest>
    {
        public IHttpMessageBuilder<IHttpRequest> Create(ISerializer serializer)
        {
            return new StubHttpRequestBuilder();
        }
    }
}

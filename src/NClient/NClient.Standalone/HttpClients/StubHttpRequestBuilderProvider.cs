using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;

namespace NClient.Standalone.HttpClients
{
    public class StubHttpRequestBuilderProvider : IHttpRequestBuilderProvider<IHttpRequest>
    {
        public IHttpRequestBuilder<IHttpRequest> Create(ISerializer serializer)
        {
            return new StubHttpRequestBuilder();
        }
    }
}

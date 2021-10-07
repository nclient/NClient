using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;

namespace NClient.Standalone.HttpClients
{
    public class StubHttpMessageBuilderProvider : IHttpMessageBuilderProvider<IHttpRequest, IHttpResponse>
    {
        public IHttpMessageBuilder<IHttpRequest, IHttpResponse> Create(ISerializer serializer)
        {
            return new StubHttpMessageBuilder();
        }
    }
}

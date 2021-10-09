using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;

namespace NClient.Standalone.Client.HttpClients
{
    internal class StubHttpMessageBuilderProvider : IHttpMessageBuilderProvider<IHttpRequest, IHttpResponse>
    {
        public IHttpMessageBuilder<IHttpRequest, IHttpResponse> Create(ISerializer serializer)
        {
            return new StubHttpMessageBuilder();
        }
    }
}

using NClient.Abstractions.Providers.Serialization;
using NClient.Abstractions.Providers.Transport;

namespace NClient.Standalone.Client.Transport
{
    internal class StubHttpMessageBuilderProvider : IHttpMessageBuilderProvider<IHttpRequest, IHttpResponse>
    {
        public IHttpMessageBuilder<IHttpRequest, IHttpResponse> Create(ISerializer serializer)
        {
            return new StubHttpMessageBuilder();
        }
    }
}

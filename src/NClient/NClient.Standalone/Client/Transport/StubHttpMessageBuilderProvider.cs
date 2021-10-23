using NClient.Providers.Serialization;
using NClient.Providers.Transport;

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

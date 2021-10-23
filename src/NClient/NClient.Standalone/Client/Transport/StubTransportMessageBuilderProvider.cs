using NClient.Providers.Serialization;
using NClient.Providers.Transport;

namespace NClient.Standalone.Client.Transport
{
    internal class StubTransportMessageBuilderProvider : ITransportMessageBuilderProvider<IHttpRequest, IHttpResponse>
    {
        public ITransportMessageBuilder<IHttpRequest, IHttpResponse> Create(ISerializer serializer)
        {
            return new StubTransportMessageBuilder();
        }
    }
}

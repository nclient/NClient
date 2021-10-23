using NClient.Providers.Serialization;
using NClient.Providers.Transport;

namespace NClient.Standalone.Client.Transport
{
    internal class StubTransportProvider : ITransportProvider<IHttpRequest, IHttpResponse>
    {
        public ITransport<IHttpRequest, IHttpResponse> Create(ISerializer? serializer = null)
        {
            return new StubTransport();
        }
    }
}

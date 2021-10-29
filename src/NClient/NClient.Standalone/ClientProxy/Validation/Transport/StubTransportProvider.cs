using NClient.Providers.Serialization;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Validation.Transport
{
    internal class StubTransportProvider : ITransportProvider<IRequest, IResponse>
    {
        public ITransport<IRequest, IResponse> Create(ISerializer? serializer = null)
        {
            return new StubTransport();
        }
    }
}

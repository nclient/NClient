using NClient.Providers;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Validation.Transport
{
    internal class StubTransportProvider : ITransportProvider<IRequest, IResponse>
    {
        public ITransport<IRequest, IResponse> Create(IToolset toolset)
        {
            return new StubTransport();
        }
    }
}

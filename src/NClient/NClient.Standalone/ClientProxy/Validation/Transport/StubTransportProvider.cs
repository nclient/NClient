using NClient.Providers;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Validation.Transport
{
    internal class StubTransportProvider<TRequest, TResponse> : ITransportProvider<IRequest, IResponse>
    {
        public ITransport<IRequest, IResponse> Create(IToolset toolset)
        {
            return new StubTransportBuilder<TRequest, TResponse>();
        }
    }
}

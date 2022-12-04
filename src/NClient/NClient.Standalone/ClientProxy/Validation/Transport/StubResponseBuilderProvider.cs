using NClient.Providers;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Validation.Transport
{
    internal class StubResponseBuilderProvider<TRequest, TResponse> : IResponseBuilderProvider<IRequest, IResponse>
    {
        public IResponseBuilder<IRequest, IResponse> Create(IToolset toolset)
        {
            return new StubResponseBuilder<TRequest, TResponse>();
        }
    }
}

using NClient.Providers;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Validation.Transport
{
    internal class StubResponseBuilderProvider : IResponseBuilderProvider<IRequest, IResponse>
    {
        public IResponseBuilder<IRequest, IResponse> Create(IToolSet toolSet)
        {
            return new StubResponseBuilder();
        }
    }
}

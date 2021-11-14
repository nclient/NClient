using NClient.Providers;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Validation.Transport
{
    internal class StubTransportRequestBuilderProvider : ITransportRequestBuilderProvider<IRequest, IResponse>
    {
        public ITransportRequestBuilder<IRequest, IResponse> Create(IToolset toolset)
        {
            return new StubTransportRequestBuilder();
        }
    }
}

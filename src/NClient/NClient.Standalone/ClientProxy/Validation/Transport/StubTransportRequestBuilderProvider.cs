using NClient.Providers;
using NClient.Providers.Transport;
using NClient.Providers.Transport.Common;

namespace NClient.Standalone.ClientProxy.Validation.Transport
{
    internal class StubTransportRequestBuilderProvider : ITransportRequestBuilderProvider<IRequest, IResponse>
    {
        public ITransportRequestBuilder<IRequest, IResponse> Create(IToolset toolset)
        {
            return new StubTransportRequestBuilder();
        }

        public ITransportRequestBuilder<IRequest, IResponse> Create(IToolset toolset, IPipelineCanceller pipelineCanceller)
        {
            pipelineCanceller.Cancel();
            return new StubTransportRequestBuilder();
        }
    }
}

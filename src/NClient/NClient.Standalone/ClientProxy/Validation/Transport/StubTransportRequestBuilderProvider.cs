using NClient.Providers;
using NClient.Providers.Transport;
using NClient.Providers.Transport.Common;

namespace NClient.Standalone.ClientProxy.Validation.Transport
{
    internal class StubTransportRequestBuilderProvider<TRequest, TResponse> : ITransportRequestBuilderProvider<IRequest, IResponse>
    {
        public ITransportRequestBuilder<IRequest, IResponse> Create(IToolset toolset)
        {
            return new StubTransportRequestBuilder<TRequest, TResponse>();
        }

        public ITransportRequestBuilder<IRequest, IResponse> Create(IToolset toolset, IPipelineCanceller pipelineCanceller)
        {
            return new StubTransportRequestBuilder<TRequest, TResponse>();
        }
    }
}

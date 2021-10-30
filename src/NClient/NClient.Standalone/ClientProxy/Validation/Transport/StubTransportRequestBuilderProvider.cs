using NClient.Providers.Serialization;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Validation.Transport
{
    internal class StubTransportRequestBuilderProvider : ITransportRequestBuilderProvider<IRequest, IResponse>
    {
        public ITransportRequestBuilder<IRequest, IResponse> Create(ISerializer serializer)
        {
            return new StubTransportRequestBuilder();
        }
    }
}

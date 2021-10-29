using NClient.Providers.Serialization;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Validation.Transport
{
    internal class StubResponseBuilderProvider : IResponseBuilderProvider<IRequest, IResponse>
    {
        public IResponseBuilder<IRequest, IResponse> Create(ISerializer serializer)
        {
            return new StubResponseBuilder();
        }
    }
}

using NClient.Providers.Serialization;
using NClient.Providers.Transport;

namespace NClient.Standalone.Client.Transport
{
    internal class StubTransportMessageBuilderProvider : ITransportMessageBuilderProvider<IRequest, IResponse>
    {
        public ITransportMessageBuilder<IRequest, IResponse> Create(ISerializer serializer)
        {
            return new StubTransportMessageBuilder();
        }
    }
}

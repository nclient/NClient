using NClient.Providers.Api;
using NClient.Providers.Serialization;

namespace NClient.Standalone.ClientProxy.Api
{
    public class StubRequestBuilderProvider : IRequestBuilderProvider
    {
        public IRequestBuilder Create(ISerializer serializer)
        {
            return new StubRequestBuilder();
        }
    }
}

using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Providers.Results.HttpMessages;

namespace NClient.Standalone.HttpClients
{
    public class StubHttpMessageBuilderProvider : IHttpMessageBuilderProvider<IHttpRequest, IHttpResponse>
    {
        public IHttpMessageBuilder<IHttpRequest, IHttpResponse> Create(ISerializer serializer)
        {
            return new StubHttpRequestBuilder();
        }
    }
}

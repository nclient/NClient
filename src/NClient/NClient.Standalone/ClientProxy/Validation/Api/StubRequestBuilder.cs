using System;
using NClient.Providers.Api;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Validation.Api
{
    public class StubRequestBuilder : IRequestBuilder
    {
        public IRequest Build(Guid requestId, string resource, IMethodInvocation methodInvocation)
        {
            return new Request(requestId, resource, RequestType.Custom);
        }
    }
}

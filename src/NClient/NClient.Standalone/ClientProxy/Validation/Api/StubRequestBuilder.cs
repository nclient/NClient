using System;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Api;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Validation.Api
{
    internal class StubRequestBuilder : IRequestBuilder
    {
        public Task<IRequest> BuildAsync(Guid requestId, string resource, IMethodInvocation methodInvocation, CancellationToken cancellationToken)
        {
            return Task.FromResult<IRequest>(new Request(requestId, resource, RequestType.Custom));
        }
    }
}

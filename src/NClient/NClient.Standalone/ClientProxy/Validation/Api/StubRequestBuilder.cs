using System;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Api;
using NClient.Providers.Authorization;
using NClient.Providers.Host;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Validation.Api
{
    internal class StubRequestBuilder : IRequestBuilder
    {
        public async Task<IRequest> BuildAsync(Guid requestId, IHost host, IAuthorization authorization, IMethodInvocation methodInvocation, CancellationToken cancellationToken)
        {
            return await Task.FromResult<IRequest>(new Request(requestId, (await host.TryGetUriAsync(cancellationToken).ConfigureAwait(false))!, RequestType.Custom)).ConfigureAwait(false);
        }
    }
}

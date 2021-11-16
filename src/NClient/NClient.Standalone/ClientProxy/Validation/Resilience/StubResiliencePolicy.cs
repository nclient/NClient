using System;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Resilience;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Validation.Resilience
{
    internal class StubResiliencePolicy<TRequest, TResponse> : IResiliencePolicy<TRequest, TResponse>
    {
        public async Task<IResponseContext<TRequest, TResponse>> ExecuteAsync(
            Func<CancellationToken, Task<IResponseContext<TRequest, TResponse>>> action, CancellationToken cancellationToken)
        {
            return await action.Invoke(cancellationToken).ConfigureAwait(false);
        }
    }
}

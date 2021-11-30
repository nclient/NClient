using System.Collections.Generic;
using System.Threading;
using NClient.Invocation;
using NClient.Providers.Resilience;

namespace NClient.Standalone.ClientProxy.Generation.Invocation
{
    internal class ClientMethodInvocation<TRequest, TResponse> : MethodInvocation
    {
        public IResiliencePolicyProvider<TRequest, TResponse>? ResiliencePolicyProvider { get; }
        public CancellationToken? CancellationToken { get; }

        public ClientMethodInvocation(
            IMethod method, IEnumerable<object> arguments, 
            IResiliencePolicyProvider<TRequest, TResponse>? resiliencePolicyProvider, CancellationToken? cancellationToken)
            : base(method, arguments)
        {
            ResiliencePolicyProvider = resiliencePolicyProvider;
            CancellationToken = cancellationToken;
        }
    }
}

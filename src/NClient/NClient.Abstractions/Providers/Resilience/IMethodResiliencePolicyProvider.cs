using NClient.Invocation;
using NClient.Providers.Transport;

namespace NClient.Providers.Resilience
{
    internal interface IMethodResiliencePolicyProvider<TRequest, TResponse>
    {
        IResiliencePolicy<TRequest, TResponse> Create(IMethod method, IRequest request, IToolSet toolset);
    }
}

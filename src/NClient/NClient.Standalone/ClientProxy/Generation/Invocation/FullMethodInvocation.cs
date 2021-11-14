using NClient.Invocation;
using NClient.Providers.Resilience;

namespace NClient.Standalone.ClientProxy.Generation.Invocation
{
    internal class FullMethodInvocation<TRequest, TResponse> : MethodInvocation
    {
        public IResiliencePolicyProvider<TRequest, TResponse>? ResiliencePolicyProvider { get; }

        public FullMethodInvocation(IMethod method, ExplicitInvocation<TRequest, TResponse> explicitInvocation)
            : base(method, explicitInvocation.Arguments)
        {
            ResiliencePolicyProvider = explicitInvocation.ResiliencePolicyProvider;
        }
    }
}

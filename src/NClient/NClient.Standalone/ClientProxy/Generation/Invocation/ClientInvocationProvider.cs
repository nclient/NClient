using NClient.Core.Extensions;
using NClient.Invocation;

namespace NClient.Standalone.ClientProxy.Generation.Invocation
{
    internal interface IClientMethodInvocationProvider<TRequest, TResponse>
    {
        ClientMethodInvocation<TRequest, TResponse> Get(IMethod method, ExplicitMethodInvocation<TRequest, TResponse> explicitMethodInvocation);
    }
    
    internal class ClientMethodInvocationProvider<TRequest, TResponse> : IClientMethodInvocationProvider<TRequest, TResponse>
    {
        public ClientMethodInvocation<TRequest, TResponse> Get(IMethod method, ExplicitMethodInvocation<TRequest, TResponse> explicitMethodInvocation)
        {
            if (explicitMethodInvocation.Method.HasNClientCancellationToken())
                return new ClientMethodInvocation<TRequest, TResponse>(
                    method,
                    explicitMethodInvocation.Method.GetArgumentsExceptNClientCancellationToken(explicitMethodInvocation.Arguments),
                    explicitMethodInvocation.ResiliencePolicyProvider,
                    explicitMethodInvocation.CancellationToken ?? explicitMethodInvocation.Method.GetNClientCancellationToken(explicitMethodInvocation.Arguments));
            
            return new ClientMethodInvocation<TRequest, TResponse>(
                method,
                explicitMethodInvocation.Arguments,
                explicitMethodInvocation.ResiliencePolicyProvider,
                explicitMethodInvocation.CancellationToken);
        }
    }
}

using System;
using System.Reflection;
using NClient.Providers.Resilience;

namespace NClient.Standalone.ClientProxy.Generation.Invocation
{
    internal class FullMethodInvocation<TRequest, TResponse> : MethodInvocation
    {
        public IResiliencePolicyProvider<TRequest, TResponse>? ResiliencePolicyProvider { get; set; }

        public FullMethodInvocation(Type clientType, MethodInfo methodInfo, object[] methodArguments, Type resultType)
            : base(clientType, methodInfo, methodArguments, resultType)
        {
        }
    }
}

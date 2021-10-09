using System;
using System.Reflection;
using NClient.Abstractions.Invocation;
using NClient.Abstractions.Resilience;

namespace NClient.Standalone.ClientProxy.Interceptors.Invocation
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

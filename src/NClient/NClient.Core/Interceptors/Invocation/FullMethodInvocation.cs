using System;
using System.Reflection;
using NClient.Abstractions.Invocation;
using NClient.Abstractions.Resilience;

namespace NClient.Core.Interceptors.Invocation
{
    internal class FullMethodInvocation<TResponse> : MethodInvocation
    {
        public IResiliencePolicyProvider<TResponse>? ResiliencePolicyProvider { get; set; }

        public FullMethodInvocation(Type clientType, MethodInfo methodInfo, object[] methodArguments, Type resultType)
            : base(clientType, methodInfo, methodArguments, resultType)
        {
        }
    }
}

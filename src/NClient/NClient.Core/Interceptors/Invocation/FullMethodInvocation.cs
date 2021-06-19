using System;
using System.Reflection;
using NClient.Abstractions.Invocation;
using NClient.Abstractions.Resilience;

namespace NClient.Core.Interceptors.Invocation
{
    internal class FullMethodInvocation : MethodInvocation
    {
        public IResiliencePolicyProvider? ResiliencePolicyProvider { get; set; }

        public FullMethodInvocation(
            Type clientType, MethodInfo methodInfo, object[] methodArguments, Type resultType) 
            : base(clientType, methodInfo, methodArguments, resultType)
        {
        }
    }
}
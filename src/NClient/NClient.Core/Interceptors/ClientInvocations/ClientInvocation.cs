using System;
using System.Reflection;
using NClient.Abstractions.Resilience;

namespace NClient.Core.Interceptors.ClientInvocations
{
    public class ClientInvocation
    {
        public Type ClientType { get; }
        public MethodInfo MethodInfo { get; }
        public object[] MethodArguments { get; }
        public IResiliencePolicyProvider? ResiliencePolicyProvider { get; set; }

        public ClientInvocation(Type clientType, MethodInfo methodInfo, object[] methodArguments)
        {
            ClientType = clientType;
            MethodInfo = methodInfo;
            MethodArguments = methodArguments;
        }
    }
}
using System;
using System.Reflection;

namespace NClient.Abstractions.Invocation
{
    public class MethodInvocation
    {
        public Type ClientType { get; }
        public MethodInfo MethodInfo { get; }
        public object[] MethodArguments { get; }
        public Type ResultType { get; }

        public MethodInvocation(
            Type clientType, MethodInfo methodInfo, object[] methodArguments, Type resultType)
        {
            ClientType = clientType;
            MethodInfo = methodInfo;
            MethodArguments = methodArguments;
            ResultType = resultType;
        }
    }
}
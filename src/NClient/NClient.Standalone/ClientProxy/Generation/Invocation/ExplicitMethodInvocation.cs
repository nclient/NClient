using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Castle.DynamicProxy;
using NClient.Providers.Resilience;

namespace NClient.Standalone.ClientProxy.Generation.Invocation
{
    internal class ExplicitMethodInvocation<TRequest, TResponse>
    {
        public object[] Arguments { get; }
        public Type[] GenericArguments { get; }
        public object? InvocationTarget { get; }
        public MethodInfo Method { get; }
        public MethodInfo? MethodInvocationTarget { get; }
        public object Proxy { get; }
        public object? ReturnValue { get; }
        public Type ReturnType { get; }
        public Type? TargetType { get; }
        public IResiliencePolicyProvider<TRequest, TResponse>? ResiliencePolicyProvider { get; }
        public CancellationToken? CancellationToken { get; }

        public ExplicitMethodInvocation(
            IInvocation invocation,
            Type returnType,
            IResiliencePolicyProvider<TRequest, TResponse>? resiliencePolicyProvider = null,
            CancellationToken? cancellationToken = null)
        {
            Arguments = invocation.Arguments?.ToArray() ?? Array.Empty<object>();
            GenericArguments = invocation.GenericArguments?.ToArray() ?? Array.Empty<Type>();
            InvocationTarget = invocation.InvocationTarget;
            Method = invocation.Method;
            MethodInvocationTarget = invocation.MethodInvocationTarget;
            Proxy = invocation.Proxy;
            ReturnValue = invocation.ReturnValue;
            TargetType = invocation.TargetType;
            ReturnType = returnType;
            ResiliencePolicyProvider = resiliencePolicyProvider;
            CancellationToken = cancellationToken;
        }
    }
}

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Castle.DynamicProxy;
using NClient.Abstractions.Clients;
using NClient.Abstractions.Resilience;

namespace NClient.Core.Interceptors.Invocation
{
    internal interface IFullMethodInvocationProvider<TRequest, TResponse>
    {
        FullMethodInvocation<TRequest, TResponse> Get(Type interfaceType, Type resultType, IInvocation invocation);
    }

    internal class FullMethodInvocationProvider<TRequest, TResponse> : IFullMethodInvocationProvider<TRequest, TResponse>
    {
        private readonly IProxyGenerator _proxyGenerator;

        public FullMethodInvocationProvider(IProxyGenerator proxyGenerator)
        {
            _proxyGenerator = proxyGenerator;
        }
        
        public FullMethodInvocation<TRequest, TResponse> Get(Type interfaceType, Type resultType, IInvocation invocation)
        {
            if (!IsNClientMethod(invocation.Method))
                return BuildInvocation(interfaceType, resultType, invocation, resiliencePolicyProvider: null);

            var clientMethodInvocation = invocation.Arguments[0];
            if (invocation.Arguments[0] is null)
                throw new ArgumentNullException(invocation.Method.GetParameters()[0].Name);

            var keepDataInterceptor = new KeepDataInterceptor();
            var proxyClient = _proxyGenerator.CreateInterfaceProxyWithoutTarget(interfaceType, keepDataInterceptor);
            ((LambdaExpression)clientMethodInvocation).Compile().DynamicInvoke(proxyClient);

            var innerInvocation = keepDataInterceptor.Invocation!;
            // TODO: Magic
            var resiliencePolicyProvider = invocation.Arguments.Length == 2 
                ? (IResiliencePolicyProvider<TRequest, TResponse>?)invocation.Arguments[1]
                : null;

            return BuildInvocation(interfaceType, resultType, innerInvocation, resiliencePolicyProvider);
        }
        
        private static FullMethodInvocation<TRequest, TResponse> BuildInvocation(
            Type interfaceType, Type resultType, IInvocation invocation, IResiliencePolicyProvider<TRequest, TResponse>? resiliencePolicyProvider)
        {
            return new FullMethodInvocation<TRequest, TResponse>(interfaceType, invocation.Method, invocation.Arguments, resultType)
            {
                ResiliencePolicyProvider = resiliencePolicyProvider
            };
        }

        private static bool IsNClientMethod(MethodInfo method)
        {
            if (typeof(IResilienceNClient<>).GetMethods().Any(x => NClientMethodEquals(x, method)))
                return true;
            if (typeof(IHttpNClient<>).GetMethods().Any(x => NClientMethodEquals(x, method)))
                return true;

            return false;
        }

        private static bool NClientMethodEquals(MethodInfo nclientMethodInfo, MethodInfo methodInfo)
        {
            if (methodInfo.DeclaringType is null || !methodInfo.DeclaringType.IsGenericType)
                return false;
            if (nclientMethodInfo.DeclaringType!.GetGenericArguments().Length != methodInfo.DeclaringType.GetGenericArguments().Length)
                return false;

            var nclientDeclaringTypeWithParam = nclientMethodInfo.DeclaringType!.MakeGenericType(methodInfo.DeclaringType.GetGenericArguments());
            var methodDeclaringTypeWithParam = methodInfo.DeclaringType;
            if (nclientDeclaringTypeWithParam != methodDeclaringTypeWithParam)
                return false;

            return nclientMethodInfo.Name == methodInfo.Name;
        }
    }
}

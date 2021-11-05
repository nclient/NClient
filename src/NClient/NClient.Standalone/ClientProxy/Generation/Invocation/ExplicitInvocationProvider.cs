using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Castle.DynamicProxy;
using NClient.Providers.Resilience;
using NClient.Standalone.ClientProxy.Generation.Interceptors;

namespace NClient.Standalone.ClientProxy.Generation.Invocation
{
    internal interface IExplicitInvocationProvider<TRequest, TResponse>
    {
        ExplicitInvocation<TRequest, TResponse> Get(Type clientType, IInvocation invocation, Type returnType);
    }

    internal class ExplicitInvocationProvider<TRequest, TResponse> : IExplicitInvocationProvider<TRequest, TResponse>
    {
        private readonly IProxyGenerator _proxyGenerator;

        public ExplicitInvocationProvider(IProxyGenerator proxyGenerator)
        {
            _proxyGenerator = proxyGenerator;
        }
        
        public ExplicitInvocation<TRequest, TResponse> Get(Type clientType, IInvocation invocation, Type returnType)
        {
            if (!IsNClientMethod(invocation.Method))
                return new ExplicitInvocation<TRequest, TResponse>(invocation, returnType);

            var clientMethodInvocation = invocation.Arguments[0];
            if (invocation.Arguments[0] is null)
                throw new ArgumentNullException(invocation.Method.GetParameters()[0].Name);

            var keepDataInterceptor = new KeepDataInterceptor();
            var proxyClient = _proxyGenerator.CreateInterfaceProxyWithoutTarget(clientType, keepDataInterceptor);
            ((LambdaExpression) clientMethodInvocation).Compile().DynamicInvoke(proxyClient);

            var innerInvocation = keepDataInterceptor.Invocation!;

            var resiliencePolicyArgumentIndex = invocation.Method.GetParameters()
                .Select((param, index) => new { Param = param, Index = index })
                .SingleOrDefault(x => x.Param.ParameterType == typeof(IResiliencePolicyProvider<TRequest, TResponse>))?
                .Index;
            var resiliencePolicyProvider = resiliencePolicyArgumentIndex.HasValue
                ? (IResiliencePolicyProvider<TRequest, TResponse>?) invocation.Arguments[resiliencePolicyArgumentIndex.Value]
                : null;

            return new ExplicitInvocation<TRequest, TResponse>(innerInvocation, returnType, resiliencePolicyProvider);
        }

        private static bool IsNClientMethod(MethodInfo method)
        {
            if (typeof(IResilienceNClient<>).GetMethods().Any(x => NClientMethodEquals(x, method)))
                return true;
            if (typeof(ITransportNClient<>).GetMethods().Any(x => NClientMethodEquals(x, method)))
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

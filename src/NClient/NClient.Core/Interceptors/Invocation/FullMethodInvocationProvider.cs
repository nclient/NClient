using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Castle.DynamicProxy;
using NClient.Abstractions.Clients;
using NClient.Abstractions.Resilience;

namespace NClient.Core.Interceptors.Invocation
{
    internal interface IFullMethodInvocationProvider
    {
        FullMethodInvocation Get(
            Type interfaceType, Type? controllerType, Type resultType, IInvocation invocation);
    }

    internal class FullMethodInvocationProvider : IFullMethodInvocationProvider
    {
        private readonly IProxyGenerator _proxyGenerator;

        public FullMethodInvocationProvider(IProxyGenerator proxyGenerator)
        {
            _proxyGenerator = proxyGenerator;
        }

        public FullMethodInvocation Get(
            Type interfaceType, Type? controllerType, Type resultType, IInvocation invocation)
        {
            if (!IsNClientMethod(invocation.Method))
                return BuildInvocation(interfaceType, controllerType, resultType, invocation, resiliencePolicyProvider: null);

            var clientMethodInvocation = invocation.Arguments[0];
            if (invocation.Arguments[0] is null)
                throw new ArgumentNullException(invocation.Method.GetParameters()[0].Name);

            var keepDataInterceptor = new KeepDataInterceptor();
            var proxyClient = _proxyGenerator.CreateInterfaceProxyWithoutTarget(interfaceType, keepDataInterceptor);
            ((LambdaExpression)clientMethodInvocation).Compile().DynamicInvoke(proxyClient);

            var innerInvocation = keepDataInterceptor.Invocation!;
            var resiliencePolicyProvider = (IResiliencePolicyProvider?)invocation.Arguments[1];

            return BuildInvocation(interfaceType, controllerType, resultType, innerInvocation, resiliencePolicyProvider);
        }

        private static FullMethodInvocation BuildInvocation(
            Type interfaceType, Type? controllerType, Type resultType, IInvocation invocation, IResiliencePolicyProvider? resiliencePolicyProvider)
        {
            var clientType = controllerType ?? interfaceType;
            var clientMethod = controllerType is null
                ? invocation.Method
                : GetMethodImpl(interfaceType, controllerType, invocation.Method);
            var clientMethodArguments = invocation.Arguments;

            return new FullMethodInvocation(clientType, clientMethod, clientMethodArguments, resultType)
            {
                ResiliencePolicyProvider = resiliencePolicyProvider
            };
        }

        private static MethodInfo GetMethodImpl(Type interfaceType, Type implType, MethodInfo interfaceMethod)
        {
            var interfaceMapping = implType.GetInterfaceMap(interfaceType);
            var methodPairs = interfaceMapping.InterfaceMethods
                .Zip(interfaceMapping.TargetMethods, (x, y) => (First: x, Second: y));
            return methodPairs.SingleOrDefault(x => x.First == interfaceMethod).Second;
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
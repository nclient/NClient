using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Castle.DynamicProxy;
using NClient.Abstractions.Clients;
using NClient.Abstractions.Resilience;
using NClient.Core.Exceptions.Factories;

namespace NClient.Core.Interceptors.ClientInvocations
{
    public interface IClientInvocationProvider
    {
        ClientInvocation Get(Type interfaceType, Type? controllerType, IInvocation invocation);
    }

    public class ClientInvocationProvider : IClientInvocationProvider
    {
        private readonly IProxyGenerator _proxyGenerator;

        public ClientInvocationProvider(IProxyGenerator proxyGenerator)
        {
            _proxyGenerator = proxyGenerator;
        }

        public ClientInvocation Get(Type interfaceType, Type? controllerType, IInvocation invocation)
        {
            if (!IsNClientMethod(invocation.Method))
                return BuildInvocation(interfaceType, controllerType, invocation, resiliencePolicyProvider: null);

            var clientMethodInvocation = invocation.Arguments[0];
            if (invocation.Arguments[0] is null)
                throw InnerExceptionFactory.NullArgument(invocation.Method.GetParameters()[0].Name);

            var keepDataInterceptor = new KeepDataInterceptor();
            var proxyClient = _proxyGenerator.CreateInterfaceProxyWithoutTarget(interfaceType, keepDataInterceptor);
            ((LambdaExpression)clientMethodInvocation).Compile().DynamicInvoke(proxyClient);

            var innerInvocation = keepDataInterceptor.Invocation!;
            var resiliencePolicyProvider = (IResiliencePolicyProvider?)invocation.Arguments[1];

            return BuildInvocation(interfaceType, controllerType, innerInvocation, resiliencePolicyProvider);
        }

        private static ClientInvocation BuildInvocation(
            Type interfaceType, Type? controllerType, IInvocation invocation, IResiliencePolicyProvider? resiliencePolicyProvider)
        {
            var clientType = controllerType ?? interfaceType;
            var clientMethod = controllerType is null
                ? invocation.Method
                : GetMethodImpl(interfaceType, controllerType, invocation.Method);
            var clientMethodArguments = invocation.Arguments;

            return new ClientInvocation(clientType, clientMethod, clientMethodArguments)
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
            
            var nclientDeclaringTypeWithParam = nclientMethodInfo.DeclaringType!.MakeGenericType(methodInfo.DeclaringType.GetGenericArguments());
            var methodDeclaringTypeWithParam = methodInfo.DeclaringType;
            if (nclientDeclaringTypeWithParam != methodDeclaringTypeWithParam)
                return false;

            return nclientMethodInfo.Name == methodInfo.Name;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using NClient.Annotations;

namespace NClient.Core.Extensions
{
    internal static class MethodCancellationTokenExtensions
    {
        public static IEnumerable<object> GetArgumentsExceptNClientCancellationToken(this MethodInfo methodInfo, IEnumerable<object> arguments)
        {
            return methodInfo.HasNClientCancellationToken()
                ? arguments.Take(methodInfo.GetParameters().Length - 1)
                : arguments;
        }
        public static IEnumerable<ParameterInfo> GetParametersExceptNClientCancellationToken(this MethodInfo methodInfo)
        {
            return methodInfo.HasNClientCancellationToken()
                ? methodInfo.GetParameters().Take(methodInfo.GetParameters().Length - 1)
                : methodInfo.GetParameters();
        }
        
        public static CancellationToken GetNClientCancellationToken(this MethodInfo methodInfo, IEnumerable<object> arguments)
        {
            return FindNClientCancellationToken(methodInfo, arguments) 
                ?? throw new ArgumentException("The arguments do not contain a NClient cancellation token.");
        }
        
        public static CancellationToken? FindNClientCancellationToken(this MethodInfo methodInfo, IEnumerable<object> arguments)
        {
            if (methodInfo.HasNClientCancellationToken())
                return (CancellationToken) arguments.Last();
            return null;
        }
        
        public static bool HasNClientCancellationToken(this MethodInfo methodInfo)
        {
            var hasLastArgumentParamAttribute = methodInfo.GetParameters().LastOrDefault()?
                .GetCustomAttributes().Any(x => x is IParamAttribute) ?? false;
            return HasCancellationToken(methodInfo) && !hasLastArgumentParamAttribute;
        }
        
        public static bool HasCancellationToken(this MethodInfo methodInfo)
        {
            return methodInfo.GetParameters().LastOrDefault()?.ParameterType == typeof(CancellationToken);
        }
    }
}

using System;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>
    /// Encapsulates an information about invocation of a client method.
    /// </summary>
    public class MethodInvocation : IMethodInvocation
    {
        /// <summary>
        /// Gets type of client interface.
        /// </summary>
        public Type ClientType { get; }

        /// <summary>
        /// Gets information about the client method. 
        /// </summary>
        public MethodInfo MethodInfo { get; }

        /// <summary>
        /// Gets values of the arguments in the client method call. 
        /// </summary>
        public object[] MethodArguments { get; }

        /// <summary>
        /// Gets type returned by the client method.
        /// </summary>
        public Type ResultType { get; }

        public MethodInvocation(Type clientType, MethodInfo methodInfo, object[] methodArguments, Type resultType)
        {
            ClientType = clientType;
            MethodInfo = methodInfo;
            MethodArguments = methodArguments;
            ResultType = resultType;
        }
    }
}

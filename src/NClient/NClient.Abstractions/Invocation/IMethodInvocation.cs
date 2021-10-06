using System;
using System.Reflection;

namespace NClient.Abstractions.Invocation
{
    public interface IMethodInvocation
    {
        /// <summary>
        /// Gets type of client interface.
        /// </summary>
        Type ClientType { get; }
        /// <summary>
        /// Gets information about the client method. 
        /// </summary>
        MethodInfo MethodInfo { get; }
        /// <summary>
        /// Gets values of the arguments in the client method call. 
        /// </summary>
        object[] MethodArguments { get; }
        /// <summary>
        /// Gets type returned by the client method.
        /// </summary>
        Type ResultType { get; }
    }
}

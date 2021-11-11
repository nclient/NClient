using NClient.Invocation;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>
    /// Encapsulates an information about invocation of a client method.
    /// </summary>
    public class MethodInvocation : IMethodInvocation
    {
        /// <summary>
        /// Gets method information.
        /// </summary>
        public IMethod Method { get; }

        /// <summary>
        /// Gets values of the arguments in the client method call. 
        /// </summary>
        public object[] Arguments { get; }

        public MethodInvocation(IMethod method, object[] methodArguments)
        {
            Method = method;
            Arguments = methodArguments;
        }
    }
}

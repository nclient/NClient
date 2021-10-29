using NClient.Invocation;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface IMethodInvocation
    {
        /// <summary>
        /// Gets method information.
        /// </summary>
        public IMethod Method { get; }

        /// <summary>
        /// Gets values of the arguments in the client method call. 
        /// </summary>
        object[] Arguments { get; }
    }
}

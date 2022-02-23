using NClient.Invocation;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>Information about the invocation of the client's method.</summary>
    public interface IMethodInvocation
    {
        /// <summary>Gets method information.</summary>
        public IMethod Method { get; }

        /// <summary>Gets values of the arguments in the client method call.</summary>
        object[] Arguments { get; }
    }
}

using System.Collections.Generic;
using System.Linq;
using NClient.Invocation;

// ReSharper disable once CheckNamespace
namespace NClient
{
    internal class MethodInvocation : IMethodInvocation
    {
        public IMethod Method { get; } 
        public object[] Arguments { get; }

        public MethodInvocation(IMethod method, IEnumerable<object> methodArguments)
        {
            Method = method;
            Arguments = methodArguments.ToArray();
        }
    }
}

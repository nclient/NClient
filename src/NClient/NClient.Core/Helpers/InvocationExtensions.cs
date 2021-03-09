using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace NClient.Core.Helpers
{
    internal static class InvocationExtensions
    {
        public static bool IsReturnTypeNotTask(this IInvocation invocation)
        {
            return !IsReturnTypeTask(invocation) && !IsReturnTypeGenericTask(invocation);
        }

        public static bool IsReturnTypeTask(this IInvocation invocation)
        {
            return invocation.Method.ReturnType == typeof(Task);
        }

        public static bool IsReturnTypeGenericTask(this IInvocation invocation)
        {
            return invocation.Method.ReturnType.IsGenericType && invocation.Method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);
        }
    }
}

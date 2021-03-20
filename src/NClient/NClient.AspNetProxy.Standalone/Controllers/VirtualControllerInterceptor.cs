using System.Linq;
using Castle.DynamicProxy;

namespace NClient.AspNetProxy.Controllers
{
    internal class VirtualControllerInterceptor : IInterceptor
    {
        private readonly object _target;

        public VirtualControllerInterceptor(object target)
        {
            _target = target;
        }

        public void Intercept(IInvocation invocation)
        {
            var methodName = invocation.Method.Name;
            var methodParamTypes = invocation.Method.GetParameters().Select(x => x.ParameterType).ToArray();

            invocation.ReturnValue = _target.GetType()
                .GetMethod(methodName, methodParamTypes)!
                .Invoke(_target, invocation.Arguments);
        }
    }
}

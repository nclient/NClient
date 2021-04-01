using System.Linq;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc;

namespace NClient.AspNetCore.Controllers
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
            if (_target is ControllerBase controllerBase)
            {
                var proxyController = invocation.InvocationTarget;
                controllerBase.ControllerContext = (ControllerContext)proxyController
                    .GetType().GetProperty(nameof(controllerBase.ControllerContext))!.GetValue(proxyController);
            }

            var methodName = invocation.Method.Name;
            var methodParamTypes = invocation.Method.GetParameters().Select(x => x.ParameterType).ToArray();

            invocation.ReturnValue = _target.GetType()
                .GetMethod(methodName, methodParamTypes)!
                .Invoke(_target, invocation.Arguments);
        }
    }
}

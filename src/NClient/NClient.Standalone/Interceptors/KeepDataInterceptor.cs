using System;
using Castle.DynamicProxy;

namespace NClient.Standalone.Interceptors
{
    internal class KeepDataInterceptor : IInterceptor
    {
        public IInvocation? Invocation { get; private set; }

        public void Intercept(IInvocation invocation)
        {
            Invocation = invocation;
            var concreteMethod = invocation.GetConcreteMethod();

            if (concreteMethod.ReturnType.IsValueType && concreteMethod.ReturnType != typeof(void))
                invocation.ReturnValue = Activator.CreateInstance(concreteMethod.ReturnType);
        }
    }
}

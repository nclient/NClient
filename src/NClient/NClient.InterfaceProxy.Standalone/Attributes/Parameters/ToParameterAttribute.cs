using System;

namespace NClient.InterfaceProxy.Attributes.Parameters
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public abstract class ToParameterAttribute : Attribute
    {
    }
}

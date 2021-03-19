using System;

namespace NClient.Core.Attributes.Clients.Parameters
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public abstract class ToParameterAttribute : Attribute
    {
    }
}

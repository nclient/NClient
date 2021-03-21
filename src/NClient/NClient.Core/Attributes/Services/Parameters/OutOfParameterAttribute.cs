using System;

namespace NClient.Core.Attributes.Services.Parameters
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public abstract class OutOfParameterAttribute : Attribute
    {
    }
}

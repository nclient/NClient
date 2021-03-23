using System;

namespace NClient.Core.Attributes.Parameters
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public abstract class ParamAttribute : Attribute
    {
    }
}

using System;

namespace NClient.Annotations.Parameters
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public abstract class ParamAttribute : Attribute
    {
    }
}

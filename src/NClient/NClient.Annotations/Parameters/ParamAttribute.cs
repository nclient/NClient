using System;

// ReSharper disable once CheckNamespace
namespace NClient.Annotations
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public abstract class ParamAttribute : Attribute, IParamAttribute
    {
    }
}

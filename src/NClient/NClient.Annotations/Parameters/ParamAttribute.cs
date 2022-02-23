using System;

// ReSharper disable once CheckNamespace
namespace NClient.Annotations
{
    /// <summary>Specifies that a parameter should be pass data in an transport message.</summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public abstract class ParamAttribute : Attribute, IParamAttribute
    {
    }
}

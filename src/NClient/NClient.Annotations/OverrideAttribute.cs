using System;

namespace NClient.Annotations
{
    /// <summary>
    /// An attribute indicating that the attributes of the overridden member will be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class OverrideAttribute : Attribute
    {
    }
}

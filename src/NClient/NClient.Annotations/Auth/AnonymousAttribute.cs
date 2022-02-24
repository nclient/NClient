using System;

namespace NClient.Annotations.Auth
{
    /// <summary>Specifies that the class or method that this attribute is applied to does not require authorization.</summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AnonymousAttribute : Attribute, IAnonymousAttribute
    {
    }
}

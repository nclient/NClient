using System;

namespace NClient.Annotations.Auth
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AnonymousAttribute : Attribute
    {

    }
}
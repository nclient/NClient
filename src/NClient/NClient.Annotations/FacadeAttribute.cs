using System;

namespace NClient.Annotations
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class FacadeAttribute : Attribute
    {
    }
}

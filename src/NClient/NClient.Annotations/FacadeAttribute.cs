using System;

namespace NClient.Annotations
{
    /// <summary>
    /// Indicates that a type and all derived types are used to serve HTTP API responses or/and to send HTTP requests.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class FacadeAttribute : Attribute
    {
    }
}

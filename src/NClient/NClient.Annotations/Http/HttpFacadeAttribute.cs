using System;

namespace NClient.Annotations.Http
{
    /// <summary>
    /// Indicates that a type and all derived types are used to serve HTTP API responses.
    /// Controllers that implement an interface with this attribute will inherit from <see cref="T:Microsoft.AspNetCore.Mvc.ControllerBase"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class HttpFacadeAttribute : FacadeAttribute
    {
    }
}

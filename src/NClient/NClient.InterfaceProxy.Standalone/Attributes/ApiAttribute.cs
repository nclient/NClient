using System;

namespace NClient.InterfaceProxy.Attributes
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ApiAttribute : Attribute
    {
        public string? Template { get; }

        public ApiAttribute(string? template = null)
        {
            Template = template;
        }
    }
}

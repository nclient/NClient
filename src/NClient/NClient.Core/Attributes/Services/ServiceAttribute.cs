using System;

namespace NClient.Core.Attributes.Services
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ServiceAttribute : Attribute
    {
        public int Order { get; set; }
        public string? Template { get; }

        public ServiceAttribute(string? template = null)
        {
            Template = template;
        }
    }
}

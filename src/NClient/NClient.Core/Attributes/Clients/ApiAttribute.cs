using System;

namespace NClient.Core.Attributes.Clients
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ApiAttribute : Attribute
    {
        public int Order { get; set; }
        public string? Template { get; }

        public ApiAttribute(string? template = null)
        {
            Template = template;
        }
    }
}

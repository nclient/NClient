using System;

namespace NClient.Core.Attributes.Clients
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ClientAttribute : Attribute
    {
        public int Order { get; set; }
        public string? Template { get; }

        public ClientAttribute(string? template = null)
        {
            Template = template;
        }
    }
}

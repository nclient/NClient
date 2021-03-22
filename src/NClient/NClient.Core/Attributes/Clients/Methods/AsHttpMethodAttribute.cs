using System;

namespace NClient.Core.Attributes.Clients.Methods
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class AsHttpMethodAttribute : Attribute
    {
        public int Order { get; set; }
        public string? Template { get; }

        protected AsHttpMethodAttribute(string? template)
        {
            Template = template;
        }
    }
}

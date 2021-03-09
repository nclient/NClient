using System;

namespace NClient.InterfaceProxy.Attributes.Methods
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class AsHttpMethodAttribute : Attribute
    {
        public string? Template { get; }

        protected AsHttpMethodAttribute(string? template)
        {
            Template = template;
        }
    }
}

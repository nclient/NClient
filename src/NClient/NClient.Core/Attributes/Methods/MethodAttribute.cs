using System;

namespace NClient.Core.Attributes.Methods
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class MethodAttribute : Attribute
    {
        public int Order { get; set; }
        public string? Template { get; }

        protected MethodAttribute(string? template)
        {
            Template = template;
        }
    }
}

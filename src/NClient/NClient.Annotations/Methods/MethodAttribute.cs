using System;

namespace NClient.Annotations.Methods
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class MethodAttribute : Attribute
    {
        public string? Name { get; set; }
        public int Order { get; set; }
        public string? Template { get; }

        protected MethodAttribute(string? template)
        {
            Template = template;
        }
    }
}

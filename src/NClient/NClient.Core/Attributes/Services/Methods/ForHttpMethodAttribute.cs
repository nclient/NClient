using System;

namespace NClient.Core.Attributes.Services.Methods
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class ForHttpMethodAttribute : Attribute
    {
        public int Order { get; set; }
        public string? Template { get; }

        protected ForHttpMethodAttribute(string? template)
        {
            Template = template;
        }
    }
}

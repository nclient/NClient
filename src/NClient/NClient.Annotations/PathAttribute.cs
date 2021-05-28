using System;
using NClient.Common.Helpers;

namespace NClient.Annotations
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class PathAttribute : Attribute
    {
        public string? Name { get; set; }
        public int Order { get; set; }
        public string Template { get; }

        public PathAttribute(string template)
        {
            Ensure.IsNotNullOrEmpty(template, nameof(template));
            
            Template = template;
        }
    }
}

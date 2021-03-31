using System;

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
            Template = template ?? throw new ArgumentNullException(nameof(template));
        }
    }
}

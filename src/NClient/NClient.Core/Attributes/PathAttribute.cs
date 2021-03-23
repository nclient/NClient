using System;

namespace NClient.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class PathAttribute : Attribute
    {
        public int Order { get; set; }
        public string Template { get; }

        public PathAttribute(string template)
        {
            Template = template;
        }
    }
}

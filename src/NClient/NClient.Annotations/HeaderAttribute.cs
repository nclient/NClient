using System;

namespace NClient.Annotations
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class HeaderAttribute : Attribute
    {
        public string Name { get; }
        public string Value { get; }

        public HeaderAttribute(string name, string value)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
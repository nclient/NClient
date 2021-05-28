using System;
using NClient.Common.Helpers;

namespace NClient.Annotations
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class HeaderAttribute : Attribute
    {
        public string Name { get; }
        public string Value { get; }

        public HeaderAttribute(string name, string value)
        {
            Ensure.IsNotNullOrEmpty(name, nameof(name));
            Ensure.IsNotNullOrEmpty(value, nameof(value));

            Name = name;
            Value = value;
        }
    }
}
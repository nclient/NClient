using System;
using NClient.Common.Helpers;

namespace NClient.Annotations
{
    /// <summary>
    /// Specifies that a method/methods should be use the request header.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class HeaderAttribute : Attribute
    {
        /// <summary>
        /// Gets header name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets header value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Creates a new <see cref="HeaderAttribute"/> with the given header.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header value.</param>
        public HeaderAttribute(string name, string value)
        {
            Ensure.IsNotNullOrEmpty(name, nameof(name));
            Ensure.IsNotNullOrEmpty(value, nameof(value));

            Name = name;
            Value = value;
        }
    }
}

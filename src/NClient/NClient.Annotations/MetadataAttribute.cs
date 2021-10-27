using System;
using NClient.Attributes;
using NClient.Common.Helpers;

namespace NClient.Annotations
{
    /// <summary>
    /// Specifies that a method/methods should be use the request metadata.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class MetadataAttribute : Attribute, IMetadataAttribute
    {
        /// <summary>
        /// Gets metadata name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets metadata value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Creates a new <see cref="MetadataAttribute"/> with the given metadata.
        /// </summary>
        /// <param name="name">The metadata name.</param>
        /// <param name="value">The metadata value.</param>
        public MetadataAttribute(string name, string value)
        {
            Ensure.IsNotNullOrEmpty(name, nameof(name));
            Ensure.IsNotNullOrEmpty(value, nameof(value));

            Name = name;
            Value = value;
        }
    }
}

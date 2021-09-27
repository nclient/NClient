using System;
using NClient.Common.Helpers;

namespace NClient.Annotations.Versioning
{
    /// <summary>
    /// Represents the metadata that describes the API version-specific implementation of a service.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ToVersionAttribute : Attribute
    {
        /// <summary>
        /// Gets the API version defined by the attribute.
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToVersionAttribute"/> class.
        /// </summary>
        /// <param name="version">The API version.</param>
        public ToVersionAttribute(string version)
        {
            Ensure.IsNotNullOrEmpty(version, nameof(version));

            Version = version;
        }
    }
}

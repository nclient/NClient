using System;
using NClient.Common.Helpers;

namespace NClient.Annotations.Versioning
{
    /// <summary>
    /// Represents the metadata that describes the API version associated with a service.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public class VersionAttribute : Attribute
    {
        /// <summary>
        /// Gets the API version defined by the attribute.
        /// </summary>
        public string Version { get; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the specified set of API versions are deprecated.
        /// </summary>
        /// <value>True if the specified set of API versions are deprecated; otherwise, false.
        /// The default value is <c>false</c>.</value>
        public bool Deprecated { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionAttribute" /> class.
        /// </summary>
        /// <param name="version">The API version.</param>
        public VersionAttribute(string version)
        {
            Ensure.IsNotNullOrEmpty(version, nameof(version));

            Version = version;
        }
    }
}
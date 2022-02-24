using System;
using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Annotations
{
    /// <summary>Represents the metadata that describes the API version associated used by the client.</summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class UseVersionAttribute : Attribute, IUseVersionAttribute
    {
        /// <summary>Gets the API version defined by the attribute.</summary>
        public string Version { get; }

        /// <summary>Initializes a new instance of the <see cref="UseVersionAttribute"/> class.</summary>
        /// <param name="version">The API version.</param>
        public UseVersionAttribute(string version)
        {
            Ensure.IsNotNullOrEmpty(version, nameof(version));

            Version = version;
        }
    }
}

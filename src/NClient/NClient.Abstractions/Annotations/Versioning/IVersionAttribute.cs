// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    public interface IVersionAttribute
    {
        /// <summary>
        /// Gets the API version defined by the attribute.
        /// </summary>
        string Version { get; }
        /// <summary>
        /// Gets or sets a value indicating whether the specified set of API versions are deprecated.
        /// </summary>
        /// <value>True if the specified set of API versions are deprecated; otherwise, false.
        /// The default value is <c>false</c>.</value>
        bool Deprecated { get; set; }
    }
}

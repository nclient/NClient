using System;

namespace NClient.Annotations.Http
{
    /// <summary>
    /// Specifies that a method/methods should be use the request header.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class HeaderAttribute : MetadataAttribute, IHeaderAttribute
    {
        /// <summary>
        /// Creates a new <see cref="HeaderAttribute"/> with the given header.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header value.</param>
        public HeaderAttribute(string name, string value) : base(name, value)
        {
        }
    }
}

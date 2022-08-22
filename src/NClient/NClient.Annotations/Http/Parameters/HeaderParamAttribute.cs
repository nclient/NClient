// ReSharper disable once CheckNamespace

namespace NClient.Annotations.Http
{
    /// <summary>Specifies that a parameter should be bound using the request headers.</summary>
    public class HeaderParamAttribute : MetadataParamAttribute, IHeaderParamAttribute
    {
        /// <summary>Initializes a new <see cref="HeaderParamAttribute"/>.</summary>
        public HeaderParamAttribute()
        {
        }

        /// <summary>Initializes a new <see cref="HeaderParamAttribute"/> with the given header parameter.</summary>
        /// <param name="name">The header parameter name.</param>
        public HeaderParamAttribute(string name) : base(name)
        {
        }
    }
}

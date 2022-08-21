// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    /// <summary>Specifies that a parameter should be pass an additional information in an transport message. Many parameters are allowed.</summary>
    public class MetadataParamAttribute : ParamAttribute, IMetadataParamAttribute
    {
        /// <summary>Gets or sets parameter name.</summary>
        public string? Name { get; set; }

        /// <summary>Initializes a new <see cref="MetadataParamAttribute"/>.</summary>
        public MetadataParamAttribute()
        {
        }

        /// <summary>Initializes a new <see cref="MetadataParamAttribute"/> with the given metadata parameter.</summary>
        /// <param name="name">The metadata parameter name.</param>
        public MetadataParamAttribute(string name)
        {
            Name = name;
        }
    }
}

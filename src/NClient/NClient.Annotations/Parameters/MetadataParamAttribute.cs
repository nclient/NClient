// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    public class MetadataParamAttribute : ParamAttribute, IMetadataParamAttribute
    {
        /// <summary>
        /// Metadata name.
        /// </summary>
        public string? Name { get; set; }
    }
}

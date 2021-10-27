using NClient.Annotations;

namespace NClient.Attributes
{
    public interface IMetadataAttribute : INameProviderAttribute
    {
        /// <summary>
        /// Gets metadata value.
        /// </summary>
        string Value { get; }
    }
}

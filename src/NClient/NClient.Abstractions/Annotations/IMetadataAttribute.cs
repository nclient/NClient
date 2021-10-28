namespace NClient.Annotations
{
    public interface IMetadataAttribute : INameProviderAttribute
    {
        /// <summary>
        /// Gets metadata value.
        /// </summary>
        string Value { get; }
        // TODO: doc
        new string Name { get; }
    }
}

namespace NClient.Annotations
{
    public interface IMetadataAttribute : INameProviderAttribute
    {
        /// <summary>Gets metadata value.</summary>
        string Value { get; }

        new string Name { get; }
    }
}

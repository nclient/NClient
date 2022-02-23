namespace NClient.Annotations
{
    /// <summary>Specifies that a method/methods should be use the request metadata.</summary>
    public interface IMetadataAttribute : INameProviderAttribute
    {
        /// <summary>Gets metadata value.</summary>
        string Value { get; }

        /// <summary>Gets metadata value.</summary>
        new string Name { get; }
    }
}

// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    /// <summary>Specifies that a parameter should be pass an additional information in an transport message. Many parameters are allowed.</summary>
    public interface IMetadataParamAttribute : IParamAttribute, INameProviderAttribute
    {
    }
}

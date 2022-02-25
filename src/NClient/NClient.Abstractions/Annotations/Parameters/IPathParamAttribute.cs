// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    /// <summary>Specifies that a parameter should be pass data in an transport message path. Many parameters are allowed.</summary>
    public interface IPathParamAttribute : IParamAttribute, INameProviderAttribute
    {
    }
}

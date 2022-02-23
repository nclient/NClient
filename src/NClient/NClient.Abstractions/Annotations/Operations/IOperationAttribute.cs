// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    /// <summary>Identifies an action that supports a given set of operations.</summary>
    public interface IOperationAttribute : INameProviderAttribute, IPathProviderAttribute
    {
    }
}

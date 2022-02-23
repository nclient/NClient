// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    /// <summary>Identifies an action that supports UPDATE operation.
    /// The UPDATE operation replaces all current representations of the target resource with the request payload.</summary>
    public interface IUpdateOperationAttribute : IOperationAttribute
    {
    }
}

// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    /// <summary>Identifies an action that supports READ operation.
    /// The READ operation requests a representation of the specified resource. Requests using READ should only retrieve data.</summary>
    public interface IReadOperationAttribute : IOperationAttribute
    {
    }
}

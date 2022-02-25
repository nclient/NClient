// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    /// <summary>Identifies an action that supports CREATE operation.
    /// The CREATE operation submit an entity to the specified resource, often causing a change in state or side effects on the server.</summary>
    public interface ICreateOperationAttribute : IOperationAttribute
    {
    }
}

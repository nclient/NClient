// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    /// <summary>Identifies an action that supports CHECK operation.
    /// The CHECK operation asks for a response identical to a READ operation, but without the response content.</summary>
    public interface ICheckOperationAttribute : IOperationAttribute
    {
    }
}

// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    /// <summary>Identifies an action that supports UPDATE operation.
    /// The UPDATE operation replaces all current representations of the target resource with the request payload.</summary>
    public class UpdateOperationAttribute : OperationAttribute, IUpdateOperationAttribute
    {
        /// <summary>Initializes a new <see cref="UpdateOperationAttribute"/> with the given path template.</summary>
        /// <param name="path">The path template.</param>
        public UpdateOperationAttribute(string? path = null) : base(path)
        {
        }
    }
}

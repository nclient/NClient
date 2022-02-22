// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    /// <summary>Identifies an action that supports DELETE operation.
    /// The DELETE operation deletes the specified resource.</summary>
    public class DeleteOperationAttribute : OperationAttribute, IDeleteOperationAttribute
    {
        /// <summary>Initializes a new <see cref="DeleteOperationAttribute"/> with the given path template.</summary>
        /// <param name="path">The path template.</param>
        public DeleteOperationAttribute(string? path = null) : base(path)
        {
        }
    }
}

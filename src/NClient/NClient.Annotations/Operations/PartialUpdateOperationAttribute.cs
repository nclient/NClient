// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    /// <summary>Identifies an action that supports PartialUpdate operation.
    /// The PartialUpdate operation applies partial modifications to a resource.</summary>
    public class PartialUpdateOperationAttribute : OperationAttribute, IPartialUpdateOperationAttribute
    {
        /// <summary>Initializes a new <see cref="PartialUpdateOperationAttribute"/> with the given path template.</summary>
        /// <param name="path">The path template.</param>
        public PartialUpdateOperationAttribute(string? path = null) : base(path)
        {
        }
    }
}

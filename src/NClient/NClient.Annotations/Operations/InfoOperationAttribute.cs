// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    /// <summary>Identifies an action that supports INFO operation.
    /// The INFO operation describes the communication options for the target resource.</summary>
    public class InfoOperationAttribute : OperationAttribute, IInfoOperationAttribute
    {
        /// <summary>Initializes a new <see cref="InfoOperationAttribute"/> with the given path template.</summary>
        /// <param name="path">The path template.</param>
        public InfoOperationAttribute(string? path = null) : base(path)
        {
        }
    }
}

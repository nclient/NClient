// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    /// <summary>Identifies an action that supports custom operation.</summary>
    public class CustomOperationAttribute : OperationAttribute, ICustomOperationAttribute
    {
        /// <summary>Initializes a new <see cref="CustomOperationAttribute"/> with the given path template.</summary>
        /// <param name="path">The path template.</param>
        public CustomOperationAttribute(string? path = null) : base(path)
        {
        }
    }
}

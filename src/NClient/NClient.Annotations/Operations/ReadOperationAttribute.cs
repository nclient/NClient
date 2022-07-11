// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    /// <summary>Identifies an action that supports READ operation.
    /// The READ operation requests a representation of the specified resource. Requests using READ should only retrieve data.</summary>
    public class ReadOperationAttribute : OperationAttribute, IReadOperationAttribute
    {
        /// <summary>Initializes a new <see cref="ReadOperationAttribute"/> with the given path template.</summary>
        /// <param name="path">The path template.</param>
        public ReadOperationAttribute(string? path = null) : base(path)
        {
        }
    }
}

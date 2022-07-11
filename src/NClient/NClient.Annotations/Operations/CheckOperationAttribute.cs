// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    /// <summary>Identifies an action that supports CHECK operation.
    /// The CHECK operation asks for a response identical to a READ operation, but without the response content.</summary>
    public class CheckOperationAttribute : OperationAttribute, ICheckOperationAttribute
    {
        /// <summary>Initializes a new <see cref="CheckOperationAttribute"/> with the given path template.</summary>
        /// <param name="path">The path template.</param>
        public CheckOperationAttribute(string? path = null) : base(path)
        {
        }
    }
}

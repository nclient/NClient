// ReSharper disable once CheckNamespace

namespace NClient.Annotations.Http
{
    /// <summary>
    /// Identifies an action that supports the HTTP PUT method.
    /// </summary>
    public class PutMethodAttribute : UpdateOperationAttribute, IPutMethodAttribute
    {
        public int Order { get; set; }
        
        /// <summary>
        /// Creates a new <see cref="PutMethodAttribute"/> with the given path template.
        /// </summary>
        /// <param name="path">The path template.</param>
        public PutMethodAttribute(string? path = null) : base(path)
        {
        }
    }
}

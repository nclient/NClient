// ReSharper disable once CheckNamespace

namespace NClient.Annotations.Http
{
    /// <summary>
    /// Identifies an action that supports the HTTP DELETE method.
    /// </summary>
    public class DeleteMethodAttribute : DeleteOperationAttribute, IOrderProviderAttribute
    {
        public int Order { get; set; }
        
        /// <summary>
        /// Creates a new <see cref="DeleteMethodAttribute" /> with the given path template.
        /// </summary>
        /// <param name="path">The path template.</param>
        public DeleteMethodAttribute(string? path = null) : base(path)
        {
        }
    }
}

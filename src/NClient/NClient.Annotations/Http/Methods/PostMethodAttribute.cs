// ReSharper disable once CheckNamespace

namespace NClient.Annotations.Http
{
    /// <summary>
    /// Identifies an action that supports the HTTP POST method.
    /// </summary>
    public class PostMethodAttribute : CreateOperationAttribute, IPostMethodAttribute
    {
        public int Order { get; set; }
        
        /// <summary>
        /// Creates a new <see cref="PostMethodAttribute"/> with the given path template.
        /// </summary>
        /// <param name="path">The path template.</param>
        public PostMethodAttribute(string? path = null) : base(path)
        {
        }
    }
}

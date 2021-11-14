// ReSharper disable once CheckNamespace

namespace NClient.Annotations.Http
{
    /// <summary>
    /// Identifies an action that supports the HTTP HEAD method.
    /// </summary>
    public class HeadMethodAttribute : CheckOperationAttribute, IHeadMethodAttribute
    {
        public int Order { get; set; }
        
        /// <summary>
        /// Creates a new <see cref="HeadMethodAttribute"/> with the given path template.
        /// </summary>
        /// <param name="path">The path template.</param>
        public HeadMethodAttribute(string? path = null) : base(path)
        {
        }
    }
}

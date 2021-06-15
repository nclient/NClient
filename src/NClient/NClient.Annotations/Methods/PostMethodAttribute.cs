namespace NClient.Annotations.Methods
{
    /// <summary>
    /// Identifies an action that supports the HTTP POST method.
    /// </summary>
    public class PostMethodAttribute : MethodAttribute
    {
        /// <summary>
        /// Creates a new <see cref="PostMethodAttribute"/> with the given route template.
        /// </summary>
        /// <param name="template">The route template.</param>
        public PostMethodAttribute(string? template = null) : base(template)
        {
        }
    }
}

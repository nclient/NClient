namespace NClient.Annotations.Methods
{
    /// <summary>
    /// Identifies an action that supports the HTTP DELETE method.
    /// </summary>
    public class DeleteMethodAttribute : MethodAttribute
    {
        /// <summary>
        /// Creates a new <see cref="DeleteMethodAttribute" /> with the given route template.
        /// </summary>
        /// <param name="template">The route template.</param>
        public DeleteMethodAttribute(string? template = null) : base(template)
        {
        }
    }
}

namespace NClient.Annotations.Methods
{
    /// <summary>
    /// Identifies an action that supports the HTTP PUT method.
    /// </summary>
    public class PutMethodAttribute : MethodAttribute
    {
        /// <summary>
        /// Creates a new <see cref="PutMethodAttribute"/> with the given route template.
        /// </summary>
        /// <param name="template">The route template.</param>
        public PutMethodAttribute(string? template = null) : base(template)
        {
        }
    }
}

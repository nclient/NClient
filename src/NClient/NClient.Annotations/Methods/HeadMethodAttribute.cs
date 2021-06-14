namespace NClient.Annotations.Methods
{
    /// <summary>
    /// Identifies an action that supports the HTTP HEAD method.
    /// </summary>
    public class HeadMethodAttribute : MethodAttribute
    {
        /// <summary>
        /// Creates a new <see cref="HeadMethodAttribute"/> with the given route template.
        /// </summary>
        /// <param name="template">The route template.</param>
        public HeadMethodAttribute(string? template = null) : base(template)
        {
        }
    }
}

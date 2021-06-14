namespace NClient.Annotations.Methods
{
    /// <summary>
    /// Identifies an action that supports the HTTP GET method.
    /// </summary>
    public class GetMethodAttribute : MethodAttribute
    {
        /// <summary>
        /// Creates a new <see cref="GetMethodAttribute"/> with the given route template.
        /// </summary>
        /// <param name="template">The route template.</param>
        public GetMethodAttribute(string? template = null) : base(template)
        {
        }
    }
}

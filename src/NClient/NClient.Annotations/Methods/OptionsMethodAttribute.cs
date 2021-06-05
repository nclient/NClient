namespace NClient.Annotations.Methods
{
    /// <summary>
    /// Identifies an action that supports the HTTP OPTIONS method.
    /// </summary>
    public class OptionsMethodAttribute : MethodAttribute
    {
        /// <summary>
        /// Creates a new <see cref="OptionsMethodAttribute"/> with the given route template.
        /// </summary>
        /// <param name="template">The route template.</param>
        public OptionsMethodAttribute(string? template = null) : base(template)
        {
        }
    }
}

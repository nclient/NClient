using NClient.Annotations.Operations;

namespace NClient.Annotations.Methods
{
    /// <summary>
    /// Identifies an action that supports the HTTP OPTIONS method.
    /// </summary>
    public class OptionsMethodAttribute : InfoOperationAttribute, IHttpMethodAttribute
    {
        public string? Name { get; set; }
        public int Order { get; set; }
        public string? Template { get; set; }
        
        /// <summary>
        /// Creates a new <see cref="OptionsMethodAttribute"/> with the given route template.
        /// </summary>
        /// <param name="template">The route template.</param>
        public OptionsMethodAttribute(string? template = null)
        {
            Template = template;
        }
    }
}

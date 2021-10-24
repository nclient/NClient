using NClient.Annotations.Operations;

namespace NClient.Annotations.Methods
{
    /// <summary>
    /// Identifies an action that supports the HTTP GET method.
    /// </summary>
    public class GetMethodAttribute : ReadOperationAttribute, IHttpMethodAttribute
    {
        public string? Name { get; set; }
        public int Order { get; set; }
        public string? Template { get; set; }
        
        /// <summary>
        /// Creates a new <see cref="GetMethodAttribute"/> with the given route template.
        /// </summary>
        /// <param name="template">The route template.</param>
        public GetMethodAttribute(string? template = null)
        {
            Template = template;
        }
    }
}

using NClient.Annotations.Operations;

namespace NClient.Annotations.Methods
{
    /// <summary>
    /// Identifies an action that supports the HTTP DELETE method.
    /// </summary>
    public class DeleteMethodAttribute : DeleteOperationAttribute, IHttpMethodAttribute
    {
        public string? Name { get; set; }
        public int Order { get; set; }
        public string? Template { get; set; }
        
        /// <summary>
        /// Creates a new <see cref="DeleteMethodAttribute" /> with the given route template.
        /// </summary>
        /// <param name="template">The route template.</param>
        public DeleteMethodAttribute(string? template = null)
        {
            Template = template;
        }
    }
}

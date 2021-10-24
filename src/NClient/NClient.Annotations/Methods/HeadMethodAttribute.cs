using NClient.Annotations.Operations;

namespace NClient.Annotations.Methods
{
    /// <summary>
    /// Identifies an action that supports the HTTP HEAD method.
    /// </summary>
    public class HeadMethodAttribute : CheckOperationAttribute, IHttpMethodAttribute
    {
        public string? Name { get; set; }
        public int Order { get; set; }
        public string? Template { get; set; }
        
        /// <summary>
        /// Creates a new <see cref="HeadMethodAttribute"/> with the given route template.
        /// </summary>
        /// <param name="template">The route template.</param>
        public HeadMethodAttribute(string? template = null)
        {
            Template = template;
        }
    }
}

using NClient.Annotations.Operations;

namespace NClient.Annotations.Methods
{
    #if !NETSTANDARD2_0
    /// <summary>
    /// Identifies an action that supports the HTTP PATCH method.
    /// </summary>
    public class PatchMethodAttribute : PartialUpdateOperationAttribute, IHttpMethodAttribute
    {
        public string? Name { get; set; }
        public int Order { get; set; }
        public string? Template { get; set; }
        
        /// <summary>
        /// Creates a new <see cref="PatchMethodAttribute"/> with the given route template.
        /// </summary>
        /// <param name="template">The route template.</param>
        public PatchMethodAttribute(string? template = null)
        {
            Template = template;
        }
    }
    #endif
}

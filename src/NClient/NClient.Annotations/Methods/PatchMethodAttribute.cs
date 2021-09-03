namespace NClient.Annotations.Methods
{
    #if !NETSTANDARD2_0
    /// <summary>
    /// Identifies an action that supports the HTTP PATCH method.
    /// </summary>
    public class PatchMethodAttribute : MethodAttribute
    {
        /// <summary>
        /// Creates a new <see cref="PatchMethodAttribute"/> with the given route template.
        /// </summary>
        /// <param name="template">The route template.</param>
        public PatchMethodAttribute(string? template = null) : base(template)
        {
        }
    }
    #endif
}

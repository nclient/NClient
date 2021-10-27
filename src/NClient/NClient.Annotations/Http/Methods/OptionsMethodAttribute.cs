// ReSharper disable once CheckNamespace

namespace NClient.Annotations.Http
{
    /// <summary>
    /// Identifies an action that supports the HTTP OPTIONS method.
    /// </summary>
    public class OptionsMethodAttribute : InfoOperationAttribute, IOrderProviderAttribute
    {
        public int Order { get; set; }
        
        /// <summary>
        /// Creates a new <see cref="OptionsMethodAttribute"/> with the given path template.
        /// </summary>
        /// <param name="path">The path template.</param>
        public OptionsMethodAttribute(string? path = null) : base(path)
        {
        }
    }
}

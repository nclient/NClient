// ReSharper disable once CheckNamespace

namespace NClient.Annotations.Http
{
    /// <summary>Identifies an action that supports the HTTP OPTIONS method.</summary>
    public class OptionsMethodAttribute : InfoOperationAttribute, IOptionsMethodAttribute
    {
        public int Order { get; set; }
        
        /// <summary>Initializes a new <see cref="OptionsMethodAttribute"/> with the given path template.</summary>
        /// <param name="path">The path template.</param>
        public OptionsMethodAttribute(string? path = null) : base(path)
        {
        }
    }
}

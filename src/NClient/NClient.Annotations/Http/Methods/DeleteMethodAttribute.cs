// ReSharper disable once CheckNamespace

namespace NClient.Annotations.Http
{
    /// <summary>Identifies an action that supports the HTTP DELETE method.</summary>
    public class DeleteMethodAttribute : DeleteOperationAttribute, IDeleteMethodAttribute
    {
        public int Order { get; set; }
        
        /// <summary>Initializes a new <see cref="DeleteMethodAttribute" /> with the given path template.</summary>
        /// <param name="path">The path template.</param>
        public DeleteMethodAttribute(string? path = null) : base(path)
        {
        }
    }
}

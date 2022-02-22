// ReSharper disable once CheckNamespace
// ReSharper disable once EmptyNamespace

namespace NClient.Annotations.Http
{
    #if !NETSTANDARD2_0
    /// <summary>Identifies an action that supports the HTTP PATCH method.</summary>
    public class PatchMethodAttribute : PartialUpdateOperationAttribute, IPatchMethodAttribute
    {
        public int Order { get; set; }
        
        /// <summary>Initializes a new <see cref="PatchMethodAttribute"/> with the given path template.</summary>
        /// <param name="path">The path template.</param>
        public PatchMethodAttribute(string? path = null) : base(path)
        {
        }
    }
    #endif
}

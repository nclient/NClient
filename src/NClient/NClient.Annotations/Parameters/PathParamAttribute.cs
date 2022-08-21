// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    /// <summary>Specifies that a parameter should be pass data in an transport message path. Many parameters are allowed.</summary>
    public class PathParamAttribute : ParamAttribute, IPathParamAttribute
    {
        /// <summary>Gets or sets parameter name.</summary>
        public string? Name { get; set; }

        /// <summary>Initializes a new <see cref="PathParamAttribute"/>.</summary>
        public PathParamAttribute()
        {
        }

        /// <summary>Initializes a new <see cref="PathParamAttribute"/> with the given path parameter.</summary>
        /// <param name="name">The path parameter name.</param>
        public PathParamAttribute(string name)
        {
            Name = name;
        }
    }
}

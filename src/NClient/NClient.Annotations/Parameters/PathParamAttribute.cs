// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    public class PathParamAttribute : ParamAttribute, IPathParamAttribute
    {
        /// <summary>
        /// Path name.
        /// </summary>
        public string? Name { get; set; }
    }
}

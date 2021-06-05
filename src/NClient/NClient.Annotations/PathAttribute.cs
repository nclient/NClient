using System;
using NClient.Common.Helpers;

namespace NClient.Annotations
{
    /// <summary>
    /// Specifies an attribute route on a controller.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class PathAttribute : Attribute
    {
        /// <summary>
        /// Gets the route name. The route name can be used to generate a link using a specific route, instead
        ///  of relying on selection of a route based on the given set of route values.
        /// </summary>
        public string? Name { get; set; }
        
        /// <summary>
        /// Gets the route order. The order determines the order of route execution. Routes with a lower
        /// order value are tried first. When a route doesn't specify a value, it gets a default value of 0.
        /// A null value for the Order property means that the user didn't specify an explicit order for the
        /// route.
        /// </summary>
        public int Order { get; set; }
        
        /// <summary>
        /// The route template. May be null.
        /// </summary>
        public string Template { get; }

        /// <summary>
        /// Creates a new <see cref="PathAttribute"/> with the given route template.
        /// </summary>
        /// <param name="template">The route template. May not be null.</param>
        public PathAttribute(string template)
        {
            Ensure.IsNotNullOrEmpty(template, nameof(template));

            Template = template;
        }
    }
}

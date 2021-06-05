using System;
using NClient.Annotations.Abstractions;

namespace NClient.Annotations.Methods
{
    /// <summary>
    /// Identifies an action that supports a given set of HTTP methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class MethodAttribute : Attribute, INameProviderAttribute, ITemplateProviderAttribute
    {
        /// <summary>
        /// Gets or sets the route name. The route name can be used to generate a link using a specific route, instead
        /// of relying on selection of a route based on the given set of route values.
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
        public string? Template { get; }

        protected MethodAttribute(string? template)
        {
            Template = template;
        }
    }
}

using System;

// ReSharper disable once CheckNamespace
namespace NClient.Annotations
{
    /// <summary>Identifies an action that supports a given set of operations.</summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class OperationAttribute : Attribute, IOperationAttribute
    {
        /// <summary>Gets or sets the route name. The route name can be used to generate a link using a specific route, instead
        /// of relying on selection of a route based on the given set of route values.</summary>
        public string? Name { get; set; }
        
        /// <summary>Gets or sets a route template.</summary>
        public string? Path { get; set; }

        protected OperationAttribute(string? path = null)
        {
            Path = path;
        }
    }
}

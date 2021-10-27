using System;

// ReSharper disable once CheckNamespace
namespace NClient.Annotations
{
    /// <summary>
    /// Identifies an action that supports a given set of operations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class OperationAttribute : Attribute, IOperationAttribute
    {
        public string? Name { get; set; }
        public string? Path { get; set; }

        protected OperationAttribute(string? path = null)
        {
            Path = path;
        }
    }
}

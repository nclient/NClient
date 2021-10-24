using System;

namespace NClient.Annotations.Operations
{
    /// <summary>
    /// Identifies an action that supports a given set of operations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class OperationAttribute : Attribute
    {
    }
}

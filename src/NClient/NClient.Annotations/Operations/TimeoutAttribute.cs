using System;

// ReSharper disable once CheckNamespace
namespace NClient.Annotations
{
    /// <summary>
    /// Identifies an action that restrict by timeout.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class TimeoutAttribute : Attribute, ITimeoutAttribute
    {
        public double Milliseconds { get; }
        
        /// <summary>
        /// Creates a new <see cref="TimeoutAttribute"/> with the given milliseconds value.
        /// </summary>
        /// <param name="milliseconds">The timeout value</param>
        public TimeoutAttribute(double milliseconds)
        {
            Milliseconds = milliseconds;
        }
    }
}

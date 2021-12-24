using System;

// ReSharper disable once CheckNamespace
namespace NClient.Annotations
{
    /// <summary>
    /// Identifies an action that restrict by timeout.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class TimeoutAttribute : Attribute
    {
        public int Order { get; set; }
        
        private double Timeout { get; }
        
        /// <summary>
        /// Creates a new <see cref="TimeoutAttribute"/> with the given seconds value.
        /// </summary>
        /// <param name="seconds">The timeout value</param>
        public TimeoutAttribute(double seconds)
        {
            Timeout = seconds;
        }
    }
}

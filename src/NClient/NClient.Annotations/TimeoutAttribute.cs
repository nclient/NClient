using System;

// ReSharper disable once CheckNamespace
namespace NClient.Annotations
{
    /// <summary>Identifies an action that restrict by timeout.</summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class TimeoutAttribute : Attribute, ITimeoutAttribute
    {
        /// <summary>The timeout value in milliseconds.</summary>
        public double Milliseconds { get; }
        
        /// <summary>Initializes a new <see cref="TimeoutAttribute"/> with the given milliseconds value.</summary>
        /// <param name="milliseconds">The timeout value in milliseconds.</param>
        public TimeoutAttribute(double milliseconds)
        {
            Milliseconds = milliseconds;
        }
    }
}

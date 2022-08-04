using System;

namespace NClient.Annotations
{
    /// <summary>Identifies an action that should be cached.</summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class CachingAttribute : Attribute, ICachingAttribute
    {
        /// <summary>The life time of cache.</summary>
        public double Milliseconds { get; }
        
        /// <summary>Initializes a new <see cref="CachingAttribute"/> with the given milliseconds value.</summary>
        /// <param name="milliseconds">The life period in milliseconds.</param>
        public CachingAttribute(double milliseconds)
        {
            Milliseconds = milliseconds;
        }
    }
}

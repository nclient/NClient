namespace NClient.Annotations
{
    /// <summary>Identifies an action that should be cached.</summary>
    public interface ICachingAttribute
    {
        /// <summary>The life time of cache.</summary>
        double Milliseconds { get; }
    }
}

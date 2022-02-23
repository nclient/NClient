namespace NClient.Annotations
{
    /// <summary>Identifies an action that restrict by timeout.</summary>
    public interface ITimeoutAttribute
    {
        /// <summary>The timeout value in milliseconds.</summary>
        double Milliseconds { get; }
    }
}

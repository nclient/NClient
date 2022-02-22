namespace NClient.Annotations
{
    public interface ITimeoutAttribute
    {
        /// <summary>The timeout value in milliseconds.</summary>
        double Milliseconds { get; }
    }
}

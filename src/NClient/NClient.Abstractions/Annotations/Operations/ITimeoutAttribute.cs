// ReSharper disable once CheckNamespace

namespace NClient.Annotations
{
    public interface ITimeoutAttribute
    {
        /// <summary>
        /// The timeout value in seconds
        /// </summary>
        double Seconds { get; }
    }
}

namespace NClient.Core.Exceptions
{
    /// <summary>
    /// Represents exceptions thrown by NClient client if an feature is not supported.
    /// </summary>
    public class NotSupportedNClientException : RequestNClientException
    {
        public NotSupportedNClientException(string message) : base(message)
        {
        }
    }
}

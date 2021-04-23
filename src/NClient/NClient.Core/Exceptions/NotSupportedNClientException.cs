namespace NClient.Core.Exceptions
{
    public class NotSupportedNClientException : RequestNClientException
    {
        public NotSupportedNClientException(string message) : base(message)
        {
        }
    }
}

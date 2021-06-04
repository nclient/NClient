namespace NClient.Core.Exceptions
{
    /// <summary>
    /// Represents exceptions thrown by NClient client if an attribute is forbidden.
    /// </summary>
    public class ForbiddenAttributeNClientException : RequestNClientException
    {
        public ForbiddenAttributeNClientException(string message) : base(message)
        {
        }
    }
}

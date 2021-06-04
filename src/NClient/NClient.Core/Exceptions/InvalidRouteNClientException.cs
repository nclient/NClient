namespace NClient.Core.Exceptions
{
    /// <summary>
    /// Represents exceptions thrown by NClient client if the route is invalid.
    /// </summary>
    public class InvalidRouteNClientException : RequestNClientException
    {
        public InvalidRouteNClientException(string message) : base(message)
        {
        }
    }
}

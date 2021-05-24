namespace NClient.Core.Exceptions
{
    public class InvalidRouteNClientException : RequestNClientException
    {
        public InvalidRouteNClientException(string message) : base(message)
        {
        }
    }
}

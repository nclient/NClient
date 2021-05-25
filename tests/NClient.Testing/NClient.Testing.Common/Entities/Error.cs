using System.Net;

namespace NClient.Testing.Common.Entities
{
    public class Error
    {
        public HttpStatusCode Code { get; set; }
        public string? Message { get; set; }
    }
}
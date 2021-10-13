using System.Net;

namespace NClient.Testing.Common.Entities
{
    public class HttpError
    {
        public HttpStatusCode Code { get; set; }
        public string? Message { get; set; }
    }
}

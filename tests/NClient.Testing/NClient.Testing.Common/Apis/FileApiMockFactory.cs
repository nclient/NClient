using System.Text;
using Microsoft.AspNetCore.Http;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace NClient.Testing.Common.Apis
{
    public static class FileApiMockFactory
    {
        public static IWireMockServer MockGetMethod(
            byte[] responseBytes, 
            Encoding encoding, 
            string contentType,
            string contentDisposition)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/file")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Encoding", encoding.WebName)
                    .WithHeader("Content-Type", contentType)
                    .WithHeader("Content-Disposition", contentDisposition)
                    .WithBody(responseBytes));

            return api;
        }
        
        public static IWireMockServer MockFailureGetMethod(
            byte[] responseBytes, 
            Encoding encoding, 
            string contentType,
            string contentDisposition)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/file")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(StatusCodes.Status400BadRequest)
                    .WithHeader("Content-Encoding", encoding.WebName)
                    .WithHeader("Content-Type", contentType)
                    .WithHeader("Content-Disposition", contentDisposition)
                    .WithBody(responseBytes));

            return api;
        }
        
        public static IWireMockServer MockPostMethod(byte[] requestBytes)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/file")
                    .WithBody(requestBytes)
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200));

            return api;
        }
    }
}

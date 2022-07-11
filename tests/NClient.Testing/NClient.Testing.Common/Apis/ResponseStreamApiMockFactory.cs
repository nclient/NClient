using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace NClient.Testing.Common.Apis
{
    public class ResponseStreamApiMockFactory
    {
        public static IWireMockServer MockSuccessGetMethod(int id)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/responseStream")
                    .WithHeader("Accept", "application/json")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(id));

            return api;
        }

        public static IWireMockServer MockFailureGetMethod(HttpStatusCode errorCode)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/responseStream")
                    .WithHeader("Accept", "application/json")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(errorCode)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(errorCode.ToString()));

            return api;
        }
    }
}

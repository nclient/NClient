using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace NClient.Testing.Common.Apis
{
    public class HeaderApiMockFactory
    {
        public static IWireMockServer MockGetMethod(int id)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/header")
                    .WithHeader("Accept", "application/json")
                    .WithHeader("id", id.ToString())
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(id));

            return api;
        }

        public static IWireMockServer MockDeleteMethod(int id)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/header")
                    .WithHeader("Accept", "application/json")
                    .WithHeader("id", id.ToString())
                    .UsingDelete())
                .RespondWith(Response.Create()
                    .WithStatusCode(200));

            return api;
        }
    }
}

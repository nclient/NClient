using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace NClient.Testing.Common.Apis
{
    public class HeaderApiMockFactory
    {
        public static IWireMockServer MockGetMethodWithSingleHeader(int id)
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
        
        public static IWireMockServer MockGetMethodWithMultipleHeaderValues(int id1, int id2)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/header")
                    .WithHeader("Accept", "application/json")
                    // TODO: Why in single string?
                    .WithHeader("id", $"{id1}, {id2}")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(new[] { id1, id2 }));

            return api;
        }
        
        public static IWireMockServer MockGetMethodWithMultipleHeaders(int id1, int id2)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/header")
                    .WithHeader("Accept", "application/json")
                    .WithHeader("id1", id1.ToString())
                    .WithHeader("id2", id2.ToString())
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(new[] { id1, id2 }));

            return api;
        }
    }
}

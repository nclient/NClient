using NClient.Testing.Common.Entities;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace NClient.Testing.Common.Apis
{
    public class BasicApiMockFactory
    {
        public static IWireMockServer MockGetMethod(int id)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/basic")
                    .WithHeader("Accept", "application/json")
                    .WithParam("id", id.ToString())
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(id));

            return api;
        }
        
        public static IWireMockServer MockPostMethod(string json)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/basic")
                    .WithHeader("Accept", "application/json")
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(json)
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200));

            return api;
        }

        public static IWireMockServer MockPostMethod(BasicEntity entity)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/basic")
                    .WithHeader("Accept", "application/json")
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(new JsonMatcher(entity))
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200));

            return api;
        }

        public static IWireMockServer MockPutMethod(BasicEntity entity)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/basic")
                    .WithHeader("Accept", "application/json")
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(new JsonMatcher(entity))
                    .UsingPut())
                .RespondWith(Response.Create()
                    .WithStatusCode(200));

            return api;
        }

        public static IWireMockServer MockDeleteMethod(int id)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/basic")
                    .WithHeader("Accept", "application/json")
                    .WithParam("id", id.ToString())
                    .UsingDelete())
                .RespondWith(Response.Create()
                    .WithStatusCode(200));

            return api;
        }
    }
}

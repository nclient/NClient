using NClient.Testing.Common.Entities;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace NClient.Testing.Common.Apis
{
    public class ReturnApiMockFactory
    {
        public static IWireMockServer MockGetAsyncMethod(int id, BasicEntity entity)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api")
                    .WithHeader("Accept", "application/json")
                    .WithParam("id", id.ToString())
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Encoding", "utf-8")
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(entity));

            return api;
        }

        public static IWireMockServer MockGetMethod(int id, BasicEntity entity)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api")
                    .WithHeader("Accept", "application/json")
                    .WithParam("id", id.ToString())
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Encoding", "utf-8")
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(entity));

            return api;
        }

        public static IWireMockServer MockPostAsyncMethod(BasicEntity entity)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api")
                    .WithHeader("Accept", "application/json")
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(new JsonMatcher(entity))
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json"));

            return api;
        }

        public static IWireMockServer MockPostMethod(BasicEntity entity)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api")
                    .WithHeader("Accept", "application/json")
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(new JsonMatcher(entity))
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json"));

            return api;
        }

        public static IWireMockServer MockInternalServerError()
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create().UsingAnyMethod())
                .RespondWith(Response.Create()
                    .WithStatusCode(500));

            return api;
        }

        public static IWireMockServer MockFlakyGetMethod(int id, BasicEntity entity)
        {
            var api = WireMockServer.Start();

            api.Given(Request.Create().UsingAnyMethod())
                .InScenario("Flaky scenario")
                .WillSetStateTo("Second request", 1)
                .RespondWith(Response.Create()
                    .WithStatusCode(500));

            api.Given(Request.Create()
                    .WithPath("/api")
                    .WithHeader("Accept", "application/json")
                    .WithParam("id", id.ToString())
                    .UsingGet())
                .InScenario("Flaky scenario")
                .WhenStateIs("Second request")
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(entity));

            return api;
        }

        public static IWireMockServer MockFlakyPostMethod(BasicEntity entity)
        {
            var api = WireMockServer.Start();

            api.Given(Request.Create().UsingAnyMethod())
                .InScenario("Flaky scenario")
                .WillSetStateTo("Second request", 1)
                .RespondWith(Response.Create()
                    .WithStatusCode(500));

            api.Given(Request.Create()
                    .WithPath("/api")
                    .WithHeader("Accept", "application/json")
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(new JsonMatcher(entity))
                    .UsingPost())
                .InScenario("Flaky scenario")
                .WhenStateIs("Second request")
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json"));

            return api;
        }
    }
}

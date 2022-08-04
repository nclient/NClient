using NClient.Testing.Common.Entities;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace NClient.Testing.Common.Apis
{
    public class CachingApiMockFactory
    {
        public static IWireMockServer MockGetAsyncMethod(int id, BasicEntity entity)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/caching")
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
    }
}

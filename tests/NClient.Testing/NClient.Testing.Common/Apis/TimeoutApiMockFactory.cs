using System;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace NClient.Testing.Common.Apis
{
    public class TimeoutApiMockFactory
    {
        public static IWireMockServer MockGetMethod(int id, TimeSpan delay)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/timeout")
                    .WithHeader("Accept", "application/json")
                    .WithParam("id", id.ToString())
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithDelay(delay)
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(id));

            return api;
        }
    }
}

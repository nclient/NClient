using System;
using FluentAssertions.Extensions;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace NClient.Testing.Common.Apis
{
    public class CancellationApiMockFactory
    {
        public static IWireMockServer MockGetMethod(int id)
        {
            return InternalMockGetMethodWithDelay(id);
        }

        public static IWireMockServer MockGetMethodWithDelay(int id)
        {
            return InternalMockGetMethodWithDelay(id, delay: 3.Seconds());
        }

        private static IWireMockServer InternalMockGetMethodWithDelay(int id, TimeSpan? delay = null)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/cancellation")
                    .WithHeader("Accept", "application/json")
                    .WithParam("id", id.ToString())
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithDelay(delay ?? 1.Microseconds())
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(id));

            return api;
        }
    }
}

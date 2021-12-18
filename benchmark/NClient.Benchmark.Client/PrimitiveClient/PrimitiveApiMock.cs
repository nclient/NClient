using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace NClient.Benchmark.Client.PrimitiveClient
{
    public static class PrimitiveApiMock
    {
        public static string EndpointPath => "api";
        public static string ParamName => "id";
        
        public static IWireMockServer MockMethod()
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/" + EndpointPath)
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithHeader("Content-Type", "application/json")
                    .WithStatusCode(200)
                    .WithBodyAsJson(1));

            return api;
        }
    }
}

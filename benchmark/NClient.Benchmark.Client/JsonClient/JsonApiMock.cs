using NClient.Benchmark.Client.Helpers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace NClient.Benchmark.Client.JsonClient
{
    public static class JsonApiMock
    {
        public static string EndpointPath => "api";
        
        public static IWireMockServer MockMethod()
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/" + EndpointPath)
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithHeader("Content-Type", "application/json")
                    .WithStatusCode(200)
                    .WithBodyAsJson(DtoProvider.Get()));

            return api;
        }
    }
}

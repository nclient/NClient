using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace NClient.Benchmark.Client.JsonHttpResponseApi
{
    public static class JsonHttpResponseApiMock
    {
        public static string EndpointPath => "api";
        
        public static IWireMockServer MockMethod(string[] ids)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/" + EndpointPath)
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithHeader("Content-Type", "application/json")
                    .WithStatusCode(200)
                    .WithBodyAsJson(ids));

            return api;
        }
    }
}

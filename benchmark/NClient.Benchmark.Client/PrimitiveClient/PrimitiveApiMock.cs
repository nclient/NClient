using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace NClient.Benchmark.Client.PrimitiveClient
{
    public static class PrimitiveApiMock
    {
        public static string EndpointPath => "api";
        public static string ParamName => "id";
        
        public static IWireMockServer MockMethod(int id)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/" + EndpointPath)
                    .WithParam(ParamName, id.ToString())
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithHeader("Content-Type", "application/json")
                    .WithStatusCode(200)
                    .WithBodyAsJson(id));

            return api;
        }
    }
}

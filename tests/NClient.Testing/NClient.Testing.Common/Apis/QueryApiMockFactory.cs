using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Web;
using NClient.Testing.Common.Entities;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace NClient.Testing.Common.Apis
{
    public class QueryApiMockFactory
    {
        public static IWireMockServer MockGetMethod(int id)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/query")
                    .WithHeader("Accept", "application/json")
                    .WithParam("id", id.ToString())
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(id));

            return api;
        }
        
        public static IWireMockServer MockGetMethod(IEnumerable<int> ids)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/query")
                    .WithHeader("Accept", "application/json")
                    .WithParam("ids", ids.Select(x => x.ToString()).ToArray())
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(ids));

            return api;
        }
        
        public static IWireMockServer MockGetMethod(string paramName, IDictionary<string, int> keyValues)
        {
            var request = Request.Create();
            foreach (var keyValue in keyValues)
            {
                request.WithParam(UrlEncoder.Default.Encode($"{paramName}[{keyValue.Key}]"), keyValue.Value.ToString());
            }
            
            var api = WireMockServer.Start();
            api.Given(request
                    .WithPath("/api/query")
                    .WithHeader("Accept", "application/json")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(keyValues));

            return api;
        }

        public static IWireMockServer MockPostMethod(BasicEntity entity)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/query")
                    .WithHeader("Accept", "application/json")
                    .WithParam("entity.Id", entity.Id.ToString())
                    .WithParam("entity.Value", entity.Value.ToString())
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200));

            return api;
        }

        public static IWireMockServer MockPutMethod(BasicEntity entity)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/query")
                    .WithHeader("Accept", "application/json")
                    .WithParam("entity.Id", entity.Id.ToString())
                    .WithParam("entity.Value", entity.Value.ToString())
                    .UsingPut())
                .RespondWith(Response.Create()
                    .WithStatusCode(200));

            return api;
        }

        public static IWireMockServer MockDeleteMethod(int id)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/query")
                    .WithHeader("Accept", "application/json")
                    .WithParam("id", id.ToString())
                    .UsingDelete())
                .RespondWith(Response.Create()
                    .WithStatusCode(200));

            return api;
        }
    }
}

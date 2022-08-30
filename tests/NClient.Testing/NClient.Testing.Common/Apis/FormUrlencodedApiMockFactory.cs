using System.Collections.Generic;
using System.Linq;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace NClient.Testing.Common.Apis
{
    public class FormUrlencodedApiMockFactory
    {
        public static IWireMockServer MockPostMethod(int id, IDictionary<string, object> keyValues)
        {
            var api = WireMockServer.Start();
            api.Given(Request.Create()
                    .WithPath("/api/formUrlencoded")
                    .WithHeader("Accept", "application/json")
                    .WithHeader("Content-Type", "application/x-www-form-urlencoded")
                    .WithBody(string.Join("&", keyValues.Select(x => $"{x.Key}={x.Value}")))
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(id));

            return api;
        }
    }
}

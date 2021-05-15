using System;
using NClient.Testing.Common.Entities;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace NClient.Testing.Common.Apis
{
    public class HttpApiMockFactory
    {
        public Uri ApiUri { get; }

        public HttpApiMockFactory(int port)
        {
            ApiUri = new UriBuilder("http", "localhost", port).Uri;
        }

        public IWireMockServer MockGetMethod(int id)
        {
            var api = WireMockServer.Start(ApiUri.ToString());
            api.Given(Request.Create()
                    .WithPath("/api/http")
                    .WithHeader("Accept", "application/json")
                    .WithParam("id", id.ToString())
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "Content-Type: application/json")
                    .WithBodyAsJson(1));

            return api;
        }

        public IWireMockServer MockPostMethod(BasicEntity entity)
        {
            var api = WireMockServer.Start(ApiUri.ToString());
            api.Given(Request.Create()
                    .WithPath("/api/http")
                    .WithHeader("Accept", "application/json")
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBody(new JsonMatcher(entity))
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithBodyAsJson(entity)
                    .WithHeader("Content-Type", "Content-Type: application/json")
                    .WithStatusCode(200));

            return api;
        }

        public IWireMockServer MockPutMethod(BasicEntity entity)
        {
            var api = WireMockServer.Start(ApiUri.ToString());
            api.Given(Request.Create()
                    .WithPath("/api/http")
                    .WithHeader("Accept", "application/json")
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBody(new JsonMatcher(entity))
                    .UsingPut())
                .RespondWith(Response.Create()
                    .WithStatusCode(200));

            return api;
        }

        public IWireMockServer MockDeleteMethod(int id)
        {
            var api = WireMockServer.Start(ApiUri.ToString());
            api.Given(Request.Create()
                    .WithPath("/api/http")
                    .WithHeader("Accept", "application/json")
                    .WithParam("id", id.ToString())
                    .UsingDelete())
                .RespondWith(Response.Create()
                    .WithStatusCode(200));

            return api;
        }
    }
}
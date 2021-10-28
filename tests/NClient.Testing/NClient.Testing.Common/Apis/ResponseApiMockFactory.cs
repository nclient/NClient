using System;
using System.Net;
using NClient.Testing.Common.Entities;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace NClient.Testing.Common.Apis
{
    public class ResponseApiMockFactory
    {
        public Uri ApiUri { get; }

        public ResponseApiMockFactory(int port)
        {
            ApiUri = new UriBuilder("http", "localhost", port).Uri;
        }

        public IWireMockServer MockGetMethod(int id)
        {
            var api = WireMockServer.Start(ApiUri.ToString());
            api.Given(Request.Create()
                    .WithPath("/api/response")
                    .WithHeader("Accept", "application/json")
                    .WithParam("id", id.ToString())
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(id));

            return api;
        }

        public IWireMockServer MockGetMethodWithBadRequest(int id)
        {
            var api = WireMockServer.Start(ApiUri.ToString());
            api.Given(Request.Create()
                    .WithPath("/api/response")
                    .WithHeader("Accept", "application/json")
                    .WithParam("id", id.ToString())
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(400)
                    .WithHeader("Content-Type", "application/json"));

            return api;
        }

        public IWireMockServer MockGetMethodWithBadRequestAndError(int id)
        {
            var api = WireMockServer.Start(ApiUri.ToString());
            api.Given(Request.Create()
                    .WithPath("/api/response")
                    .WithHeader("Accept", "application/json")
                    .WithParam("id", id.ToString())
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(400)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(new HttpError { Code = HttpStatusCode.BadRequest, Message = "Error" }));

            return api;
        }

        public IWireMockServer MockPostMethod(BasicEntity entity)
        {
            var api = WireMockServer.Start(ApiUri.ToString());
            api.Given(Request.Create()
                    .WithPath("/api/response")
                    .WithHeader("Accept", "application/json")
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(new JsonMatcher(entity))
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200));

            return api;
        }

        public IWireMockServer MockPostMethodWithBadRequest(BasicEntity entity)
        {
            var api = WireMockServer.Start(ApiUri.ToString());
            api.Given(Request.Create()
                    .WithPath("/api/response")
                    .WithHeader("Accept", "application/json")
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(new JsonMatcher(entity))
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(400)
                    .WithHeader("Content-Type", "application/json"));

            return api;
        }

        public IWireMockServer MockPostMethodWithBadRequestAndError(BasicEntity entity)
        {
            var api = WireMockServer.Start(ApiUri.ToString());
            api.Given(Request.Create()
                    .WithPath("/api/response")
                    .WithHeader("Accept", "application/json")
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(new JsonMatcher(entity))
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(400)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(new HttpError { Code = HttpStatusCode.BadRequest, Message = "Error" }));

            return api;
        }

        public IWireMockServer MockInternalServerError()
        {
            var api = WireMockServer.Start(ApiUri.ToString());
            api.Given(Request.Create().UsingAnyMethod())
                .RespondWith(Response.Create()
                    .WithStatusCode(500)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(new HttpError { Code = HttpStatusCode.InternalServerError, Message = "Error" }));

            return api;
        }
    }
}

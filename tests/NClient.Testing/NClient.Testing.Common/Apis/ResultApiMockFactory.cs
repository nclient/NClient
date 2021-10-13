using System;
using System.Net;
using NClient.Testing.Common.Entities;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace NClient.Testing.Common.Apis
{
    public class ResultApiMockFactory
    {
        public Uri ApiUri { get; }

        public ResultApiMockFactory(int port)
        {
            ApiUri = new UriBuilder("http", "localhost", port).Uri;
        }

        public IWireMockServer MockGetIntMethod(int id)
        {
            var api = WireMockServer.Start(ApiUri.ToString());
            api.Given(Request.Create()
                    .WithPath("/api/result/ints")
                    .WithHeader("Accept", "application/json")
                    .WithParam("id", id.ToString())
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(id));

            return api;
        }
        
        public IWireMockServer MockGetNotFoundIntMethod(int id)
        {
            var api = WireMockServer.Start(ApiUri.ToString());
            api.Given(Request.Create()
                    .WithPath("/api/result/ints")
                    .WithHeader("Accept", "application/json")
                    .WithParam("id", id.ToString())
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(404)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(new HttpError { Code = HttpStatusCode.BadRequest, Message = "Error" }));

            return api;
        }

        public IWireMockServer MockGetEntityMethod(int id)
        {
            var api = WireMockServer.Start(ApiUri.ToString());
            api.Given(Request.Create()
                    .WithPath("/api/result/entities")
                    .WithHeader("Accept", "application/json")
                    .WithParam("id", id.ToString())
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(new BasicEntity { Id = id, Value = 2 }));

            return api;
        }
        
        public IWireMockServer MockGetNotFoundEntityMethod(int id)
        {
            var api = WireMockServer.Start(ApiUri.ToString());
            api.Given(Request.Create()
                    .WithPath("/api/result/entities")
                    .WithHeader("Accept", "application/json")
                    .WithParam("id", id.ToString())
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(404)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(new HttpError { Code = HttpStatusCode.BadRequest, Message = "Error" }));

            return api;
        }
    }
}

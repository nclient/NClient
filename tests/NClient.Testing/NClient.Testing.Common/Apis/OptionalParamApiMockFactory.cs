﻿using System;
using NClient.Testing.Common.Entities;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace NClient.Testing.Common.Apis
{
    public class OptionalParamApiMockFactory
    {
        public Uri ApiUri { get; }

        public OptionalParamApiMockFactory(int port)
        {
            ApiUri = new UriBuilder("http", "localhost", port).Uri;
        }

        public IWireMockServer MockGetMethod(int id)
        {
            var api = WireMockServer.Start(ApiUri.ToString());
            api.Given(Request.Create()
                    .WithPath("/api/optionalParam")
                    .WithHeader("Accept", "application/json")
                    .WithParam("id", id.ToString())
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(id));

            return api;
        }

        public IWireMockServer MockPostMethod(BasicEntity entity)
        {
            var api = WireMockServer.Start(ApiUri.ToString());
            api.Given(Request.Create()
                    .WithPath("/api/optionalParam")
                    .WithHeader("Accept", "application/json")
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBody(new JsonMatcher(entity))
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200));

            return api;
        }

        public IWireMockServer MockPostMethod()
        {
            var api = WireMockServer.Start(ApiUri.ToString());
            api.Given(Request.Create()
                    .WithPath("/api/optionalParam")
                    .WithHeader("Accept", "application/json")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200));

            return api;
        }
    }
}

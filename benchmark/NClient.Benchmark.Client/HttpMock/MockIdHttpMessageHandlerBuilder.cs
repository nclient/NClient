using System;
using System.Text.Json;
using RichardSzalay.MockHttp;

namespace NClient.Benchmark.Client.HttpMock
{
    public static class MockIdHttpMessageHandlerBuilder
    {
        public static Uri Uri { get; } = new("http://localhost:5000/api");
        public static string Host { get; } = Uri.GetLeftPart(UriPartial.Authority);
        public static string Path { get; } = Uri.GetLeftPart(UriPartial.Path);
        
        public static MockHttpMessageHandler Build<T>(string paramName, T response) where T : struct
        {
            var mock = new MockHttpMessageHandler();
            mock
                .When("*")
                .Respond(mediaType: "application/json", content: JsonSerializer.Serialize(response));
            return mock;
        }
    }
}

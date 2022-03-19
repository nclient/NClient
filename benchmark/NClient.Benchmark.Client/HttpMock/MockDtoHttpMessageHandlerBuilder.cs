using System;
using System.Text.Json;
using RichardSzalay.MockHttp;

namespace NClient.Benchmark.Client.HttpMock
{
    public static class MockDtoHttpMessageHandlerBuilder
    {
        public static Uri Uri { get; } = new("http://localhost:5000/api");
        public static string Host { get; } = Uri.GetLeftPart(UriPartial.Authority);
        public static string Path { get; } = Uri.PathAndQuery;
        
        public static MockHttpMessageHandler Build<T>(T response)
        {
            var mock = new MockHttpMessageHandler();
            mock
                .When(Uri.ToString())
                .Respond(mediaType: "application/json", content: JsonSerializer.Serialize(response));
            return mock;
        }
    }
}

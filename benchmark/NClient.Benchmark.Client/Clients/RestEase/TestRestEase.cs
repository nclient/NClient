using System;
using System.Text.Json;
using NClient.Benchmark.Client.Helpers;
using NClient.Benchmark.Client.HttpMock;
using RestEase;

namespace NClient.Benchmark.Client.Clients.RestEase
{
    public class TestRestEase<TClient> where TClient : class
    {
        private readonly JsonSerializerOptions? _jsonSerializerOptions;
        private readonly TClient _restEaseClient;

        // ReSharper disable once UnusedParameter.Local
        public TestRestEase(JsonSerializerOptions jsonSerializerOptions)
        {
            _jsonSerializerOptions = jsonSerializerOptions;

            _restEaseClient = new RestClient(new System.Net.Http.HttpClient(MockDtoHttpMessageHandlerBuilder.Build(DtoProvider.Get()))
            {
                BaseAddress = new Uri(MockDtoHttpMessageHandlerBuilder.Host)
            }).For<TClient>();
        }
        
        public TestRestEase()
        {
            _restEaseClient = new RestClient(new System.Net.Http.HttpClient(MockIdHttpMessageHandlerBuilder
                .Build(paramName: "id", IdProvider.Get()))
            {
                BaseAddress = new Uri(MockIdHttpMessageHandlerBuilder.Host)
            }).For<TClient>();
        }
        
        public void Send(Action<TClient> action)
        {
            action(_restEaseClient);
        }
    }
}

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using NClient.Benchmark.Client.Helpers;
using NClient.Benchmark.Client.HttpMock;

namespace NClient.Benchmark.Client.Clients.HttpClient
{
    public class TestHttpClient
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly System.Net.Http.HttpClient _httpClient;
        
        public TestHttpClient(JsonSerializerOptions jsonSerializerOptions)
        {
            _jsonSerializerOptions = jsonSerializerOptions;
            
            _httpClient = new System.Net.Http.HttpClient(MockDtoHttpMessageHandlerBuilder.Build(DtoProvider.Get()))
            {
                BaseAddress = new Uri(MockDtoHttpMessageHandlerBuilder.Host)
            };
        }        
        
        public TestHttpClient()
        {
            _httpClient = new System.Net.Http.HttpClient(MockIdHttpMessageHandlerBuilder
                .Build(paramName: "id", IdProvider.Get()))
            {
                BaseAddress = new Uri(MockIdHttpMessageHandlerBuilder.Host)
            };
        }
        
        public void Send<T>(T data)
        {
            var json = JsonSerializer.Serialize(data, _jsonSerializerOptions);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var result = _httpClient
                .PostAsync(MockDtoHttpMessageHandlerBuilder.Path, httpContent)
                .GetAwaiter()
                .GetResult();
            result.EnsureSuccessStatusCode();
            JsonSerializer.DeserializeAsync<T>(result.Content.ReadAsStream(), _jsonSerializerOptions);
        }

        public void SendWithoutBody<T>(T data) where T : struct
        {
            var result = _httpClient
                .GetAsync(MockIdHttpMessageHandlerBuilder.Path + $"?{"id"}={data.ToString()}")
                .GetAwaiter()
                .GetResult();
            result.EnsureSuccessStatusCode();
            result.Content.ReadFromJsonAsync(typeof(T)).GetAwaiter().GetResult();
        }
    }
}

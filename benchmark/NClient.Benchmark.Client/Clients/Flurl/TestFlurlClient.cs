using System.Text.Json;
using Flurl.Http;
using NClient.Benchmark.Client.Helpers;
using NClient.Benchmark.Client.HttpMock;

namespace NClient.Benchmark.Client.Clients.Flurl
{
    public class TestFlurlClient
    {
        private readonly IFlurlClient _flurlClient;
        
        public TestFlurlClient(JsonSerializerOptions jsonSerializerOptions)
        {
            _flurlClient = new FlurlClient(MockDtoHttpMessageHandlerBuilder.Host).Configure(settings => 
            {
                settings.HttpClientFactory = new FlurlHttpClientFactory(MockDtoHttpMessageHandlerBuilder.Build(DtoProvider.Get()));
                settings.JsonSerializer = new FlurlSystemJsonSerializer(jsonSerializerOptions);
            });
        }
        
        public TestFlurlClient()
        {
            _flurlClient = new FlurlClient(MockIdHttpMessageHandlerBuilder.Host).Configure(settings => 
            {
                settings.HttpClientFactory = new FlurlHttpClientFactory(MockIdHttpMessageHandlerBuilder
                    .Build(paramName: "id", IdProvider.Get()));
                settings.JsonSerializer = new FlurlSystemJsonSerializer(new JsonSerializerOptions());
            });
        }
        
        public void Send<T>(T data)
        {
            var response = _flurlClient.Request(MockDtoHttpMessageHandlerBuilder.Path)
                .PostJsonAsync(data).GetAwaiter().GetResult();
            response.GetJsonAsync<T>().GetAwaiter().GetResult();
        }

        public void SendWithoutBody<T>(T data) where T : struct
        {
            var response = _flurlClient.Request(MockIdHttpMessageHandlerBuilder.Path)
                .SetQueryParam("id", data.ToString())
                .GetAsync().GetAwaiter().GetResult();
            response.GetJsonAsync<T>().GetAwaiter().GetResult();
        }
    }
}

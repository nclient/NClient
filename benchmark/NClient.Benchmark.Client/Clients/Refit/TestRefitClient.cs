using System;
using System.Text.Json;
using NClient.Benchmark.Client.Helpers;
using NClient.Benchmark.Client.HttpMock;
using Refit;

namespace NClient.Benchmark.Client.Clients.Refit
{
    public class TestRefitClient<TClient> where TClient : class
    {
        private readonly TClient _refitClient;

        public TestRefitClient(JsonSerializerOptions jsonSerializerOptions)
        {
            _refitClient = RestService.For<TClient>(MockDtoHttpMessageHandlerBuilder.Host, new RefitSettings 
            {
                ContentSerializer = new SystemTextJsonContentSerializer(jsonSerializerOptions),
                HttpMessageHandlerFactory = () => MockDtoHttpMessageHandlerBuilder.Build(DtoProvider.Get())
            });
        }
        
        public TestRefitClient()
        {
            _refitClient = RestService.For<TClient>(MockIdHttpMessageHandlerBuilder.Host, new RefitSettings 
            {
                ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions()),
                HttpMessageHandlerFactory = () => MockIdHttpMessageHandlerBuilder
                    .Build(paramName: "id", IdProvider.Get())
            });
        }
        
        public void Send(Action<TClient> action)
        {
            action(_refitClient);
        }
    }
}

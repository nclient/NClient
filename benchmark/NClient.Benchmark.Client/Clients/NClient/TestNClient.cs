using System;
using System.Text.Json;
using System.Threading;
using NClient.Benchmark.Client.Helpers;
using NClient.Benchmark.Client.HttpMock;

namespace NClient.Benchmark.Client.Clients.NClient
{
    public class TestNClient<TClient> where TClient : class
    {
        private readonly TClient _nclient;
        
        public TestNClient(JsonSerializerOptions jsonSerializerOptions)
        {
            _nclient = NClientGallery.Clients.GetCustom().For<TClient>(MockDtoHttpMessageHandlerBuilder.Host)
                .UsingRestApi()
                .UsingSystemNetHttpTransport(new System.Net.Http.HttpClient(MockDtoHttpMessageHandlerBuilder.Build(DtoProvider.Get()))
                {
                    Timeout = Timeout.InfiniteTimeSpan
                })
                .UsingSystemTextJsonSerialization(jsonSerializerOptions)
                .WithResponseToHttpResponseMapping()
                .Build();
        }        
        
        public TestNClient()
        {
            _nclient = NClientGallery.Clients.GetCustom().For<TClient>(MockIdHttpMessageHandlerBuilder.Host)
                .UsingRestApi()
                .UsingSystemNetHttpTransport(new System.Net.Http.HttpClient(MockIdHttpMessageHandlerBuilder
                    .Build(paramName: "id", IdProvider.Get()))
                {
                    Timeout = Timeout.InfiniteTimeSpan
                })
                .UsingSystemTextJsonSerialization(new JsonSerializerOptions())
                .WithResponseToHttpResponseMapping()
                .Build();
        }

        public void Send(Action<TClient> action)
        {
            action(_nclient);
        }
    }
}

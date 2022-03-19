using System.Text.Json;
using NClient.Benchmark.Client.Helpers;
using NClient.Benchmark.Client.HttpMock;
using RestSharp;
using RestSharp.Serializers.Json;

namespace NClient.Benchmark.Client.Clients.RestSharp
{
    public class TestRestSharpClient
    {
        private readonly RestClient _restSharpClient;

        public TestRestSharpClient(JsonSerializerOptions jsonSerializerOptions)
        {
            _restSharpClient = new RestClient(new RestClientOptions 
            {
                ConfigureMessageHandler = _ => MockDtoHttpMessageHandlerBuilder
                    .Build(DtoProvider.Get())
            });
            _restSharpClient.UseJson().UseSystemTextJson(jsonSerializerOptions);
        }
        
        public TestRestSharpClient()
        {
            _restSharpClient = new RestClient(new RestClientOptions 
            {
                ConfigureMessageHandler = _ => MockIdHttpMessageHandlerBuilder
                    .Build(paramName: "id", IdProvider.Get())
            });
            _restSharpClient.UseJson().UseSystemTextJson(new JsonSerializerOptions());
        }

        public void Send<T>(T data) where T : class
        {
            var request = new RestRequest(MockDtoHttpMessageHandlerBuilder.Uri);
            request.AddJsonBody(data);
            _restSharpClient.PostAsync<T>(request).GetAwaiter().GetResult();
        }
        
        public void SendWithoutBody<T>(T data) where T : struct
        {
            var request = new RestRequest(MockIdHttpMessageHandlerBuilder.Uri);
            request.AddQueryParameter("id", data.ToString());
            _restSharpClient.GetAsync<T>(request).GetAwaiter().GetResult();
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Flurl.Http;
using NClient.Benchmark.Client.Dtos;
using NClient.Benchmark.Client.Helpers;
using NClient.Benchmark.Client.JsonClient;
using Refit;
using RestSharp;
using RestSharp.Serializers.Json;
using WireMock.Server;

// ReSharper disable RedundantNameQualifier
namespace NClient.Benchmark.Client.JsonSourceGeneratorClient
{
    [SimpleJob(invocationCount: 2000, targetCount: 10)]
    public class JsonSourceGeneratorClientBenchmark
    {
        private IWireMockServer _api = null!;
        private JsonSerializerOptions _jsonSerializerOptions = null!;
        
        private HttpClient _httpClient = null!;
        private RestSharp.RestClient _restSharpClient = null!;
        private FlurlClient _flurlClient = null!;
        private IJsonClient _nclient = null!;
        private IJsonClient _refitClient = null!;
        private IJsonClient _restEaseClient = null!;

        [GlobalSetup]
        public void Setup()
        {
            _api = JsonApiMock.MockMethod();

            _jsonSerializerOptions = new JsonSerializerOptions();
            _jsonSerializerOptions.AddContext<DtoJsonContext>();

            _httpClient = new HttpClient();
            HttpClient_Send();
            
            _restSharpClient = new RestSharp.RestClient(_api.Urls.First());
            _restSharpClient.UseJson().UseSystemTextJson(_jsonSerializerOptions);
            RestSharp_Send();

            // TODO: Use JsonSerializerOptions
            _flurlClient = new FlurlClient(_api.Urls.First());
            Flurl_Send();
            
            _nclient = NClientGallery.Clients.GetRest().For<IJsonClient>(new Uri(_api.Urls.First()))
                .WithSystemTextJsonSerialization(_jsonSerializerOptions)
                .Build();
            NClient_Send();
            
            _refitClient = Refit.RestService.For<IJsonClient>(_api.Urls.First(), new RefitSettings 
            {
                ContentSerializer = new SystemTextJsonContentSerializer(_jsonSerializerOptions)
            });
            Refit_Send();
            
            // TODO: Use JsonSerializerOptions
            _restEaseClient = RestEase.RestClient.For<IJsonClient>(_api.Urls.First());
            RestEase_Send();
        }
        
        [Benchmark]
        public void HttpClient_CreateAndSend()
        {
            var httpClient = new HttpClient();
            var json = JsonSerializer.Serialize(DtoProvider.Get(), _jsonSerializerOptions);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var result = httpClient
                .PostAsync(Path.Combine(_api.Urls.First(), JsonApiMock.EndpointPath), httpContent)
                .GetAwaiter()
                .GetResult();
            result.EnsureSuccessStatusCode();
            JsonSerializer.Deserialize<Dto>(result.Content.ReadAsStream(), _jsonSerializerOptions);
        }
        
        [Benchmark]
        public void RestSharp_CreateAndSend()
        {
            var restSharpClient = new RestSharp.RestClient(_api.Urls.First());
            var request = new RestSharp.RestRequest(JsonApiMock.EndpointPath);
            request.AddJsonBody(DtoProvider.Get());
            restSharpClient.PostAsync<Dto>(request).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void Flurl_CreateAndSend()
        {
            var flurlClient = new FlurlClient(_api.Urls.First());
            var response = flurlClient.Request(JsonApiMock.EndpointPath).PostJsonAsync(DtoProvider.Get()).GetAwaiter().GetResult();
            response.GetJsonAsync<Dto>().GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void NClient_CreateAndSend()
        {
            var nclient = NClientGallery.Clients.GetRest().For<IJsonClient>(_api.Urls.First()).Build();
            nclient.SendAsync(DtoProvider.Get()).GetAwaiter().GetResult();
        }

        [Benchmark]
        public void Refit_CreateAndSend()
        {
            var refitClient = Refit.RestService.For<IJsonClient>(_api.Urls.First());
            refitClient.SendAsync(DtoProvider.Get()).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void RestEase_CreateAndSend()
        {
            var restEasyClient = RestEase.RestClient.For<IJsonClient>(_api.Urls.First());
            restEasyClient.SendAsync(DtoProvider.Get()).GetAwaiter().GetResult();
        }

        [Benchmark]
        public void HttpClient_Send()
        {
            var json = JsonSerializer.Serialize(DtoProvider.Get(), _jsonSerializerOptions);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var result = _httpClient
                .PostAsync(Path.Combine(_api.Urls.First(), JsonApiMock.EndpointPath), httpContent)
                .GetAwaiter()
                .GetResult();
            result.EnsureSuccessStatusCode();
            JsonSerializer.DeserializeAsync<Dto>(result.Content.ReadAsStream(), _jsonSerializerOptions);
        }
        
        [Benchmark]
        public void RestSharp_Send()
        {
            var request = new RestSharp.RestRequest(JsonApiMock.EndpointPath);
            request.AddJsonBody(DtoProvider.Get());
            _restSharpClient.PostAsync<Dto>(request).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void Flurl_Send()
        {
            var response = _flurlClient.Request(JsonApiMock.EndpointPath).PostJsonAsync(DtoProvider.Get()).GetAwaiter().GetResult();
            response.GetJsonAsync<Dto>().GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void NClient_Send()
        {
            _nclient.SendAsync(DtoProvider.Get()).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void Refit_Send()
        {
            _refitClient.SendAsync(DtoProvider.Get()).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void RestEase_Send()
        {
            _restEaseClient.SendAsync(DtoProvider.Get()).GetAwaiter().GetResult();
        }
    }
}

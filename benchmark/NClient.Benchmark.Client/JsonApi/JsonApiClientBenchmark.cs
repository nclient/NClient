using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Flurl.Http;
using RestSharp;
using WireMock.Server;

// ReSharper disable RedundantNameQualifier
namespace NClient.Benchmark.Client.JsonApi
{
    [SimpleJob(invocationCount: 2000, targetCount: 10)]
    public class JsonApiClientBenchmark
    {
        private static readonly string[] Ids = { "id-1", "id-2", "id-3", "id-4" };
        
        private IWireMockServer _api = null!;
        
        private HttpClient _httpClient = null!;
        private RestSharp.RestClient _restSharpClient = null!;
        private FlurlClient _flurlClient = null!;
        private IJsonApiClient _nclient = null!;
        private IJsonApiClient _refitClient = null!;
        private IJsonApiClient _restEaseClient = null!;

        [GlobalSetup]
        public void Setup()
        {
            _api = JsonApiMock.MockMethod(Ids);
            
            _httpClient = new HttpClient();
            _restSharpClient = new RestSharp.RestClient(_api.Urls.First());
            _flurlClient = new FlurlClient(_api.Urls.First());
            _nclient = NClientGallery.Clients.GetRest().For<IJsonApiClient>(_api.Urls.First()).Build();
            _refitClient = Refit.RestService.For<IJsonApiClient>(_api.Urls.First());
            _restEaseClient = RestEase.RestClient.For<IJsonApiClient>(_api.Urls.First());
        }
        
        [Benchmark]
        public void HttpClient_CreateAndSend()
        {
            var httpClient = new HttpClient();
            var json = JsonSerializer.Serialize(Ids);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var result = httpClient
                .PostAsync(Path.Combine(_api.Urls.First(), JsonApiMock.EndpointPath), httpContent)
                .GetAwaiter()
                .GetResult();
            result.EnsureSuccessStatusCode();
            result.Content.ReadFromJsonAsync(typeof(List<string>)).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void RestSharp_CreateAndSend()
        {
            var restSharpClient = new RestSharp.RestClient(_api.Urls.First());
            var request = new RestSharp.RestRequest(JsonApiMock.EndpointPath, DataFormat.Json);
            request.AddJsonBody(Ids);
            restSharpClient.PostAsync<List<string>>(request).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void Flurl_CreateAndSend()
        {
            var flurlClient = new FlurlClient(_api.Urls.First());
            var response = flurlClient.Request(JsonApiMock.EndpointPath).PostJsonAsync(Ids).GetAwaiter().GetResult();
            response.GetJsonAsync<List<string>>().GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void NClient_CreateAndSend()
        {
            var nclient = NClientGallery.Clients.GetRest().For<IJsonApiClient>(_api.Urls.First()).Build();
            nclient.SendAsync(Ids).GetAwaiter().GetResult();
        }

        [Benchmark]
        public void Refit_CreateAndSend()
        {
            var refitClient = Refit.RestService.For<IJsonApiClient>(_api.Urls.First());
            refitClient.SendAsync(Ids).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void RestEase_CreateAndSend()
        {
            var restEasyClient = RestEase.RestClient.For<IJsonApiClient>(_api.Urls.First());
            restEasyClient.SendAsync(Ids).GetAwaiter().GetResult();
        }

        [Benchmark]
        public void HttpClient_Send()
        {
            var json = JsonSerializer.Serialize(Ids);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var result = _httpClient
                .PostAsync(Path.Combine(_api.Urls.First(), JsonApiMock.EndpointPath), httpContent)
                .GetAwaiter()
                .GetResult();
            result.EnsureSuccessStatusCode();
            result.Content.ReadFromJsonAsync(typeof(List<string>)).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void RestSharp_Send()
        {
            var request = new RestSharp.RestRequest(JsonApiMock.EndpointPath, DataFormat.Json);
            request.AddJsonBody(Ids);
            _restSharpClient.PostAsync<List<string>>(request).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void Flurl_Send()
        {
            var response = _flurlClient.Request(JsonApiMock.EndpointPath).PostJsonAsync(Ids).GetAwaiter().GetResult();
            response.GetJsonAsync<List<string>>().GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void NClient_Send()
        {
            _nclient.SendAsync(Ids).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void Refit_Send()
        {
            _refitClient.SendAsync(Ids).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void RestEase_Send()
        {
            _restEaseClient.SendAsync(Ids).GetAwaiter().GetResult();
        }
    }
}

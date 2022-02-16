using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Flurl.Http;
using NClient.Benchmark.Client.Helpers;
using RestSharp;
using WireMock.Server;

// ReSharper disable RedundantNameQualifier
namespace NClient.Benchmark.Client.JsonClient
{
    [SimpleJob(invocationCount: 2000, targetCount: 10)]
    public class JsonClientBenchmark
    {
        private IWireMockServer _api = null!;
        
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
            
            _httpClient = new HttpClient();
            HttpClient_Send();
            
            _restSharpClient = new RestSharp.RestClient(_api.Urls.First());
            RestSharp_Send();
            
            _flurlClient = new FlurlClient(_api.Urls.First());
            Flurl_Send();
            
            _nclient = NClientGallery.Clients.GetRest().For<IJsonClient>(_api.Urls.First()).Build();
            NClient_Send();
            
            _refitClient = Refit.RestService.For<IJsonClient>(_api.Urls.First());
            Refit_Send();
            
            _restEaseClient = RestEase.RestClient.For<IJsonClient>(_api.Urls.First());
            RestEase_Send();
        }
        
        [Benchmark]
        public void HttpClient_CreateAndSend()
        {
            var httpClient = new HttpClient();
            var json = JsonSerializer.Serialize(ArrayProvider.Get());
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
            request.AddJsonBody(ArrayProvider.Get());
            restSharpClient.PostAsync<List<string>>(request).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void Flurl_CreateAndSend()
        {
            var flurlClient = new FlurlClient(_api.Urls.First());
            var response = flurlClient.Request(JsonApiMock.EndpointPath).PostJsonAsync(ArrayProvider.Get()).GetAwaiter().GetResult();
            response.GetJsonAsync<List<string>>().GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void NClient_CreateAndSend()
        {
            var nclient = NClientGallery.Clients.GetRest().For<IJsonClient>(_api.Urls.First()).Build();
            nclient.SendAsync(ArrayProvider.Get()).GetAwaiter().GetResult();
        }

        [Benchmark]
        public void Refit_CreateAndSend()
        {
            var refitClient = Refit.RestService.For<IJsonClient>(_api.Urls.First());
            refitClient.SendAsync(ArrayProvider.Get()).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void RestEase_CreateAndSend()
        {
            var restEasyClient = RestEase.RestClient.For<IJsonClient>(_api.Urls.First());
            restEasyClient.SendAsync(ArrayProvider.Get()).GetAwaiter().GetResult();
        }

        [Benchmark]
        public void HttpClient_Send()
        {
            var json = JsonSerializer.Serialize(ArrayProvider.Get());
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
            request.AddJsonBody(ArrayProvider.Get());
            _restSharpClient.PostAsync<List<string>>(request).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void Flurl_Send()
        {
            var response = _flurlClient.Request(JsonApiMock.EndpointPath).PostJsonAsync(ArrayProvider.Get()).GetAwaiter().GetResult();
            response.GetJsonAsync<List<string>>().GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void NClient_Send()
        {
            _nclient.SendAsync(ArrayProvider.Get()).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void Refit_Send()
        {
            _refitClient.SendAsync(ArrayProvider.Get()).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void RestEase_Send()
        {
            _restEaseClient.SendAsync(ArrayProvider.Get()).GetAwaiter().GetResult();
        }
    }
}

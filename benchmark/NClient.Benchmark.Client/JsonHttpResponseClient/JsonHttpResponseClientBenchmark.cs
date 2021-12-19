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
using NClient.Benchmark.Client.JsonClient;
using RestSharp;
using WireMock.Server;

// ReSharper disable RedundantNameQualifier
namespace NClient.Benchmark.Client.JsonHttpResponseClient
{
    [SimpleJob(invocationCount: 2000, targetCount: 10)]
    public class JsonHttpResponseClientBenchmark
    {
        private IWireMockServer _api = null!;
        
        private HttpClient _httpClient = null!;
        private RestSharp.RestClient _restSharpClient = null!;
        private FlurlClient _flurlClient = null!;
        private INClientJsonHttpResponseClient _nclient = null!;
        private IRefitJsonHttpResponseClient _refitClient = null!;
        private IRestEaseJsonHttpResponseClient _restEaseClient = null!;

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
            
            _nclient = NClientGallery.Clients.GetRest().For<INClientJsonHttpResponseClient>(_api.Urls.First()).Build();
            NClient_Send();
            
            _refitClient = Refit.RestService.For<IRefitJsonHttpResponseClient>(_api.Urls.First());
            Refit_Send();
            
            _restEaseClient = RestEase.RestClient.For<IRestEaseJsonHttpResponseClient>(_api.Urls.First());
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
            var response = restSharpClient.ExecutePostAsync<List<string>>(request).GetAwaiter().GetResult();
            if (!response.IsSuccessful)
                throw response.ErrorException!;
            var _ = response.Data;
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
            var nclient = NClientGallery.Clients.GetRest().For<INClientJsonHttpResponseClient>(_api.Urls.First()).Build();
            var response = nclient.SendAsync(ArrayProvider.Get()).GetAwaiter().GetResult();
            response.EnsureSuccess();
            var _ = response.Data;
        }

        [Benchmark]
        public void Refit_CreateAndSend()
        {
            var refitClient = Refit.RestService.For<IRefitJsonHttpResponseClient>(_api.Urls.First());
            var response = refitClient.SendAsync(ArrayProvider.Get()).GetAwaiter().GetResult();
            if (!response.IsSuccessStatusCode)
                throw response.Error!;
            var _ = response.Content;
        }
        
        [Benchmark]
        public void RestEase_CreateAndSend()
        {
            var restEasyClient = RestEase.RestClient.For<IRestEaseJsonHttpResponseClient>(_api.Urls.First());
            var response = restEasyClient.SendAsync(ArrayProvider.Get()).GetAwaiter().GetResult();
            response.ResponseMessage.EnsureSuccessStatusCode();
            var _ = response.GetContent();
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
            var response = _restSharpClient.ExecutePostAsync<List<string>>(request).GetAwaiter().GetResult();
            if (!response.IsSuccessful)
                throw response.ErrorException!;
            var _ = response.Data;
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
            var response = _nclient.SendAsync(ArrayProvider.Get()).GetAwaiter().GetResult();
            response.EnsureSuccess();
            var _ = response.Data;
        }
        
        [Benchmark]
        public void Refit_Send()
        {
            var response = _refitClient.SendAsync(ArrayProvider.Get()).GetAwaiter().GetResult();
            if (!response.IsSuccessStatusCode)
                throw response.Error!;
            var _ = response.Content;
        }
        
        [Benchmark]
        public void RestEase_Send()
        {
            var response = _restEaseClient.SendAsync(ArrayProvider.Get()).GetAwaiter().GetResult();
            response.ResponseMessage.EnsureSuccessStatusCode();
            var _ = response.GetContent();
        }
    }
}

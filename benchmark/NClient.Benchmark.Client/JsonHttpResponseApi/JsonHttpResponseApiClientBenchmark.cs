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
namespace NClient.Benchmark.Client.JsonHttpResponseApi
{
    [SimpleJob(invocationCount: 2000, targetCount: 10)]
    public class JsonHttpResponseApiClientBenchmark
    {
        private static readonly string[] Ids = { "id-1", "id-2", "id-3", "id-4" };
        
        private IWireMockServer _api = null!;
        
        private HttpClient _httpClient = null!;
        private RestSharp.RestClient _restSharpClient = null!;
        private FlurlClient _flurlClient = null!;
        private INClientJsonHttpResponseApiClient _nclient = null!;
        private IRefitJsonHttpResponseApiClient _refitClient = null!;
        private IRestEaseJsonHttpResponseApiClient _restEaseClient = null!;

        [GlobalSetup]
        public void Setup()
        {
            _api = JsonHttpResponseApiMock.MockMethod(Ids);
            
            _httpClient = new HttpClient();
            _restSharpClient = new RestSharp.RestClient(_api.Urls.First());
            _flurlClient = new FlurlClient(_api.Urls.First());
            _nclient = NClientGallery.Clients.GetRest().For<INClientJsonHttpResponseApiClient>(_api.Urls.First()).Build();
            _refitClient = Refit.RestService.For<IRefitJsonHttpResponseApiClient>(_api.Urls.First());
            _restEaseClient = RestEase.RestClient.For<IRestEaseJsonHttpResponseApiClient>(_api.Urls.First());
        }
        
        [Benchmark]
        public void HttpClient_CreateAndSend()
        {
            var httpClient = new HttpClient();
            var json = JsonSerializer.Serialize(Ids);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var result = httpClient
                .PostAsync(Path.Combine(_api.Urls.First(), JsonHttpResponseApiMock.EndpointPath), httpContent)
                .GetAwaiter()
                .GetResult();
            result.EnsureSuccessStatusCode();
            result.Content.ReadFromJsonAsync(typeof(List<string>)).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void RestSharp_CreateAndSend()
        {
            var restSharpClient = new RestSharp.RestClient(_api.Urls.First());
            var request = new RestSharp.RestRequest(JsonHttpResponseApiMock.EndpointPath, DataFormat.Json);
            request.AddJsonBody(Ids);
            var response = restSharpClient.ExecutePostAsync<List<string>>(request).GetAwaiter().GetResult();
            if (!response.IsSuccessful)
                throw response.ErrorException!;
            var _ = response.Data;
        }
        
        [Benchmark]
        public void Flurl_CreateAndSend()
        {
            var flurlClient = new FlurlClient(_api.Urls.First());
            var response = flurlClient.Request(JsonHttpResponseApiMock.EndpointPath).PostJsonAsync(Ids).GetAwaiter().GetResult();
            response.GetJsonAsync<List<string>>().GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void NClient_CreateAndSend()
        {
            var nclient = NClientGallery.Clients.GetRest().For<INClientJsonHttpResponseApiClient>(_api.Urls.First()).Build();
            var response = nclient.SendAsync(Ids).GetAwaiter().GetResult();
            response.EnsureSuccess();
            var _ = response.Data;
        }

        [Benchmark]
        public void Refit_CreateAndSend()
        {
            var refitClient = Refit.RestService.For<IRefitJsonHttpResponseApiClient>(_api.Urls.First());
            var response = refitClient.SendAsync(Ids).GetAwaiter().GetResult();
            if (!response.IsSuccessStatusCode)
                throw response.Error!;
            var _ = response.Content;
        }
        
        [Benchmark]
        public void RestEase_CreateAndSend()
        {
            var restEasyClient = RestEase.RestClient.For<IRestEaseJsonHttpResponseApiClient>(_api.Urls.First());
            var response = restEasyClient.SendAsync(Ids).GetAwaiter().GetResult();
            response.ResponseMessage.EnsureSuccessStatusCode();
            var _ = response.GetContent();
        }

        [Benchmark]
        public void HttpClient_Send()
        {
            var json = JsonSerializer.Serialize(Ids);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var result = _httpClient
                .PostAsync(Path.Combine(_api.Urls.First(), JsonHttpResponseApiMock.EndpointPath), httpContent)
                .GetAwaiter()
                .GetResult();
            result.EnsureSuccessStatusCode();
            result.Content.ReadFromJsonAsync(typeof(List<string>)).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void RestSharp_Send()
        {
            var request = new RestSharp.RestRequest(JsonHttpResponseApiMock.EndpointPath, DataFormat.Json);
            request.AddJsonBody(Ids);
            var response = _restSharpClient.ExecutePostAsync<List<string>>(request).GetAwaiter().GetResult();
            if (!response.IsSuccessful)
                throw response.ErrorException!;
            var _ = response.Data;
        }
        
        [Benchmark]
        public void Flurl_Send()
        {
            var response = _flurlClient.Request(JsonHttpResponseApiMock.EndpointPath).PostJsonAsync(Ids).GetAwaiter().GetResult();
            response.GetJsonAsync<List<string>>().GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void NClient_Send()
        {
            var response = _nclient.SendAsync(Ids).GetAwaiter().GetResult();
            response.EnsureSuccess();
            var _ = response.Data;
        }
        
        [Benchmark]
        public void Refit_Send()
        {
            var response = _refitClient.SendAsync(Ids).GetAwaiter().GetResult();
            if (!response.IsSuccessStatusCode)
                throw response.Error!;
            var _ = response.Content;
        }
        
        [Benchmark]
        public void RestEase_Send()
        {
            var response = _restEaseClient.SendAsync(Ids).GetAwaiter().GetResult();
            response.ResponseMessage.EnsureSuccessStatusCode();
            var _ = response.GetContent();
        }
    }
}

using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using BenchmarkDotNet.Attributes;
using Flurl.Http;
using RestSharp;
using WireMock.Server;

// ReSharper disable RedundantNameQualifier
namespace NClient.Benchmark.Client.PrimitiveHttpResponseApi
{
    [SimpleJob(invocationCount: 2000, targetCount: 10)]
    public class PrimitiveHttpResponseApiClientBenchmark
    {
        private const int Id = 1;
        
        private IWireMockServer _api = null!;
        
        private HttpClient _httpClient = null!;
        private RestSharp.RestClient _restSharpClient = null!;
        private FlurlClient _flurlClient = null!;
        private INClientPrimitiveHttpResponseApiClient _nclient = null!;
        private IRefitPrimitiveHttpResponseApiClient _refitClient = null!;
        private IRestEasePrimitiveHttpResponseApiClient _restEaseClient = null!;

        [GlobalSetup]
        public void Setup()
        {
            _api = PrimitiveHttpResponseApiMock.MockMethod(Id);
            
            _httpClient = new HttpClient();
            _restSharpClient = new RestSharp.RestClient(_api.Urls.First());
            _flurlClient = new FlurlClient(_api.Urls.First());
            _nclient = NClientGallery.Clients.GetRest().For<INClientPrimitiveHttpResponseApiClient>(_api.Urls.First()).Build();
            _refitClient = Refit.RestService.For<IRefitPrimitiveHttpResponseApiClient>(_api.Urls.First());
            _restEaseClient = RestEase.RestClient.For<IRestEasePrimitiveHttpResponseApiClient>(_api.Urls.First());
        }
        
        [Benchmark]
        public void HttpClient_CreateAndSend()
        {
            var httpClient = new HttpClient();
            var result = httpClient
                .GetAsync(Path.Combine(_api.Urls.First(), PrimitiveHttpResponseApiMock.EndpointPath + $"?{PrimitiveHttpResponseApiMock.ParamName}=" + Id))
                .GetAwaiter()
                .GetResult();
            result.EnsureSuccessStatusCode();
            result.Content.ReadFromJsonAsync(typeof(int)).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void RestSharp_CreateAndSend()
        {
            var restSharpClient = new RestSharp.RestClient(_api.Urls.First());
            var request = new RestSharp.RestRequest(PrimitiveHttpResponseApiMock.EndpointPath, DataFormat.Json);
            request.AddQueryParameter(PrimitiveHttpResponseApiMock.ParamName, Id.ToString());
            var response = restSharpClient.ExecuteGetAsync<int>(request).GetAwaiter().GetResult();
            if (!response.IsSuccessful)
                throw response.ErrorException!;
            var _ = response.Data;
        }
        
        [Benchmark]
        public void Flurl_CreateAndSend()
        {
            var flurlClient = new FlurlClient(_api.Urls.First());
            var response = flurlClient.Request(PrimitiveHttpResponseApiMock.EndpointPath).SetQueryParam(PrimitiveHttpResponseApiMock.ParamName, Id).GetAsync().GetAwaiter().GetResult();
            response.GetJsonAsync<int>().GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void NClient_CreateAndSend()
        {
            var nclient = NClientGallery.Clients.GetRest().For<INClientPrimitiveHttpResponseApiClient>(_api.Urls.First()).Build();
            var response = nclient.SendAsync(Id).GetAwaiter().GetResult();
            response.EnsureSuccess();
            var _ = response.Data;
        }

        [Benchmark]
        public void Refit_CreateAndSend()
        {
            var refitClient = Refit.RestService.For<IRefitPrimitiveHttpResponseApiClient>(_api.Urls.First());
            var response = refitClient.SendAsync(Id).GetAwaiter().GetResult();
            if (!response.IsSuccessStatusCode)
                throw response.Error!;
            var _ = response.Content;
        }
        
        [Benchmark]
        public void RestEase_CreateAndSend()
        {
            var restEasyClient = RestEase.RestClient.For<IRestEasePrimitiveHttpResponseApiClient>(_api.Urls.First());
            var response = restEasyClient.SendAsync(Id).GetAwaiter().GetResult();
            response.ResponseMessage.EnsureSuccessStatusCode();
            var _ = response.GetContent();
        }

        [Benchmark]
        public void HttpClient_Send()
        {
            var result = _httpClient
                .GetAsync(Path.Combine(_api.Urls.First(), PrimitiveHttpResponseApiMock.EndpointPath + $"?{PrimitiveHttpResponseApiMock.ParamName}=" + Id))
                .GetAwaiter()
                .GetResult();
            result.EnsureSuccessStatusCode();
            result.Content.ReadFromJsonAsync(typeof(int)).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void RestSharp_Send()
        {
            var request = new RestSharp.RestRequest(PrimitiveHttpResponseApiMock.EndpointPath, DataFormat.Json);
            request.AddQueryParameter(PrimitiveHttpResponseApiMock.ParamName, Id.ToString());
            var response = _restSharpClient.ExecuteGetAsync<int>(request).GetAwaiter().GetResult();
            if (!response.IsSuccessful)
                throw response.ErrorException!;
            var _ = response.Data;
        }
        
        [Benchmark]
        public void Flurl_Send()
        {
            var response = _flurlClient.Request(PrimitiveHttpResponseApiMock.EndpointPath).SetQueryParam(PrimitiveHttpResponseApiMock.ParamName, Id).GetAsync().GetAwaiter().GetResult();
            response.GetJsonAsync<int>().GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void NClient_Send()
        {
            var response = _nclient.SendAsync(Id).GetAwaiter().GetResult();
            response.EnsureSuccess();
            var _ = response.Data;
        }
        
        [Benchmark]
        public void Refit_Send()
        {
            var response = _refitClient.SendAsync(Id).GetAwaiter().GetResult();
            if (!response.IsSuccessStatusCode)
                throw response.Error!;
            var _ = response.Content;
        }
        
        [Benchmark]
        public void RestEase_Send()
        {
            var response = _restEaseClient.SendAsync(Id).GetAwaiter().GetResult();
            response.ResponseMessage.EnsureSuccessStatusCode();
            var _ = response.GetContent();
        }
    }
}

using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using BenchmarkDotNet.Attributes;
using Flurl.Http;
using NClient.Benchmark.Client.Helpers;
using RestSharp;
using WireMock.Server;

// ReSharper disable RedundantNameQualifier
namespace NClient.Benchmark.Client.PrimitiveClient
{
    [SimpleJob(invocationCount: 2000, targetCount: 10)]
    public class PrimitiveClientBenchmark
    {
        private IWireMockServer _api = null!;
        
        private HttpClient _httpClient = null!;
        private RestSharp.RestClient _restSharpClient = null!;
        private FlurlClient _flurlClient = null!;
        private IPrimitiveClient _nclient = null!;
        private IPrimitiveClient _refitClient = null!;
        private IPrimitiveClient _restEaseClient = null!;

        [GlobalSetup]
        public void Setup()
        {
            _api = PrimitiveApiMock.MockMethod();
            
            _httpClient = new HttpClient();
            _restSharpClient = new RestSharp.RestClient(_api.Urls.First());
            _flurlClient = new FlurlClient(_api.Urls.First());
            _nclient = NClientGallery.Clients.GetRest().For<IPrimitiveClient>(_api.Urls.First()).Build();
            _refitClient = Refit.RestService.For<IPrimitiveClient>(_api.Urls.First());
            _restEaseClient = RestEase.RestClient.For<IPrimitiveClient>(_api.Urls.First());
        }
        
        [Benchmark]
        public void HttpClient_CreateAndSend()
        {
            var httpClient = new HttpClient();
            var result = httpClient
                .GetAsync(Path.Combine(_api.Urls.First(), PrimitiveApiMock.EndpointPath + $"?{PrimitiveApiMock.ParamName}=" + IdProvider.Get()))
                .GetAwaiter()
                .GetResult();
            result.EnsureSuccessStatusCode();
            result.Content.ReadFromJsonAsync(typeof(int)).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void RestSharp_CreateAndSend()
        {
            var restSharpClient = new RestSharp.RestClient(_api.Urls.First());
            var request = new RestSharp.RestRequest(PrimitiveApiMock.EndpointPath, DataFormat.Json);
            request.AddQueryParameter(PrimitiveApiMock.ParamName, IdProvider.Get().ToString());
            restSharpClient.GetAsync<int>(request).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void Flurl_CreateAndSend()
        {
            var flurlClient = new FlurlClient(_api.Urls.First());
            var response = flurlClient.Request(PrimitiveApiMock.EndpointPath).SetQueryParam(PrimitiveApiMock.ParamName, IdProvider.Get()).GetAsync().GetAwaiter().GetResult();
            response.GetJsonAsync<int>().GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void NClient_CreateAndSend()
        {
            var nclient = NClientGallery.Clients.GetRest().For<IPrimitiveClient>(_api.Urls.First()).Build();
            nclient.SendAsync(IdProvider.Get()).GetAwaiter().GetResult();
        }

        [Benchmark]
        public void Refit_CreateAndSend()
        {
            var refitClient = Refit.RestService.For<IPrimitiveClient>(_api.Urls.First());
            refitClient.SendAsync(IdProvider.Get()).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void RestEase_CreateAndSend()
        {
            var restEasyClient = RestEase.RestClient.For<IPrimitiveClient>(_api.Urls.First());
            restEasyClient.SendAsync(IdProvider.Get()).GetAwaiter().GetResult();
        }

        [Benchmark]
        public void HttpClient_Send()
        {
            var result = _httpClient
                .GetAsync(Path.Combine(_api.Urls.First(), PrimitiveApiMock.EndpointPath + $"?{PrimitiveApiMock.ParamName}=" + IdProvider.Get()))
                .GetAwaiter()
                .GetResult();
            result.EnsureSuccessStatusCode();
            result.Content.ReadFromJsonAsync(typeof(int)).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void RestSharp_Send()
        {
            var request = new RestSharp.RestRequest(PrimitiveApiMock.EndpointPath, DataFormat.Json);
            request.AddQueryParameter(PrimitiveApiMock.ParamName, IdProvider.Get().ToString());
            _restSharpClient.GetAsync<int>(request).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void Flurl_Send()
        {
            var response = _flurlClient.Request(PrimitiveApiMock.EndpointPath).SetQueryParam(PrimitiveApiMock.ParamName, IdProvider.Get()).GetAsync().GetAwaiter().GetResult();
            response.GetJsonAsync<int>().GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void NClient_Send()
        {
            _nclient.SendAsync(IdProvider.Get()).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void Refit_Send()
        {
            _refitClient.SendAsync(IdProvider.Get()).GetAwaiter().GetResult();
        }
        
        [Benchmark]
        public void RestEase_Send()
        {
            _restEaseClient.SendAsync(IdProvider.Get()).GetAwaiter().GetResult();
        }
    }
}

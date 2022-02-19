using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using BenchmarkDotNet.Attributes;
using Flurl.Http;
using NClient.Benchmark.Client.Helpers;
using NClient.Benchmark.Client.PrimitiveClient;
using RestSharp;
using WireMock.Server;

// ReSharper disable RedundantNameQualifier
namespace NClient.Benchmark.Client.PrimitiveHttpResponseClient
{
    [SimpleJob(invocationCount: 2000, targetCount: 10)]
    public class PrimitiveHttpResponseClientBenchmark
    {
        private IWireMockServer _api = null!;
        
        private HttpClient _httpClient = null!;
        private RestSharp.RestClient _restSharpClient = null!;
        private FlurlClient _flurlClient = null!;
        private INClientPrimitiveHttpResponseClient _nclient = null!;
        private IRefitPrimitiveHttpResponseClient _refitClient = null!;
        private IRestEasePrimitiveHttpResponseClient _restEaseClient = null!;

        [GlobalSetup]
        public void Setup()
        {
            _api = PrimitiveApiMock.MockMethod();
            
            _httpClient = new HttpClient();
            HttpClient_Send();
            
            _restSharpClient = new RestSharp.RestClient(new Uri(_api.Urls.First()));
            RestSharp_Send();
            
            _flurlClient = new FlurlClient(_api.Urls.First());
            Flurl_Send();
            
            _nclient = NClientGallery.Clients.GetRest().For<INClientPrimitiveHttpResponseClient>(new Uri(_api.Urls.First())).Build();
            NClient_Send();
            
            _refitClient = Refit.RestService.For<IRefitPrimitiveHttpResponseClient>(_api.Urls.First());
            Refit_Send();
            
            _restEaseClient = RestEase.RestClient.For<IRestEasePrimitiveHttpResponseClient>(_api.Urls.First());
            RestSharp_Send();
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
            var response = restSharpClient.ExecuteGetAsync<int>(request).GetAwaiter().GetResult();
            if (!response.IsSuccessful)
                throw response.ErrorException!;
            var _ = response.Data;
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
            var nclient = NClientGallery.Clients.GetRest().For<INClientPrimitiveHttpResponseClient>(new Uri(_api.Urls.First())).Build();
            var response = nclient.SendAsync(IdProvider.Get()).GetAwaiter().GetResult();
            response.EnsureSuccess();
            var _ = response.Data;
        }

        [Benchmark]
        public void Refit_CreateAndSend()
        {
            var refitClient = Refit.RestService.For<IRefitPrimitiveHttpResponseClient>(_api.Urls.First());
            var response = refitClient.SendAsync(IdProvider.Get()).GetAwaiter().GetResult();
            if (!response.IsSuccessStatusCode)
                throw response.Error!;
            var _ = response.Content;
        }
        
        [Benchmark]
        public void RestEase_CreateAndSend()
        {
            var restEasyClient = RestEase.RestClient.For<IRestEasePrimitiveHttpResponseClient>(_api.Urls.First());
            var response = restEasyClient.SendAsync(IdProvider.Get()).GetAwaiter().GetResult();
            response.ResponseMessage.EnsureSuccessStatusCode();
            var _ = response.GetContent();
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
            var response = _restSharpClient.ExecuteGetAsync<int>(request).GetAwaiter().GetResult();
            if (!response.IsSuccessful)
                throw response.ErrorException!;
            var _ = response.Data;
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
            var response = _nclient.SendAsync(IdProvider.Get()).GetAwaiter().GetResult();
            response.EnsureSuccess();
            var _ = response.Data;
        }
        
        [Benchmark]
        public void Refit_Send()
        {
            var response = _refitClient.SendAsync(IdProvider.Get()).GetAwaiter().GetResult();
            if (!response.IsSuccessStatusCode)
                throw response.Error!;
            var _ = response.Content;
        }
        
        [Benchmark]
        public void RestEase_Send()
        {
            var response = _restEaseClient.SendAsync(IdProvider.Get()).GetAwaiter().GetResult();
            response.ResponseMessage.EnsureSuccessStatusCode();
            var _ = response.GetContent();
        }
    }
}

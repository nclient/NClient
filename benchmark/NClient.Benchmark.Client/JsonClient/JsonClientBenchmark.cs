using System.Text.Json;
using BenchmarkDotNet.Attributes;
using NClient.Benchmark.Client.Clients.Flurl;
using NClient.Benchmark.Client.Clients.HttpClient;
using NClient.Benchmark.Client.Clients.NClient;
using NClient.Benchmark.Client.Clients.Refit;
using NClient.Benchmark.Client.Clients.RestEase;
using NClient.Benchmark.Client.Clients.RestSharp;
using NClient.Benchmark.Client.Helpers;

// ReSharper disable RedundantNameQualifier
namespace NClient.Benchmark.Client.JsonClient
{
    [SimpleJob(invocationCount: 3000, targetCount: 10)]
    public class JsonClientBenchmark
    {
        private TestHttpClient _httpClient = null!;
        private TestRestSharpClient _restSharpSharpClient = null!;
        private TestFlurlClient _flurlClient = null!;
        private TestNClient<IJsonClient> _nclient = null!;
        private TestRefitClient<IJsonClient> _refitClient = null!;
        private TestRestEase<IJsonClient> _restEaseClient = null!;

        [GlobalSetup]
        public void Setup()
        {
            var jsonSerializerOptions = new JsonSerializerOptions();
            
            _httpClient = new TestHttpClient(jsonSerializerOptions);
            HttpClient_Send();
            
            _restSharpSharpClient = new TestRestSharpClient(jsonSerializerOptions);
            RestSharp_Send();
            
            _flurlClient = new TestFlurlClient(jsonSerializerOptions); 
            Flurl_Send();
            
            _nclient = new TestNClient<IJsonClient>(jsonSerializerOptions);
            NClient_Send();

            _refitClient = new TestRefitClient<IJsonClient>(jsonSerializerOptions);
            Refit_Send();
            
            _restEaseClient = new TestRestEase<IJsonClient>(jsonSerializerOptions);
            RestEase_Send();
        }

        [Benchmark]
        public void HttpClient_Send() => _httpClient.Send(data: DtoProvider.Get());

        [Benchmark]
        public void RestSharp_Send() => _restSharpSharpClient.Send(DtoProvider.Get());

        [Benchmark]
        public void Flurl_Send() => _flurlClient.Send(DtoProvider.Get());

        [Benchmark]
        public void NClient_Send() => _nclient.Send(x => x.SendAsync(DtoProvider.Get()).GetAwaiter().GetResult());

        [Benchmark]
        public void Refit_Send() => _refitClient.Send(x => x.SendAsync(DtoProvider.Get()).GetAwaiter().GetResult());

        [Benchmark]
        public void RestEase_Send() => _restEaseClient.Send(x => x.SendAsync(DtoProvider.Get()).GetAwaiter().GetResult());
    }
}

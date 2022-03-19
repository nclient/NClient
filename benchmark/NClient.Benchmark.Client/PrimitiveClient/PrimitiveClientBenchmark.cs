using BenchmarkDotNet.Attributes;
using NClient.Benchmark.Client.Clients.Flurl;
using NClient.Benchmark.Client.Clients.HttpClient;
using NClient.Benchmark.Client.Clients.NClient;
using NClient.Benchmark.Client.Clients.Refit;
using NClient.Benchmark.Client.Clients.RestEase;
using NClient.Benchmark.Client.Clients.RestSharp;
using NClient.Benchmark.Client.Helpers;

// ReSharper disable RedundantNameQualifier
namespace NClient.Benchmark.Client.PrimitiveClient
{
    [SimpleJob(invocationCount: 10000, targetCount: 10)]
    public class PrimitiveClientBenchmark
    {
        private TestHttpClient _httpClient = null!;
        private TestRestSharpClient _restSharpSharpClient = null!;
        private TestFlurlClient _flurlClient = null!;
        private TestNClient<IPrimitiveClient> _nclient = null!;
        private TestRefitClient<IPrimitiveClient> _refitClient = null!;
        private TestRestEase<IPrimitiveClient> _restEaseClient = null!;

        [GlobalSetup]
        public void Setup()
        {
            _httpClient = new TestHttpClient();
            HttpClient_Send();
            
            _restSharpSharpClient = new TestRestSharpClient();
            RestSharp_Send();
            
            _flurlClient = new TestFlurlClient(); 
            Flurl_Send();
            
            _nclient = new TestNClient<IPrimitiveClient>();
            NClient_Send();

            _refitClient = new TestRefitClient<IPrimitiveClient>();
            Refit_Send();
            
            _restEaseClient = new TestRestEase<IPrimitiveClient>();
            RestEase_Send();
        }

        [Benchmark]
        public void HttpClient_Send() => _httpClient.SendWithoutBody(IdProvider.Get());

        [Benchmark]
        public void RestSharp_Send() => _restSharpSharpClient.SendWithoutBody(IdProvider.Get());

        [Benchmark]
        public void Flurl_Send() => _flurlClient.SendWithoutBody(IdProvider.Get());

        [Benchmark]
        public void NClient_Send() => _nclient.Send(x => x.SendAsync(IdProvider.Get()).GetAwaiter().GetResult());

        [Benchmark]
        public void Refit_Send() => _refitClient.Send(x => x.SendAsync(IdProvider.Get()).GetAwaiter().GetResult());

        [Benchmark]
        public void RestEase_Send() => _restEaseClient.Send(x => x.SendAsync(IdProvider.Get()).GetAwaiter().GetResult());
    }
}

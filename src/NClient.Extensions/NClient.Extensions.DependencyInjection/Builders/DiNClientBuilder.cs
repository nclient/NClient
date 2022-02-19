using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    internal class DiNClientBuilder<TClient, TRequest, TResult> 
        : IHttpClientBuilder, IDiNClientBuilder<TClient, TRequest, TResult>
        where TClient : class
    {
        private readonly IHttpClientBuilder _httpClientBuilder;

        public string Name => _httpClientBuilder.Name;
        public IServiceCollection Services => _httpClientBuilder.Services;

        public DiNClientBuilder(IHttpClientBuilder httpClientBuilder)
        {
            _httpClientBuilder = httpClientBuilder;
        }
    }
}

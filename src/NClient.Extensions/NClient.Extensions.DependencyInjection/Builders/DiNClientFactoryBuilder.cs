using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    internal class DiNClientFactoryBuilder<TRequest, TResult> 
        : IHttpClientBuilder, IDiNClientFactoryBuilder<TRequest, TResult>
    {
        private readonly IHttpClientBuilder _httpClientBuilder;

        public string Name => _httpClientBuilder.Name;
        public IServiceCollection Services => _httpClientBuilder.Services;

        public DiNClientFactoryBuilder(IHttpClientBuilder httpClientBuilder)
        {
            _httpClientBuilder = httpClientBuilder;
        }
    }
}

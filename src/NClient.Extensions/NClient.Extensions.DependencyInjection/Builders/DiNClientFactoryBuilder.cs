using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    internal class DiNClientFactoryBuilder<TRequest, TResult> 
        : IHttpClientBuilder, IDiNClientFactoryBuilder<TRequest, TResult>
    {
        public string Name { get; }
        public IServiceCollection Services { get; }

        public DiNClientFactoryBuilder(string name, IServiceCollection services)
        {
            Name = name;
            Services = services;
        } 
        
        public DiNClientFactoryBuilder(IHttpClientBuilder httpClientBuilder)
        {
            Name = httpClientBuilder.Name;
            Services = httpClientBuilder.Services;
        }
    }
}

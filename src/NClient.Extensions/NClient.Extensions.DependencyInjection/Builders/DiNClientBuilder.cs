using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    internal class DiNClientBuilder<TClient, TRequest, TResult> 
        : IHttpClientBuilder, IDiNClientBuilder<TClient, TRequest, TResult>
        where TClient : class
    {
        public string Name { get; }
        public IServiceCollection Services { get; }

        public DiNClientBuilder(string name, IServiceCollection services)
        {
            Name = name;
            Services = services;
        } 
        
        public DiNClientBuilder(IHttpClientBuilder httpClientBuilder)
        {
            Name = httpClientBuilder.Name;
            Services = httpClientBuilder.Services;
        }
    }
}

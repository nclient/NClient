using NClient.Abstractions.Building;
using NClient.Providers.HttpClient.System;
using RestSharp;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class EnsuringSuccessExtensions
    {
        public static INClientOptionalBuilder<TClient, IRestRequest, IRestResponse> EnsuringRestSharpSuccess<TClient>(
            this INClientOptionalBuilder<TClient, IRestRequest, IRestResponse> factoryOptionalBuilder) 
            where TClient : class
        {
            return factoryOptionalBuilder.EnsuringCustomSuccess(new DefaultRestSharpEnsuringSettings());
        }
        
        public static INClientFactoryOptionalBuilder<IRestRequest, IRestResponse> EnsuringRestSharpSuccess(
            this INClientFactoryOptionalBuilder<IRestRequest, IRestResponse> factoryOptionalBuilder)
        {
            return factoryOptionalBuilder.EnsuringCustomSuccess(new DefaultRestSharpEnsuringSettings());
        }
    }
}

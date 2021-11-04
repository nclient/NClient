﻿using System.Net.Http;
using NClient.Providers.Api.Rest.Extensions;

namespace NClient
{
    public interface INClientBasicFactoryBuilder
    {
        INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> For(string factoryName);
    }
    
    /// <summary>
    /// The builder used to create the client factory.
    /// </summary>
    public class NClientBasicFactoryBuilder : INClientBasicFactoryBuilder
    {
        public INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> For(string factoryName)
        {
            return new NClientAdvancedFactoryBuilder()
                .For(factoryName)
                .UsingRestApi()
                .UsingHttpTransport()
                .UsingJsonSerializer()
                .WithResponseValidation()
                .WithoutHandling()
                .WithoutResilience()
                .WithResults()
                .WithoutLogging();
        }
    }
}
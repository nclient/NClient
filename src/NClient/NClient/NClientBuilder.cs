using System;
using System.Net.Http;
using NClient.Abstractions.Builders;
using NClient.Abstractions.Exceptions;

namespace NClient
{
    /// <summary>
    /// The builder used to create the client.
    /// </summary>
    public class NClientBuilder
    {
        public INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(string host) 
            where TClient : class
        {
            return new CustomNClientBuilder()
                .For<TClient>(host)
                .UsingHttpClient()
                .UsingJsonSerializer()
                // TODO: to extension or provider
                .EnsuringSuccess(
                    successCondition: x => x.Response.IsSuccessStatusCode,
                    onFailure: x =>
                    {
                        try
                        {
                            x.Response.EnsureSuccessStatusCode();
                        }
                        catch (HttpRequestException e)
                        {
                            throw new HttpClientException<HttpRequestMessage, HttpResponseMessage>(x.Request, x.Response, e.Message, e);
                        }
                    })
                .WithoutHandling()
                .WithoutResilience()
                .WithoutLogging();
        }
    }
}

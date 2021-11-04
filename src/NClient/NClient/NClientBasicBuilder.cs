using System.Net.Http;
using NClient.Providers.Api.Rest.Extensions;

namespace NClient
{
    public interface INClientBasicBuilder
    {
        INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(string host) 
            where TClient : class;
    }
    
    /// <summary>
    /// The builder used to create the client.
    /// </summary>
    public class NClientBasicBuilder : INClientBasicBuilder
    {
        public INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(string host) 
            where TClient : class
        {
            return new NClientAdvancedBuilder()
                .For<TClient>(host)
                .UsingRestApi()
                .UsingHttpTransport()
                .UsingJsonSerializer()
                .WithResponseValidation(x => x
                    .ForTransport().UseSystemResponseValidation())
                .WithoutHandling()
                .WithoutResilience()
                .WithResults(x => x
                    .ForTransport().UseHttpResults()
                    .ForClient().UseResults())
                .WithoutLogging()
                .AsBasic();
        }
    }
}

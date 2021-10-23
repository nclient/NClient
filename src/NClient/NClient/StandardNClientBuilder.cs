using System.Net.Http;

namespace NClient
{
    public interface IStandardNClientBuilder
    {
        INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(string host) 
            where TClient : class;
    }
    
    /// <summary>
    /// The builder used to create the client.
    /// </summary>
    public class StandardNClientBuilder : IStandardNClientBuilder
    {
        public INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> For<TClient>(string host) 
            where TClient : class
        {
            return new CustomNClientBuilder()
                .For<TClient>(host)
                .UsingHttpClient()
                .UsingJsonSerializer()
                .WithResponseValidation()
                .WithoutHandling()
                .WithIdempotentResilience()
                .WithResults()
                .WithoutLogging();
        }
    }
}

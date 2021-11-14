using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace NClient.Providers.Transport.Http.RestSharp
{
    internal class RestSharpTransport : ITransport<IRestRequest, IRestResponse>
    {
        private readonly IAuthenticator? _authenticator;

        public RestSharpTransport(IAuthenticator? authenticator = null)
        {
            _authenticator = authenticator;
        }

        public async Task<IRestResponse> ExecuteAsync(IRestRequest transportRequest)
        {
            var restClient = new RestClient
            {
                Authenticator = _authenticator
            };
            
            return await restClient.ExecuteAsync(transportRequest).ConfigureAwait(false);
        }
    }
}

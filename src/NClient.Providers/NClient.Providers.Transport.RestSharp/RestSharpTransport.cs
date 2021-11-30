﻿using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace NClient.Providers.Transport.RestSharp
{
    internal class RestSharpTransport : ITransport<IRestRequest, IRestResponse>
    {
        private readonly IAuthenticator? _authenticator;

        public RestSharpTransport(IAuthenticator? authenticator = null)
        {
            _authenticator = authenticator;
        }

        public async Task<IRestResponse> ExecuteAsync(IRestRequest transportRequest, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var restClient = new RestClient
            {
                Authenticator = _authenticator
            };
            
            return await restClient.ExecuteAsync(transportRequest, cancellationToken).ConfigureAwait(false);
        }
    }
}

using System;

namespace NClient.Providers.Authorization
{
    internal class SingleAccessToken : IAccessTokens
    {
        private readonly IAccessToken _accessToken;

        public SingleAccessToken(IAccessToken accessToken)
        {
            _accessToken = accessToken;
        }
        
        public IAccessToken TryGet(Uri uri)
        {
            return _accessToken;
        }
    }
}

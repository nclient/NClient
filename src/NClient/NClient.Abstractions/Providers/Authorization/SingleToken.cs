using System;

namespace NClient.Providers.Authorization
{
    internal class SingleToken : ITokens
    {
        private readonly IToken _token;

        public SingleToken(IToken token)
        {
            _token = token;
        }
        
        public IToken TryGetToken(Uri uri)
        {
            return _token;
        }
    }
}

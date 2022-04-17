using NClient.Providers;
using NClient.Providers.Authorization;

namespace NClient.Standalone.Client.Authorization
{
    internal class AuthorizationProvider : IAuthorizationProvider
    {
        private readonly ITokens? _tokens;

        public AuthorizationProvider(ITokens? tokens)
        {
            _tokens = tokens;
        }
        
        public IAuthorization Create(IToolset toolset)
        {
            if (_tokens is not null)
                return new Authorization(_tokens);

            return new Authorization();
        }
    }
}

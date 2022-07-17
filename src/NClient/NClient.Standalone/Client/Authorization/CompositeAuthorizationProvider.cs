using System.Collections.Generic;
using System.Linq;
using NClient.Providers;
using NClient.Providers.Authorization;

namespace NClient.Standalone.Client.Authorization
{
    internal class CompositeAuthorizationProvider : IAuthorizationProvider
    {
        private readonly IReadOnlyCollection<IAuthorizationProvider> _authorizationProviders;

        public CompositeAuthorizationProvider(IReadOnlyCollection<IAuthorizationProvider> authorizationProviders)
        {
            _authorizationProviders = authorizationProviders;
        }
        
        public IAuthorization Create(IToolset toolset)
        {
            return new CompositeAuthorization(_authorizationProviders
                .Select(x => x.Create(toolset))
                .ToArray());
        }
    }
}

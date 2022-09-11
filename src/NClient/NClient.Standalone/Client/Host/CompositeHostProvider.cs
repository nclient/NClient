using System.Collections.Generic;
using System.Linq;
using NClient.Providers;
using NClient.Providers.Host;

namespace NClient.Standalone.Client.Host
{
    internal class CompositeHostProvider : IHostProvider
    {
        private readonly IReadOnlyCollection<IHostProvider> _hostProviders;

        public CompositeHostProvider(IReadOnlyCollection<IHostProvider> hostProviders)
        {
            _hostProviders = hostProviders;
        }
        
        public IHost Create(IToolset toolset)
        {
            return new CompositeHost(_hostProviders
                .Select(x => x.Create(toolset))
                .ToArray());
        }
    }
}

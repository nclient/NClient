using System;

namespace NClient.DotNetTool.Loaders
{
    public class LoaderFactory : ILoaderFactory
    {
        public ISpecificationLoader Create(CommandLineOptions opts)
        {
            return Uri.TryCreate(opts.Spec, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
                    ? new NetworkLoader(uriResult)
                    : new FileLoader(opts.Spec);
        }
    }
}
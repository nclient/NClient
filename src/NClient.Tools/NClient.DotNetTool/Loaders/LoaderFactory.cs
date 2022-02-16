using System;
using NClient.DotNetTool.Options;

namespace NClient.DotNetTool.Loaders
{
    public class LoaderFactory : ILoaderFactory
    {
        public ISpecificationLoader Create(InterfaceGenerationOptions generationOptions)
        {
            return Uri.TryCreate(generationOptions.Spec, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
                    ? new NetworkLoader(uriResult)
                    : new FileLoader(generationOptions.Spec);
        }
    }
}
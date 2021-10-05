using System;

// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    public static class NClientGalleryExtensions
    {
        public static IInjectedNClientBuilder GetInjected(this NativeNClientGallery _, IServiceProvider serviceProvider, string? httpClientName = null) => 
            new InjectedNClientBuilder(serviceProvider, httpClientName);
    }
}

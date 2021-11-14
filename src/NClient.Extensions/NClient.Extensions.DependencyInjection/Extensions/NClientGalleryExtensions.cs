using System;

// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    public static class NClientGalleryExtensions
    {
        public static INClientInjectedBuilder GetInjected(this ClientGallery _, IServiceProvider serviceProvider, string? httpClientName = null) => 
            new NClientInjectedBuilder(serviceProvider, httpClientName);
    }
}

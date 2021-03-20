using System;
using Microsoft.Extensions.DependencyInjection;
using NClient.AspNetProxy.Controllers;

namespace NClient.AspNetProxy.Extensions
{
    public static class AddNClientControllerExtensions
    {
        public static IServiceCollection AddNClientControllers(this IServiceCollection serviceCollection,
            Func<IControllerListOptions, IControllerListOptions> configure)
        {
            var options = configure(new VirtualControllerListOptions(serviceCollection));
            var optionsImpl = (VirtualControllerListOptions)options;
            optionsImpl.ApplyChanges();
            return optionsImpl.ServiceCollection;
        }
    }
}

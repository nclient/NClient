using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NClient.AspNetProxy.Attributes;
using NClient.AspNetProxy.Controllers;

namespace NClient.AspNetProxy.Extensions
{
    public static class AddNClientControllerExtensions
    {
        public static IServiceCollection AddNClientControllers(this IServiceCollection serviceCollection,
            Func<VirtualControllerConfigurator, VirtualControllerConfigurator> configure)
        {
            var configurator = configure(new VirtualControllerConfigurator(serviceCollection));
            configurator.ApplyChanges();
            return configurator.ServiceCollection;
        }
    }
}

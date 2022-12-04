using Microsoft.Extensions.DependencyInjection;

namespace NClient.Extensions.DependencyInjection.Tests.Helpers
{
    public static class ServiceLifetimeChecker
    {
        public static bool ServiceRegistered<T>(this IServiceCollection? serviceCollection, ServiceLifetime serviceLifetime)
        {
            var serviceDescriptors = serviceCollection?.GetEnumerator();

            if (serviceDescriptors is null)
                return false;

            while (serviceDescriptors.MoveNext())
            {
                var current = serviceDescriptors.Current;

                if (current.Lifetime == serviceLifetime && current.ServiceType == typeof(T))
                    return true;
            }

            return false;
        }
    }
}

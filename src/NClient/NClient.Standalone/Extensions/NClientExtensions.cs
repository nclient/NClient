using NClient.Abstractions;
using NClient.Abstractions.Clients;
using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class NClientExtensions
    {
        public static IResilienceNClient<T> AsResilient<T>(this T client) where T : class, INClient
        {
            Ensure.IsNotNull(client, nameof(client));
            Ensure.IsCompatibleWith<IResilienceNClient<T>>(client, nameof(client));

            return (IResilienceNClient<T>)client;
        }
    }
}

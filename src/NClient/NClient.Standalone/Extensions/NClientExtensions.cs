using NClient.Common.Helpers;
using NClient.Exceptions;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class NClientExtensions
    {
        public static IResilienceNClient<T> AsResilient<T>(this T client) where T : class, INClient
        {
            Ensure.IsNotNull(client, nameof(client));
            Ensure.IsCompatibleWith<IResilienceNClient<T>>(client, nameof(client));

            // ReSharper disable once SuspiciousTypeConversion.Global
            return client as IResilienceNClient<T>
                ?? throw new NClientException($"The client '{client.GetType().Name}' does not implement the interface '{typeof(IResilienceNClient<T>)}'.");
        }

        public static ITransportNClient<T> AsTransport<T>(this T client) where T : class, INClient
        {
            Ensure.IsNotNull(client, nameof(client));
            Ensure.IsCompatibleWith<ITransportNClient<T>>(client, nameof(client));

            // ReSharper disable once SuspiciousTypeConversion.Global
            return client as ITransportNClient<T>
                ?? throw new NClientException($"The client '{client.GetType().Name}' does not implement the interface '{typeof(ITransportNClient<T>)}'.");
        }
    }
}

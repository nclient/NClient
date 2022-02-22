using NClient.Common.Helpers;
using NClient.Exceptions;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class NClientExtensions
    {
        /// <summary>Casts the custom client into a IResilienceNClient client that allows executing requests with resilience policy.</summary>
        public static IResilienceNClient<TClient> AsResilient<TClient>(this TClient client) where TClient : class, INClient
        {
            Ensure.IsNotNull(client, nameof(client));
            Ensure.IsCompatibleWith<IResilienceNClient<TClient>>(client, nameof(client));

            // ReSharper disable once SuspiciousTypeConversion.Global
            return client as IResilienceNClient<TClient>
                ?? throw new NClientException($"The client '{client.GetType().Name}' does not implement the interface '{typeof(IResilienceNClient<TClient>)}'.");
        }

        /// <summary>Casts the custom client into a ITransportNClient client that allows returning NClient response with the DTO.</summary>
        public static ITransportNClient<TClient> AsTransport<TClient>(this TClient client) where TClient : class, INClient
        {
            Ensure.IsNotNull(client, nameof(client));
            Ensure.IsCompatibleWith<ITransportNClient<TClient>>(client, nameof(client));

            // ReSharper disable once SuspiciousTypeConversion.Global
            return client as ITransportNClient<TClient>
                ?? throw new NClientException($"The client '{client.GetType().Name}' does not implement the interface '{typeof(ITransportNClient<TClient>)}'.");
        }
    }
}

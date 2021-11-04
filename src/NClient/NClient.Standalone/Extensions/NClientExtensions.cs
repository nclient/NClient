using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class NClientExtensions
    {
        // TODO: validation
        public static IResilienceNClient<T> AsResilient<T>(this T client) where T : class, INClient
        {
            Ensure.IsNotNull(client, nameof(client));
            Ensure.IsCompatibleWith<IResilienceNClient<T>>(client, nameof(client));

            return (IResilienceNClient<T>)client;
        }

        public static ITransportNClient<T> AsHttp<T>(this T client) where T : class, INClient
        {
            Ensure.IsNotNull(client, nameof(client));
            Ensure.IsCompatibleWith<ITransportNClient<T>>(client, nameof(client));

            return (ITransportNClient<T>)client;
        }
    }
}

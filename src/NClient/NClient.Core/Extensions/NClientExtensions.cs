using NClient.Abstractions.Clients;
using NClient.Common.Helpers;

namespace NClient.Core.Extensions
{
    public static class NClientExtensions
    {
        public static IResilienceNClient<T> AsResilience<T>(this T client) where T : class, INClient
        {
            Ensure.IsNotNull(client, nameof(client));
            Ensure.IsCompatibleWith<IResilienceNClient<T>>(client, nameof(client));

            return (IResilienceNClient<T>)client;
        }

        public static IHttpNClient<T> AsHttp<T>(this T client) where T : class, INClient
        {
            Ensure.IsNotNull(client, nameof(client));
            Ensure.IsCompatibleWith<IHttpNClient<T>>(client, nameof(client));

            return (IHttpNClient<T>)client;
        }
    }
}

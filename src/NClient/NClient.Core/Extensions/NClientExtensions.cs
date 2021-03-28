using NClient.Abstractions.Clients;

namespace NClient.Core.Extensions
{
    public static class NClientExtensions
    {
        public static IResilienceNClient<T> AsResilience<T>(this T client) where T : INClient
        {
            return (IResilienceNClient<T>)client;
        }

        public static IHttpNClient<T> AsHttp<T>(this T client) where T : INClient
        {
            return (IHttpNClient<T>)client;
        }
    }
}

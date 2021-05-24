using System;

namespace NClient.Abstractions.Exceptions.Providers
{
    public interface IClientInfoProviderException
    {
        string ClientName { get; }
        string MethodName { get; }
        public Exception WithRequestInfo(string clientName, string methodName);
    }
}
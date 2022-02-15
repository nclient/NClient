using System.Collections.Generic;
using System.Net.Http;
using NClient.Exceptions;

namespace NClient.Providers.Transport.SystemNetHttp.Helpers
{
    internal interface ISystemNetHttpMethodMapper
    {
        HttpMethod Map(RequestType requestType);
    }
    
    internal class SystemNetHttpMethodMapper : ISystemNetHttpMethodMapper
    {
        private static readonly Dictionary<RequestType, HttpMethod> HttpMethodByRequestTypes = new()
        {
            [RequestType.Info] = HttpMethod.Options,
            [RequestType.Check] = HttpMethod.Head,
            [RequestType.Read] = HttpMethod.Get,
            [RequestType.Create] = HttpMethod.Post,
            [RequestType.Update] = HttpMethod.Put,
            #if !NETSTANDARD2_0
            [RequestType.PartialUpdate] = HttpMethod.Patch,
            #endif
            [RequestType.Delete] = HttpMethod.Delete
        };

        public HttpMethod Map(RequestType requestType)
        {
            if (HttpMethodByRequestTypes.TryGetValue(requestType, out var method))
                return method;
            throw new ClientValidationException($"The request type '{requestType}' is not supported by the selected transport implementation.");
        }
    }
}

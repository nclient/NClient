using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using NClient.Exceptions;

namespace NClient.Providers.Transport.SystemNetHttp.Helpers
{
    internal interface ISystemHttpMethodMapper
    {
        HttpMethod Map(RequestType requestType);
        RequestType Map(HttpMethod method);
    }
    
    internal class SystemHttpMethodMapper : ISystemHttpMethodMapper
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
        
        private static readonly Dictionary<HttpMethod, RequestType> RequestTypesByHttpMethod = HttpMethodByRequestTypes
            .ToDictionary(x => x.Value, x => x.Key);
        
        public HttpMethod Map(RequestType requestType)
        {
            if (HttpMethodByRequestTypes.TryGetValue(requestType, out var method))
                return method;
            throw new ClientValidationException($"The request type '{requestType}' is not supported by the selected transport implementation.");
        }
        
        public RequestType Map(HttpMethod method)
        {
            if (RequestTypesByHttpMethod.TryGetValue(method, out var requestType))
                return requestType;
            throw new ClientValidationException($"The HTTP method '{method}' is not supported by the selected transport implementation.");
        }
    }
}

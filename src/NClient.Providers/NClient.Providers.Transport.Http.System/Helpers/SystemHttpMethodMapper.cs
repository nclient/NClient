using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace NClient.Providers.Transport.Http.System.Helpers
{
    public interface ISystemHttpMethodMapper
    {
        HttpMethod Map(RequestType requestType);
        RequestType Map(HttpMethod method);
    }
    
    public class SystemHttpMethodMapper : ISystemHttpMethodMapper
    {
        private static readonly Dictionary<RequestType, HttpMethod> HttpMethodByRequestTypes = new()
        {
            [RequestType.Read] = HttpMethod.Get,
            [RequestType.Create] = HttpMethod.Post,
            [RequestType.Update] = HttpMethod.Put,
            [RequestType.Delete] = HttpMethod.Delete,
            [RequestType.Head] = HttpMethod.Head,
            [RequestType.Options] = HttpMethod.Options,
            [RequestType.Trace] = HttpMethod.Trace,
            #if !NETSTANDARD2_0
            [RequestType.Patch] = HttpMethod.Patch
            #endif
        };
        
        private static readonly Dictionary<HttpMethod, RequestType> RequestTypesByHttpMethod = HttpMethodByRequestTypes
            .ToDictionary(x => x.Value, x => x.Key);
        
        public HttpMethod Map(RequestType requestType)
        {
            return HttpMethodByRequestTypes[requestType];
        }
        
        public RequestType Map(HttpMethod method)
        {
            return RequestTypesByHttpMethod[method];
        }
    }
}

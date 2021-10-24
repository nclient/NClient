using System.Collections.Generic;
using System.Linq;
using RestSharp;

namespace NClient.Providers.Transport.Http.RestSharp.Helpers
{
    public interface IRestSharpMethodMapper
    {
        Method Map(RequestType requestType);
        RequestType Map(Method method);
    }
    
    public class RestSharpMethodMapper : IRestSharpMethodMapper
    {
        private static readonly Dictionary<RequestType, Method> MethodByRequestTypes = new()
        {
            [RequestType.Read] = Method.GET,
            [RequestType.Create] = Method.POST,
            [RequestType.Update] = Method.PUT,
            [RequestType.Delete] = Method.DELETE,
            [RequestType.Head] = Method.HEAD,
            [RequestType.Options] = Method.OPTIONS,
            [RequestType.Patch] = Method.PATCH
        };
        
        private static readonly Dictionary<Method, RequestType> RequestTypesByMethod = MethodByRequestTypes
            .ToDictionary(x => x.Value, x => x.Key);
        
        public Method Map(RequestType requestType)
        {
            return MethodByRequestTypes[requestType];
        }
        
        public RequestType Map(Method method)
        {
            return RequestTypesByMethod[method];
        }
    }
}

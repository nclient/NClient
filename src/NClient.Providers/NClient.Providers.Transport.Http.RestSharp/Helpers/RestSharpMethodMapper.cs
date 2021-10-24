using System.Collections.Generic;
using System.Linq;
using NClient.Exceptions;
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
            [RequestType.Info] = Method.OPTIONS,
            [RequestType.Check] = Method.HEAD,
            [RequestType.Read] = Method.GET,
            [RequestType.Create] = Method.POST,
            [RequestType.Update] = Method.PUT,
            [RequestType.PartialUpdate] = Method.PATCH,
            [RequestType.Delete] = Method.DELETE
        };
        
        private static readonly Dictionary<Method, RequestType> RequestTypesByMethod = MethodByRequestTypes
            .ToDictionary(x => x.Value, x => x.Key);
        
        public Method Map(RequestType requestType)
        {
            if (MethodByRequestTypes.TryGetValue(requestType, out var method))
                return method;
            throw new ClientValidationException($"The request type '{requestType}' is not supported by the selected transport implementation.");
        }
        
        public RequestType Map(Method method)
        {
            if (RequestTypesByMethod.TryGetValue(method, out var requestType))
                return requestType;
            throw new ClientValidationException($"The RestSharp method '{method}' is not supported by the selected transport implementation.");
        }
    }
}

using System.Collections.Generic;
using NClient.Exceptions;
using RestSharp;

namespace NClient.Providers.Transport.RestSharp.Helpers
{
    internal interface IRestSharpMethodMapper
    {
        Method Map(RequestType requestType);
    }
    
    internal class RestSharpMethodMapper : IRestSharpMethodMapper
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
        
        public Method Map(RequestType requestType)
        {
            if (MethodByRequestTypes.TryGetValue(requestType, out var method))
                return method;
            throw new ClientValidationException($"The request type '{requestType}' is not supported by the selected transport implementation.");
        }
    }
}

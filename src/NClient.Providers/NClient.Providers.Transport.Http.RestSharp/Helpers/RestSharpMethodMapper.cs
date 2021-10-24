using System;
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
        public Method Map(RequestType requestType)
        {
            Enum.TryParse(requestType.ToString(), out Method method);
            return method;
        }
        
        public RequestType Map(Method method)
        {
            // TODO: обработать результат TryParse
            Enum.TryParse(method.ToString(), ignoreCase: true, out RequestType transportMethod);
            return transportMethod;
        }
    }
}

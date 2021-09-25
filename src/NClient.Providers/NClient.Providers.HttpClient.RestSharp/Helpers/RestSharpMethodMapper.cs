using System;
using System.Net.Http;
using RestSharp;

namespace NClient.Providers.HttpClient.RestSharp.Helpers
{
    public interface IRestSharpMethodMapper
    {
        Method Map(HttpMethod httpMethod);
        HttpMethod Map(Method method);
    }
    
    public class RestSharpMethodMapper : IRestSharpMethodMapper
    {
        public Method Map(HttpMethod httpMethod)
        {
            Enum.TryParse(httpMethod.ToString(), out Method method);
            return method;
        }
        
        public HttpMethod Map(Method method)
        {
            return new HttpMethod(method.ToString());
        }
    }
}

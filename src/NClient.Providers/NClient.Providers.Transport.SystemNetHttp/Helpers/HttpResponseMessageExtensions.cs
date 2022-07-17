using System.Globalization;
using System.Net;
using System.Net.Http;

namespace NClient.Providers.Transport.SystemNetHttp.Helpers
{
    public static class HttpResponseMessageExtensions
    {
        public static HttpRequestException? TryGetException(this HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.IsSuccessStatusCode)
                return null;
            
            #if NET5_0_OR_GREATER
            return new HttpRequestException(
                GetErrorMessage(httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase), 
                inner: null, 
                httpResponseMessage.StatusCode);
            #else
            return new HttpRequestException(
                GetErrorMessage(httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase), 
                inner: null);
            #endif
        }
        
        private static string GetErrorMessage(HttpStatusCode httpStatusCode, string reasonPhrase)
        {
            return string.Format(
                CultureInfo.InvariantCulture, 
                format: "Response status code does not indicate success: {0} ({1}).", 
                httpStatusCode, reasonPhrase);
        }
    }
}

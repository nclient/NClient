using System;
using System.Collections.Generic;
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
        private readonly Dictionary<string, HttpMethod> _httpMethodByNames = new(StringComparer.OrdinalIgnoreCase)
        {
            [HttpMethod.Get.ToString()] = HttpMethod.Get,
            [HttpMethod.Post.ToString()] = HttpMethod.Post,
            [HttpMethod.Put.ToString()] = HttpMethod.Put,
            [HttpMethod.Delete.ToString()] = HttpMethod.Delete,
            [HttpMethod.Head.ToString()] = HttpMethod.Head,
            [HttpMethod.Options.ToString()] = HttpMethod.Options,
            [HttpMethod.Trace.ToString()] = HttpMethod.Trace,
            #if !NETSTANDARD2_0
            [HttpMethod.Patch.ToString()] = HttpMethod.Patch
            #endif
        };
        
        public HttpMethod Map(RequestType requestType)
        {
            return _httpMethodByNames[requestType.ToString()];
        }
        
        public RequestType Map(HttpMethod method)
        {
            // TODO: обработать результат TryParse
            Enum.TryParse(method.ToString(), ignoreCase: true, out RequestType transportMethod);
            return transportMethod;
        }
    }
}

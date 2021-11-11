using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NClient.Providers.Transport.Http.RestSharp.Builders;
using NClient.Providers.Transport.Http.RestSharp.Helpers;
using RestSharp;

namespace NClient.Providers.Transport.Http.RestSharp
{
    internal class RestSharpResponseBuilder : IResponseBuilder<IRestRequest, IRestResponse>
    {
        private static readonly HashSet<string> ContentHeaderNames = new(new HttpContentKnownHeaderNames());
        
        private readonly IFinalHttpRequestBuilder _finalHttpRequestBuilder;

        public RestSharpResponseBuilder(IFinalHttpRequestBuilder finalHttpRequestBuilder)
        {
            _finalHttpRequestBuilder = finalHttpRequestBuilder;
        }

        public Task<IResponse> BuildAsync(IRequest request, IResponseContext<IRestRequest, IRestResponse> responseContext)
        {
            var finalRequest = _finalHttpRequestBuilder.Build(request, responseContext.Response.Request);
            
            var allHeaders = responseContext.Response.Headers
                .Where(x => x.Name != null)
                .Select(x => new Metadata(x.Name!, x.Value?.ToString() ?? ""))
                .ToArray();
            var responseHeaders = allHeaders
                .Where(x => !ContentHeaderNames.Contains(x.Name))
                .ToArray();
            var contentHeaders = allHeaders
                .Where(x => ContentHeaderNames.Contains(x.Name))
                .ToArray();
            
            var response = new Response(finalRequest)
            {
                Content = new Content(responseContext.Response.RawBytes, responseContext.Response.ContentEncoding, new MetadataContainer(contentHeaders)),
                StatusCode = (int) responseContext.Response.StatusCode,
                Endpoint = responseContext.Response.ResponseUri.ToString(),
                Metadatas = new MetadataContainer(responseHeaders),
                ErrorMessage = responseContext.Response.ErrorMessage,
                ErrorException = responseContext.Response.ErrorException,
                ProtocolVersion = responseContext.Response.ProtocolVersion,
                IsSuccessful = responseContext.Response.IsSuccessful
            };

            return Task.FromResult<IResponse>(response);
        }
    }
}

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Transport.RestSharp.Helpers;
using RestSharp;

namespace NClient.Providers.Transport.RestSharp
{
    internal class RestSharpResponseBuilder : IResponseBuilder<IRestRequest, IRestResponse>
    {
        private static readonly HashSet<string> ContentHeaderNames = new(new HttpContentKnownHeaderNames());

        public Task<IResponse> BuildAsync(IRequest request, 
            IResponseContext<IRestRequest, IRestResponse> responseContext, bool allocateMemoryForContent,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

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
            
            var response = new Response(request)
            {
                Content = new Content(
                    new MemoryStream(responseContext.Response.RawBytes), 
                    responseContext.Response.ContentEncoding, 
                    new MetadataContainer(contentHeaders)),
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

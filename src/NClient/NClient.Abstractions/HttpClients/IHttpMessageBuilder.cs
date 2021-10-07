using System;
using System.Threading.Tasks;

namespace NClient.Abstractions.HttpClients
{
    // TODO: doc
    public interface IHttpMessageBuilder<TRequest, TResponse>
    {
        Task<TRequest> BuildRequestAsync(IHttpRequest httpRequest);
        Task<IHttpResponse> BuildResponseAsync(Guid requestId, Type? requestDataType, TResponse response);
        Task<IHttpResponse> BuildResponseWithDataAsync(object? data, Type dataType, IHttpResponse httpResponse);
        Task<IHttpResponse> BuildResponseWithErrorAsync(object? error, Type errorType, IHttpResponse httpResponse);
        Task<IHttpResponse> BuildResponseWithDataAndErrorAsync(object? data, Type dataType, object? error, Type errorType, IHttpResponse httpResponse);
    }
}

using System;
using System.Net;

namespace NClient.Annotations
{
    public interface IResponseAttribute
    {
        /// <summary>
        /// Gets or sets the type of the value returned by an action.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Gets or sets the HTTP status code of the response.
        /// </summary>
        HttpStatusCode StatusCode { get; }
    }
}

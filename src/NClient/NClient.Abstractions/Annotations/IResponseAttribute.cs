using System;
using System.Net;

namespace NClient.Annotations
{
    /// <summary>A filter that specifies the type of the value and status code returned by the action.</summary>
    public interface IResponseAttribute
    {
        /// <summary>Gets or sets the type of the value returned by an action.</summary>
        Type Type { get; }

        /// <summary>Gets or sets the HTTP status code of the response.</summary>
        HttpStatusCode StatusCode { get; }
    }
}
